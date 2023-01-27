//using ET.EventType;
//using System;

//namespace ET
//{
//    public static class OuterEntry
//    {
//        public static void Start()
//        {
//            Log.Debug("OuterEntry Start");
//            var types = AssemblyHelper.GetAssemblyTypes(typeof(OuterEntry).Assembly);
//            foreach (var item in types)
//            {
//                CodeLoader.Instance.types.Add(item.Key, item.Value);
//            }
//        }
//    }
//}