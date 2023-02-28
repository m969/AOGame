using System;
using ET;
using GameUtils;
using Unity.Mathematics;
using TComp = AO.UnitLevelComponent;

namespace AO
{
    public static class UnitLevelComponentSystem
    {
        public static void Level_Changed(this TComp self)
        {
            Log.Console($"UnitLevelComponentSystem Level_Changed {self} {self.Level}");
        }
    }
}