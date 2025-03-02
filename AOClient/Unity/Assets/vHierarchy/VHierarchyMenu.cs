#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static VHierarchy.Libs.VUtils;
using static VHierarchy.Libs.VGUI;



namespace VHierarchy
{
    class VHierarchyMenu
    {

        public static bool componentMinimapEnabled { get => EditorPrefs.GetBool("vHierarchy-componentMinimapEnabled", false); set => EditorPrefs.SetBool("vHierarchy-componentMinimapEnabled", value); }
        public static bool hierarchyLinesEnabled { get => EditorPrefs.GetBool("vHierarchy-hierarchyLinesEnabled", false); set => EditorPrefs.SetBool("vHierarchy-hierarchyLinesEnabled", value); }
        public static bool minimalModeEnabled { get => EditorPrefs.GetBool("vHierarchy-minimalModeEnabled", false); set => EditorPrefs.SetBool("vHierarchy-minimalModeEnabled", value); }
        public static bool zebraStripingEnabled { get => EditorPrefs.GetBool("vHierarchy-zebraStripingEnabled", false); set => EditorPrefs.SetBool("vHierarchy-zebraStripingEnabled", value); }
        public static bool activationToggleEnabled { get => EditorPrefs.GetBool("vHierarchy-acctivationToggleEnabled", false); set => EditorPrefs.SetBool("vHierarchy-acctivationToggleEnabled", value); }
        public static bool collapseAllButtonEnabled { get => EditorPrefs.GetBool("vHierarchy-collapseAllButtonEnabled", false); set => EditorPrefs.SetBool("vHierarchy-collapseAllButtonEnabled", value); }
        public static bool editLightingButtonEnabled { get => EditorPrefs.GetBool("vHierarchy-editLightingButtonEnabled", false); set => EditorPrefs.SetBool("vHierarchy-editLightingButtonEnabled", value); }

        public static bool toggleActiveEnabled { get => EditorPrefs.GetBool("vHierarchy-toggleActiveEnabled", true); set => EditorPrefs.SetBool("vHierarchy-toggleActiveEnabled", value); }
        public static bool focusEnabled { get => EditorPrefs.GetBool("vHierarchy-focusEnabled", true); set => EditorPrefs.SetBool("vHierarchy-focusEnabled", value); }
        public static bool deleteEnabled { get => EditorPrefs.GetBool("vHierarchy-deleteEnabled", true); set => EditorPrefs.SetBool("vHierarchy-deleteEnabled", value); }
        public static bool toggleExpandedEnabled { get => EditorPrefs.GetBool("vHierarchy-toggleExpandedEnabled", true); set => EditorPrefs.SetBool("vHierarchy-toggleExpandedEnabled", value); }
        public static bool collapseEverythingElseEnabled { get => EditorPrefs.GetBool("vHierarchy-collapseEverythingElseEnabled", true); set => EditorPrefs.SetBool("vHierarchy-collapseEverythingElseEnabled", value); }
        public static bool collapseEverythingEnabled { get => EditorPrefs.GetBool("vHierarchy-collapseEverythingEnabled", true); set => EditorPrefs.SetBool("vHierarchy-collapseEverythingEnabled", value); }

        public static bool pluginDisabled { get => EditorPrefs.GetBool("vHierarchy-pluginDisabled", false); set => EditorPrefs.SetBool("vHierarchy-pluginDisabled", value); }




        const string dir = "Tools/vHierarchy/";

        const string componentMinimap = dir + "Component minimap";
        const string hierarchyLines = dir + "Hierarchy lines";
        const string minimalMode = dir + "Minimal mode";
        const string zebraStriping = dir + "Zebra striping";
        const string activationToggle = dir + "Activation toggle";
        const string collapseAllButton = dir + "Collapse All button";
        const string editLightingButton = dir + "Edit Lighting button";

        const string toggleActive = dir + "A to toggle active";
        const string focus = dir + "F to focus";
        const string delete = dir + "X to delete";
        const string toggleExpanded = dir + "E to expand or collapse";
        const string collapseEverythingElse = dir + "Shift-E to collapse everything else";
        const string collapseEverything = dir + "Ctrl-Shift-E to collapse everything";

        const string disablePlugin = dir + "Disable vHierarchy";






        [MenuItem(dir + "Features", false, 1)] static void daasddsas() { }
        [MenuItem(dir + "Features", true, 1)] static bool dadsdasas123() => false;

        [MenuItem(componentMinimap, false, 2)] static void daadsdsadasdadsas() { componentMinimapEnabled = !componentMinimapEnabled; EditorApplication.RepaintHierarchyWindow(); }
        [MenuItem(componentMinimap, true, 2)] static bool dadsadasddasadsas() { Menu.SetChecked(componentMinimap, componentMinimapEnabled); return !pluginDisabled; }

        [MenuItem(hierarchyLines, false, 3)] static void dadsadadsadadasss() { hierarchyLinesEnabled = !hierarchyLinesEnabled; EditorApplication.RepaintHierarchyWindow(); }
        [MenuItem(hierarchyLines, true, 3)] static bool dadsaddasdasaasddsas() { Menu.SetChecked(hierarchyLines, hierarchyLinesEnabled); return !pluginDisabled; }

        [MenuItem(minimalMode, false, 4)] static void dadsadadasdsdasadadasss() { minimalModeEnabled = !minimalModeEnabled; EditorApplication.RepaintHierarchyWindow(); }
        [MenuItem(minimalMode, true, 4)] static bool dadsaddadsasdadsasaasddsas() { Menu.SetChecked(minimalMode, minimalModeEnabled); return !pluginDisabled; }

        [MenuItem(zebraStriping, false, 5)] static void dadsadadadssadsadass() { zebraStripingEnabled = !zebraStripingEnabled; EditorApplication.RepaintHierarchyWindow(); }
        [MenuItem(zebraStriping, true, 5)] static bool dadsaddadaadsssadsaasddsas() { Menu.SetChecked(zebraStriping, zebraStripingEnabled); return !pluginDisabled; }

        [MenuItem(activationToggle, false, 6)] static void daadsdsadadsasdadsas() { activationToggleEnabled = !activationToggleEnabled; EditorApplication.RepaintHierarchyWindow(); }
        [MenuItem(activationToggle, true, 6)] static bool dadsadasdsaddasadsas() { Menu.SetChecked(activationToggle, activationToggleEnabled); return !pluginDisabled; }

        [MenuItem(collapseAllButton, false, 7)] static void daadsdsadadsadadsas() { collapseAllButtonEnabled = !collapseAllButtonEnabled; EditorApplication.RepaintHierarchyWindow(); }
        [MenuItem(collapseAllButton, true, 7)] static bool dadsadasdsaddasdsas() { Menu.SetChecked(collapseAllButton, collapseAllButtonEnabled); return !pluginDisabled; }

        [MenuItem(editLightingButton, false, 8)] static void daadsdsasdadadsadadsas() { editLightingButtonEnabled = !editLightingButtonEnabled; EditorApplication.RepaintHierarchyWindow(); }
        [MenuItem(editLightingButton, true, 8)] static bool dadsadasdsadsaddasdsas() { Menu.SetChecked(editLightingButton, editLightingButtonEnabled); return !pluginDisabled; }




        [MenuItem(dir + "Shortcuts", false, 101)] static void dadsas() { }
        [MenuItem(dir + "Shortcuts", true, 101)] static bool dadsas123() => false;

        [MenuItem(toggleActive, false, 102)] static void dadsadadsas() => toggleActiveEnabled = !toggleActiveEnabled;
        [MenuItem(toggleActive, true, 102)] static bool dadsaddasadsas() { Menu.SetChecked(toggleActive, toggleActiveEnabled); return !pluginDisabled; }

        [MenuItem(focus, false, 103)] static void dadsadasdadsas() => focusEnabled = !focusEnabled;
        [MenuItem(focus, true, 103)] static bool dadsadsaddasadsas() { Menu.SetChecked(focus, focusEnabled); return !pluginDisabled; }

        [MenuItem(delete, false, 104)] static void dadsadsadasdadsas() => deleteEnabled = !deleteEnabled;
        [MenuItem(delete, true, 104)] static bool dadsaddsasaddasadsas() { Menu.SetChecked(delete, deleteEnabled); return !pluginDisabled; }

        [MenuItem(toggleExpanded, false, 105)] static void dadsadsadasdsadadsas() => toggleExpandedEnabled = !toggleExpandedEnabled;
        [MenuItem(toggleExpanded, true, 105)] static bool dadsaddsasadadsdasadsas() { Menu.SetChecked(toggleExpanded, toggleExpandedEnabled); return !pluginDisabled; }

        [MenuItem(collapseEverythingElse, false, 106)] static void dadsadsasdadasdsadadsas() => collapseEverythingElseEnabled = !collapseEverythingElseEnabled;
        [MenuItem(collapseEverythingElse, true, 106)] static bool dadsaddsdasasadadsdasadsas() { Menu.SetChecked(collapseEverythingElse, collapseEverythingElseEnabled); return !pluginDisabled; }

        [MenuItem(collapseEverything, false, 107)] static void dadsadsdasadasdsadadsas() => collapseEverythingEnabled = !collapseEverythingEnabled;
        [MenuItem(collapseEverything, true, 107)] static bool dadsaddssdaasadadsdasadsas() { Menu.SetChecked(collapseEverything, collapseEverythingEnabled); return !pluginDisabled; }




        [MenuItem(dir + "More", false, 1001)] static void daasadsddsas() { }
        [MenuItem(dir + "More", true, 1001)] static bool dadsadsdasas123() => false;

        [MenuItem(dir + "Join our Discord", false, 1002)]
        static void dadasdsas() => Application.OpenURL("https://discord.gg/4dG9KsbspG");

        [MenuItem(dir + "Deals ending soon/Get vInspector 2 at 50% off", false, 1003)]
        static void dadadssadsas() => Application.OpenURL("https://assetstore.unity.com/packages/slug/252297?aid=1100lGLBn&pubref=deal50menu");

        [MenuItem(dir + "Deals ending soon/Get vFolders 2 at 50% off", false, 1004)]
        static void dadadssadasdsas() => Application.OpenURL("https://assetstore.unity.com/packages/slug/263644?aid=1100lGLBn&pubref=deal50menu");

        [MenuItem(dir + "Deals ending soon/Get vTabs 2 at 50% off", false, 1005)]
        static void dadadadsssadsas() => Application.OpenURL("https://assetstore.unity.com/packages/slug/263645?aid=1100lGLBn&pubref=deal50menu");

        [MenuItem(dir + "Deals ending soon/Get vFavorites 2 at 50% off", false, 1006)]
        static void dadadadsssadsadsas() => Application.OpenURL("https://assetstore.unity.com/packages/slug/263643?aid=1100lGLBn&pubref=deal50menu");





        [MenuItem(disablePlugin, false, 10001)] static void dadsadsdasadasdasdsadadsas() { pluginDisabled = !pluginDisabled; UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation(); }
        [MenuItem(disablePlugin, true, 10001)] static bool dadsaddssdaasadsadadsdasadsas() { Menu.SetChecked(disablePlugin, pluginDisabled); return true; }




        // [MenuItem(dir + "Clear cache", false, 10001)]
        // static void dassaadsdc() => VHierarchyCache.Clear();

    }
}
#endif