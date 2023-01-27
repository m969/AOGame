using AO;
using Init = ET.Init;
using Entry = ET.Entry;

Init.Start();
ET.Options.Instance.Console = 1;
ET.Options.Instance.LogLevel = 1;

ET.Log.Console($"-> server app start ");
Entry.Start("server");

var rootScene = ET.Root.Instance.Scene;

AOGame.Start(rootScene, "AllInOneServer");

while (true)
{
    Thread.Sleep(1);
    try
    {
        AOGame.Run(rootScene);
        Init.Update();
        Init.LateUpdate();
        Init.FrameFinishUpdate();
    }
    catch (Exception e)
    {
        ET.Log.Error(e);
    }
}
