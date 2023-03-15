namespace AO
{
    using AO;
    using ET;
    using EGamePlay.Combat;
    using EGamePlay;
    using TComp = AO.WorldMapComponent;
    using ET.Server;
    using Unity.Mathematics;
    using SharpCompress.Common;
    using Amazon.Runtime.Internal;

    public static partial class WorldMapComponentSystem
    {
        public class AwakeHandler : AwakeSystem<TComp>
        {
            protected override void Awake(TComp self)
            {
                foreach (var item in CfgTables.TbMaps.DataList)
                {
                    self.TypeMapCfgs.Add(item.Type, item);
                }
            }
        }

        public static int GetMapConfigId(this TComp self, string mapType)
        {
            var mapcfg = self.TypeMapCfgs[mapType];
            return mapcfg.Id;
        }
    }
}