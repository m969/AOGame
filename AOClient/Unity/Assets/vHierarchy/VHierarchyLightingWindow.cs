#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using static VHierarchy.Libs.VUtils;
using static VHierarchy.Libs.VGUI;


namespace VHierarchy
{
    public class VHierarchyLightingWindow : EditorWindow
    {
        void OnGUI()
        {
            void updateSize()
            {
                var r = ExpandWidthLabelRect();

                if (!curEvent.isRepaint) return;

                var curHeight = r.y;

                this.position = position.SetWidth(initWidth).SetHeight(curHeight);

                this.minSize = Vector2.zero;
                this.maxSize = Vector2.one * 123212;

            }

            void header()
            {
                var height = 22f;

                var headerRect = Rect.zero.SetHeight(height).SetWidth(position.width);
                var pinButtonRect = headerRect.SetWidthFromRight(17).SetHeightFromMid(17).Move(-21, .5f);
                var closeButtonRect = headerRect.SetWidthFromRight(16).SetHeightFromMid(16).Move(-3, .5f);

                void startDragging()
                {
                    if (isDragged) return;
                    if (!curEvent.isMouseDrag) return;
                    if (!headerRect.IsHovered()) return;


                    isDragged = true;

                    dragStartMousePos = EditorGUIUtility.GUIToScreenPoint(curEvent.mousePosition);
                    dragStartWindowPos = position.position;


                    isPinned = true;

                    EditorApplication.RepaintHierarchyWindow();


                }
                void updateDragging()
                {
                    if (!isDragged) return;

                    if (!curEvent.isRepaint) // ??
                        position = position.SetPos(dragStartWindowPos + EditorGUIUtility.GUIToScreenPoint(curEvent.mousePosition) - dragStartMousePos);

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
                    headerRect.Draw(EditorGUIUtility.isProSkin ? Greyscale(.185f) : Greyscale(.7f));
                }
                void title_()
                {
                    SetGUIColor(Greyscale(.8f));
                    SetLabelAlignmentCenter();

                    GUI.Label(headerRect, "Lighting");

                    ResetLabelStyle();
                    ResetGUIColor();

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

                }
                void closeButton()
                {

                    SetGUIColor(Color.clear);

                    if (GUI.Button(closeButtonRect, "") || (curEvent.isKeyDown && curEvent.keyCode == KeyCode.Escape))
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
                title_();
                pinButton();
                closeButton();

                Space(height);

            }
            void directionalLight()
            {
                var light = FindObjects<Light>().Where(r => r.type == LightType.Directional && r.gameObject.scene == EditorSceneManager.GetActiveScene()).FirstOrDefault();

                if (!light) return;

                light.RecordUndo();
                light.transform.RecordUndo();


                ObjectFieldWidhoutPicker("Directional Light", light);


                Space(2);
                BeginIndent(8);
                EditorGUIUtility.labelWidth += 2;

                var rotX = light.transform.eulerAngles.x.Loop(-180, 180).Round();
                var rotY = light.transform.eulerAngles.y.Loop(-180, 180).Round();
                rotX = EditorGUILayout.Slider("Rotation X", rotX, 0, 90);
                rotY = EditorGUILayout.Slider("Rotation Y", rotY, -179, 180);
                if (light.transform.rotation != Quaternion.Euler(rotX, rotY, light.transform.eulerAngles.z))
                    light.transform.rotation = Quaternion.Euler(rotX, rotY, light.transform.eulerAngles.z);


                Space(3);
                light.intensity = EditorGUILayout.Slider("Intensity", light.intensity, 0, 2);
                light.color = SmallColorField(ExpandWidthLabelRect().AddWidthFromMid(-1).MoveX(-.5f), "Color", light.color, true);

                EndIndent();

            }
            void ambientLight()
            {
                RenderSettings.ambientMode = (UnityEngine.Rendering.AmbientMode)EditorGUILayout.IntPopup("Ambient Light", (int)RenderSettings.ambientMode, new[] { "\u2009Skybox", "\u2009Gradient", "\u2009Color" }, new[] { 0, 1, 3 });

                foreach (var r in FindObjects<RenderSettings>())
                    r.RecordUndo();


                Space(2);
                BeginIndent(8);
                EditorGUIUtility.labelWidth += 4;

                if (RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Flat)
                {


                    Color.RGBToHSV(RenderSettings.ambientSkyColor, out float h, out float s, out float v);
                    v = EditorGUILayout.Slider("Intensity", v, .01f, 2);
                    RenderSettings.ambientSkyColor = Color.HSVToRGB(h, s, v, true);

                    RenderSettings.ambientSkyColor = SmallColorField("Color", RenderSettings.ambientSkyColor, false, true);


                }

                if (RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Skybox)
                    RenderSettings.ambientIntensity = EditorGUILayout.Slider("Intensity", RenderSettings.ambientIntensity, 0, 2);

                if (RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Trilight)
                {
                    RenderSettings.ambientSkyColor = SmallColorField("Color Sky", RenderSettings.ambientSkyColor, false, true);
                    RenderSettings.ambientEquatorColor = SmallColorField("Color Horizon", RenderSettings.ambientEquatorColor, false, true);
                    RenderSettings.ambientGroundColor = SmallColorField("Color Ground", RenderSettings.ambientGroundColor, false, true);
                }

                EndIndent();

            }
            void fog()
            {
                var mode = EditorGUILayout.IntPopup("Fog", RenderSettings.fog ? (int)RenderSettings.fogMode : 0, new[] { "\u2009Off", "\u2009Linear", "\u2009Exponential", "\u2009Exponential Squared" }, new[] { 0, 1, 2, 3 });

                if (RenderSettings.fog = mode != 0)
                    RenderSettings.fogMode = (FogMode)mode;

                if (!RenderSettings.fog) return;

                Space(2);
                BeginIndent(8);
                EditorGUIUtility.labelWidth += 4;

                if (RenderSettings.fogMode == FogMode.Linear)
                {
                    RenderSettings.fogStartDistance = EditorGUILayout.FloatField("Start", RenderSettings.fogStartDistance);
                    RenderSettings.fogEndDistance = EditorGUILayout.FloatField("End", RenderSettings.fogEndDistance);

                }
                else
                    RenderSettings.fogDensity = ExpSlider(ExpandWidthLabelRect().AddWidthFromRight(1.5f), "Density", RenderSettings.fogDensity, 0, .05f);


                RenderSettings.fogColor = SmallColorField("Color", RenderSettings.fogColor, true, false);

                EndIndent();

            }


            header();


            BeginIndent(6);

            EditorGUIUtility.labelWidth = 115;

            Space(11);
            directionalLight();

            Space(18);
            ambientLight();

            Space(18);
            fog();

            EndIndent(6);

            Space(21);

            updateSize();


            if (Application.platform != RuntimePlatform.OSXEditor)
                position.SetPos(0, 0).DrawOutline(Greyscale(.1f));

            EditorGUIUtility.labelWidth = 0;

            Repaint();


        }

        bool isDragged;
        Vector2 dragStartMousePos;
        Vector2 dragStartWindowPos;



        void OnLostFocus()
        {
            if (isPinned) return;

            Close();

        }

        public bool isPinned;




        public static void CreateInstance(Vector2 position)
        {
            instance = ScriptableObject.CreateInstance<VHierarchyLightingWindow>();

            instance.ShowPopup();

            instance.position = Rect.zero.SetPos(position).SetSize(initWidth, initHeight);

            instance.minSize = Vector2.zero;
            instance.maxSize = Vector2.one * 123212;


        }

        public static VHierarchyLightingWindow instance;




        static float initWidth => 250;
        static float initHeight => 320;
    }
}

#endif