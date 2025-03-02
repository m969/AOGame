#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using System.IO;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using Type = System.Type;
using static VFolders.VFoldersData;
using static VFolders.VFoldersCache;
using static VFolders.Libs.VUtils;
using static VFolders.Libs.VGUI;



namespace VFolders
{
    public static class VFolders
    {
        static void RowGUI(string guid, Rect rowRect)
        {
            var fullRowRect = rowRect.AddWidthFromRight(rowRect.x);

            var isRowHovered = fullRowRect.IsHovered();

            var isListArea = rowRect.x == 14;

            var isFolder = AssetDatabase.IsValidFolder(guid.ToPath());
            var isAsset = !isFolder && !guid.IsNullOrEmpty();
            var isFavorite = !isFolder && !isAsset && rowRect.x != 16;

            var isFavoritesRoot = rowRect.x == 16 && !isFolder && rowRect.y == 0;
            var isAssetsRoot = rowRect.x == 16 && isFolder && guid.ToPath() == "Assets";
            var isPackagesRoot = rowRect.x == 16 && !isFavoritesRoot && !isAssetsRoot && guid.IsNullOrEmpty();

            var useClearerRows = VFoldersMenu.clearerRowsEnabled && !isListArea;
            var useMinimalMode = VFoldersMenu.minimalModeEnabled && !isListArea;
            var useContentMinimap = VFoldersMenu.contentMinimapEnabled && !isListArea;
            var useHierarchyLines = VFoldersMenu.hierarchyLinesEnabled && !isListArea;
            var useZebraStriping = VFoldersMenu.zebraStripingEnabled && !isListArea;



            EditorWindow browser = null;
            Tree tree = null;
            TreeViewItem treeItem = null;

            var isTreeFocused = false;
            var isRowSelected = false;
            var isRowBeingRenamed = false;

            void setObjects()
            {
                if (isListArea) return;

                if (!curEvent.isRepaint) // only needed for drawing,
                    if (!curEvent.isMouseUp) // altClick and
                        if (!isRowHovered) // setting last hovered tree item
                            return;


                void set_browser()
                {
                    var pointInsideWindow = EditorGUIUtility.GUIToScreenPoint(rowRect.center);

                    browser = allProjectBrowsers.FirstOrDefault(r => r.position.AddHeight(30).Contains(pointInsideWindow) && r.hasFocus);

                }
                void set_tree()
                {
                    if (browser == null) return;

                    tree = GetTree(browser);

                }
                void set_treeItem_byRect()
                {
                    if (tree == null) return;
                    if (tree.animatingExpansion) return;

                    var offest = tree.isTwoColumns ? -15 : -4;


                    if ((rowRect.y + offest) % 16 != 0) return;

                    var rowIndex = ((rowRect.y + offest) / 16).ToInt();


                    if (rowIndex < 0) return;
                    if (tree.data == null) return;

                    treeItem = (TreeViewItem)Tree.mi_data_GetItem.Invoke(tree.data, new object[] { rowIndex });

                }
                void set_treeItem_byInstanceId()
                {
                    if (tree == null) return;
                    if (treeItem != null) return;
                    if (isFavorite || isFavoritesRoot) return;

                    var instanceId = typeof(AssetDatabase).InvokeMethod("GetMainAssetOrInProgressProxyInstanceID", guid.ToPath());

                    treeItem = tree.treeViewController.InvokeMethod<TreeViewItem>("FindItem", instanceId);

                }

                set_browser();
                set_tree();
                set_treeItem_byRect();
                set_treeItem_byInstanceId();

            }
            void setState()
            {
                void set_isTreeFocused()
                {
                    if (!curEvent.isRepaint) return;
                    if (tree?.treeViewController == null) return;
                    if (isListArea) return;

                    isTreeFocused = EditorWindow.focusedWindow == browser &&
                                    GUIUtility.keyboardControl == tree.treeViewController.GetFieldValue<int>("m_KeyboardControlID");

                }
                void set_isRowSelected_oneColumn()
                {
                    if (!curEvent.isRepaint) return;
                    if (isListArea) return;
                    if (tree == null) return;
                    if (treeItem == null) return;
                    if (tree.isTwoColumns) return;

#if UNITY_2021_1_OR_NEWER
                    var dragSelectionList = tree.treeViewController?.GetFieldValue("m_DragSelection")?.GetFieldValue<List<int>>("m_List");
#else
                    var dragSelectionList = tree.treeViewController?.GetFieldValue<List<int>>("m_DragSelection");
#endif

                    var dragging = dragSelectionList != null && dragSelectionList.Any();

                    isRowSelected = dragging ? (dragSelectionList.Contains(treeItem.id)) : Selection.Contains(treeItem.id);

                }
                void set_isRowSelected_TwoColumns()
                {
                    if (!curEvent.isRepaint) return;
                    if (isListArea) return;
                    if (tree == null) return;
                    if (treeItem == null) return;
                    if (!tree.isTwoColumns) return;

#if UNITY_2021_1_OR_NEWER
                    var dragSelectionList = tree.treeViewController?.GetFieldValue("m_DragSelection")?.GetFieldValue<List<int>>("m_List");
                    var normalSelectionList = tree.treeViewController?.GetFieldValue("m_CachedSelection")?.GetFieldValue<List<int>>("m_List");
#else
                    var dragSelectionList = tree.treeViewController?.GetFieldValue<List<int>>("m_DragSelection");
                    var normalSelectionList = tree.treeViewController?.GetMemberValue("state").GetMemberValue<List<int>>("selectedIDs");
#endif

                    var dragging = dragSelectionList != null && dragSelectionList.Any();

                    isRowSelected = dragging ? (dragSelectionList.Contains(treeItem.id)) : normalSelectionList != null && normalSelectionList.Contains(treeItem.id);

                }
                void set_isRowBeingRenamed()
                {
                    if (!curEvent.isRepaint) return;
                    if (isListArea) return;

                    isRowBeingRenamed = EditorGUIUtility.editingTextField &&
                                           isRowSelected &&
                                           tree?.treeViewController?.GetMemberValue("state")?.GetMemberValue("renameOverlay")?.InvokeMethod<bool>("IsRenaming") == true;

                }
                void set_lastHovered()
                {
                    if (isListArea) return;
                    if (!curEvent.isRepaint) return;
                    if (!isRowHovered) return;

                    lastHoveredRowRect_screenSpace = EditorGUIUtility.GUIToScreenRect(fullRowRect);
                    lastHoveredTreeItem = treeItem;

                }

                set_isTreeFocused();
                set_isRowSelected_oneColumn();
                set_isRowSelected_TwoColumns();
                set_isRowBeingRenamed();
                set_lastHovered();

                lastKnownMousePosition_screenSpace = curEvent.mousePosition_screenSpace;

            }


            void drawing()
            {
                if (!curEvent.isRepaint) { hierarchyLines_isFirstRowDrawn = false; return; }

                var folderInfo = GetFolderInfo(guid, createDataIfDoesntExist: false);

                var showBackgroundColor = folderInfo.hasColor && useClearerRows;// && !(isRowSelected && isTreeFocused);
                var showCustomIcon = useClearerRows ? folderInfo.hasIcon : (folderInfo.hasIcon || folderInfo.hasColor);
                var showDefaultIcon = !showCustomIcon && (!useMinimalMode || isRowBeingRenamed);

                var makeTriangleBrighter = showBackgroundColor && isDarkTheme;
                var makeNameBrighter = showBackgroundColor && isDarkTheme;
                var makeIconBrighter = showBackgroundColor;

                var hideProperly = showBackgroundColor || (!showCustomIcon && !showDefaultIcon);

                Color defaultBackground = default;


                void calcDefaultBackground()
                {
                    if (!isFolder && !isFavoritesRoot && !isPackagesRoot) return;
                    if (!hideProperly) return;

                    var selectedFocused = GUIColors.selectedBackground;
                    var selectedUnfocused = isDarkTheme ? Greyscale(.3f) : Greyscale(.68f);
                    var normal = isListArea ? Greyscale(.2f) : GUIColors.windowBackground;

                    if (isRowSelected && !isRowBeingRenamed)
                        defaultBackground = isTreeFocused ? selectedFocused : selectedUnfocused;

                    else
                        defaultBackground = normal;

                }
                void hideName_proper()
                {
                    if (!showBackgroundColor && (showCustomIcon || showDefaultIcon)) return;
                    if (!isFolder && !isFavoritesRoot && !isPackagesRoot) return;
                    if (!hideProperly) return;

                    var name = isListArea ? guid.ToPath().GetFilename() : treeItem != null ? treeItem.displayName : "Favorites";

                    var nameRect = rowRect.SetWidth(name.GetLabelWidth() + 3).MoveX(16).MoveX(isListArea ? 3 : 0);

                    nameRect.Draw(defaultBackground);

                }
                void hideDefaultIcon_proper()
                {
                    if (showDefaultIcon) return;
                    if (!isFolder && !isFavoritesRoot && !isPackagesRoot) return;
                    if (!hideProperly) return;

                    var iconRect = rowRect.SetWidth(16).MoveX(isListArea ? 3 : 0);

                    iconRect.Draw(defaultBackground);



                }
                void hideDefaultIcon_fast()
                {
                    if (showDefaultIcon) return;
                    if (hideProperly) return;

                    var iconRect = rowRect.SetWidth(16).MoveX(isListArea ? 3 : 0);

                    SetGUIColor(EditorGUIUtility.isProSkin ? Greyscale(.28f) : Color.white);

                    GUI.DrawTexture(iconRect, EditorGUIUtility.FindTexture("d_Folder Icon"));
                    GUI.DrawTexture(iconRect, EditorGUIUtility.FindTexture("d_FolderOpened Icon"));

                    ResetGUIColor();

                }

                void backgroundColor()
                {
                    if (!showBackgroundColor) return;
                    if (!isFolder && !isFavoritesRoot && !isPackagesRoot) return;


                    var hasLeftGradient = rowRect.x > 32;



                    var colorRect = rowRect.AddWidthFromRight(30).AddWidth(16);

                    if (!isRowSelected)
                        colorRect = colorRect.AddHeightFromMid(EditorGUIUtility.pixelsPerPoint >= 2 ? -.5f : -1);

                    if (hasLeftGradient)
                        colorRect = colorRect.AddWidthFromRight(2);




                    var leftGradientWith = hasLeftGradient ? 22 : 0;
                    var rightGradientWidth = (fullRowRect.width * .77f).Min(colorRect.width);

                    var leftGradientRect = colorRect.SetWidth(leftGradientWith);
                    var rightGradientRect = colorRect.SetWidthFromRight(rightGradientWidth - leftGradientWith);

                    var flatColorRect = colorRect.SetX(leftGradientRect.xMax).SetXMax(rightGradientRect.x);




                    var colorWithFlatness = folderInfo.color;

                    var flatness = colorWithFlatness.a;

                    var color = (isDarkTheme ? colorWithFlatness * .64f : Color.Lerp(colorWithFlatness, Greyscale(.8f), .5f)).SetAlpha(1);

                    if (isRowHovered)
                        color *= 1.1f;

                    if (isRowSelected)
                        color *= 1.2f;




                    leftGradientRect.Draw(color.SetAlpha((flatness - .1f) / .9f));
                    leftGradientRect.DrawCurtainLeft(color);

                    flatColorRect.AddWidth(1).Draw(color);

                    rightGradientRect.Draw(color.MultiplyAlpha(flatness));
                    rightGradientRect.DrawCurtainRight(color);


                }
                void triangle()
                {
                    if (!showBackgroundColor) return;
                    if (treeItem == null) return;
                    if (!treeItem.hasChildren) return;
                    if (!isFolder && !isFavoritesRoot && !isPackagesRoot) return;


                    var triangleRect = rowRect.MoveX(-15).SetWidth(16).Resize(-1);

                    GUI.Label(triangleRect, EditorGUIUtility.IconContent(tree.IsExpanded(treeItem) ? "IN_foldout_on" : "IN_foldout"));


                    if (!makeTriangleBrighter) return;

                    GUI.Label(triangleRect, EditorGUIUtility.IconContent(tree.IsExpanded(treeItem) ? "IN_foldout_on" : "IN_foldout"));

                }
                void name()
                {
                    if (!showBackgroundColor && (showCustomIcon || showDefaultIcon)) return;
                    if (!isFolder && !isFavoritesRoot && !isPackagesRoot) return;
                    if (isRowBeingRenamed) return;


                    var nameRect = rowRect.MoveX(18);

                    if (isListArea)
                        nameRect = nameRect.MoveX(3);

                    if (useMinimalMode && !showCustomIcon && !showDefaultIcon)
                        nameRect = nameRect.MoveX(-17);

                    if (makeNameBrighter)
                        nameRect = nameRect.MoveX(-2).MoveY(-.5f);



                    var styleName = makeNameBrighter ? "WhiteLabel" : "TV Line";

                    if (isFavoritesRoot || isAssetsRoot || isPackagesRoot)
                        styleName = "BoldLabel";



                    var name = isFavoritesRoot ? "Favorites" :
                               isPackagesRoot ? "Packages" :
                               isListArea || treeItem == null ? guid.ToPath().GetFilename() :
                               treeItem.displayName;



                    if (makeNameBrighter)
                        SetGUIColor(Greyscale(isRowSelected ? 1 : .93f));

                    GUI.skin.GetStyle(styleName).Draw(nameRect, name, false, false, isRowSelected, isTreeFocused && styleName != "BoldLabel");

                    if (makeNameBrighter)
                        ResetGUIColor();

                }
                void defaultIcon()
                {
                    if (!isFolder) return;
                    if (!showBackgroundColor) return;
                    if (!showDefaultIcon) return;


                    var iconRect = rowRect.SetWidth(16).MoveX(isListArea ? 3 : 0);


                    SetLabelAlignmentCenter();

                    if (makeNameBrighter)
                        SetGUIColor(Greyscale(.88f));

                    if (makeNameBrighter)
                        GUI.Label(iconRect.Resize(-2), EditorGUIUtility.IconContent(folderInfo.isEmpty ? "FolderEmpty On Icon" : "Folder On Icon"));
                    else
                        GUI.Label(iconRect.Resize(-2), EditorGUIUtility.IconContent(folderInfo.isEmpty ? "FolderEmpty Icon" : "Folder Icon"));

                    if (makeNameBrighter)
                        ResetGUIColor();

                    ResetLabelStyle();

                }
                void customIcon()
                {
                    if (!isFolder) return;
                    if (!showCustomIcon) return;


                    var icon = useClearerRows ? EditorIcons.GetIcon(folderInfo.iconNameOrPath)
                                              : GetSmallFolderIcon(folderInfo);

                    if (!icon) return;


                    var iconRect = rowRect.SetWidth(16).MoveX(isListArea ? 3 : 0);

                    iconRect = iconRect.SetWidth(iconRect.height / icon.height * icon.width);


                    GUI.DrawTexture(iconRect, icon);

                }
                void hierarchyLines()
                {
                    if (!useHierarchyLines) return;
                    if (isListArea) return;
                    if (treeItem == null) return;


                    var lineThickness = 1f;
                    var lineContrast = isDarkTheme ? .35f : .55f;

                    if (isRowSelected)
                        if (isTreeFocused)
                            lineContrast += isDarkTheme ? .1f : -.25f;
                        else
                            lineContrast += isDarkTheme ? .05f : -.05f;


                    var depth = ((rowRect.x - 16) / 14).RoundToInt();

                    bool isLastChild(TreeViewItem item) => item.parent?.children?.LastOrDefault() == item;
                    bool hasChilren(TreeViewItem item) => item.children != null && item.children.Count > 0;

                    void calcVerticalGaps_beforeFirstRowDrawn()
                    {
                        if (hierarchyLines_isFirstRowDrawn) return;

                        hierarchyLines_verticalGaps.Clear();

                        var curItem = treeItem.parent;
                        var curDepth = depth - 1;

                        while (curItem != null && curItem.parent != null)
                        {
                            if (isLastChild(curItem))
                                hierarchyLines_verticalGaps.Add(curDepth - 1);

                            curItem = curItem.parent;
                            curDepth--;
                        }

                    }
                    void updateVerticalGaps_beforeNextRowDrawn()
                    {
                        if (isLastChild(treeItem))
                            hierarchyLines_verticalGaps.Add(depth - 1);

                        if (depth < hierarchyLines_prevRowDepth)
                            hierarchyLines_verticalGaps.RemoveAll(r => r >= depth);

                    }

                    void drawVerticals()
                    {
                        for (int i = 1; i < depth; i++)
                            if (!hierarchyLines_verticalGaps.Contains(i))
                                rowRect.SetX(9 + i * 14 - lineThickness / 2)
                                       .SetWidth(lineThickness)
                                       .SetHeight(isLastChild(treeItem) && i == depth - 1 ? 8 + lineThickness / 2 : 16)
                                       .Draw(Greyscale(lineContrast));

                    }
                    void drawHorizontals()
                    {
                        if (depth == 0) return;
                        if (depth == 1) return;

                        rowRect.MoveX(-21)
                               .SetHeightFromMid(lineThickness)
                               .SetWidth(hasChilren(treeItem) ? 7 : 17)
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
                    if (!useZebraStriping) return;

                    var contrast = isDarkTheme ? .033f : .05f;

                    var t = 1 - rowRect.y.PingPong(16f) / 16f;

                    fullRowRect.Draw(Greyscale(isDarkTheme ? 1 : 0, contrast * t));


                }
                void hoveredRow()
                {
                    if (!isRowHovered) return;
                    if (isListArea) return;
                    if (EditorWindow.mouseOverWindow == VFoldersPaletteWindow.instance) return;

                    fullRowRect.Draw(Greyscale(isDarkTheme ? 1 : 0, .06f));

                }

                void contentMinimap()
                {
                    if (!isFolder) return;
                    if (!useContentMinimap) return;
                    if (guid.IsNullOrEmpty()) return;

                    void icon(Rect rect, string name)
                    {
                        var icon = EditorIcons.GetIcon(name);

                        if (!icon) return;


                        SetGUIColor(Greyscale(1, isDarkTheme ? .49f : .7f));

                        GUI.DrawTexture(rect, icon);

                        ResetGUIColor();

                    }


                    var iconDistance = 13;
                    var minButtonX = rowRect.x + guid.ToPath().GetFilename().GetLabelWidth() + iconDistance + 2;
                    var iconRect = fullRowRect.SetWidthFromRight(iconDistance).SetSizeFromMid(12, 12).MoveX(-1.5f);

                    foreach (var iconName in folderInfo.folderState.contentMinimapIconNames)
                    {
                        if (iconRect.x < minButtonX) continue;

                        icon(iconRect, iconName);

                        iconRect = iconRect.MoveX(-iconDistance);

                    }

                }


                fullRowRect.MarkInteractive();

                calcDefaultBackground();
                hideName_proper();
                hideDefaultIcon_proper();
                hideDefaultIcon_fast();

                hierarchyLines();
                backgroundColor();
                triangle();
                defaultIcon();
                customIcon();
                name();
                zebraStriping();
                hoveredRow();

                contentMinimap();

            }

            void altDrag()
            {
                if (!curEvent.holdingAlt) return;

                void mouseDown()
                {
                    if (!curEvent.isMouseDown) return;
                    if (!isRowHovered) return;

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
                    DragAndDrop.objectReferences = new[] { AssetDatabase.LoadAssetAtPath<Object>(guid.ToPath()) };
                    DragAndDrop.StartDrag(guid.ToPath().GetFilename());

                }

                mouseDown();
                mouseDrag();

                // altdrag has to be set up manually before altClick because altClick will use() mouseDown event to prevent selection change
            }
            void altClick()
            {
                if (!isRowHovered) return;
                if (!curEvent.holdingAlt) return;
                if (!isFolder) return;

                void mouseDown()
                {
                    if (!curEvent.isMouseDown) return;

                    curEvent.Use();

                }
                void mouseUp()
                {
                    if (!curEvent.isMouseUp) return;

                    var selectedGuids = isListArea
                                               ?
                                               Selection.objects.Where(r => r is DefaultAsset).Select(r => r.GetPath().ToGuid())
                                               :
#if UNITY_2021_1_OR_NEWER
                                               tree.treeViewController.GetFieldValue("m_CachedSelection").GetFieldValue<List<int>>("m_List")
#else
                                               tree.treeViewController?.GetMemberValue("state").GetMemberValue<List<int>>("selectedIDs")
#endif
                                 .Select(id => tree.treeViewController.InvokeMethod("FindItem", id)                                                 // todo to somethinf else (finditem reveals selected item for some reason)
                                                                      ?.GetPropertyValue<string>("Guid", exceptionIfNotFound: false))
                                                                      .Where(r => r != null);


                    var editMultiSelection = selectedGuids.Count() > 1 && selectedGuids.Contains(guid);

                    var guidsToEdit = (editMultiSelection ? selectedGuids.Where(r => AssetDatabase.IsValidFolder(r.ToPath())) : new[] { guid }).ToList();


                    if (VFoldersPaletteWindow.instance && VFoldersPaletteWindow.instance.guids.SequenceEqual(guidsToEdit)) { VFoldersPaletteWindow.instance.Close(); return; }

                    var openNearRect = rowRect;// editMultiSelection ? lastVisibleSelectedCellRect : cellRect;
                    var position = EditorGUIUtility.GUIToScreenPoint(new Vector2(curEvent.mousePosition.x + 20, openNearRect.y - 13));

                    if (!VFoldersPaletteWindow.instance)
                        VFoldersPaletteWindow.CreateInstance(position);

                    VFoldersPaletteWindow.instance.Init(guidsToEdit);
                    VFoldersPaletteWindow.instance.Focus();

                    VFoldersPaletteWindow.instance.targetPosition = position;

                    if (editMultiSelection)
                        Selection.objects = null;
                }

                mouseDown();
                mouseUp();

            }



            setObjects();
            setState();

            drawing();

            altDrag();
            altClick();

        }

        static List<int> hierarchyLines_verticalGaps = new List<int>();
        static bool hierarchyLines_isFirstRowDrawn;
        static int hierarchyLines_prevRowDepth;

        static Vector2 lastKnownMousePosition_screenSpace;
        static Rect lastHoveredRowRect_screenSpace;
        static TreeViewItem lastHoveredTreeItem;




        static void CellGUI(string guid, Rect cellRect)
        {
            if (!AssetDatabase.IsValidFolder(guid.ToPath())) return;

            var folderInfo = GetFolderInfo(guid, createDataIfDoesntExist: false);


            void setLastVisibleSelectedForAltClick()
            {
                if (!curEvent.isRepaint) return;
                if (!Selection.objects.Contains(AssetDatabase.LoadAssetAtPath<DefaultAsset>(guid.ToPath()))) return;

                lastVisibleSelectedCellRect = cellRect;

            }

            void hideIcon()
            {
                if (!curEvent.isRepaint) return;
                if (!folderInfo.hasColor) return;

                cellRect.SetHeight(cellRect.width).Resize(4).Draw(EditorGUIUtility.isProSkin ? Greyscale(.2f) : Greyscale(.75f));

            }
            void icon()
            {
                if (!curEvent.isRepaint) return;
                if (!folderInfo.hasColor && !folderInfo.hasIcon) return;

                DrawBigFolderIcon(cellRect, folderInfo);

            }

            void altDrag()
            {
                if (!curEvent.holdingAlt) return;

                void mouseDown()
                {
                    if (!curEvent.isMouseDown) return;
                    if (!cellRect.IsHovered()) return;

                    mouseDownPos = curEvent.mousePosition;

                }
                void mouseDrag()
                {
                    if (!curEvent.isMouseDrag) return;
                    if ((curEvent.mousePosition - mouseDownPos).magnitude < 5) return;
                    if (!cellRect.Contains(mouseDownPos)) return;
                    if (!cellRect.Contains(curEvent.mousePosition - curEvent.mouseDelta)) return;
                    if (DragAndDrop.objectReferences.Any()) return;

                    DragAndDrop.PrepareStartDrag();
                    DragAndDrop.objectReferences = new[] { AssetDatabase.LoadAssetAtPath<Object>(guid.ToPath()) };
                    DragAndDrop.StartDrag(guid.ToPath().GetFilename());

                }

                mouseDown();
                mouseDrag();

            }
            void altClick()
            {
                if (!cellRect.IsHovered()) return;
                if (!curEvent.holdingAlt) return;

                void mouseDown()
                {
                    if (!curEvent.isMouseDown) return;

                    curEvent.Use();

                }
                void mouseUp()
                {
                    if (!curEvent.isMouseUp) return;

                    var selectedFoldersGuids = Selection.objects.Where(r => r is DefaultAsset).Select(r => r.GetPath().ToGuid());

                    var editMultiSelection = selectedFoldersGuids.Count() > 1 && selectedFoldersGuids.Contains(guid);

                    var guidsToEdit = (editMultiSelection ? selectedFoldersGuids : new[] { guid }).ToList();


                    if (VFoldersPaletteWindow.instance && VFoldersPaletteWindow.instance.guids.SequenceEqual(guidsToEdit)) { VFoldersPaletteWindow.instance.Close(); return; }

                    var openNearRect = editMultiSelection ? lastVisibleSelectedCellRect : cellRect;
                    var position = EditorGUIUtility.GUIToScreenPoint(new Vector2(openNearRect.xMax + 8, openNearRect.y - 5));

                    if (!VFoldersPaletteWindow.instance)
                        VFoldersPaletteWindow.CreateInstance(position);

                    VFoldersPaletteWindow.instance.Init(guidsToEdit);
                    VFoldersPaletteWindow.instance.Focus();

                    VFoldersPaletteWindow.instance.targetPosition = position;

                    if (editMultiSelection)
                        Selection.objects = null;
                }

                mouseDown();
                mouseUp();

            }


            setLastVisibleSelectedForAltClick();

            hideIcon();
            icon();

            altDrag();
            altClick();

        }

        static Rect lastVisibleSelectedCellRect;



        static void ProjectBrowserItemGUI(string guid, Rect itemRect)
        {
            void callRowGUI()
            {
                if (itemRect.height != 16) return;

                RowGUI(guid, itemRect);

            }
            void callCellGUI()
            {
                if (itemRect.height == 16) return;

                CellGUI(guid, itemRect);

            }
            void updateExpandQueues()
            {
                if (!curEvent.isLayout) return;

                foreach (var tree in trees_byBrowser.Values)
                    tree.UpdateExpandQueue();

            }
            void createFoldersFirstUpdater()
            {
                if (!curEvent.isLayout) return;
                if (!VFoldersMenu.foldersFirstEnabled) return;

                var pointInsideWindow = EditorGUIUtility.GUIToScreenPoint(itemRect.center);

                var browser = allProjectBrowsers.FirstOrDefault(r => r.position.AddHeight(30).Contains(pointInsideWindow) && r.hasFocus);

                if (browser == null) return;
                if (foldersFirstUpdater_byBrowser.ContainsKey(browser)) return;

                var updater = new FoldersFirstUpdater(browser);

                foldersFirstUpdater_byBrowser[browser] = updater;

                updater.Update();

            }

            callRowGUI();
            callCellGUI();
            updateExpandQueues();
            createFoldersFirstUpdater();

        }

        static Vector2 mouseDownPos;










        public static Texture2D GetSmallFolderIcon(FolderInfo folderInfo)
        {
            var key = new object[] { folderInfo.iconNameOrPath, folderInfo.color, folderInfo.isEmpty, isDarkTheme }.Aggregate(0, (hash, r) => (hash * 2) ^ r.GetHashCode());

            Texture2D icon = null;

            void getCached()
            {
                if (!cache.HasIcon(key)) return;

                icon = cache.GetIcon(key);

            }
            void generateAndCache()
            {
                if (icon != null) return;
                if (Event.current != null) return;  // interactions with gpu in OnGUI may interfere with gui rendering

                var iconSizeX = folderInfo.hasIcon ? 36 : 32;
                var iconSizeY = 32;

                var assetIconSize = 20; // 20 21
                var assetIconOffsetX = 16;
                var assetIconOffsetY = -2; // -2 -3

                var folderIconSize = iconSizeY;
                var folderIconOffsetY_ifHasAssetIcon = 1;

                Color[] iconPixels;

                Texture2D folderIcon;
                Color[] folderIconPixels;


                void createIcon()
                {
                    icon = new Texture2D(iconSizeX, iconSizeY, TextureFormat.RGBA32, 1, false);
                    icon.hideFlags = HideFlags.DontSave;
                    icon.SetPropertyValue("pixelsPerPoint", 2);

                    iconPixels = new Color[iconSizeX * iconSizeY];

                }
                void createFolderIcon()
                {
                    // var folderIconName = folderInfo.isEmpty ? "FolderEmpty Icon" : "Project@2x";
                    var folderIconName = folderInfo.hasColor ? (folderInfo.isEmpty ? "FolderEmpty On Icon" : "Folder On Icon") :
                                                               (folderInfo.isEmpty ? "FolderEmpty Icon" : "Folder Icon");


                    folderIcon = EditorGUIUtility.FindTexture(folderIconName);

                    if (folderIcon.width != folderIconSize)
                        folderIcon = folderIcon.CreateResizedCopy(folderIconSize, folderIconSize);
                    else
                        folderIcon = folderIcon.CreateCopy();

                    folderIconPixels = folderIcon.GetPixels(0);

                }
                void copyFolderIcon()
                {
                    if (!folderInfo.hasIcon) { iconPixels = folderIconPixels; return; }

                    for (int x = 0; x < folderIcon.width; x++)
                        for (int y = 0; y < folderIcon.height - folderIconOffsetY_ifHasAssetIcon; y++)
                            iconPixels[x + (y + folderIconOffsetY_ifHasAssetIcon) * icon.width] = folderIconPixels[x + y * folderIcon.width];

                }
                void applyColor()
                {
                    if (!folderInfo.hasColor) return;

                    for (int i = 0; i < iconPixels.Length; i++)
                        iconPixels[i] *= folderInfo.color.SetAlpha(1);

                }
                void insertAssetIcon()
                {
                    if (!folderInfo.hasIcon) return;


                    var assetIconOriginal = EditorIcons.GetIcon(folderInfo.iconNameOrPath);

                    if (!assetIconOriginal) return;


                    var prevFilter = assetIconOriginal.filterMode;

                    assetIconOriginal.filterMode = FilterMode.Bilinear;
                    var assetIconPixels_bilinear = assetIconOriginal.CreateResizedCopy(assetIconSize, assetIconSize).GetPixels();

                    assetIconOriginal.filterMode = FilterMode.Point;
                    var assetIconPixels_point = assetIconOriginal.CreateResizedCopy(assetIconSize, assetIconSize).GetPixels();


                    assetIconOriginal.filterMode = prevFilter;


                    for (int x = 0; x < iconSizeX; x++)
                        for (int y = 0; y < iconSizeY; y++)
                        {
                            var xAssetIcon = x - assetIconOffsetX;
                            var yAssetIcon = y - assetIconOffsetY - folderIconOffsetY_ifHasAssetIcon;

                            if (!xAssetIcon.IsInRange(0, assetIconSize - 1)) continue;
                            if (!yAssetIcon.IsInRange(0, assetIconSize - 1)) continue;


                            var innerRadius = (folderInfo.iconNameOrPath == "AudioClip Icon" ? .2f : .4f);
                            var isInnerPixel = (new Vector2(xAssetIcon, yAssetIcon) / (assetIconSize - 1) - Vector2.one * .5f).magnitude < innerRadius;

                            var isOutlinePixel = false;
                            var outlineRadius = isInnerPixel ? 2 : 1;
                            for (int xx = xAssetIcon - outlineRadius; xx <= xAssetIcon + outlineRadius; xx++)
                                if (!isOutlinePixel)
                                    for (int yy = yAssetIcon - outlineRadius; yy <= yAssetIcon + outlineRadius; yy++)
                                        if (!isOutlinePixel)
                                            if (xx.IsInRange(0, assetIconSize - 1) && yy.IsInRange(0, assetIconSize - 1))
                                                if (assetIconPixels_bilinear[xx + yy * assetIconSize].a > .2f)
                                                    isOutlinePixel = true;


                            var pxBilinear = assetIconPixels_bilinear[xAssetIcon + yAssetIcon * assetIconSize];
                            var pxPoint = assetIconPixels_point[xAssetIcon + yAssetIcon * assetIconSize];
                            var pxCombined = new Color(pxPoint.r, pxPoint.g, pxPoint.b, pxBilinear.a);

                            if (pxCombined.a == 0 && !isOutlinePixel) continue;

                            iconPixels[x + y * iconSizeX] = pxCombined;

                        }

                }


                createIcon();
                createFolderIcon();
                copyFolderIcon();
                applyColor();
                insertAssetIcon();

                icon.SetPixels(iconPixels);
                icon.Apply();

                cache.AddIcon(key, icon);

            }
            void queueGeneration()
            {
                if (icon != null) return;

                toCallInUpdate.Add(generateAndCache);

            }

            getCached();
            generateAndCache();
            queueGeneration();

            return icon ?? EditorGUIUtility.FindTexture(folderInfo.isEmpty ? "FolderEmpty Icon" : "Project@2x");

        }
        public static Texture2D GetSmallFolderIcon_forVTabs(string folderGuid)
        {
            var folderInfo = GetFolderInfo(folderGuid, false);

            if (folderInfo.hasColor || folderInfo.hasIcon)
                return GetSmallFolderIcon(folderInfo);

            return null;

        }

        static void Update()
        {
            foreach (var r in toCallInUpdate)
                r.Invoke();

            toCallInUpdate.Clear();

        }

        static List<System.Action> toCallInUpdate = new List<System.Action>();





        public static void DrawBigFolderIcon(Rect rect, FolderInfo folderInfo)
        {
            Rect folderIconRect;
            Rect assetIconRect;


            void calcRects()
            {
                folderIconRect = rect.SetHeight(rect.width);

#if UNITY_2022_3_OR_NEWER
#else
                if (Application.platform == RuntimePlatform.OSXEditor)
                    if (folderIconRect.width > 64)
                        folderIconRect = folderIconRect.SetSizeFromMid(64, 64);
#endif


                var assetIconOffsetMin = new Vector2(4.5f, 3.5f);
                var assetIconSizeMin = 10;

                var assetIconOffsetMax = new Vector2(19, 15);
                var assetIconSizeMax = 24.5f; // 25

                var t = ((folderIconRect.width - 16) / (64 - 16));

#if UNITY_2022_3_OR_NEWER
#else
                if (Application.platform == RuntimePlatform.OSXEditor)
                    t = t.Clamp01();
#endif

                var assetIconOffset = Lerp(assetIconOffsetMin, assetIconOffsetMax, t);
                var assetIconSize = Lerp(assetIconSizeMin, assetIconSizeMax, t);

                assetIconRect = folderIconRect.Move(assetIconOffset).SetSizeFromMid(assetIconSize, assetIconSize).AlignToPixelGrid();

            }

            void color()
            {
                if (!folderInfo.hasColor) return;


                SetGUIColor(folderInfo.color.SetAlpha(1));

                GUI.DrawTexture(folderIconRect, EditorGUIUtility.FindTexture(folderInfo.isEmpty ? "FolderEmpty On Icon" : "Folder On Icon"));

                ResetGUIColor();

            }
            void assetIcon()
            {
                if (!folderInfo.hasIcon) return;

                var texture = EditorIcons.GetIcon(folderInfo.iconNameOrPath);

                if (!texture) return;

                void material()
                {
                    if (outlineMaterial) return;

                    outlineMaterial = new Material(Shader.Find("Hidden/VFoldersOutline"));

                    outlineMaterial.SetColor("_Color", EditorGUIUtility.isProSkin ? Greyscale(.2f, .6f) : Greyscale(.75f));

                }

                void shadow()
                {
                    var contrast = isDarkTheme ? .6f : .2f; // was .65 then .6

                    assetIconRect.SetSizeFromMid(assetIconRect.width * .8f).DrawBlurred(Greyscale(.2f, contrast), assetIconRect.width * .55f);

                }
                void outline()
                {
                    var outlineRect = assetIconRect.Resize(rect.height >= 70 && EditorGUIUtility.pixelsPerPoint >= 2 ? -1f / EditorGUIUtility.pixelsPerPoint : 0).AlignToPixelGrid();

                    EditorGUI.DrawPreviewTexture(outlineRect.Move(-1, -1), texture, outlineMaterial);
                    EditorGUI.DrawPreviewTexture(outlineRect.Move(-1, 1), texture, outlineMaterial);
                    EditorGUI.DrawPreviewTexture(outlineRect.Move(1, 1), texture, outlineMaterial);
                    EditorGUI.DrawPreviewTexture(outlineRect.Move(1, -1), texture, outlineMaterial);

                }
                void background()
                {
                    for (int i = 0; i < assetIconRect.size.x; i++)
                        EditorGUI.DrawPreviewTexture(assetIconRect.Resize(i * .5f + 1), texture, outlineMaterial);

                }
                void icon()
                {
                    GUI.DrawTexture(assetIconRect, texture);
                }

                material();

                shadow();
                outline();
                background();
                icon();

            }


            calcRects();

            color();
            assetIcon();

        }
        public static void DrawBigFolderIcon_forVFavorites(Rect rect, string folderGuid)
        {
            DrawBigFolderIcon(rect, GetFolderInfo(folderGuid, false));
        }

        static Material outlineMaterial;











        static void CheckShortcuts() // called from globalEventHandler
        {
            if (!hoveredProjectBrowser) return;
            if (!curEvent.isKeyDown) return;
            if (curEvent.keyCode == KeyCode.None) return;
            if (EditorWindow.mouseOverWindow.GetType() != t_ProjectBrowser) return;

            void toggleExpanded()
            {
                if (!curEvent.isKeyDown) return;
                if (curEvent.keyCode != KeyCode.E) return;
                if (curEvent.holdingAnyModifierKey) return;
                if (!VFoldersMenu.toggleExpandedEnabled) return;

                if (lastHoveredTreeItem == null) return;
                if (!lastHoveredRowRect_screenSpace.Contains(lastKnownMousePosition_screenSpace)) return;

                curEvent.Use();

                if (lastHoveredTreeItem.children == null) return;
                if (lastHoveredTreeItem.children.Count == 0) return;

                var tree = GetTree(hoveredProjectBrowser);

                tree.SetExpandedWithAnimation(lastHoveredTreeItem.id, !tree.expandedIds.Contains(lastHoveredTreeItem.id));

                EditorApplication.RepaintProjectWindow();

            }
            void collapseEverything()
            {
                if (!curEvent.isKeyDown) return;
                if (curEvent.keyCode != KeyCode.E) return;
                if (curEvent.modifiers != (EventModifiers.Shift | EventModifiers.Command) && curEvent.modifiers != (EventModifiers.Shift | EventModifiers.Control)) return;
                if (!VFoldersMenu.collapseEverythingEnabled) return;

                curEvent.Use();

                var tree = GetTree(hoveredProjectBrowser);


                var idsToCollapse_roots = tree.expandedIds.Where(id => EditorUtility.InstanceIDToObject(id).GetPath() is string path &&
                                                                       path.HasParentPath() &&
                                                                      (path.GetParentPath() == "Assets" || path.GetParentPath() == "Packages"));


                var idsToCollapse_children = tree.expandedIds.Where(id => EditorUtility.InstanceIDToObject(id).GetPath() is string path &&
                                                                         !path.IsNullOrEmpty() &&
                                                                          path != "Assets" &&
                                                                          path != "Packages" &&
                                                                         !idsToCollapse_roots.Contains(id));


                tree.expandQueue_toCollapseAfterAnimation = idsToCollapse_children.ToList();

                tree.expandQueue_toAnimate = idsToCollapse_roots.Select(id => new Tree.ExpandQueueEntry { id = id, expand = false })
                                                                .OrderBy(row => tree.VisibleRowIndex(row.id)).ToList();


                EditorApplication.RepaintProjectWindow();

            }
            void collapseEverythingElse()
            {
                if (!curEvent.isKeyDown) return;
                if (curEvent.keyCode != KeyCode.E) return;
                if (curEvent.modifiers != EventModifiers.Shift) return;
                if (!VFoldersMenu.collapseEverythingElseEnabled) return;

                if (lastHoveredTreeItem == null) return;
                if (!lastHoveredRowRect_screenSpace.Contains(lastKnownMousePosition_screenSpace)) return;

                curEvent.Use();

                if (lastHoveredTreeItem.children == null) return;
                if (lastHoveredTreeItem.children.Count == 0) return;

                var tree = GetTree(hoveredProjectBrowser);



                var parents = new List<TreeViewItem>();

                for (var cur = lastHoveredTreeItem.parent; cur != null; cur = cur.parent)
                    parents.Add(cur);



                var idsToCollapse = new List<int>();

                foreach (var id in tree.expandedIds.ToList())
                    if (!parents.Any(r => r.id == id))
                        if (id != lastHoveredTreeItem.id)
                            idsToCollapse.Add(id);



                tree.expandQueue_toAnimate = idsToCollapse.Select(id => new Tree.ExpandQueueEntry { id = id, expand = false })
                                                          .Append(new Tree.ExpandQueueEntry { id = lastHoveredTreeItem.id, expand = true })
                                                          .OrderBy(r => tree.VisibleRowIndex(r.id)).ToList();


                EditorApplication.RepaintProjectWindow();

            }

            toggleExpanded();
            collapseEverything();
            collapseEverythingElse();

        }







        class Tree
        {
            public void UpdateExpandQueue() // called from gui because reflected methods rely on event.current
            {
                if (animatingExpansion) return;

                if (!expandQueue_toAnimate.Any())
                {
                    if (!expandQueue_toCollapseAfterAnimation.Any()) return;

                    foreach (var id in expandQueue_toCollapseAfterAnimation)
                        SetExpanded(id, false);

                    expandQueue_toCollapseAfterAnimation.Clear();

                    return;
                }

                var iid = expandQueue_toAnimate.First().id;
                var expand = expandQueue_toAnimate.First().expand;

                if (expandedIds.Contains(iid) != expand)
                    SetExpandedWithAnimation(iid, expand);

                expandQueue_toAnimate.RemoveAt(0);

            }

            public void SetExpandedWithAnimation(int instanceId, bool expanded) => treeViewController.InvokeMethod("ChangeFoldingForSingleItem", instanceId, expanded);
            public void SetExpanded(int instanceId, bool expanded) => treeViewController.GetPropertyValue("data").InvokeMethod("SetExpanded", instanceId, expanded);

            public int VisibleRowIndex(int instanceId) => treeViewController.GetPropertyValue("data").InvokeMethod<int>("GetRow", instanceId);

            public bool animatingExpansion => treeViewController.GetPropertyValue<bool>("animatingExpansion");

            public bool IsExpanded(TreeViewItem treeItem) => expandedIds.Contains(treeItem.id);


            public List<ExpandQueueEntry> expandQueue_toAnimate = new List<ExpandQueueEntry>();
            public List<int> expandQueue_toCollapseAfterAnimation = new List<int>();

            public struct ExpandQueueEntry { public int id; public bool expand; }





            public void UpdateState() // delayCall loop
            {
                isTwoColumns = browser.GetFieldValue<int>("m_ViewMode") == 1;

                treeViewController = browser.GetFieldValue(isTwoColumns ? "m_FolderTree" : "m_AssetTree");

                data = treeViewController?.GetPropertyValue("data");

                expandedIds = treeViewController?.GetPropertyValue("state")?.GetPropertyValue<List<int>>("expandedIDs") ?? new List<int>();


                EditorApplication.delayCall -= UpdateState;
                EditorApplication.delayCall += UpdateState;

            }

            public bool isTwoColumns;

            public object treeViewController;

            public object data;

            public List<int> expandedIds = new List<int>();





            public Tree(EditorWindow browser) => this.browser = browser;

            public EditorWindow browser;




            public static MethodInfo mi_data_GetItem = typeof(Editor).Assembly.GetType("UnityEditor.IMGUI.Controls.ITreeViewDataSource").GetMethod("GetItem", maxBindingFlags);

        }

        static Tree GetTree(EditorWindow browser)
        {
            if (trees_byBrowser.TryGetValue(browser, out var existingState)) return existingState;


            var tree = new Tree(browser);

            tree.UpdateState();

            trees_byBrowser[browser] = tree;

            return tree;

        }

        static Dictionary<EditorWindow, Tree> trees_byBrowser = new Dictionary<EditorWindow, Tree>();







        class FoldersFirstUpdater
        {
            public void Update() // delayCall loop 
            {
                if (browser == null) { foldersFirstUpdater_byBrowser.Remove(browser); return; }


                var isTwoColumns = browser.GetFieldValue<int>("m_ViewMode") == 1;

                void oneColumn()
                {
                    if (isTwoColumns) return;
                    if (initedForOneColumn) return;


                    var m_AssetTree = browser.GetFieldValue("m_AssetTree");

                    if (m_AssetTree == null) return;

                    m_AssetTree.GetPropertyValue("data").SetPropertyValue("foldersFirst", true);
                    m_AssetTree.InvokeMethod("ReloadData");


                    initedForOneColumn = true;
                    initedForTwoColumns = false;

                }
                void twoColumns()
                {
                    if (!isTwoColumns) return;
                    if (initedForTwoColumns) return;


                    var m_ListArea = browser.GetFieldValue("m_ListArea");

                    if (m_ListArea == null) return;

                    m_ListArea.SetPropertyValue("foldersFirst", true);
                    browser.InvokeMethod("InitListArea");


                    initedForOneColumn = false;
                    initedForTwoColumns = true;

                }


                oneColumn();
                twoColumns();


                EditorApplication.delayCall -= Update;
                EditorApplication.delayCall += Update;

            }

            bool initedForOneColumn;
            bool initedForTwoColumns;




            public FoldersFirstUpdater(EditorWindow browser) => this.browser = browser;

            public EditorWindow browser;


        }

        static Dictionary<EditorWindow, FoldersFirstUpdater> foldersFirstUpdater_byBrowser = new Dictionary<EditorWindow, FoldersFirstUpdater>();













        public static FolderInfo GetFolderInfo(string guid, bool createDataIfDoesntExist)
        {
            var folderInfo = new FolderInfo();

            folderInfo.folderData = GetFolderData(guid, createDataIfDoesntExist);
            folderInfo.folderState = GetFolderState(guid);

            return folderInfo;

        }

        public class FolderInfo
        {
            public string iconNameOrGuid
            {
                get
                {
                    var iconNameOrGuid = folderData?.iconNameOrGuid;

                    if (iconNameOrGuid == "" || iconNameOrGuid == null)
                        iconNameOrGuid = VFoldersMenu.autoIconsEnabled ? folderState.autoIconName : "";

                    if (iconNameOrGuid == "none")
                        iconNameOrGuid = "";

                    return iconNameOrGuid;

                }
                set
                {
                    if (!VFoldersMenu.autoIconsEnabled) { folderData.iconNameOrGuid = value; return; }

                    if (value == folderState.autoIconName)
                        folderData.iconNameOrGuid = "";

                    else if (value == "")
                        folderData.iconNameOrGuid = "none";

                    else
                        folderData.iconNameOrGuid = value;


                    // if (value == folderState.autoIconName && VFoldersMenu.autoIconsEnabled)
                    //     folderData.iconNameOrGuid = "";

                    // else if (value == "")
                    //     folderData.iconNameOrGuid = "none";

                    // else
                    //     folderData.iconNameOrGuid = value;

                }

            }
            public string iconNameOrPath => iconNameOrGuid.Length == 32 ? iconNameOrGuid.ToPath() : iconNameOrGuid;

            public Color color
            {
                get
                {
                    if (!hasColor) return default;
                    // return VFoldersPalette.GetDefaultColor(folderData.colorIndex - 1);

                    if (palette)
                        return palette.colors[folderData.colorIndex - 1];
                    else
                        return VFoldersPalette.GetDefaultColor(folderData.colorIndex - 1);

                }
            }

            public bool hasIcon => (folderData != null && folderData.iconNameOrGuid != "" && folderData.iconNameOrGuid != "none") || (VFoldersMenu.autoIconsEnabled && folderState.autoIconName != "" && (folderData == null || folderData.iconNameOrGuid != "none"));
            public bool hasColor => folderData != null && folderData.colorIndex.IsInRange(1, VFoldersPalette.colorsCount);
            public bool isEmpty => folderState.isEmpty;

            public FolderData folderData;
            public FolderState folderState;

        }



        public static FolderData GetFolderData(string guid, bool createDataIfDoesntExist)
        {
            if (!data) return null;

            FolderData folderData = null;

            void fromScripableObject()
            {
                if (VFoldersData.storeDataInMetaFiles) return;

                data.folderDatas_byGuid.TryGetValue(guid, out folderData);


                if (folderData != null || !createDataIfDoesntExist) return;

                folderData = new FolderData();

                data.folderDatas_byGuid[guid] = folderData;

            }
            void fromMetaFile()
            {
                if (!VFoldersData.storeDataInMetaFiles) return;

                folderDatasFromMetaFiles_byGuid.TryGetValue(guid, out folderData);


                if (folderData != null) return;

                var importer = AssetImporter.GetAtPath(guid.ToPath());

                try { folderData = JsonUtility.FromJson<FolderData>(importer.userData); } catch { }

                folderDatasFromMetaFiles_byGuid[guid] = folderData;


                if (folderData != null || !createDataIfDoesntExist) return;

                folderData = new FolderData();

                folderDatasFromMetaFiles_byGuid[guid] = folderData;

            }

            fromScripableObject();
            fromMetaFile();

            return folderData;

        }

        public static Dictionary<string, FolderData> folderDatasFromMetaFiles_byGuid = new Dictionary<string, FolderData>();



        public static FolderState GetFolderState(string guid)
        {
            FolderState folderState = null;

            void getFromCache()
            {
                cache.folderStates_byGuid.TryGetValue(guid, out folderState);
            }
            void create()
            {
                if (folderState != null) return;

                folderState = new FolderState();

                folderState.needsUpdate = true;

                cache.folderStates_byGuid[guid] = folderState;

            }
            void update()
            {
                if (!folderState.needsUpdate) return;
                if (!Directory.Exists(guid.ToPath())) { folderState.needsUpdate = false; return; }

                void isEmpty()
                {
                    folderState.isEmpty = !Directory.EnumerateFileSystemEntries(guid.ToPath()).Any();
                }
                void contentTypeNames()
                {
                    var types = Directory.GetFiles(guid.ToPath(), "*.*")
                                         .Select(r => AssetDatabase.GetMainAssetTypeAtPath(r))
                                         .Where(r => r != null);


                    var iconNames = new List<string>();

                    foreach (var type in types)

                        if (type == typeof(Texture2D))
                            iconNames.Add("Texture Icon");

                        else if (type == typeof(GameObject))
                            iconNames.Add("Prefab Icon");

                        else if (type.BaseType == typeof(ScriptableObject) || type.BaseType?.BaseType == typeof(ScriptableObject))
                            iconNames.Add("ScriptableObject Icon");

                        // else if (type == typeof(MonoScript) || type == typeof(AssemblyDefinitionAsset) || type == typeof(AssemblyDefinitionReferenceAsset))
                        else if (type == typeof(MonoScript))// || type == typeof(AssemblyDefinitionAsset) || type == typeof(AssemblyDefinitionReferenceAsset))
                            iconNames.Add("cs Script Icon");

                        else if (AssetPreview.GetMiniTypeThumbnail(type)?.name is string iconName)
                            iconNames.Add(iconName);


                    folderState.contentMinimapIconNames = iconNames.Distinct().OrderBy(r => r).ToList();

                }
                void autoIconName()
                {
                    folderState.autoIconName = "";

                    var types = Directory.GetFiles(guid.ToPath(), "*.*")
                                         .Select(r => AssetDatabase.GetMainAssetTypeAtPath(r))
                                         .Where(r => r != null);

                    if (!types.Any()) return;
                    if (!types.All(r => r == types.First())) return;

                    var type = types.First();

                    if (type == typeof(SceneAsset))
                        folderState.autoIconName = "SceneAsset Icon";

                    else if (type == typeof(GameObject))
                        folderState.autoIconName = "Prefab Icon";

                    else if (type == typeof(Material))
                        folderState.autoIconName = "Material Icon";

                    else if (type == typeof(Texture))
                        folderState.autoIconName = "Texture Icon";

                    else if (type.BaseType == typeof(ScriptableObject))
                        folderState.autoIconName = "ScriptableObject Icon";

                    else if (type == typeof(TerrainData))
                        folderState.autoIconName = "TerrainData Icon";

                    else if (type == typeof(AudioClip))
                        folderState.autoIconName = "AudioClip Icon";

                    else if (type == typeof(Shader))
                        folderState.autoIconName = "Shader Icon";

                    else if (type == typeof(ComputeShader))
                        folderState.autoIconName = "ComputeShader Icon";

                    else if (type == typeof(MonoScript) || type == typeof(AssemblyDefinitionAsset) || type == typeof(AssemblyDefinitionReferenceAsset))
                        folderState.autoIconName = "cs Script Icon";
                }

                isEmpty();
                contentTypeNames();
                autoIconName();

                folderState.needsUpdate = false;

            }

            getFromCache();
            create();
            update();

            return folderState;

        }

        class FolderStateChangeDetector : AssetPostprocessor
        {
#if UNITY_2021_2_OR_NEWER
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
#else
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
#endif
            {
                foreach (var path in importedAssets.Concat(deletedAssets).Concat(movedAssets).Concat(movedFromAssetPaths))
                    if (path.HasParentPath())
                        if (cache.folderStates_byGuid.TryGetValue(path.GetParentPath().ToGuid(), out var folderState))
                            folderState.needsUpdate = true;
            }
        }

        public static VFoldersCache cache => VFoldersCache.instance;













        [InitializeOnLoadMethod]
        static void Init()
        {
            if (VFoldersMenu.pluginDisabled) return;

            void subscribe()
            {
                EditorApplication.projectWindowItemOnGUI -= ProjectBrowserItemGUI;
                EditorApplication.projectWindowItemOnGUI = ProjectBrowserItemGUI + EditorApplication.projectWindowItemOnGUI;

                EditorApplication.update -= Update;
                EditorApplication.update += Update;

                var globalEventHandler = typeof(EditorApplication).GetFieldValue<EditorApplication.CallbackFunction>("globalEventHandler");
                typeof(EditorApplication).SetFieldValue("globalEventHandler", CheckShortcuts + (globalEventHandler - CheckShortcuts));


                // EditorApplication.playModeStateChanged += (PlayModeStateChange obj) => UpdateFoldersFirst();
                // EditorApplication.projectChanged += UpdateFoldersFirst;

            }
            void loadData()
            {
                data = AssetDatabase.LoadAssetAtPath<VFoldersData>(EditorPrefs.GetString("vFolders-lastKnownDataPath-" + GetProjectId()));


                if (data) return;

                data = AssetDatabase.FindAssets("t:VFoldersData").Select(guid => AssetDatabase.LoadAssetAtPath<VFoldersData>(guid.ToPath())).FirstOrDefault();


                if (!data) return;

                EditorPrefs.SetString("vFolders-lastKnownDataPath-" + GetProjectId(), data.GetPath());

            }
            void loadPalette()
            {
                palette = AssetDatabase.LoadAssetAtPath<VFoldersPalette>(EditorPrefs.GetString("vFolders-lastKnownPalettePath-" + GetProjectId()));


                if (palette) return;

                palette = AssetDatabase.FindAssets("t:VFoldersPalette").Select(guid => AssetDatabase.LoadAssetAtPath<VFoldersPalette>(guid.ToPath())).FirstOrDefault();


                if (!palette) return;

                EditorPrefs.SetString("vFolders-lastKnownPalettePath-" + GetProjectId(), palette.GetPath());

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
                if (EditorPrefs.GetBool("vFolders-dataMigrationFromV1Attempted-" + GetProjectId(), false)) return;

                EditorPrefs.SetBool("vFolders-dataMigrationFromV1Attempted-" + GetProjectId(), true);

                var lines = System.IO.File.ReadAllLines(data.GetPath());

                if (lines.Length < 15 || !lines[14].Contains("folderDatasByGuid")) return;

                var guids = new List<string>();
                var icons = new List<string>();
                var colors = new List<int>();

                void parseGudis()
                {
                    for (int i = 16; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("values:")) break;

                        var startIndex = lines[i].IndexOf("- ") + 2;

                        if (startIndex < lines[i].Length)
                            guids.Add(lines[i].Substring(startIndex));
                        else
                            guids.Add("");

                    }

                }
                void parseIcons()
                {
                    for (int i = 0; i < guids.Count; i++)
                        if (lines[29 + i * 5 + 3] is string line)
                            if (line.Length > line.IndexOf(": ") + 2)
                                icons.Add(line.Substring(line.IndexOf(": ") + 2));
                            else
                                icons.Add("");

                }
                void parseColors()
                {
                    for (int i = 0; i < guids.Count; i++)
                        if (lines[29 + i * 5 + 1] is string line)
                            if (line.Length > line.IndexOf(": ") + 2)
                                colors.Add(int.Parse(line.Substring(line.IndexOf(": ") + 2)));
                            else
                                colors.Add(0);

                }

                void remapColors()
                {
                    for (int i = 0; i < colors.Count; i++)
                        if (colors[i] == 10)
                            colors[i] = 1;
                        else if (colors[i] != 0)
                            colors[i]++;

                }
                void fillData()
                {
                    for (int i = 0; i < guids.Count; i++)
                        if (icons[i] != "" || colors[i] != 0)
                            data.folderDatas_byGuid[guids[i]] = new FolderData { iconNameOrGuid = icons[i], colorIndex = colors[i] };

                    data.Dirty();
                    data.Save();

                }


                try
                {
                    parseGudis();
                    parseIcons();
                    parseColors();

                    remapColors();
                    fillData();

                }
                catch { }

            }
            void fixIconNamesForUnity6()
            {
                if (!Application.unityVersion.Contains("6000")) return;
                if (EditorPrefs.GetBool("vFolders-iconNamesForUnity6Fixed-" + GetProjectId(), false)) return;
                if (!palette) return;
                if (!data) return;

                foreach (var iconRow in palette.iconRows)
                    if (iconRow.builtinIcons.Contains("PhysicMaterial Icon"))
                        iconRow.builtinIcons[iconRow.builtinIcons.IndexOf("PhysicMaterial Icon")] = "PhysicsMaterial Icon";

                foreach (var folderData in data.folderDatas_byGuid.Values)
                    if (folderData.iconNameOrGuid == "PhysicMaterial Icon")
                        folderData.iconNameOrGuid = "PhysicsMaterial Icon";

                EditorPrefs.SetBool("vFolders-iconNamesForUnity6Fixed-" + GetProjectId(), true);

            }

            subscribe();
            loadData();
            loadPalette();
            loadDataAndPaletteDelayed();
            migrateDataFromV1();
            fixIconNamesForUnity6();

        }

        public static VFoldersData data;
        public static VFoldersPalette palette;





        static EditorWindow hoveredProjectBrowser => EditorWindow.mouseOverWindow?.GetType() == t_ProjectBrowser ? EditorWindow.mouseOverWindow : null;

        static IEnumerable<EditorWindow> allProjectBrowsers => _allProjectBrowsers ??= t_ProjectBrowser.GetFieldValue<IList>("s_ProjectBrowsers").Cast<EditorWindow>();
        static IEnumerable<EditorWindow> _allProjectBrowsers;

        static Type t_ProjectBrowser = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");







        const string version = "2.0.11";

    }
}
#endif
