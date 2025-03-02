#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using UnityEditorInternal;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using static VFolders.Libs.VUtils;
using static VFolders.Libs.VGUI;
using static VFolders.VFoldersPalette;


namespace VFolders
{
    [CustomEditor(typeof(VFoldersPalette))]
    public class VFoldersPaletteEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            void colors()
            {
                var rowRect = ExpandWidthLabelRect(cellSize).SetX(rowsOffsetX).SetWidth(rowWidth);

                void backgroundHovered()
                {
                    if (!rowRect.IsHovered()) return;
                    if (pickingColor) return;
                    if (draggingRow) return;

                    rowRect.Draw(hoveredRowBackground);

                }
                void toggle()
                {
                    var toggleRect = rowRect.SetWidth(16).MoveX(5);

                    var prevEnabled = palette.colorsEnabled;
                    var newEnabled = EditorGUI.Toggle(toggleRect, palette.colorsEnabled);

                    if (prevEnabled != newEnabled)
                        palette.RecordUndo();

                    palette.colorsEnabled = newEnabled;

                    if (prevEnabled != newEnabled)
                        palette.Dirty();

                }
                void crossIcon()
                {
                    var crossIconRect = rowRect.SetX(rowsOffsetX + iconsOffsetX + iconSpacing / 2).SetWidth(iconSize).SetHeightFromMid(iconSize);

                    SetGUIColor(palette.colorsEnabled ? Color.white : disabledRowTint);
                    SetLabelAlignmentCenter();

                    GUI.Label(crossIconRect, EditorGUIUtility.IconContent("CrossIcon"));

                    ResetGUIColor();
                    ResetLabelStyle();

                }
                void color(int i)
                {
                    var cellRect = rowRect.MoveX(iconsOffsetX + (i + 1) * cellSize).SetWidth(cellSize).SetHeightFromMid(cellSize);

                    void backgroundPicking()
                    {
                        if (!pickingColor) return;
                        if (i != pickingColorAtIndex) return;

                        cellRect.DrawWithRoundedCorners(pickingBackground, 2);

                    }
                    void color()
                    {
                        var tint = palette.colorsEnabled ? Color.white : disabledRowTint;

                        var brightness = 1.00f;
                        var outlineColor = Greyscale(.15f, .2f);

                        cellRect.Resize(3).DrawWithRoundedCorners(outlineColor * tint, 4);
                        cellRect.Resize(4).DrawWithRoundedCorners(palette.colors[i].SetAlpha(1) * brightness * tint, 3);
                        cellRect.Resize(4).AddWidthFromRight(-2).DrawCurtainLeft(GUIColors.windowBackground.SetAlpha((1 - palette.colors[i].a) * .5f));

                    }
                    void startPickingColorButton()
                    {
                        if (!palette.colorsEnabled) return;
                        if (!cellRect.IsHovered()) return;
                        if (pickingColor) return;

                        var clicked = GUI.Button(cellRect.Resize(1), "");

                        GUI.Label(cellRect.Resize(.5f), EditorGUIUtility.IconContent("Preset.Context"));


                        if (!clicked) return;

                        colorPicker = OpenColorPicker((c) => { palette.RecordUndo(); palette.Dirty(); palette.colors[i] = c; }, palette.colors[i], showAlpha: true, false);

                        colorPicker.MoveTo(EditorGUIUtility.GUIToScreenPoint(cellRect.Move(-3, 50).position));

                        pickingColor = true;
                        pickingColorAtIndex = i;

                    }
                    void updatePickingColor()
                    {
                        if (!pickingColor) return;

                        EditorApplication.RepaintProjectWindow();

                    }
                    void stopPickingColor()
                    {
                        if (!pickingColor) return;
                        if (colorPicker) return;

                        pickingColor = false;

                    }


                    cellRect.MarkInteractive();

                    backgroundPicking();
                    color();
                    startPickingColorButton();
                    updatePickingColor();
                    stopPickingColor();

                }

                backgroundHovered();
                toggle();
                crossIcon();

                for (int i = 0; i < palette.colors.Count; i++)
                    color(i);

                Space(rowSpacing - 2);

            }
            void icons()
            {
                void row(Rect rowRect, IconRow row)
                {
                    var isLastRow = row == palette.iconRows.Last();
                    var isDraggedRow = row == draggedRow;
                    var spaceForCrossIcon = 0f;


                    void startPickingIcon(int i, Rect cellRect)
                    {
                        iconPicker = OpenObjectPicker<Texture2D>(AssetDatabase.LoadAssetAtPath<Texture2D>(row.customIcons[i].ToPath()), controlID: 123);

                        iconPicker.MoveTo(EditorGUIUtility.GUIToScreenPoint(cellRect.Move(-3, 50).position));

                        pickingIcon = true;
                        pickingIconAtIndex = i;
                        pickingIconAtRow = row;

                    }
                    void updatePickingIcon()
                    {
                        if (!pickingIcon) return;
                        if (pickingIconAtRow != row) return;
                        if (EditorGUIUtility.GetObjectPickerControlID() != 123) return;
                        if (pickingIconAtIndex >= row.customIcons.Count) return; // somehow happens if RecordUndo is used

                        palette.RecordUndo();
                        palette.Dirty();

                        row.customIcons[pickingIconAtIndex] = (EditorGUIUtility.GetObjectPickerObject() as Texture2D).GetPath().ToGuid();

                    }
                    void stopPickingIcon()
                    {
                        if (!pickingIcon) return;
                        if (pickingIconAtRow != row) return;
                        if (iconPicker) return;

                        if (pickingIconAtIndex < row.customIcons.Count)
                            if (row.customIcons[pickingIconAtIndex] == null)
                                row.customIcons.RemoveAt(pickingIconAtIndex);

                        pickingIcon = false;

                    }
                    void dragndrop()
                    {
                        if (!rowRect.IsHovered()) return;

                        if (curEvent.isDragUpdate && DragAndDrop.objectReferences.First() is Texture2D)
                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                        if (!curEvent.isDragPerform) return;
                        if (!(DragAndDrop.objectReferences.Any(r => r is Texture2D))) return;

                        DragAndDrop.AcceptDrag();

                        palette.RecordUndo();
                        palette.Dirty();

                        foreach (var icon in DragAndDrop.objectReferences.Where(r => r is Texture2D))
                            row.customIcons.Add(icon.GetPath().ToGuid());

                    }

                    void calcSpaceForCrossIcon()
                    {
                        if (row == curFirstEnabledRow)
                            spaceForCrossIcon = crossIconAnimationT * cellSize;

                        if (row == crossIconAnimationSourceRow)
                            spaceForCrossIcon = (1 - crossIconAnimationT) * cellSize;

                    }

                    void backgroundHovered()
                    {
                        if (!rowRect.IsHovered()) return;
                        if (pickingColor) return;
                        if (pickingIcon) return;
                        if (draggingRow) return;

                        rowRect.Draw(hoveredRowBackground);

                    }
                    void backgroundDragged()
                    {
                        if (!isDraggedRow) return;

                        rowRect.DrawBlurred(Greyscale(0, .3f), 12);
                        rowRect.Draw(draggedRowBackground);

                    }
                    void toggle()
                    {
                        var prevEnabled = row.enabled;
                        var newEnabled = EditorGUI.Toggle(rowRect.SetWidth(16).MoveX(5), row.enabled);

                        if (prevEnabled != newEnabled)
                            palette.RecordUndo();

                        row.enabled = newEnabled;

                        if (prevEnabled != newEnabled)
                            palette.Dirty();

                    }
                    void addIconButton()
                    {
                        if (!row.isCustom) return;

                        var cellRect = rowRect.MoveX(iconsOffsetX + row.customIcons.Count * cellSize + spaceForCrossIcon).SetWidth(cellSize).SetHeightFromMid(cellSize);



                        SetGUIColor(Greyscale(1, row.enabled ? 1 : .5f));

                        var clicked = GUI.Button(cellRect.Resize(1), "");

                        ResetGUIColor();



                        SetGUIColor(Greyscale(1, row.enabled ? 1 : .5f));
                        SetLabelAlignmentCenter();

                        GUI.Label(cellRect.Resize(1), EditorGUIUtility.IconContent("Toolbar Plus"));

                        ResetLabelStyle();
                        ResetGUIColor();



                        if (!clicked) return;

                        palette.RecordUndo();

                        row.customIcons.Add(null);

                        startPickingIcon(row.customIcons.Count - 1, cellRect);

                    }
                    void icon(int i)
                    {
                        var cellRect = rowRect.MoveX(iconsOffsetX + spaceForCrossIcon + i * cellSize).SetWidth(cellSize).SetHeightFromMid(cellSize);
                        var isCustomIcon = i > row.builtinIcons.Count - 1;

                        void backgroundPicking()
                        {
                            if (!pickingIcon) return;
                            if (row != pickingIconAtRow) return;
                            if (i != pickingIconAtIndex) return;

                            cellRect.Resize(1).DrawWithRoundedCorners(pickingBackground, 2);

                        }
                        void drawBuiltin()
                        {
                            if (isCustomIcon) return;

                            SetLabelAlignmentCenter();
                            SetGUIColor(row.enabled ? Color.white : disabledRowTint);

                            GUI.Label(cellRect.SetSizeFromMid(iconSize), EditorGUIUtility.IconContent(row.builtinIcons[i]));

                            ResetLabelStyle();
                            ResetGUIColor();

                        }
                        void drawCustom()
                        {
                            if (!isCustomIcon) return;
                            if (cellRect.IsHovered()) return;
                            if (!(AssetDatabase.LoadAssetAtPath<Texture2D>(row.customIcons[i - row.builtinIcons.Count].ToPath()) is Texture2D texture)) return;

                            SetGUIColor(row.enabled ? Color.white : disabledRowTint);

                            GUI.DrawTexture(cellRect.SetSizeFromMid(iconSize), texture);

                            ResetGUIColor();

                        }
                        void editCustomButton()
                        {
                            if (!isCustomIcon) return;
                            if (!cellRect.IsHovered()) return;
                            if (pickingIcon) return;

                            var clicked = GUI.Button(cellRect.Resize(1), "");

                            GUI.Label(cellRect.Resize(.5f), EditorGUIUtility.IconContent("Preset.Context"));



                            if (!clicked) return;

                            GenericMenu menu = new GenericMenu();

                            menu.AddItem(new GUIContent("Replace icon"), false, () => { palette.RecordUndo(); palette.Dirty(); startPickingIcon(i, cellRect.MoveY(75)); });
                            menu.AddItem(new GUIContent("Remove icon"), false, () => { palette.RecordUndo(); row.customIcons.RemoveAt(i); palette.Dirty(); });

                            menu.ShowAsContext();



                        }

                        cellRect.MarkInteractive();

                        backgroundPicking();
                        drawBuiltin();
                        drawCustom();
                        editCustomButton();

                    }


                    rowRect.MarkInteractive();

                    updatePickingIcon();
                    stopPickingIcon();
                    dragndrop();

                    calcSpaceForCrossIcon();
                    backgroundHovered();
                    backgroundDragged();
                    toggle();
                    addIconButton();

                    for (int i = 0; i < row.iconCount; i++)
                        icon(i);

                }

                void updateRowsCount()
                {
                    palette.iconRows.RemoveAll(r => r.isEmpty && r != palette.iconRows.Last());

                    if (!palette.iconRows.Last().isEmpty)
                        palette.iconRows.Add(new IconRow());

                }
                void updateRowGapsCount()
                {
                    while (rowGaps.Count < palette.iconRows.Count)
                        rowGaps.Add(0);

                    while (rowGaps.Count > palette.iconRows.Count)
                        rowGaps.RemoveLast();

                }

                void normalRow(int i)
                {
                    Space(rowGaps[i] * (cellSize + rowSpacing));

                    if (i == 0 && lastRect.y != 0)
                        firstRowY = lastRect.y;

                    Space(cellSize + rowSpacing);

                    var rowRect = Rect.zero.SetPos(rowsOffsetX, lastRect.y).SetSize(rowWidth, cellSize);

                    if (curEvent.isRepaint)
                        if (rowRect.IsHovered())
                            hoveredRow = palette.iconRows[i];


                    row(rowRect, palette.iconRows[i]);

                }
                void draggedRow_()
                {
                    if (!draggingRow) return;

                    draggedRowY = (curEvent.mousePosition.y + draggedRowHoldOffset).Clamp(firstRowY, firstRowY + (palette.iconRows.Count - 1) * (cellSize + rowSpacing));

                    var rowRect = Rect.zero.SetPos(rowsOffsetX, draggedRowY).SetSize(rowWidth, cellSize);

                    row(rowRect, draggedRow);

                }
                void crossIcon()
                {
                    if (!palette.iconRows.Any(r => r.enabled)) return;

                    var rect = Rect.zero.SetPos(rowsOffsetX + iconsOffsetX, crossIconY).SetSize(cellSize, cellSize).Resize(iconSpacing / 2);

                    SetLabelAlignmentCenter();

                    GUI.Label(rect, EditorGUIUtility.IconContent("CrossIcon"));

                    ResetLabelStyle();

                }


                updateRowsCount();
                updateRowGapsCount();


                if (curEvent.isRepaint)
                    hoveredRow = null;

                for (int i = 0; i < palette.iconRows.Count; i++)
                    normalRow(i);

                crossIcon();

                draggedRow_();


            }
            void tutor()
            {
                SetGUIEnabled(false);


                GUILayout.Label("Click a color to edit it");

                Space(4);
                GUILayout.Label("Click '+' to add a custom icon");

                Space(4);
                GUILayout.Label("Click a custom icon to replace or remove it");

                Space(4);
                GUILayout.Label("Drag rows to reorder them");


                ResetGUIEnabled();

            }


            Space(15);
            colors();

            Space(15);
            icons();

            Space(25);
            tutor();

            UpdateAnimations();

            UpdateDragging();

            palette.Dirty();

            if (draggingRow || animatingCrossIcon)
                Repaint();

        }

        float iconSize => 18;
        float iconSpacing => 2;
        float cellSize => iconSize + iconSpacing;
        float rowSpacing = 1;
        float rowsOffsetX => 14;
        float iconsOffsetX => 27;

        Color hoveredRowBackground => Greyscale(isDarkTheme ? 1 : 0, .05f);
        Color draggedRowBackground => Greyscale(isDarkTheme ? .3f : .9f);
        Color pickingBackground => new Color(.3f, .5f, .7f, .8f);// Greyscale(1, .17f);
        Color colorOutline => Greyscale(.2f, .5f);
        Color disabledRowTint => Greyscale(1, .45f);

        float rowWidth => cellSize * Mathf.Max(palette.colors.Count, palette.iconRows.Max(r => r.iconCount + 1)) + 55;

        bool pickingColor;
        int pickingColorAtIndex;
        EditorWindow colorPicker;

        bool pickingIcon;
        int pickingIconAtIndex;
        IconRow pickingIconAtRow;
        EditorWindow iconPicker;

        IconRow hoveredRow;

        float firstRowY = 51;






        void UpdateAnimations()
        {
            void calcDeltaTime()
            {
                if (!curEvent.isLayout) return;

                deltaTime = (float)(EditorApplication.timeSinceStartup - lastLayoutTime);

                if (deltaTime > .05f)
                    deltaTime = .0166f;

                lastLayoutTime = EditorApplication.timeSinceStartup;

            }

            void lerpRowGaps()
            {
                if (!curEvent.isLayout) return;

                var lerpSpeed = draggingRow ? 12 : 12321;

                for (int i = 0; i < rowGaps.Count; i++)
                    rowGaps[i] = Lerp(rowGaps[i], draggingRow && i == insertDraggedRowAtIndex ? 1 : 0, lerpSpeed, deltaTime);// todo deltatime

                for (int i = 0; i < rowGaps.Count; i++)
                    if (rowGaps[i].Approx(0))
                        rowGaps[i] = 0;
                    else if (rowGaps[i].Approx(1))
                        rowGaps[i] = 1;


            }

            void lerpCrossIconAnimationT()
            {
                if (!curEvent.isLayout) return;

                var lerpSpeed = 12;

                Lerp(ref crossIconAnimationT, 1, lerpSpeed, deltaTime);

            }
            void startCrossIconAnimation()
            {
                if (prevFirstEnabledRow == null) { prevFirstEnabledRow = curFirstEnabledRow; return; }
                if (prevFirstEnabledRow == curFirstEnabledRow) return;

                crossIconAnimationT = 0;
                crossIconAnimationSourceRow = prevFirstEnabledRow;

                prevFirstEnabledRow = curFirstEnabledRow;

            }
            void stopCrossIconAnimation()
            {
                if (!crossIconAnimationT.Approx(1)) return;

                crossIconAnimationT = 1;
                crossIconAnimationSourceRow = null;

            }
            void calcCrossIconY()
            {
                var indexOfFirstEnabled = palette.iconRows.IndexOfFirst(r => r.enabled);
                var yOfFirstEnabled = firstRowY + indexOfFirstEnabled * (cellSize + rowSpacing);
                for (int i = 0; i < indexOfFirstEnabled + 1; i++)
                    yOfFirstEnabled += rowGaps[i] * (cellSize + rowSpacing);


                var indexOfSourceRow = palette.iconRows.IndexOf(crossIconAnimationSourceRow);
                var yOfSourceRow = firstRowY + indexOfSourceRow * (cellSize + rowSpacing);
                for (int i = 0; i < indexOfSourceRow + 1; i++)
                    yOfSourceRow += rowGaps[i] * (cellSize + rowSpacing);

                if (crossIconAnimationSourceRow == draggedRow)
                    yOfSourceRow = draggedRowY;


                crossIconY = Lerp(yOfSourceRow, yOfFirstEnabled, crossIconAnimationT);

                if (indexOfFirstEnabled == indexOfSourceRow) { crossIconAnimationT = 1; }

            }


            calcDeltaTime();

            lerpRowGaps();

            lerpCrossIconAnimationT();
            startCrossIconAnimation();
            stopCrossIconAnimation();
            calcCrossIconY();

        }

        List<float> rowGaps = new List<float>();

        float deltaTime;
        double lastLayoutTime;

        float crossIconY = 51;
        float crossIconAnimationT = 1;
        IconRow crossIconAnimationSourceRow;
        bool animatingCrossIcon => crossIconAnimationT != 1;

        IconRow prevFirstEnabledRow;
        IconRow curFirstEnabledRow => palette.iconRows.FirstOrDefault(r => r.enabled);






        void UpdateDragging()
        {
            void startDragging()
            {
                if (draggingRow) return;
                if (!curEvent.isMouseDrag) return;
                if (hoveredRow == null) return;
                if (hoveredRow == palette.iconRows.Last()) return;

                palette.RecordUndo();

                draggingRow = true;
                draggedRow = hoveredRow;
                draggingRowFromIndex = palette.iconRows.IndexOf(hoveredRow);
                draggedRowHoldOffset = firstRowY + draggingRowFromIndex * (cellSize + rowSpacing) - curEvent.mousePosition.y;

                palette.iconRows.Remove(hoveredRow);
                rowGaps[draggingRowFromIndex] = 1;

            }
            void updateDragging()
            {
                if (!draggingRow) return;

                insertDraggedRowAtIndex = ((curEvent.mousePosition.y - firstRowY) / (cellSize + rowSpacing)).FloorToInt().Clamp(0, palette.iconRows.Count - 1);

                EditorGUIUtility.hotControl = EditorGUIUtility.GetControlID(FocusType.Passive);

            }
            void stopDragging()
            {
                if (!draggingRow) return;
                if (!curEvent.isMouseUp) return;

                palette.RecordUndo();
                palette.Dirty();

                palette.iconRows.AddAt(draggedRow, insertDraggedRowAtIndex);

                rowGaps[insertDraggedRowAtIndex] = 0;

                draggingRow = false;
                draggedRow = null;

                EditorGUIUtility.hotControl = 0;

            }


            startDragging();
            updateDragging();
            stopDragging();

        }

        IconRow draggedRow;
        bool draggingRow;
        int draggingRowFromIndex;
        float draggedRowHoldOffset;
        float draggedRowY;
        int insertDraggedRowAtIndex;






        VFoldersPalette palette => target as VFoldersPalette;

    }
}
#endif