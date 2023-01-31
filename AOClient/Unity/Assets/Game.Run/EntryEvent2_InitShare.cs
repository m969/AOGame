namespace ET
{
    using ET;
    using AO;
    using System.IO;
    using Puerts;
    using UnityEngine;
    using SimpleJSON;

    [Event(SceneType.Process)]
    public class EntryEvent2_InitShare: AEvent<EventType.EntryEvent2>
    {
        protected override async ETTask Run(Scene scene, EventType.EntryEvent2 args)
        {
            Log.Debug("EntryEvent2_InitShare Run");

            AOGame.Start(Root.Instance.Scene);

            await TimerComponent.Instance.WaitAsync(1000);

            var tables = new cfg.Tables(LoadByteBuf);

            Log.Console($"{tables.TbItem.Get(10000).Desc}");

            await ETTask.CompletedTask;
        }

        private static JSONNode LoadByteBuf(string file)
        {
            return JSON.Parse(File.ReadAllText(Application.dataPath + "/Bundles/ExcelTablesData/" + file + ".json", System.Text.Encoding.UTF8));
        }
    }
}