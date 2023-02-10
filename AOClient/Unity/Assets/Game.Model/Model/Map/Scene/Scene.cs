using System.Diagnostics;

namespace ET
{
    [EnableMethod]
    //[DebuggerDisplay("ViewName,nq")]
    [ChildOf]
    public sealed class Scene: Entity, IAwake<string>
    {
        public string Type
        {
            get;
            set;
        }

#if UNITY
        /// <summary>
        /// 本地当前场景
        /// </summary>
        public static Scene CurrentScene { get; set; }
#endif
    }
}