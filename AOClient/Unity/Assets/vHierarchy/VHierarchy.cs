#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor.IMGUI.Controls;
using UnityEditor.Experimental.SceneManagement;
using Type = System.Type;
using static VHierarchy.VHierarchyData;
using static VHierarchy.VHierarchyCache;
using static VHierarchy.Libs.VUtils;
using static VHierarchy.Libs.VGUI;



namespace VHierarchy
{
    public static class VHierarchy
    {
        static void GameObjectRowGUI(GameObject go, Rect rowRect)
        {
            var fullRowRect = rowRect.SetX(32).SetXMax(rowRect.xMax + 16);

            var isRowHovered = fullRowRect.AddWidthFromRight(32).IsHovered();


            var isRowSelected = false;
            var isRowBeingRenamed = false;
            var isTreeFocused = false;

            void setState()
            {
                void set_isRowSelected()
                {
                    if (!curEvent.isRepaint) return;

#if UNITY_2021_1_OR_NEWER
                    var dragSelectionList = treeViewController?.GetFieldValue("m_DragSelection")?.GetFieldValue<List<int>>("m_List");
#else
                    var dragSelectionList = treeViewController?.GetFieldValue<List<int>>("m_DragSelection");
#endif

                    var dragging = dragSelectionList != null && dragSelectionList.Any();

                    isRowSelected = dragging ? (dragSelectionList.Contains(go.GetInstanceID())) : Selection.Contains(go);

                }
                void set_isRowBeingRenamed()
                {
                    if (!curEvent.isRepaint) return;

                    isRowBeingRenamed = EditorGUIUtility.editingTextField &&
                                        isRowSelected &&
                                        treeViewController?.GetMemberValue("state")?.GetMemberValue("renameOverlay")?.InvokeMethod<bool>("IsRenaming") == true;

                }
                void set_isTreeFocused()
                {
                    if (!curEvent.isRepaint) return;

                    isTreeFocused = EditorWindow.focusedWindow == hierarchyWindow &&
                                    GUIUtility.keyboardControl == hierarchyWindow?.GetMemberValue("sceneHierarchy")?.GetMemberValue<int>("m_TreeViewKeyboardControlID");

                }
                void set_lastVisibleSelectedRowRect()
                {
                    if (!Selection.gameObjects.Contains(go)) return;

                    lastVisibleSelectedRowRect = rowRect;

                }
                void set_mousePressed()
                {
                    if (curEvent.isMouseDown && isRowHovered)
                        mousePressed = true;

                    if (curEvent.isMouseUp || curEvent.isMouseLeaveWindow || curEvent.isDragPerform)
                        mousePressed = false;

                }
                void set_hoveredGo()
                {
                    if (curEvent.isLayout)
                        hoveredGo = null;

                    if (curEvent.isRepaint && isRowHovered)
                        hoveredGo = go;

                }

                set_isRowSelected();
                set_isRowBeingRenamed();
                set_isTreeFocused();
                set_lastVisibleSelectedRowRect();
                set_mousePressed();
                set_hoveredGo();

            }


            void drawing()
            {
                if (!curEvent.isRepaint) { hierarchyLines_isFirstRowDrawn = false; return; }

                var goData = GetGameObjectData(go, createDataIfDoesntExist: false);

                var showBackgroundColor = goData != null && goData.colorIndex.IsInRange(1, VHierarchyPalette.colorsCount);// && !(isRowSelected && isHierarchyFocused);
                var showCustomIcon = goData != null && !goData.iconNameOrGuid.IsNullOrEmpty();
                var showDefaultIcon = !showCustomIcon && (isRowBeingRenamed || (!VHierarchyMenu.minimalModeEnabled || (PrefabUtility.IsAddedGameObjectOverride(go) && PrefabUtility.IsPartOfPrefabInstance(go))));

                var makeTriangleBrighter = showBackgroundColor && goData.colorIndex > VHierarchyPalette.greyColorsCount && isDarkTheme;
                var makeNameBrighter = showBackgroundColor && goData.colorIndex > VHierarchyPalette.greyColorsCount && isDarkTheme;

                Color defaultBackground;


                void calcDefaultBackground()
                {
                    var selectedFocused = GUIColors.selectedBackground;
                    var selectedUnfocused = isDarkTheme ? Greyscale(.3f) : Greyscale(.68f);
                    var hovered = isDarkTheme ? Greyscale(.265f) : Greyscale(.7f);
                    var normal = GUIColors.windowBackground;

                    if (isRowSelected && !isRowBeingRenamed)
                        defaultBackground = isTreeFocused ? selectedFocused : selectedUnfocused;

                    else if (isRowHovered)
                        defaultBackground = hovered;

                    else
                        defaultBackground = normal;

                }
                void hideDefaultIcon()
                {
                    if (showDefaultIcon) return;

                    rowRect.SetWidth(16).Draw(defaultBackground);

                }
                void hideName()
                {
                    if (!showBackgroundColor && (showCustomIcon || showDefaultIcon)) return;

                    var nameRect = rowRect.MoveX(16).SetWidth(go.name.GetLabelWidth());
#if UNITY_2023_2_OR_NEWER
                    if (!go.activeInHierarchy && PrefabUtility.IsPartOfPrefabInstance(go))
                        nameRect.width *= 1.1f;
#endif

                    nameRect.Draw(defaultBackground);

                }

                void backgroundColor()
                {
                    if (!showBackgroundColor) return;


                    var hasLeftGradient = go.transform.parent;



                    var colorRect = rowRect.AddWidthFromRight(28).AddWidth(16);

                    if (!isRowSelected)
                        colorRect = colorRect.AddHeightFromMid(EditorGUIUtility.pixelsPerPoint >= 2 ? -.5f : -1);

                    if (hasLeftGradient)
                        colorRect = colorRect.AddWidthFromRight(3);

                    if (PrefabUtility.HasPrefabInstanceAnyOverrides(go, false) && !hasLeftGradient)
                        colorRect = colorRect.AddWidthFromRight(EditorGUIUtility.pixelsPerPoint >= 2 ? -2.5f : -3);




                    var leftGradientWith = go.transform.parent ? 22 : 0;
                    var rightGradientWidth = (fullRowRect.width * .77f).Min(colorRect.width - leftGradientWith);

                    var leftGradientRect = colorRect.SetWidth(leftGradientWith);
                    var rightGradientRect = colorRect.SetWidthFromRight(rightGradientWidth);

                    var flatColorRect = colorRect.SetX(leftGradientRect.xMax).SetXMax(rightGradientRect.x);




                    var colorWithFlatness = palette ? palette.colors[goData.colorIndex - 1] : VHierarchyPalette.GetDefaultColor(goData.colorIndex - 1);

                    var flatness = colorWithFlatness.a;

                    var color = colorWithFlatness.SetAlpha(1);

                    if (isRowHovered)
                        color *= 1.1f;

                    if (isRowSelected)
                        color *= 1.2f;




                    leftGradientRect.AddWidth(1).Draw(color.SetAlpha((flatness - .1f) / .9f));
                    leftGradientRect.AddWidth(1).DrawCurtainLeft(color);

                    flatColorRect.AddWidth(1).Draw(color);

                    rightGradientRect.Draw(color.MultiplyAlpha(flatness));
                    rightGradientRect.DrawCurtainRight(color);


                }
                void triangle()
                {
                    if (!showBackgroundColor) return;
                    if (go.transform.childCount == 0) return;

                    var triangleRect = rowRect.MoveX(-15.5f).SetWidth(16).Resize(1.5f);

                    GUI.DrawTexture(triangleRect, EditorIcons.GetIcon(IsExpanded(go) ? "IN_foldout_on" : "IN_foldout"));


                    if (!makeTriangleBrighter) return;

                    GUI.DrawTexture(triangleRect, EditorIcons.GetIcon(IsExpanded(go) ? "IN_foldout_on" : "IN_foldout"));

                }
                void name()
                {
                    if (!showBackgroundColor && (showCustomIcon || showDefaultIcon)) return;
                    if (isRowBeingRenamed) return;


                    var nameRect = rowRect.MoveX(18);

                    if (VHierarchyMenu.minimalModeEnabled && !showCustomIcon && !showDefaultIcon)
                        nameRect = nameRect.MoveX(-17);

                    if (showBackgroundColor && goData.colorIndex <= VHierarchyPalette.greyColorsCount)
                        nameRect = nameRect.MoveY(.5f);

                    if (!go.activeInHierarchy) // correcting unity's style padding inconsistencies
                        if (PrefabUtility.IsPartOfAnyPrefab(go))
                            nameRect = nameRect.MoveY(-1);
                        else
                            nameRect = nameRect.Move(-1, -1.5f);

                    if (makeNameBrighter && go.activeInHierarchy)
                        nameRect = nameRect.MoveX(-2).MoveY(-.5f);



                    var styleName = PrefabUtility.IsPartOfAnyPrefab(go) ?
                        (go.activeInHierarchy ? "PR PrefabLabel" : "PR DisabledPrefabLabel") :
                        (go.activeInHierarchy ? "TV Line" : "PR DisabledLabel");

                    if (makeNameBrighter && go.activeInHierarchy)
                        styleName = "WhiteLabel";



                    if (makeNameBrighter)
                        SetGUIColor(Greyscale(!go.activeInHierarchy ? 1.4f : isRowSelected ? 1 : .9f));

                    GUI.skin.GetStyle(styleName).Draw(nameRect, go.name, false, false, isRowSelected, hierarchyWindow == EditorWindow.focusedWindow);

                    if (makeNameBrighter)
                        ResetGUIColor();

                }
                void defaultIcon()
                {
                    if (!showBackgroundColor) return;
                    if (!showDefaultIcon) return;

                    var iconRect = rowRect.SetWidth(16);

                    SetGUIColor(go.activeInHierarchy ? Color.white : Greyscale(1, .4f));

                    GUI.DrawTexture(iconRect, PrefabUtility.GetIconForGameObject(go));

                    if (PrefabUtility.IsAddedGameObjectOverride(go))
                        GUI.DrawTexture(iconRect, EditorIcons.GetIcon("PrefabOverlayAdded Icon"));

                    ResetGUIColor();

                }
                void customIcon()
                {
                    if (!showCustomIcon) return;

                    var iconRect = rowRect.SetWidth(16);
                    var iconNameOrPath = goData.iconNameOrGuid.Length == 32 ? goData.iconNameOrGuid.ToPath() : goData.iconNameOrGuid;

                    SetGUIColor(go.activeInHierarchy ? Color.white : Greyscale(1, .4f));

                    GUI.DrawTexture(iconRect, EditorIcons.GetIcon(iconNameOrPath) ?? Texture2D.blackTexture);

                    ResetGUIColor();

                }
                void hierarchyLines()
                {
                    if (!VHierarchyMenu.hierarchyLinesEnabled) return;

                    var lineThickness = 1f;
                    var lineContrast = isDarkTheme ? .35f : .55f;

                    if (isRowSelected)
                        if (isTreeFocused)
                            lineContrast += isDarkTheme ? .1f : -.25f;
                        else
                            lineContrast += isDarkTheme ? .05f : -.05f;


                    var depth = ((rowRect.x - 60) / 14).RoundToInt();

                    bool isLastChild(Transform transform) => transform.parent?.GetChild(transform.parent.childCount - 1) == transform;
                    bool hasChilren(Transform transform) => transform.childCount > 0;

                    void calcVerticalGaps_beforeFirstRowDrawn()
                    {
                        if (hierarchyLines_isFirstRowDrawn) return;

                        hierarchyLines_verticalGaps.Clear();

                        var curTransform = go.transform.parent;
                        var curDepth = depth - 1;

                        while (curTransform != null && curTransform.parent != null)
                        {
                            if (isLastChild(curTransform))
                                hierarchyLines_verticalGaps.Add(curDepth - 1);

                            curTransform = curTransform.parent;
                            curDepth--;
                        }

                    }
                    void updateVerticalGaps_beforeNextRowDrawn()
                    {
                        if (isLastChild(go.transform))
                            hierarchyLines_verticalGaps.Add(depth - 1);

                        if (depth < hierarchyLines_prevRowDepth)
                            hierarchyLines_verticalGaps.RemoveAll(r => r >= depth);

                    }

                    void drawVerticals()
                    {
                        for (int i = 0; i < depth; i++)
                            if (!hierarchyLines_verticalGaps.Contains(i))
                                rowRect.SetX(53 + i * 14 - lineThickness / 2)
                                       .SetWidth(lineThickness)
                                       .SetHeight(isLastChild(go.transform) && i == depth - 1 ? 8 + lineThickness / 2 : 16)
                                       .Draw(Greyscale(lineContrast));

                    }
                    void drawHorizontals()
                    {
                        if (depth == 0) return;

                        rowRect.MoveX(-21)
                               .SetHeightFromMid(lineThickness)
                               .SetWidth(hasChilren(go.transform) ? 7 : 17)
                               .Draw(Greyscale(lineContrast));

                    }



                    calcVerticalGaps_beforeFirstRowDrawn();

                    drawVerticals();
                    drawHorizontals();

                    updateVerticalGaps_beforeNextRowDrawn();

                    hierarchyLines_prevRowDepth = depth;
                    hierarchyLines_isFirstRowDrawn = true;

                }
                void zebraStriping()
                {
                    if (!VHierarchyMenu.zebraStripingEnabled) return;
                    if (isRowSelected) return;

                    var contrast = isDarkTheme ? .033f : .05f;

                    var t = rowRect.y.PingPong(16f) / 16f;

                    fullRowRect.Draw(Greyscale(isDarkTheme ? 1 : 0, contrast * t));

                }


                calcDefaultBackground();
                hideDefaultIcon();
                hideName();

                hierarchyLines();
                backgroundColor();
                triangle();
                name();
                defaultIcon();
                customIcon();
                zebraStriping();

            }

            void componentMinimap()
            {
                if (!VHierarchyMenu.componentMinimapEnabled) return;

                void componentButton(Rect buttonRect, Component component)
                {
                    void componentIcon()
                    {
                        if (!curEvent.isRepaint) return;


                        var normalOpacity = isDarkTheme ? .47f : .7f;
                        var activeOpacity = 1;
                        var pressedOpacity = isDarkTheme ? .65f : .9f;

                        var isActive = (buttonRect.IsHovered() && curEvent.holdingAlt) || VHierarchyComponentWindow.floatingInstance?.component == component;
                        var isPressed = buttonRect.IsHovered() && mousePressed;

                        var icon = GetComponentIcon(component);


                        if (!icon) return;

                        SetGUIColor(Greyscale(1, isActive ? (isPressed ? pressedOpacity : activeOpacity) : normalOpacity));

                        GUI.DrawTexture(buttonRect.SetSizeFromMid(12, 12), icon);

                        ResetGUIColor();

                    }

                    void mouseDown()
                    {
                        if (!curEvent.holdingAlt) return;
                        if (!curEvent.isMouseDown) return;
                        if (!buttonRect.IsHovered()) return;

                        curEvent.Use();

                        mouseDownPos = curEvent.mousePosition;

                    }
                    void mouseUp()
                    {
                        if (!curEvent.holdingAlt) return;
                        if (!curEvent.isMouseUp) return;
                        if (!buttonRect.IsHovered()) return;

                        curEvent.Use();

                        if (VHierarchyComponentWindow.floatingInstance?.component == component) { VHierarchyComponentWindow.floatingInstance.Close(); return; }


                        var position = EditorGUIUtility.GUIToScreenPoint(new Vector2(rowRect.xMax + 25, rowRect.y));

                        if (!VHierarchyComponentWindow.floatingInstance)
                            VHierarchyComponentWindow.CreateFloatingInstance(position);

                        VHierarchyComponentWindow.floatingInstance.Init(component);
                        VHierarchyComponentWindow.floatingInstance.Focus();

                        VHierarchyComponentWindow.floatingInstance.targetPosition = position;

                    }


                    if (curEvent.holdingAlt)
                        buttonRect.MarkInteractive();

                    componentIcon();

                    mouseDown();
                    mouseUp();

                }

                void transformComponent()
                {
                    if (!isRowHovered) return;
                    if (!curEvent.holdingAlt) return;
                    if (!go.GetComponent<Transform>()) return;

                    componentButton(fullRowRect.SetWidth(13).MoveX(1.5f), go.GetComponent<Transform>());

                }
                void otherComponetns()
                {
                    var buttonWidth = 13;
                    var minButtonX = rowRect.x + go.name.GetLabelWidth() + buttonWidth + 2;
                    var buttonRect = fullRowRect.SetWidthFromRight(buttonWidth).MoveX(-1.5f);

                    if (PrefabUtility.IsAnyPrefabInstanceRoot(go) && !PrefabUtility.IsPartOfModelPrefab(go))
                        buttonRect = buttonRect.MoveX(-13);

                    foreach (var component in go.GetComponents<Component>())
                    {
                        if (component is Transform) continue;
                        if (buttonRect.x < minButtonX) continue;

                        componentButton(buttonRect, component);

                        buttonRect = buttonRect.MoveX(-buttonWidth);

                    }


                }

                transformComponent();
                otherComponetns();

            }
            void activationToggle()
            {
                if (!VHierarchyMenu.activationToggleEnabled) return;
                if (!isRowHovered) return;

                var toggleRect = fullRowRect.SetWidth(16).MoveX(1);


                SetGUIColor(Greyscale(1, .9f));

                var newActiveSelf = EditorGUI.Toggle(toggleRect, go.activeSelf);

                ResetGUIColor();


                if (newActiveSelf == go.activeSelf) return;

                var gos = Selection.gameObjects.Contains(go) ? Selection.gameObjects : new[] { go };
                var newActive = gos != null && !gos.Any(r => r && r.activeSelf);

                foreach (var r in gos)
                    r.RecordUndo();

                foreach (var r in gos)
                    r.SetActive(newActiveSelf);

                GUI.FocusControl(null);

            }

            void altDrag()
            {
                if (!curEvent.holdingAlt) return;

                void mouseDown()
                {
                    if (!curEvent.isMouseDown) return;
                    if (!rowRect.IsHovered()) return;

                    mouseDownPos = curEvent.mousePosition;

                }
                void mouseDrag()
                {
                    if (!curEvent.isMouseDrag) return;
                    if ((curEvent.mousePosition - mouseDownPos).magnitude < 5) return;
                    if (!rowRect.Contains(mouseDownPos)) return;
                    if (!rowRect.Contains(curEvent.mousePosition - curEvent.mouseDelta)) return;
                    if (DragAndDrop.objectReferences.Any()) return;

                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new[] { go };
                    DragAndDrop.StartDrag(go.name);

                }

                mouseDown();
                mouseDrag();

                // altdrag has to be set up manually before altClick because altClick will use() mouseDown event to prevent selection change
            }
            void altClick()
            {
                if (!isRowHovered) return;
                if (!curEvent.holdingAlt) return;
                if (Application.isPlaying) return;

                void mouseDown()
                {
                    if (!curEvent.isMouseDown) return;

                    curEvent.Use();

                }
                void mouseUp()
                {
                    if (!curEvent.isMouseUp) return;

                    var editMultiSelection = Selection.gameObjects.Length > 1 && Selection.gameObjects.Contains(go);

                    var gosToEdit = (editMultiSelection ? Selection.gameObjects : new[] { go }).ToList();


                    if (VHierarchyPaletteWindow.instance && VHierarchyPaletteWindow.instance.gameObjects.SequenceEqual(gosToEdit)) { VHierarchyPaletteWindow.instance.Close(); return; }

                    var openNearRect = editMultiSelection ? lastVisibleSelectedRowRect : rowRect;
                    var position = EditorGUIUtility.GUIToScreenPoint(new Vector2(openNearRect.x - 14, openNearRect.y + 18));

                    if (!VHierarchyPaletteWindow.instance)
                        VHierarchyPaletteWindow.CreateInstance(position);

                    VHierarchyPaletteWindow.instance.Init(gosToEdit);
                    VHierarchyPaletteWindow.instance.Focus();

                    VHierarchyPaletteWindow.instance.targetPosition = position;

                    if (editMultiSelection)
                        Selection.objects = null;

                }

                mouseDown();
                mouseUp();

            }


            setState();

            drawing();

            componentMinimap();
            activationToggle();

            altDrag();
            altClick();

        }

        static List<int> hierarchyLines_verticalGaps = new List<int>();
        static bool hierarchyLines_isFirstRowDrawn;
        static int hierarchyLines_prevRowDepth;

        static bool mousePressed;
        static GameObject hoveredGo;
        static Vector2 mouseDownPos;

        static Rect lastVisibleSelectedRowRect;




        static void SceneRowGUI(Scene scene, Rect rowRect)
        {

            void collapseAll()
            {
                if (!VHierarchyMenu.collapseAllButtonEnabled) return;

                var buttonRect = rowRect.SetWidthFromRight(18).MoveX(VHierarchyMenu.editLightingButtonEnabled ? -22 : -4);


                SetGUIColor(Color.clear);

                var clicked = GUI.Button(buttonRect, "");

                var normalColor = isDarkTheme ? Greyscale(.85f) : Greyscale(.1f);
                var hoveredColor = isDarkTheme ? Color.white : normalColor;

                SetGUIColor(buttonRect.IsHovered() ? hoveredColor : normalColor);
                GUI.Label(buttonRect.Resize(1.5f).MoveY(-.5f), EditorGUIUtility.IconContent("PreviewCollapse"));

                ResetGUIColor();


                if (!clicked) return;

                var expandedRoots = new List<GameObject>();
                var expandedChildren = new List<GameObject>();

                foreach (var iid in expandedIds)
                    if (EditorUtility.InstanceIDToObject(iid) is GameObject expandedGo && expandedGo.scene == scene)
                        if (expandedGo.transform.parent)
                            expandedChildren.Add(expandedGo);
                        else
                            expandedRoots.Add(expandedGo);

                expandQueue_toCollapseAfterAnimation = expandedChildren;
                expandQueue_toAnimate = expandedRoots.Select(r => new ExpandQueueEntry { instanceId = r.GetInstanceID(), expand = false })
                                                     .OrderBy(r => VisibleRowIndex(r.instanceId)).ToList();

                EditorApplication.RepaintHierarchyWindow();

            }
            void lighting()
            {
                if (!VHierarchyMenu.editLightingButtonEnabled) return;

                var buttonRect = rowRect.SetWidthFromRight(18).MoveX(-4);


                SetGUIColor(Color.clear);

                var clicked = GUI.Button(buttonRect, "");

                var normalColor = isDarkTheme ? Greyscale(.9f) : Greyscale(1f, .9f);
                var hoveredColor = isDarkTheme ? Color.white : normalColor;

                SetGUIColor(buttonRect.IsHovered() ? hoveredColor : normalColor);

                GUI.Label(buttonRect.Resize(1).MoveY(-.5f), EditorGUIUtility.IconContent("Lighting"));

                ResetGUIColor();


                if (!clicked) return;

                VHierarchyLightingWindow.CreateInstance(EditorGUIUtility.GUIToScreenPoint(curEvent.mousePosition) + new Vector2(8, -8));

                VHierarchyLightingWindow.instance.Focus();

            }

            collapseAll();
            lighting();

        }

        static void RowGUI(int instanceId, Rect rowRect)
        {
            // GUIStopwatch.OnGUIBeginning(iterations: 350); // toremove
            // EditorApplication.RepaintHierarchyWindow();


            if (curEvent.isLayout)
                UpdateExpandQueue();

            if (expandedIds == null)
                UpdateExpandedIdsList();

            if (EditorUtility.InstanceIDToObject(instanceId) is GameObject go)
                GameObjectRowGUI(go, rowRect);
            else
            {
                var iScene = -1;

                for (int i = 0; i < EditorSceneManager.sceneCount; i++)
                    if (EditorSceneManager.GetSceneAt(i).GetHashCode() == instanceId)
                        iScene = i;

                if (iScene != -1)
                    SceneRowGUI(EditorSceneManager.GetSceneAt(iScene), rowRect);
            }

        }





        static void CheckShortcuts() // globalEventHandler
        {
            if (EditorWindow.mouseOverWindow?.GetType() != t_SceneHierarchyWindow) return;
            if (!curEvent.isKeyDown) return;
            if (curEvent.keyCode == KeyCode.None) return;


            void updateHierarchyWindow()
            {
                if (hierarchyWindow == EditorWindow.mouseOverWindow) return;

                _hierarchyWindow = EditorWindow.mouseOverWindow;

                UpdateExpandedIdsList();

            }

            void toggleExpanded()
            {
                if (!hoveredGo) return;
                if (curEvent.holdingAnyModifierKey) return;
                if (!curEvent.isKeyDown || curEvent.keyCode != KeyCode.E) return;
                if (Tools.viewTool == ViewTool.FPS) return;
                if (!VHierarchyMenu.toggleExpandedEnabled) return;

                curEvent.Use();

                if (transformToolNeedsReset = Application.unityVersion.Contains("2022"))
                    previousTransformTool = Tools.current;


                if (hoveredGo.transform.childCount == 0) return;

                SetExpandedWithAnimation(hoveredGo.GetInstanceID(), !expandedIds.Contains(hoveredGo.GetInstanceID()));

                EditorApplication.RepaintHierarchyWindow();

            }
            void toggleActive()
            {
                if (!hoveredGo) return;
                if (curEvent.isNull) return;    // tocheck 
                if (curEvent.holdingAnyModifierKey) return;
                if (!curEvent.isKeyDown || curEvent.keyCode != KeyCode.A) return;
                if (Tools.viewTool == ViewTool.FPS) return;
                if (!VHierarchyMenu.toggleActiveEnabled) return;

                var gos = Selection.gameObjects.Contains(hoveredGo) ? Selection.gameObjects : new[] { hoveredGo };
                var active = !gos.Any(r => r.activeSelf);

                foreach (var r in gos)
                {
                    r.RecordUndo();
                    r.SetActive(active);
                }

                curEvent.Use();

            }
            void delete()
            {
                if (!hoveredGo) return;
                if (curEvent.holdingAnyModifierKey) return;
                if (!curEvent.isKeyDown || curEvent.keyCode != KeyCode.X) return;
                if (!VHierarchyMenu.deleteEnabled) return;

                var gos = Selection.gameObjects.Contains(hoveredGo) ? Selection.gameObjects : new[] { hoveredGo };

                foreach (var r in gos)
                    Undo.DestroyObjectImmediate(r);

                curEvent.Use();
            }
            void collapseEverything()
            {
                if (curEvent.modifiers != (EventModifiers.Shift | EventModifiers.Command) && curEvent.modifiers != (EventModifiers.Shift | EventModifiers.Control)) return;
                if (!curEvent.isKeyDown || curEvent.keyCode != KeyCode.E) return;
                if (!VHierarchyMenu.collapseEverythingEnabled) return;

                curEvent.Use();

                var expandedRoots = new List<GameObject>();
                var expandedChildren = new List<GameObject>();

                foreach (var iid in expandedIds)
                    if (EditorUtility.InstanceIDToObject(iid) is GameObject expandedGo)
                        if (expandedGo.transform.parent)
                            expandedChildren.Add(expandedGo);
                        else
                            expandedRoots.Add(expandedGo);

                expandQueue_toCollapseAfterAnimation = expandedChildren;
                expandQueue_toAnimate = expandedRoots.Select(r => new ExpandQueueEntry { instanceId = r.GetInstanceID(), expand = false })
                                                     .OrderBy(r => VisibleRowIndex(r.instanceId)).ToList();

                EditorApplication.RepaintHierarchyWindow();

            }
            void collapseEverythingElse()
            {
                if (!hoveredGo) return;
                if (curEvent.modifiers != EventModifiers.Shift) return;
                if (!curEvent.isKeyDown || curEvent.keyCode != KeyCode.E) return;
                if (!VHierarchyMenu.collapseEverythingElseEnabled) return;

                curEvent.Use();

                if (hoveredGo.transform.childCount == 0) return;

                var parents = new List<GameObject>();

                var cur = hoveredGo;
                while (cur = cur.transform.parent?.gameObject)
                    parents.Add(cur);

                var toCollapse = new List<GameObject>();

                foreach (var iid in expandedIds.ToList())
                    if (EditorUtility.InstanceIDToObject(iid) is GameObject expandedGo && !parents.Contains(expandedGo) && expandedGo != hoveredGo)
                        toCollapse.Add(expandedGo);


                expandQueue_toAnimate = toCollapse.Select(r => new ExpandQueueEntry { instanceId = r.GetInstanceID(), expand = false })
                                                  .Append(new ExpandQueueEntry { instanceId = hoveredGo.GetInstanceID(), expand = true })
                                                  .OrderBy(r => VisibleRowIndex(r.instanceId)).ToList();

                EditorApplication.RepaintHierarchyWindow();


            }
            void focus()
            {
                if (!curEvent.isKeyDown) return;
                if (curEvent.modifiers != EventModifiers.None) return;
                if (curEvent.keyCode != KeyCode.F) return;
                if (SceneView.sceneViews.Count == 0) return;
                if (!hoveredGo) return;
                if (!VHierarchyMenu.focusEnabled) return;

                var sv = SceneView.lastActiveSceneView;

                if (!sv || !sv.hasFocus)
                    sv = SceneView.sceneViews.ToArray().FirstOrDefault(r => (r as SceneView).hasFocus) as SceneView;

                if (!sv)
                    (sv = SceneView.lastActiveSceneView ?? SceneView.sceneViews[0] as SceneView).Focus();

                sv.Frame(hoveredGo.GetBounds(), false);

            }


            updateHierarchyWindow();

            toggleExpanded();
            toggleActive();
            delete();
            collapseEverything();
            collapseEverythingElse();
            focus();

        }



        static void UpdateExpandQueue() // called from gui because reflected methods rely on event.current
        {
            if (treeViewController.GetPropertyValue<bool>("animatingExpansion")) return;

            if (!expandQueue_toAnimate.Any())
            {
                if (!expandQueue_toCollapseAfterAnimation.Any()) return;

                foreach (var r in expandQueue_toCollapseAfterAnimation)
                    SetExpanded(r.GetInstanceID(), false);

                expandQueue_toCollapseAfterAnimation.Clear();

                return;
            }

            var iid = expandQueue_toAnimate.First().instanceId;
            var expand = expandQueue_toAnimate.First().expand;

            if (expandedIds.Contains(iid) != expand)
                SetExpandedWithAnimation(iid, expand);

            expandQueue_toAnimate.RemoveAt(0);

        }

        static List<ExpandQueueEntry> expandQueue_toAnimate = new List<ExpandQueueEntry>();
        static List<GameObject> expandQueue_toCollapseAfterAnimation = new List<GameObject>();

        struct ExpandQueueEntry { public int instanceId; public bool expand; }


        static void UpdateExpandedIdsList() // delayCall loop
        {
            expandedIds = hierarchyWindow?.GetFieldValue("m_SceneHierarchy")?.GetFieldValue("m_TreeViewState")?.GetPropertyValue<List<int>>("expandedIDs") ?? new List<int>();

            EditorApplication.delayCall -= UpdateExpandedIdsList;
            EditorApplication.delayCall += UpdateExpandedIdsList;

        }

        static List<int> expandedIds = new List<int>();

        static bool IsExpanded(GameObject go) => expandedIds.Contains(go.GetInstanceID());
        static bool IsVisible(GameObject go) => !go.transform.parent || (IsExpanded(go.transform.parent.gameObject) && IsVisible(go.transform.parent.gameObject));

        static void SetExpandedWithAnimation(int instanceId, bool expanded) => treeViewController.InvokeMethod("ChangeFoldingForSingleItem", instanceId, expanded);
        static void SetExpanded(int instanceId, bool expanded) => treeViewController.GetPropertyValue("data").InvokeMethod("SetExpanded", instanceId, expanded); // static void SetExpanded(int instanceId, bool expanded) => hierarchyWindow.InvokeMethod("SetExpanded", instanceId, expanded);
        static int VisibleRowIndex(int instanceId) => treeViewController.GetPropertyValue("data").InvokeMethod<int>("GetRow", instanceId);






        public static string GetComponentName(Component component)
        {
            var s = new GUIContent(EditorGUIUtility.ObjectContent(component, component.GetType())).text;
            s = s.Substring(s.LastIndexOf('(') + 1);
            s = s.Substring(0, s.Length - 1);

            return s;
        }

        public static Texture GetComponentIcon(Component component)
        {
            if (!component) return null;

            if (!componentIcons_byType.ContainsKey(component.GetType()))
                componentIcons_byType[component.GetType()] = EditorGUIUtility.ObjectContent(component, component.GetType()).image;

            return componentIcons_byType[component.GetType()];

        }

        static Dictionary<System.Type, Texture> componentIcons_byType = new Dictionary<Type, Texture>();












        public static GameObjectData GetGameObjectData(GameObject go, bool createDataIfDoesntExist)
        {
            if (!data) return null;
            if (firstDataCacheLayer.TryGetValue(go, out var cachedResult)) return cachedResult;

            GameObjectData goData = null;
            SceneData sceneData = null;

            void sceneObject()
            {
                if (StageUtility.GetCurrentStage() is PrefabStage) return;


                SceneIdMap sceneIdMap = null;

                var currentSceneGuid = go.scene.path.ToGuid();
                var originalSceneGuid = cache.originalSceneGuids_byInstanceId.GetValueOrDefault(go.GetInstanceID()) ?? currentSceneGuid;


                void getSceneDataFromComponents()
                {
                    if (!dataComponents_byScene.ContainsKey(go.scene))
                        dataComponents_byScene[go.scene] = Resources.FindObjectsOfTypeAll<VHierarchyDataComponent>().FirstOrDefault(r => r.gameObject?.scene == go.scene);

                    if (dataComponents_byScene[go.scene])
                        sceneData = dataComponents_byScene[go.scene].sceneData;

                }
                void getSceneDataFromScriptableObject()
                {
                    if (sceneData != null) return;

                    data.sceneDatas_byGuid.TryGetValue(originalSceneGuid, out sceneData);

                }
                void createSceneData()
                {
                    if (sceneData != null) return;
                    if (!createDataIfDoesntExist) return;

                    sceneData = new SceneData();

                    data.sceneDatas_byGuid[originalSceneGuid] = sceneData;

                }

                void getSceneIdMap()
                {
                    if (sceneData == null) return;

                    cache.sceneIdMaps_bySceneGuid.TryGetValue(originalSceneGuid, out sceneIdMap);

                }
                void createSceneIdMap()
                {
                    if (sceneIdMap != null) return;
                    if (sceneData == null) return;
                    if (currentSceneGuid != originalSceneGuid) return;

                    sceneIdMap = new SceneIdMap();

                    cache.sceneIdMaps_bySceneGuid[currentSceneGuid] = sceneIdMap;

                }
                void updateSceneIdMapAndOriginalSceneGuids()
                {
                    if (sceneIdMap == null) return;
                    if (currentSceneGuid != originalSceneGuid) return;


                    var curInstanceIdsHash = go.scene.GetRootGameObjects().FirstOrDefault()?.GetInstanceID() ?? 0;
                    var curGlobalIdsHash = sceneData.goDatas_byGlobalId.Keys.Aggregate(0, (hash, r) => hash ^= r.GetHashCode());

                    if (sceneIdMap.instanceIdsHash == curInstanceIdsHash && sceneIdMap.globalIdsHash == curGlobalIdsHash) return;


                    var globalIds = sceneData.goDatas_byGlobalId.Keys.ToList();
                    var instanceIds = globalIds.GetObjectInstanceIds();

                    void clearSceneGuids()
                    {
                        foreach (var instanceId in sceneIdMap.globalIds_byInstanceId.Keys)
                            cache.originalSceneGuids_byInstanceId.Remove(instanceId);

                    }
                    void fillIdMap()
                    {
                        sceneIdMap.globalIds_byInstanceId = new SerializableDictionary<int, GlobalID>();

                        for (int i = 0; i < instanceIds.Length; i++)
                            if (instanceIds[i] != 0)
                                sceneIdMap.globalIds_byInstanceId[instanceIds[i]] = globalIds[i];

                    }
                    void fillSceneGuids()
                    {
                        for (int i = 0; i < instanceIds.Length; i++)
                            cache.originalSceneGuids_byInstanceId[instanceIds[i]] = currentSceneGuid;

                    }


                    clearSceneGuids();
                    fillIdMap();
                    fillSceneGuids();

                    sceneIdMap.instanceIdsHash = curInstanceIdsHash;
                    sceneIdMap.globalIdsHash = curGlobalIdsHash;

                }

                void getGoData()
                {
                    if (sceneData == null) return;
                    if (sceneIdMap == null) return;
                    if (!sceneIdMap.globalIds_byInstanceId.TryGetValue(go.GetInstanceID(), out var globalId)) return;

                    sceneData.goDatas_byGlobalId.TryGetValue(globalId, out goData);

                }
                void moveGoDataToCurrentSceneGuid() // totest
                {
                    if (goData == null) return;
                    if (currentSceneGuid == originalSceneGuid) return;
                    if (Application.isPlaying) return;

                    var originalSceneData = sceneData;
                    var currentSceneData = dataComponents_byScene.GetValueOrDefault(go.scene)?.sceneData ?? data.sceneDatas_byGuid.GetValueOrDefault(currentSceneGuid);

                    if (originalSceneData == null) return;
                    if (currentSceneData == null) return;

                    var globalId = go.GetGlobalID();

                    originalSceneData.goDatas_byGlobalId.Remove(originalSceneData.goDatas_byGlobalId.First(r => r.Value == goData).Key);
                    currentSceneData.goDatas_byGlobalId[go.GetGlobalID()] = goData;

                }
                void createGoData()
                {
                    if (goData != null) return;
                    if (!createDataIfDoesntExist) return;

                    goData = new GameObjectData();

                    sceneData.goDatas_byGlobalId[go.GetGlobalID()] = goData;

                }


                getSceneDataFromComponents();
                getSceneDataFromScriptableObject();
                createSceneData();

                getSceneIdMap();
                createSceneIdMap();
                updateSceneIdMapAndOriginalSceneGuids();

                getGoData();
                moveGoDataToCurrentSceneGuid();
                createGoData();

            }
            void prefabObject()
            {
                if (!(StageUtility.GetCurrentStage() is PrefabStage prefabStage)) return;


                var prefabGuid = prefabStage.assetPath.ToGuid();
                var sourceGlobalId = new GlobalID(go.GetGlobalID().ToString().Replace("-2-", "-1-").Replace("00000000000000000000000000000000", prefabGuid));


                void fixGlobalId_2023_2()
                {

#if UNITY_2023_2_OR_NEWER

                var so = new SerializedObject(go);

                so.SetPropertyValue("inspectorMode", UnityEditor.InspectorMode.Debug);

                var fileId = so.FindProperty("m_LocalIdentfierInFile").longValue;


                sourceGlobalId = new GlobalID($"GlobalObjectId_V1-1-{prefabGuid}-{fileId}-0");


#endif
                }

                void getSceneDataFromScriptableObject()
                {
                    data.sceneDatas_byGuid.TryGetValue(prefabGuid, out sceneData);
                }
                void createSceneData()
                {
                    if (sceneData != null) return;
                    if (!createDataIfDoesntExist) return;

                    sceneData = new SceneData();

                    data.sceneDatas_byGuid[prefabGuid] = sceneData;

                }

                void getGoData()
                {
                    if (sceneData == null) return;

                    sceneData.goDatas_byGlobalId.TryGetValue(sourceGlobalId, out goData);

                }
                void createGoData()
                {
                    if (goData != null) return;
                    if (!createDataIfDoesntExist) return;

                    goData = new GameObjectData();

                    sceneData.goDatas_byGlobalId[sourceGlobalId] = goData;

                }


                fixGlobalId_2023_2();

                getSceneDataFromScriptableObject();
                createSceneData();

                getGoData();
                createGoData();

            }
            void prefabInstance()
            {
                if (!PrefabUtility.IsPartOfPrefabInstance(go)) return;
                if (goData != null) return;


                var globalId = PrefabUtility.GetCorrespondingObjectFromOriginalSource(go).GetGlobalID();
                var prefabGuid = globalId.guid;


                void getSceneDataFromScriptableObject()
                {
                    data.sceneDatas_byGuid.TryGetValue(prefabGuid, out sceneData);
                }

                void getGoData()
                {
                    if (sceneData == null) return;

                    sceneData.goDatas_byGlobalId.TryGetValue(globalId, out goData);
                }


                getSceneDataFromScriptableObject();

                getGoData();

            }

            sceneObject();
            prefabObject();
            prefabInstance();

            if (goData != null)
                goData.sceneData = sceneData;

            firstDataCacheLayer[go] = goData;

            return goData;

        }

        public static Dictionary<GameObject, GameObjectData> firstDataCacheLayer = new Dictionary<GameObject, GameObjectData>(); // cleared on data serialization callbacks, ie when data is added or removed

        public static Dictionary<Scene, VHierarchyDataComponent> dataComponents_byScene = new Dictionary<Scene, VHierarchyDataComponent>();

        static VHierarchyCache cache => VHierarchyCache.instance;








        static Texture2D GetIcon_forVTabs(GameObject gameObject)
        {
            var goData = GetGameObjectData(gameObject, false);

            if (goData == null) return null;

            var iconNameOrPath = goData.iconNameOrGuid.Length == 32 ? goData.iconNameOrGuid.ToPath() : goData.iconNameOrGuid;

            if (!iconNameOrPath.IsNullOrEmpty())
                return EditorIcons.GetIcon(iconNameOrPath);

            return null;

        }

        static string GetIconName_forVFavorites(GameObject gameObject)
        {
            var goData = GetGameObjectData(gameObject, false);

            if (goData == null) return "";

            var iconNameOrPath = goData.iconNameOrGuid.Length == 32 ? goData.iconNameOrGuid.ToPath() : goData.iconNameOrGuid;

            return iconNameOrPath;

        }
        static string GetIconName_forVInspector(GameObject gameObject)
        {
            return GetIconName_forVFavorites(gameObject);
        }







        static void RepaintOnAlt() // Update 
        {
            var lastEvent = typeof(Event).GetFieldValue<Event>("s_Current");

            if (lastEvent.alt != wasAlt)
                if (EditorWindow.mouseOverWindow?.GetType() == t_SceneHierarchyWindow)
                    EditorApplication.RepaintHierarchyWindow();

            wasAlt = lastEvent.alt;

        }

        static bool wasAlt;




        static void SetPreviousTransformTool()
        {
            if (!transformToolNeedsReset) return;

            Tools.current = previousTransformTool;

            transformToolNeedsReset = false;

            // E shortcut changes transform tool in 2022
            // here we undo this

        }

        static bool transformToolNeedsReset;
        static Tool previousTransformTool;







        static void DuplicateSceneData(string originalSceneGuid, string duplicatedSceneGuid)
        {
            var originalSceneData = data.sceneDatas_byGuid[originalSceneGuid];
            var duplicatedSceneData = data.sceneDatas_byGuid[duplicatedSceneGuid] = new SceneData();

            foreach (var kvp in originalSceneData.goDatas_byGlobalId)
            {
                var duplicatedGlobalId = new GlobalID(kvp.Key.ToString().Replace(originalSceneGuid, duplicatedSceneGuid));
                var duplicatedGoData = new GameObjectData() { colorIndex = kvp.Value.colorIndex, iconNameOrGuid = kvp.Value.iconNameOrGuid };

                duplicatedSceneData.goDatas_byGlobalId[duplicatedGlobalId] = duplicatedGoData;

            }

        }

        static void OnSceneImported(string importedScenePath)
        {
            if (curEvent.commandName != "Duplicate" && curEvent.commandName != "Paste") return;


            var copiedAssets_paths = new List<string>();

            var assetClipboard = typeof(Editor).Assembly.GetType("UnityEditor.AssetClipboardUtility").GetMemberValue("assetClipboard").InvokeMethod<IEnumerator>("GetEnumerator");

            while (assetClipboard.MoveNext())
                copiedAssets_paths.Add(assetClipboard.Current.GetMemberValue<GUID>("guid").ToString().ToPath());



            var originalScenePath = copiedAssets_paths.FirstOrDefault(r => File.Exists(r) && new FileInfo(r).Length
                                                                                          == new FileInfo(importedScenePath).Length);
            var originalSceneGuid = originalScenePath.ToGuid();
            var duplicatedSceneGuid = importedScenePath.ToGuid();

            if (!data.sceneDatas_byGuid.ContainsKey(originalSceneGuid)) return;
            if (data.sceneDatas_byGuid.ContainsKey(duplicatedSceneGuid)) return;

            DuplicateSceneData(originalSceneGuid, duplicatedSceneGuid);

        }

        class SceneImportDetector : AssetPostprocessor
        {
            // scene data duplication won't work on earlier versions anyway
#if UNITY_2021_2_OR_NEWER 
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
            {
                if (!data) return;

                foreach (var r in importedAssets)
                    if (r.EndsWith(".unity"))
                        OnSceneImported(r);

            }
#endif
        }







        [InitializeOnLoadMethod]
        static void Init()
        {
            if (VHierarchyMenu.pluginDisabled) return;

            void subscribe()
            {
                EditorApplication.hierarchyWindowItemOnGUI -= RowGUI;
                EditorApplication.hierarchyWindowItemOnGUI = RowGUI + EditorApplication.hierarchyWindowItemOnGUI;

                EditorApplication.update -= RepaintOnAlt;
                EditorApplication.update += RepaintOnAlt;

                EditorApplication.update -= SetPreviousTransformTool;
                EditorApplication.update += SetPreviousTransformTool;

                var globalEventHandler = typeof(EditorApplication).GetFieldValue<EditorApplication.CallbackFunction>("globalEventHandler");
                typeof(EditorApplication).SetFieldValue("globalEventHandler", CheckShortcuts + (globalEventHandler - CheckShortcuts));

                var projectWasLoaded = typeof(EditorApplication).GetFieldValue<UnityEngine.Events.UnityAction>("projectWasLoaded");
                typeof(EditorApplication).SetFieldValue("projectWasLoaded", (projectWasLoaded - ClearCacheOnProjectLoaded) + ClearCacheOnProjectLoaded);

            }
            void loadData()
            {
                data = AssetDatabase.LoadAssetAtPath<VHierarchyData>(EditorPrefs.GetString("vHierarchy-lastKnownDataPath-" + GetProjectId()));


                if (data) return;

                data = AssetDatabase.FindAssets("t:VHierarchyData").Select(guid => AssetDatabase.LoadAssetAtPath<VHierarchyData>(guid.ToPath())).FirstOrDefault();


                if (!data) return;

                EditorPrefs.SetString("vHierarchy-lastKnownDataPath-" + GetProjectId(), data.GetPath());

            }
            void loadPalette()
            {
                palette = AssetDatabase.LoadAssetAtPath<VHierarchyPalette>(EditorPrefs.GetString("vHierarchy-lastKnownPalettePath-" + GetProjectId()));


                if (palette) return;

                palette = AssetDatabase.FindAssets("t:VHierarchyPalette").Select(guid => AssetDatabase.LoadAssetAtPath<VHierarchyPalette>(guid.ToPath())).FirstOrDefault();


                if (!palette) return;

                EditorPrefs.SetString("vHierarchy-lastKnownPalettePath-" + GetProjectId(), palette.GetPath());

            }
            void loadDataAndPaletteDelayed()
            {
                if (!data)
                    EditorApplication.delayCall += () => EditorApplication.delayCall += loadData;

                if (!palette)
                    EditorApplication.delayCall += () => EditorApplication.delayCall += loadPalette;

                // AssetDatabase isn't up to date at this point (it gets updated after InitializeOnLoadMethod)
                // and if current AssetDatabase state doesn't contain the data - it won't be loaded during Init()
                // so here we schedule an additional, delayed attempt to load the data
                // this addresses reports of data loss when trying to load it on a new machine

            }
            void migrateDataFromV1()
            {
                if (!data) return;
                if (EditorPrefs.GetBool("vHierarchy-dataMigrationFromV1Attempted-" + GetProjectId(), false)) return;

                EditorPrefs.SetBool("vHierarchy-dataMigrationFromV1Attempted-" + GetProjectId(), true);

                var lines = System.IO.File.ReadAllLines(data.GetPath());

                if (lines.Length < 15 || !lines[14].Contains("sceneDatasByGuid")) return;

                var sceneGuids = new List<string>();
                var globalIdLists = new List<List<string>>();
                var goDatasByInstanceIdCounts = new List<int>();
                var sceneDatas = new List<SceneData>();


                void parseSceneGuids()
                {
                    for (int i = 16; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("values:")) break;

                        var startIndex = lines[i].IndexOf("- ") + 2;

                        if (startIndex < lines[i].Length)
                            sceneGuids.Add(lines[i].Substring(startIndex));
                        else
                            sceneGuids.Add("");

                    }

                }
                void parseGlobalIdLists_andCountGoDatasByInstanceId()
                {
                    var parsingGlobalIdList = false;
                    var parsingGlobalIdListAtIndex = -1;

                    for (int i = 0; i < lines.Length; i++)
                    {
                        var line = lines[i];

                        void startParsing()
                        {
                            if (!line.Contains("goDatasByGlobalId")) return;

                            parsingGlobalIdList = true;
                            parsingGlobalIdListAtIndex++;

                            globalIdLists.Add(new List<string>());

                        }
                        void parse()
                        {
                            if (!parsingGlobalIdList) return;
                            if (!line.Contains("- GlobalObjectId")) return;

                            var startIndex = line.IndexOf("- ") + 2;

                            if (startIndex < line.Length)
                                globalIdLists[parsingGlobalIdListAtIndex].Add(line.Substring(startIndex));
                            else
                                globalIdLists[parsingGlobalIdListAtIndex].Add("");

                        }
                        void stopParsing_andCountDatasByInstanceId()
                        {
                            if (!line.Contains("goDatasByInstanceId")) return;


                            parsingGlobalIdList = false;


                            var goDatasByInstanceId_keysLine = lines[i + 1];
                            var goDatasByInstanceId_count = (goDatasByInstanceId_keysLine.Length - 14) / 8;

                            goDatasByInstanceIdCounts.Add(goDatasByInstanceId_count);


                        }

                        startParsing();
                        parse();
                        stopParsing_andCountDatasByInstanceId();

                    }

                }
                void parseSceneDatas()
                {
                    var firstLineIndexOfFirstSceneData = 17 + sceneGuids.Count;



                    void parseSceneData(int sceneDataIndex)
                    {
                        var sceneData = new SceneData();

                        var globalIds = globalIdLists[sceneDataIndex];
                        var firstLineIndex = getFirstLineIndex(sceneDataIndex);



                        void parseGoData(int iGoData)
                        {
                            var goData = new GameObjectData();


                            var colorLine = lines[getColorLineIndex(iGoData)];

                            if (colorLine.Length > 18)
                                goData.colorIndex = int.Parse(colorLine.Substring(18));


                            var iconLine = lines[getIconLineIndex(iGoData)];

                            if (iconLine.Length > 16)
                                goData.iconNameOrGuid = iconLine.Substring(16);



                            var globalIdString = globalIdLists[sceneDataIndex][iGoData];

                            var globalId = new GlobalID(globalIdString);



                            sceneData.goDatas_byGlobalId[globalId] = goData;
                            // sceneData.goDatas_byGlobalId.Add(globalId, goData);

                        }

                        int getColorLineIndex(int goDataIndex)
                        {
                            var index = firstLineIndex; // - goDatasByGlobalId:

                            index += 1; // keys:
                            index += globalIds.Count;

                            index += 1; // values:
                            index += 1; // zeroth godata
                            index += goDataIndex * 2;

                            return index;

                        }
                        int getIconLineIndex(int goDataIndex) => getColorLineIndex(goDataIndex) + 1;



                        for (int i = 0; i < globalIds.Count; i++)
                            parseGoData(i);

                        sceneDatas.Add(sceneData);

                    }

                    int getSceneDataLength(int sceneDataIndex)
                    {
                        int length = 0;

                        length += 1; // - goDatasByGlobalId:

                        length += 1; // - keys:
                        length += globalIdLists[sceneDataIndex].Count;

                        length += 1; // - values:
                        length += globalIdLists[sceneDataIndex].Count * 2;



                        length += 1; // - goDatasByInstanceId:

                        length += 1; // - keys: 123123123

                        length += 1; // - values:
                        length += goDatasByInstanceIdCounts[sceneDataIndex] * 2;


                        return length;

                    }
                    int getFirstLineIndex(int sceneDataIndex)
                    {
                        var index = firstLineIndexOfFirstSceneData;

                        for (int i = 0; i < sceneDataIndex; i++)
                            index += getSceneDataLength(i);

                        return index;

                    }



                    for (int i = 0; i < sceneGuids.Count; i++)
                        parseSceneData(i);

                }

                void remapColorIndexes()
                {
                    foreach (var sceneData in sceneDatas)
                        foreach (var goData in sceneData.goDatas_byGlobalId.Values)
                            if (goData.colorIndex == 7)
                                goData.colorIndex = 1;
                            else if (goData.colorIndex == 8)
                                goData.colorIndex = 2;
                            else if (goData.colorIndex >= 2)
                                goData.colorIndex += 2;

                }
                void setSceneDatasToData()
                {
                    for (int i = 0; i < sceneDatas.Count; i++)
                        data.sceneDatas_byGuid[sceneGuids[i]] = sceneDatas[i];

                    data.Dirty();
                    data.Save();

                }


                try
                {
                    parseSceneGuids();
                    parseGlobalIdLists_andCountGoDatasByInstanceId();
                    parseSceneDatas();

                    remapColorIndexes();
                    setSceneDatasToData();

                }
                catch { }

            }

            subscribe();
            loadData();
            loadPalette();
            loadDataAndPaletteDelayed();
            migrateDataFromV1();

            UpdateExpandedIdsList();

        }

        public static VHierarchyData data;
        public static VHierarchyPalette palette;



        [UnityEditor.Callbacks.PostProcessBuild]
        public static void ClearCacheAfterBuild(BuildTarget _, string __) => VHierarchyCache.Clear();

        static void ClearCacheOnProjectLoaded() => VHierarchyCache.Clear();








        static EditorWindow hierarchyWindow
        {
            get
            {
                if (_hierarchyWindow != null && _hierarchyWindow.GetType() != t_SceneHierarchyWindow) // happens on 2022.3.22f1 with enter playmode options on
                    _hierarchyWindow = null;

                if (_hierarchyWindow == null)
                    _hierarchyWindow = Resources.FindObjectsOfTypeAll(t_SceneHierarchyWindow).FirstOrDefault() as EditorWindow;

                return _hierarchyWindow;

            }
        }
        static EditorWindow _hierarchyWindow;

        static object treeViewController => hierarchyWindow?.GetFieldValue("m_SceneHierarchy").GetFieldValue("m_TreeView"); // recreated on prefab mode enter/exit


        static Type t_SceneHierarchyWindow = typeof(Editor).Assembly.GetType("UnityEditor.SceneHierarchyWindow");





        public const string version = "2.0.17";

    }
}
#endif


