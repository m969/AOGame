//using ET;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AO
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public static class AOProcess
//    {
//        public static async ETTask Execute<T>(T process) where T : AFuncProcess
//        {
//            await process.Execute();
//        }

//        public static async ETTask Execute<T, A>(T process, A a) where T : AFuncProcess<A>
//        {
//            await process.Execute(a);
//        }

//        public static async ETTask Execute<T, A1, A2>(T process, A1 a1, A2 a2) where T : AFuncProcess<A1, A2>
//        {
//            await process.Execute(a1, a2);
//        }
//    }
//}
