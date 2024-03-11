using System;

namespace AO
{
    public class OpenWindowCmd : AExecuteCommand
    {
        public IUIWindow Window;
    }

    /// <summary>
    /// UI框架开放方法类
    /// </summary>
    public static class UIFunctions
    {
        public static T GetOrAddWindow<T>()
        {
            return default(T);
        }

        public static T Open<T>(Action<T> beforeOpen = null) where T : class, IUIWindow, new()
        {
            var cmd = new OpenWindowCmd();
            cmd.Window = Create<T>(null);
            cmd.ExecuteAction = () =>
            {
                beforeOpen?.Invoke((T)cmd.Window);
                //cmd.Window.GObject.Visible = true;
                cmd.Window.OnPopUp();
                cmd.Window.OnOpen();
            };
            AOCmd.Execute(cmd);
            return cmd.Window as T;
        }

        public static void OpenIn()
        {

        }

        public static void Close()
        {

        }

        public static T Create<T>(object uiObj) where T : class, IUIWindow, new()
        {
            var t = new T();
            t.OnCreate(null);
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
