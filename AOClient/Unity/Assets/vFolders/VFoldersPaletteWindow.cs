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
using static VFolders.Libs.VUtils;
using static VFolders.Libs.VGUI;
using static VFolders.VFolders;
using static VFolders.VFoldersData;
using static VFolders.VFoldersPalette;
using static VFolders.VFoldersCache;


namespace VFolders
{
    public class VFoldersPaletteWindow : EditorWindow
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

                        var brightness = 1.00f;
                        var outlineColor = Greyscale(.15f, .2f);

                        cellRect.Resize(3).DrawWithRoundedCorners(outlineColor, 4);
                        cellRect.Resize(4).DrawWithRoundedCorners((palette.colors[i - 1] * brightness).SetAlpha(1), 3);
                        cellRect.Resize(4).AddWidthFromRight(-2).DrawCurtainLeft(GUIColors.windowBackground.SetAlpha((1 - palette.colors[i - 1].a) * .45f));
                        // cellRect.Resize(4).AddWidthFromRight(-2).DrawCurtainLeft(GUIColors.windowBackground.SetAlpha((1 - .1f) * .45f));

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

                if (!currentPosition.magnitude.Approx(targetPosition.magnitude))
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

            EditorApplication.RepaintProjectWindow();

            EditorApplication.delayCall += EditorApplication.RepaintProjectWindow; // to show icons that will be generated in update

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
        Color colorOutline => Greyscale(.2f, .5f);

        public Vector2 targetPosition;
        public Vector2 currentPosition;
        Vector2 positionDeriv;
        float deltaTime;
        double lastLayoutTime;






        void SetIcon(string iconNameOrGuid)
        {
            foreach (var r in folderInfos)
                r.iconNameOrGuid = iconNameOrGuid;
        }
        void SetColor(int colorIndex)
        {
            foreach (var r in folderInfos)
                r.folderData.colorIndex = colorIndex;
        }

        void SetInitialIcons()
        {
            for (int i = 0; i < folderInfos.Count; i++)
                folderInfos[i].iconNameOrGuid = initialIconNamesOrGuids[i];
        }
        void SetInitialColors()
        {
            for (int i = 0; i < folderInfos.Count; i++)
                folderInfos[i].folderData.colorIndex = initialColorIndexes[i];
        }

        void RemoveEmptyFolderDatas()
        {
            if (VFoldersData.storeDataInMetaFiles) return; // empties removed from meta files in SaveData()

            var toRemove = folderInfos.Select(r => r.folderData).Where(r => r.iconNameOrGuid == "" && r.colorIndex == 0);

            foreach (var r in toRemove)
                data.folderDatas_byGuid.RemoveValue(r);

            if (toRemove.Any())
                Undo.CollapseUndoOperations(Undo.GetCurrentGroup() - 1);

        }

        void RecordUndoOnDatas()
        {
            if (!VFoldersData.storeDataInMetaFiles)
                if (data)
                    data.RecordUndo();

            if (VFoldersData.storeDataInMetaFiles)
                foreach (var r in guids)
                    AssetImporter.GetAtPath(r.ToPath()).RecordUndo();

        }
        void MarkDatasDirty()
        {
            if (!VFoldersData.storeDataInMetaFiles)
                if (data)
                    data.Dirty();

            if (VFoldersData.storeDataInMetaFiles)
                VFolders.folderDatasFromMetaFiles_byGuid.Clear();

        }
        void SaveData()
        {
            if (!VFoldersData.storeDataInMetaFiles) { data.Save(); return; }

            for (int i = 0; i < guids.Count; i++)
                AssetImporter.GetAtPath(guids[i].ToPath()).userData = folderInfos[i].folderData.iconNameOrGuid == "" && folderInfos[i].folderData.colorIndex == 0 ? "" : JsonUtility.ToJson(folderInfos[i].folderData);

            for (int i = 0; i < guids.Count; i++)
                AssetImporter.GetAtPath(guids[i].ToPath()).SaveAndReimport();

        }







        void OnLostFocus()
        {
            if (curEvent.holdingAlt && EditorWindow.focusedWindow?.GetType().Name == "ProjectBrowser")
                CloseNextFrameIfNotRefocused();
            else
                Close();

        }

        void CloseNextFrameIfNotRefocused()
        {
            EditorApplication.delayCall += () => { if (EditorWindow.focusedWindow != this) Close(); };
        }










        public void Init(List<string> guids)
        {
            void createData()
            {
                if (VFolders.data) return;

                VFolders.data = ScriptableObject.CreateInstance<VFoldersData>();

                AssetDatabase.CreateAsset(VFolders.data, GetScriptPath("VFolders").GetParentPath().CombinePath("vFolders Data.asset"));

            }
            void createPalette()
            {
                if (VFolders.palette) return;

                VFolders.palette = ScriptableObject.CreateInstance<VFoldersPalette>();

                AssetDatabase.CreateAsset(VFolders.palette, GetScriptPath("VFolders").GetParentPath().CombinePath("vFolders Palette.asset"));

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
            void getInfos()
            {
                folderInfos.Clear();

                foreach (var r in guids)
                    folderInfos.Add(VFolders.GetFolderInfo(r, createDataIfDoesntExist: true));

            }
            void getInitColorsAndIcons()
            {
                initialColorIndexes.Clear();
                initialIconNamesOrGuids.Clear();

                foreach (var r in folderInfos)
                    initialColorIndexes.Add(r.folderData.colorIndex);

                foreach (var r in folderInfos)
                    initialIconNamesOrGuids.Add(r.iconNameOrGuid);

            }


            this.guids = guids;

            RecordUndoOnDatas();

            createData();
            createPalette();
            setSize();
            getInfos();
            getInitColorsAndIcons();

            Undo.undoRedoPerformed -= EditorApplication.RepaintProjectWindow;
            Undo.undoRedoPerformed += EditorApplication.RepaintProjectWindow;

        }

        void OnDestroy()
        {
            RemoveEmptyFolderDatas();
            MarkDatasDirty();
            SaveData();

        }

        public List<string> guids = new List<string>();
        public List<FolderInfo> folderInfos = new List<FolderInfo>();

        public List<int> initialColorIndexes = new List<int>();
        public List<string> initialIconNamesOrGuids = new List<string>();

        static VFoldersPalette palette => VFolders.palette;
        static VFoldersData data => VFolders.data;







        public static void CreateInstance(Vector2 position)
        {
            instance = ScriptableObject.CreateInstance<VFoldersPaletteWindow>();

            instance.ShowPopup();

            instance.position = instance.position.SetPos(position).SetSize(200, 300);
            instance.targetPosition = position;

        }

        public static VFoldersPaletteWindow instance;

    }
}
#endif