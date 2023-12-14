using System;

namespace AO
{
    public struct OpenWindowCmd : IExecuteCommand
    {
        public Action ExecuteAction { get; set; }
        public string UIName;
        public object UIObj;
    }

    public static class UIFunctions
    {
        public static T GetReference<T>()
        {
            return default(T);
        }

        public static void Open()
        {
            var cmd = new OpenWindowCmd();
            cmd.ExecuteAction = () =>
            {
                //var ui = Create<UIBase>(cmd.UIObj);
                //ui.Open();
            };
            CmdUtils.Execute(cmd);
        }

        public static void OpenIn()
        {

        }

        public static void Close()
        {

        }

        public static T Create<T>(object uiObj) where T : new()
        {
            var t = new T();
            return t;
        }

        public static void Destroy()
        {

        }

        public static void OpenFrontWindow()
        {

        }

        public static void OpenMiddWindow()
        {

        }

        public static void OpenBackWindow()
        {

        }
    }
}
