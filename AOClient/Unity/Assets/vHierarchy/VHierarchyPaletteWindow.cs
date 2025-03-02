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
using static VHierarchy.VHierarchyPalette;
using static VHierarchy.Libs.VUtils;
using static VHierarchy.Libs.VGUI;



namespace VHierarchy
{
    public class VHierarchyPaletteWindow : EditorWindow
    {
        void OnGUI()
        {
            if (!palette) { Close(); return; }

            int hoveredColorIndex = -1;
            string hoveredIconNameOrGuid = null;

            void background()
            {
                position.SetPos(0, 0).Draw(windowBackground);
            }
            void outline()
            {
                if (Application.platform == RuntimePlatform.OSXEditor) return;

                position.SetPos(0, 0).DrawOutline(Greyscale(.1f));

            }
            void colors()
            {
                if (!palette.colorsEnabled) { Space(-spaceAfterColors); return; }

                var rowRect = ExpandWidthLabelRect(height: cellSize).SetX(paddingX);

                void color(int i)
                {
                    var cellRect = rowRect.MoveX(i * cellSize).SetWidth(cellSize).SetHeightFromMid(cellSize);

                    void backgroundSelected()
                    {
                        if (!initialColorIndexes.Contains(i)) return;

                        cellRect.Resize(1).DrawWithRoundedCorners(selectedBackground, 2);

                    }
                    void backgroundHovered()
                    {
                        if (!cellRect.IsHovered()) return;

                        cellRect.Resize(1).DrawWithRoundedCorners(this.hoveredBackground, 2);

                    }
                    void crossIcon()
                    {
                        if (i != 0) return;

                        SetLabelAlignmentCenter();

                        GUI.Label(cellRect.SetSizeFromMid(iconSize), EditorGUIUtility.IconContent("CrossIcon"));

                        ResetLabelStyle();

                    }
                    void color()
                    {
                        if (i == 0) return;

                        var brightness = i <= VHierarchyPalette.greyColorsCount ? 1.02f : 1.35f;
                        var outlineColor = i <= VHierarchyPalette.greyColorsCount ? Greyscale(.0f, .4f) : Greyscale(.15f, .2f);

                        cellRect.Resize(3).DrawWithRoundedCorners(outlineColor, 4);
                        cellRect.Resize(4).DrawWithRoundedCorners((palette.colors[i - 1] * brightness).SetAlpha(1), 3);
                        cellRect.Resize(4).AddWidthFromRight(-2).DrawCurtainLeft(GUIColors.windowBackground.SetAlpha((1 - palette.colors[i - 1].a) * .45f));

                    }
                    void setHovered()
                    {
                        if (!cellRect.IsHovered()) return;

                        hoveredColorIndex = i;

                    }
                    void closeOnClick()
                    {
                        if (!cellRect.IsHovered()) return;
                        if (!curEvent.isMouseDown) return;

                        Close();

                    }


                    cellRect.MarkInteractive();

                    backgroundSelected();
                    backgroundHovered();
                    crossIcon();
                    color();
                    setHovered();
                    closeOnClick();

                }


                for (int i = 0; i < palette.colors.Count + 1; i++)
                    color(i);

            }
            void icons()
            {
                void row(IconRow iconRow)
                {
                    if (!iconRow.enabled) return;
                    if (iconRow.isEmpty) return;

                    var rowRect = ExpandWidthLabelRect(height: cellSize).SetX(paddingX);
                    var isFirstEnabledRow = palette.iconRows.First(r => r.enabled) == iconRow;

                    void icon(int i)
                    {
                        var cellRect = rowRect.MoveX(i * cellSize).SetWidth(cellSize).SetHeightFromMid(cellSize);

                        var isCrossIcon = isFirstEnabledRow && i == 0;
                        var actualIconIndex = isFirstEnabledRow ? i - 1 : i;
                        var isBuiltinIcon = !isCrossIcon && actualIconIndex < iconRow.builtinIcons.Count;
                        var isCustomIcon = !isCrossIcon && actualIconIndex >= iconRow.builtinIcons.Count;
                        var iconNameOrGuid = isCrossIcon ? "" : isCustomIcon ? iconRow.customIcons[actualIconIndex - iconRow.builtinIcons.Count] : iconRow.builtinIcons[actualIconIndex];

                        void backgroundSelected()
                        {
                            if (!initialIconNamesOrGuids.Contains(iconNameOrGuid)) return;

                            cellRect.Resize(1).DrawWithRoundedCorners(selectedBackground, 2);

                        }
                        void backgroundHovered()
                        {
                            if (!cellRect.IsHovered()) return;

                            cellRect.Resize(1).DrawWithRoundedCorners(this.hoveredBackground, 2);

                        }
                        void crossIcon()
                        {
                            if (!isCrossIcon) return;

                            SetLabelAlignmentCenter();

                            GUI.Label(cellRect.SetSizeFromMid(iconSize), EditorGUIUtility.IconContent("CrossIcon"));

                            ResetLabelStyle();

                        }
                        void builtinIcon()
                        {
                            if (!isBuiltinIcon) return;

                            SetLabelAlignmentCenter();

                            GUI.Label(cellRect.SetSizeFromMid(iconSize), EditorGUIUtility.IconContent(iconNameOrGuid));

                            ResetLabelStyle();

                        }
                        void customIcon()
                        {
                            if (!isCustomIcon) return;

                            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(iconNameOrGuid.ToPath());

                            GUI.DrawTexture(cellRect.SetSizeFromMid(iconSize), texture ?? Texture2D.blackTexture);

                        }
                        void setHovered()
                        {
                            if (!cellRect.IsHovered()) return;

                            hoveredIconNameOrGuid = iconNameOrGuid;

                        }
                        void closeOnClick()
                        {
                            if (!cellRect.IsHovered()) return;
                            if (!curEvent.isMouseDown) return;

                            Close();

                        }


                        cellRect.MarkInteractive();

                        backgroundSelected();
                        backgroundHovered();
                        crossIcon();
                        builtinIcon();
                        customIcon();
                        setHovered();
                        closeOnClick();

                    }

                    for (int i = 0; i < iconRow.iconCount + (isFirstEnabledRow ? 1 : 0); i++)
                        icon(i);

                    Space(rowSpacing - 2);

                }

                for (int i = 0; i < palette.iconRows.Count; i++)
                    row(palette.iconRows[i]);

            }

            void setColorsAndIcons()
            {
                if (!curEvent.isRepaint) return;


                if (palette.iconRows.Any(r => r.enabled))
                    if (hoveredIconNameOrGuid != null)
                        SetIcon(hoveredIconNameOrGuid);
                    else
                        SetInitialIcons();


                if (palette.colorsEnabled)
                    if (hoveredColorIndex != -1)
                        SetColor(hoveredColorIndex);
                    else
                        SetInitialColors();

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
                    if (currentPosition != default) return;

                    currentPosition = position.position; // position.position is always int, which can't be used for lerping

                }
                void lerpCurPos()
                {
                    var speed = 9;

                    SmoothDamp(ref currentPosition, targetPosition, speed, ref positionDeriv, deltaTime);
                    // Lerp(ref currentPosition, targetPosition, speed, deltaTime);

                }
                void setCurPos()
                {
                    position = position.SetPos(currentPosition);
                }

                calcDeltaTime();
                resetCurPos();
                lerpCurPos();
                setCurPos();

                if (!currentPosition.y.Approx(targetPosition.y))
                    Repaint();

            }
            void closeOnEscape()
            {
                if (!curEvent.isKeyDown) return;
                if (curEvent.keyCode != KeyCode.Escape) return;

                SetInitialColors();
                SetInitialIcons();

                Close();

            }


            RecordUndoOnDatas();

            background();
            outline();

            Space(paddingY);
            colors();

            Space(spaceAfterColors);
            icons();

            setColorsAndIcons();
            updatePosition();
            closeOnEscape();

            EditorApplication.RepaintHierarchyWindow();

        }

        static float iconSize => 18;
        static float iconSpacing => 2;
        static float cellSize => iconSize + iconSpacing;
        static float spaceAfterColors => 11;
        public float rowSpacing = 1;
        static float paddingX => 12;
        static float paddingY => 12;

        Color windowBackground => isDarkTheme ? Greyscale(.23f) : Greyscale(.7f);
        Color selectedBackground => isDarkTheme ? new Color(.3f, .5f, .7f, .8f) : new Color(.3f, .5f, .7f, .4f) * 1.35f;
        Color hoveredBackground = Greyscale(1, .3f);

        public Vector2 targetPosition;
        public Vector2 currentPosition;
        Vector2 positionDeriv;
        float deltaTime;
        double lastLayoutTime;






        void SetIcon(string iconNameOrGuid)
        {
            foreach (var r in goDatas)
                r.iconNameOrGuid = iconNameOrGuid;
        }
        void SetColor(int colorIndex)
        {
            foreach (var r in goDatas)
                r.colorIndex = colorIndex;
        }

        void SetInitialIcons()
        {
            for (int i = 0; i < goDatas.Count; i++)
                goDatas[i].iconNameOrGuid = initialIconNamesOrGuids[i];

        }
        void SetInitialColors()
        {
            for (int i = 0; i < goDatas.Count; i++)
                goDatas[i].colorIndex = initialColorIndexes[i];

        }

        void RemoveEmptyGoDatas()
        {
            var toRemove = goDatas.Where(r => r.iconNameOrGuid == "" && r.colorIndex == 0);

            foreach (var goData in toRemove)
                goData.sceneData.goDatas_byGlobalId.RemoveValue(goData);

            if (toRemove.Any())
                Undo.CollapseUndoOperations(Undo.GetCurrentGroup() - 1);

        }

        void RecordUndoOnDatas()
        {
            if (usingDataSO)
                if (data)
                    data.RecordUndo();

            foreach (var r in usedDataComponents)
                r.RecordUndo();

        }
        void MarkDatasDirty()
        {
            if (usingDataSO)
                if (data)
                    data.Dirty();

            foreach (var r in usedDataComponents)
                r.Dirty();
        }
        void SaveData()
        {
            if (usingDataSO)
                data.Save();
        }

        bool usingDataSO => !gameObjects.Select(r => r.scene).All(r => VHierarchy.dataComponents_byScene.GetValueOrDefault(r) != null);
        IEnumerable<VHierarchyDataComponent> usedDataComponents => VHierarchy.dataComponents_byScene.Where(kvp => kvp.Value && gameObjects.Select(r => r.scene).Contains(kvp.Key)).Select(kvp => kvp.Value);







        void OnLostFocus()
        {
            if (curEvent.holdingAlt && EditorWindow.focusedWindow?.GetType().Name == "SceneHierarchyWindow")
                CloseNextFrameIfNotRefocused();
            else
                Close();

        }

        void CloseNextFrameIfNotRefocused()
        {
            EditorApplication.delayCall += () => { if (EditorWindow.focusedWindow != this) Close(); };
        }







        public void Init(List<GameObject> gameObjects)
        {
            void createData()
            {
                if (VHierarchy.data) return;

                VHierarchy.data = ScriptableObject.CreateInstance<VHierarchyData>();

                AssetDatabase.CreateAsset(VHierarchy.data, GetScriptPath("VHierarchy").GetParentPath().CombinePath("vHierarchy Data.asset"));

            }
            void createPalette()
            {
                if (VHierarchy.palette) return;

                VHierarchy.palette = ScriptableObject.CreateInstance<VHierarchyPalette>();

                AssetDatabase.CreateAsset(VHierarchy.palette, GetScriptPath("VHierarchy").GetParentPath().CombinePath("vHierarchy Palette.asset"));

            }
            void setSize()
            {
                var rowCellCounts = new List<int>();

                if (palette.colorsEnabled)
                    rowCellCounts.Add(palette.colors.Count + 1);

                foreach (var r in palette.iconRows.Where(r => r.enabled))
                    rowCellCounts.Add(r.iconCount + (r == palette.iconRows.First(r => r.enabled) ? 1 : 0));

                var width = rowCellCounts.Max() * cellSize + paddingX * 2;



                var iconRowCount = palette.iconRows.Count(r => r.enabled && !r.isEmpty);
                var rowCount = iconRowCount + (palette.colorsEnabled ? 1 : 0);

                var height = rowCount * (cellSize + rowSpacing) + (palette.colorsEnabled && palette.iconRows.Any(r => r.enabled && !r.isEmpty) ? spaceAfterColors : 0) + paddingY * 2;



                position = position.SetSize(width, height).SetPos(targetPosition);

            }
            void getDatas()
            {
                goDatas.Clear();

                foreach (var r in gameObjects)
                    goDatas.Add(VHierarchy.GetGameObjectData(r, createDataIfDoesntExist: true));

            }
            void getInitColorsAndIcons()
            {
                initialColorIndexes.Clear();
                initialIconNamesOrGuids.Clear();

                foreach (var r in goDatas)
                    initialColorIndexes.Add(r.colorIndex);

                foreach (var r in goDatas)
                    initialIconNamesOrGuids.Add(r.iconNameOrGuid);

            }


            this.gameObjects = gameObjects;

            RecordUndoOnDatas();

            createData();
            createPalette();
            setSize();
            getDatas();
            getInitColorsAndIcons();

        }

        void OnDestroy()
        {
            RemoveEmptyGoDatas();
            MarkDatasDirty();
            SaveData();

        }

        public List<GameObject> gameObjects = new List<GameObject>();
        public List<GameObjectData> goDatas = new List<GameObjectData>();

        public List<int> initialColorIndexes = new List<int>();
        public List<string> initialIconNamesOrGuids = new List<string>();

        static VHierarchyPalette palette => VHierarchy.palette;
        static VHierarchyData data => VHierarchy.data;






        public static void CreateInstance(Vector2 position)
        {
            instance = ScriptableObject.CreateInstance<VHierarchyPaletteWindow>();

            instance.ShowPopup();

            instance.position = instance.position.SetPos(position).SetSize(200, 300);
            instance.targetPosition = position;

        }

        public static VHierarchyPaletteWindow instance;

    }
}
#endif