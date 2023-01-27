//using ET.EventType;
//using System;

//namespace ET
//{
//    public static class HotfixEntry
//    {
//        public static void Start()
//        {
//            StartAsync().Coroutine();
//        }
        
//        private static async ETTask StartAsync()
//        {
//            var types = AssemblyHelper.GetAssemblyTypes(typeof (Game).Assembly, typeof(Entry).Assembly, typeof(HotfixEntry).Assembly);
//            foreach (var item in types)
//            {
//                CodeLoader.Instance.types.Add(item.Key, item.Value);
//            }

//            EventSystem.Instance.Add(CodeLoader.Instance.types);

//            WinPeriod.Init();
            
//            MongoHelper.Init();
//            ProtobufHelper.Init();
            
//            Game.AddSingleton<NetServices>();
//            Game.AddSingleton<Root>();
//            //await Game.AddSingleton<ConfigComponent>().LoadAsync();

//            await EventSystem.Instance.PublishAsync(Root.Instance.Scene, new EventType.EntryEvent1());
//            await EventSystem.Instance.PublishAsync(Root.Instance.Scene, new EventType.EntryEvent2());
//            await EventSystem.Instance.PublishAsync(Root.Instance.Scene, new EventType.EntryEvent3());
//        }
//    }
//}