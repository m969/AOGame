using System.Diagnostics;
using Unity.Mathematics;

namespace ET
{
    [EnableMethod]
    //[DebuggerDisplay("ViewName,nq")]
    [ChildOf]
    public sealed class Scene: Entity, IAwake<string>, AO.IMapUnit
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int ConfigId { get; set; }
        public float3 Position { get; set; }

#if UNITY
        /// <summary>
        /// 本地当前场景
        /// </summary>
        public static Scene CurrentScene { get; set; }
#endif
    }
}