using AO;
using Init = ET.Init;
using Entry = ET.Entry;

Init.Start();
ET.Options.Instance.Console = 1;
ET.Options.Instance.LogLevel = 4;

ET.Log.Console($"-> server app start ");
Entry.Start("server");

AOGame.Start(ET.ETRoot.Root, ET.Options.Instance.AppType);

while (true)
{
    Thread.Sleep(1);
    try
    {
        AOGame.Run();
        Init.Update();
        Init.LateUpdate();
        Init.FrameFinishUpdate();
    }
    catch (Exception e)
    {
        ET.Log.Error(e);
    }
}
