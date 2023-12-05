﻿using AO;
using ET.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;

namespace ET
{
    public static class CacheUtils
    {
        public static void Cache(this Entity entity)
        {
            var zone = entity.DomainZone();
            Log.Console($"DBCacheUtils Cache {zone} {entity.GetType().Name}");
            var dbcomp = AOGame.DBConnectApp.GetComponent<DBManagerComponent>().GetZoneDB(zone);
            dbcomp.Save(entity).Coroutine();
            foreach (var item in entity.Components.Values)
            {
                if (item is not IUnitDBComponent)
                {
                    continue;
                }
                dbcomp.Save(item).Coroutine();
            }
        }

        public static void CacheSave(this Entity entity)
        {
            Cache(entity);
        }

        public static async ETTask Query(long id, List<string> collectionNames, List<Entity> result)
        {
            var zone = UnitIdStruct.GetUnitZone(id);
            var dbcomp = AOGame.DBConnectApp.GetComponent<DBManagerComponent>().GetZoneDB(zone);
            await dbcomp.Query(id, collectionNames, result);
        }

        public static async ETTask<T> Query<T>(long id) where T : Entity
        {
            var type = typeof(T);
            var zone = UnitIdStruct.GetUnitZone(id);
            Log.Console($"DBCacheUtils Query {zone} {type.Name}");
            var dbcomp = AOGame.DBConnectApp.GetComponent<DBManagerComponent>().GetZoneDB(zone);
            return await  dbcomp.Query<T>(id, type.Name);
        }

        public static async ETTask<List<T>> Query<T>(int zone, Expression<Func<T, bool>> filter) where T : Entity
        {
            var type = typeof(T);
            Log.Console($"DBCacheUtils Query {zone} {type.Name}");
            var dbcomp = AOGame.DBConnectApp.GetComponent<DBManagerComponent>().GetZoneDB(zone);
            return await dbcomp.Query(filter);
        }
    }
}