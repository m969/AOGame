using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using ITnnovative.AOP.Processing.Editor;

namespace ET
{
    [InitializeOnLoad]
    public static class MyEditorToolbar
    {
        private static readonly Type kToolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
        private static ScriptableObject sCurrentToolbar;

        static MyEditorToolbar()
        {
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            if (sCurrentToolbar == null)
            {
                UnityEngine.Object[] toolbars = Resources.FindObjectsOfTypeAll(kToolbarType);
                sCurrentToolbar = toolbars.Length > 0 ? (ScriptableObject)toolbars[0] : null;
                if (sCurrentToolbar != null)
                {
                    FieldInfo root = sCurrentToolbar.GetType().GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);
                    VisualElement concreteRoot = root.GetValue(sCurrentToolbar) as VisualElement;
                    VisualElement toolbarZone = concreteRoot.Q("ToolbarZoneRightAlign");
                    VisualElement parent = new VisualElement()
                    {
                        style = {
flexGrow = 1,
flexDirection = FlexDirection.Row,
}
                    };
                    IMGUIContainer container = new IMGUIContainer();
                    container.onGUIHandler += OnGuiBody;
                    parent.Add(container);
                    toolbarZone.Add(parent);
                }
            }
        }

        private static void OnGuiBody()
        {
            //自定义按钮加在此处
            GUILayout.BeginHorizontal();
            if (EditorApplication.isPlaying)
            {
                if (GUILayout.Button(new GUIContent("热重载", EditorGUIUtility.FindTexture("PlayButton"))))
                {
                    //BuildEditor.BuildHotfix();
                    //// 热重载代码
                    //CodeLoader.Instance.LoadHotfix();
                    //EventSystem.Instance.Load();
                    //Log.Debug("hot reload success!");
                }
            }
            else
            {
                if (GUILayout.Button(new GUIContent("编译启动", EditorGUIUtility.FindTexture("PlayButton"))))
                {
                    //BuildEditor.BuildModelAndHotfix();
                    CodeProcessor.WeaveEditorAssemblies();
                    EditorApplication.isPlaying = true;
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}