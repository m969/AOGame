#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using Type = System.Type;
using static VHierarchy.VHierarchyData;
using static VHierarchy.Libs.VUtils;
using static VHierarchy.Libs.VGUI;



namespace VHierarchy
{
    public class VHierarchyComponentWindow : EditorWindow
    {
        void OnGUI()
        {
            if (!component) { Close(); return; } // todo script components break on playmode


            void background()
            {
                position.SetPos(0, 0).Draw(GUIColors.windowBackground);
            }
            void outline()
            {
                if (Application.platform == RuntimePlatform.OSXEditor) return;

                position.SetPos(0, 0).DrawOutline(Greyscale(.1f));

            }
            void header()
            {
                var headerRect = ExpandWidthLabelRect(18).Resize(-1).AddWidthFromMid(6);
                var pinButtonRect = headerRect.SetWidthFromRight(17).SetHeightFromMid(17).Move(-21, .5f);
                var closeButtonRect = headerRect.SetWidthFromRight(16).SetHeightFromMid(16).Move(-3, .5f);

                var backgroundColor = isDarkTheme ? Greyscale(.25f) : GUIColors.windowBackground;

                void startDragging()
                {
                    if (isResizing) return;
                    if (isDragged) return;
                    if (!curEvent.isMouseDrag) return;
                    if (!headerRect.IsHovered()) return;


                    isDragged = true;

                    dragStartMousePos = EditorGUIUtility.GUIToScreenPoint(curEvent.mousePosition);
                    dragStartWindowPos = position.position;


                    isPinned = true;

                    if (floatingInstance == this)
                        floatingInstance = null;

                    EditorApplication.RepaintHierarchyWindow();


                }
                void updateDragging()
                {
                    if (!isDragged) return;


                    var draggedPosition = dragStartWindowPos + EditorGUIUtility.GUIToScreenPoint(curEvent.mousePosition) - dragStartMousePos;

                    if (!curEvent.isRepaint)
                        position = position.SetPos(draggedPosition);


                    EditorGUIUtility.hotControl = EditorGUIUtility.GetControlID(FocusType.Passive);

                }
                void stopDragging()
                {
                    if (!isDragged) return;
                    if (!curEvent.isMouseUp) return;


                    isDragged = false;

                    EditorGUIUtility.hotControl = 0;

                }

                void background()
                {
                    headerRect.Draw(backgroundColor);

                    headerRect.SetHeightFromBottom(1).Draw(isDarkTheme ? Greyscale(.2f) : Greyscale(.7f));

                }
                void icon()
                {
                    var iconRect = headerRect.SetWidth(20).MoveX(14).MoveY(-1);

                    GUI.Label(iconRect, VHierarchy.GetComponentIcon(component));

                }
                void toggle()
                {
                    var toggleRect = headerRect.MoveX(36).SetSize(20, 20);


                    var pi_enabled = component.GetType().GetProperty("enabled") ??
                                     component.GetType().BaseType?.GetProperty("enabled") ??
                                     component.GetType().BaseType?.BaseType?.GetProperty("enabled") ??
                                     component.GetType().BaseType?.BaseType?.BaseType?.GetProperty("enabled");


                    if (pi_enabled == null) return;

                    var enabled = (bool)pi_enabled.GetValue(component);


                    if (GUI.Toggle(toggleRect, enabled, "") == enabled) return;

                    component.RecordUndo();
                    pi_enabled.SetValue(component, !enabled);

                }
                void name()
                {
                    var nameRect = headerRect.MoveX(54).MoveY(-1);

                    var s = VHierarchy.GetComponentName(component);

                    if (isPinned)
                        s += " of " + component.gameObject.name;


                    SetLabelBold();

                    GUI.Label(nameRect, s);

                    ResetLabelStyle();

                }
                void nameCurtain()
                {
                    var flatColorRect = headerRect.SetX(pinButtonRect.x + 3).SetXMax(headerRect.xMax);
                    var gradientRect = headerRect.SetXMax(flatColorRect.x).SetWidthFromRight(30);

                    flatColorRect.Draw(backgroundColor);
                    gradientRect.DrawCurtainLeft(backgroundColor);

                }
                void pinButton()
                {
                    if (!isPinned && closeButtonRect.IsHovered()) return;


                    var normalColor = isDarkTheme ? Greyscale(.65f) : Greyscale(.8f);
                    var hoveredColor = isDarkTheme ? Greyscale(.9f) : normalColor;
                    var activeColor = Color.white;



                    SetGUIColor(isPinned ? activeColor : pinButtonRect.IsHovered() ? hoveredColor : normalColor);

                    GUI.Label(pinButtonRect, EditorGUIUtility.IconContent("pinned"));

                    ResetGUIColor();


                    SetGUIColor(Color.clear);

                    var clicked = GUI.Button(pinButtonRect, "");

                    ResetGUIColor();


                    if (!clicked) return;

                    isPinned = !isPinned;

                    if (isPinned && floatingInstance == this)
                        floatingInstance = null;

                    if (!isPinned && !floatingInstance)
                        floatingInstance = this;

                    EditorApplication.RepaintHierarchyWindow();


                }
                void closeButton()
                {

                    SetGUIColor(Color.clear);

                    if (GUI.Button(closeButtonRect, ""))
                        Close();

                    ResetGUIColor();


                    var normalColor = isDarkTheme ? Greyscale(.65f) : Greyscale(.35f);
                    var hoveredColor = isDarkTheme ? Greyscale(.9f) : normalColor;


                    SetGUIColor(closeButtonRect.IsHovered() ? hoveredColor : normalColor);

                    GUI.Label(closeButtonRect, EditorGUIUtility.IconContent("CrossIcon"));

                    ResetGUIColor();


                    if (isPinned) return;

                    var escRect = closeButtonRect.Move(-22, -1).SetWidth(70);

                    SetGUIEnabled(false);

                    if (closeButtonRect.IsHovered())
                        GUI.Label(escRect, "Esc");

                    ResetGUIEnabled();

                }

                startDragging();
                updateDragging();
                stopDragging();

                background();
                icon();
                toggle();
                name();
                nameCurtain();
                pinButton();
                closeButton();

            }
            void body()
            {
                BeginIndent(16);

                editor?.OnInspectorGUI();

                EndIndent(0);

            }

            void updateHeight()
            {
                var r = ExpandWidthLabelRect();

                if (curEvent.isRepaint)
                    position = position.SetHeight(lastRect.y);

            }
            void updatePosition()
            {
                if (!curEvent.isLayout) return;

                void calcDeltaTime()
                {
                    deltaTime = (float)(EditorApplication.timeSinceStartup - lastLayoutTime);

                    if (deltaTime > .05f)
                        deltaTime = .0166f;

                    lastLayoutTime = EditorApplication.timeSinceStartup;

                }
                void resetCurPos()
                {
                    if (currentPosition != default && !isPinned) return;

                    currentPosition = position.position; // position.position is always int, which can't be used for lerping

                }
                void lerpCurPos()
                {
                    if (isPinned) return;

                    var speed = 9;

                    SmoothDamp(ref currentPosition, targetPosition, speed, ref positionDeriv, deltaTime);
                    // Lerp(ref currentPosition, targetPosition, speed, deltaTime);

                }
                void setCurPos()
                {
                    if (isPinned) return;

                    position = position.SetPos(currentPosition);

                }

                calcDeltaTime();
                resetCurPos();
                lerpCurPos();
                setCurPos();

            }
            void closeOnEscape()
            {
                if (!curEvent.isKeyDown) return;
                if (curEvent.keyCode != KeyCode.Escape) return;

                Close();
            }

            void horizontalResize()
            {
                var resizeArea = this.position.SetPos(0, 0).SetWidthFromRight(5).AddHeightFromBottom(-20);

                void startResize()
                {
                    if (isDragged) return;
                    if (isResizing) return;
                    if (!curEvent.isMouseDown && !curEvent.isMouseDrag) return;
                    if (!resizeArea.IsHovered()) return;

                    isResizing = true;

                    resizeStartMousePos = EditorGUIUtility.GUIToScreenPoint(curEvent.mousePosition);
                    resizeStartWindowSize = this.position.size;

                }
                void updateResize()
                {
                    if (!isResizing) return;


                    var resizedWidth = resizeStartWindowSize.x + EditorGUIUtility.GUIToScreenPoint(curEvent.mousePosition).x - resizeStartMousePos.x;

                    var width = resizedWidth.Max(minWidth);

                    if (!curEvent.isRepaint)
                        position = position.SetWidth(width);


                    EditorGUIUtility.hotControl = EditorGUIUtility.GetControlID(FocusType.Passive);

                }
                void stopResize()
                {
                    if (!isResizing) return;
                    if (!curEvent.isMouseUp) return;

                    isResizing = false;

                    EditorGUIUtility.hotControl = 0;

                }


                EditorGUIUtility.AddCursorRect(resizeArea, MouseCursor.ResizeHorizontal);

                startResize();
                updateResize();
                stopResize();

            }


            background();
            outline();

            header();

            Space(3);
            body();

            Space(7);


            updateHeight();
            updatePosition();
            closeOnEscape();

            horizontalResize();

            if (!isPinned)
                Repaint();

        }

        public Vector2 targetPosition;
        public Vector2 currentPosition;
        Vector2 positionDeriv;
        float deltaTime;
        double lastLayoutTime;

        bool isDragged;
        Vector2 dragStartMousePos;
        Vector2 dragStartWindowPos;

        bool isResizing;
        Vector2 resizeStartMousePos;
        Vector2 resizeStartWindowSize;





        void OnLostFocus()
        {
            if (isPinned) return;

            if (curEvent.holdingAlt && EditorWindow.focusedWindow.GetType().Name == "SceneHierarchyWindow")
                CloseNextFrameIfNotRefocused();
            else
                Close();

        }

        void CloseNextFrameIfNotRefocused()
        {
            EditorApplication.delayCall += () => { if (EditorWindow.focusedWindow != this) Close(); };
        }

        public bool isPinned;





        public void Init(Component component)
        {
            if (editor)
                editor.DestroyImmediate();

            this.component = component;
            this.editor = Editor.CreateEditor(component);

        }

        void OnDestroy()
        {
            editor?.DestroyImmediate();

            editor = null;
            component = null;

            EditorPrefs.SetFloat("vHierarchy-componentWindowWidth", position.width);

        }

        public Component component;
        public Editor editor;





        public static void CreateFloatingInstance(Vector2 position)
        {
            floatingInstance = ScriptableObject.CreateInstance<VHierarchyComponentWindow>();

            floatingInstance.ShowPopup();


            var savedWidth = EditorPrefs.GetFloat("vHierarchy-componentWindowWidth", minWidth);

            var width = savedWidth.Max(minWidth);

            floatingInstance.position = Rect.zero.SetPos(position).SetWidth(width).SetHeight(200);

            floatingInstance.targetPosition = position;

        }

        public static VHierarchyComponentWindow floatingInstance;

        public static float minWidth => 300;

    }
}
#endif