using AO;
using ET.EventType;
using System;

namespace ET
{
    namespace EventType
    {
        public struct EntryEvent1
        {
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
        
        public static void Start()
        {
            StartAsync().Coroutine();
        }
        
        private static async ETTask StartAsync()
        {
            WinPeriod.Init();

            MongoHelper.Init();
            ProtobufHelper.Init();

            Game.AddSingleton<NetServices>();
            Game.AddSingleton<UnitGroup>();
            Game.AddSingleton<CameraGroup>();
            AOGame.Start(ETRoot.Root);
            //await Game.AddSingleton<ConfigComponent>().LoadAsync();

            await EventSystem.Instance.PublishAsync(ETRoot.Root, new EventType.EntryEvent1());
            await EventSystem.Instance.PublishAsync(ETRoot.Root, new EventType.EntryEvent2());
            await EventSystem.Instance.PublishAsync(ETRoot.Root, new EventType.EntryEvent3());
        }
    }
}