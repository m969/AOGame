namespace AO
{
    using AO;
    using cfg.Map;
    using ET;
    using System.Collections.Generic;

    public class WorldMapComponent : Entity, IAwake
    {
        public Dictionary<string, MapCfg> TypeMapCfgs = new();

        public Dictionary<int, long> NormalMaps = new();
        public Dictionary<int, List<long>> CopyMaps = new();
    }
}