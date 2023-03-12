using AO;
using ET.Server;
using System.Collections.Generic;
using System.IO;

namespace ET
{
    public static class DBCacheUtils
    {
        public static void Cache(this Entity entity)
        {
            var zone = AOGame.DBConnectApp.Zone;
            Log.Console($"DBCacheUtils {zone} {entity.GetType().Name}");
            AOGame.DBConnectApp.GetComponent<DBManagerComponent>().GetZoneDB(zone).Save(entity).Coroutine();
        }
    }
}