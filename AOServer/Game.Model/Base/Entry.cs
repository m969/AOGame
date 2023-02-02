namespace ET
{
    namespace EventType
    {
        public struct EntryEvent1
        {
            public string args;
        }   
        
        public struct EntryEvent2
        {
        } 
        
        public struct EntryEvent3
        {
        } 
    }
    
    public static class Entry
    {
        public static void Init()
        {
            
        }
        
        public static void Start(string appType)
        {
            StartAsync(appType).Coroutine();
        }
        
        private static async ETTask StartAsync(string appType)
        {
            WinPeriod.Init();
            
            MongoHelper.Init();
            ProtobufHelper.Init();
            
            Game.AddSingleton<NetServices>();
            Game.AddSingleton<Root>();
            //await Game.AddSingleton<ConfigComponent>().LoadAsync();

            await EventSystem.Instance.PublishAsync(Root.Instance.Scene, new EventType.EntryEvent1() { args = appType });
            await EventSystem.Instance.PublishAsync(Root.Instance.Scene, new EventType.EntryEvent2());
            await EventSystem.Instance.PublishAsync(Root.Instance.Scene, new EventType.EntryEvent3());
        }
    }
}