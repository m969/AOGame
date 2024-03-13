using AO;
using System;
using System.Collections.Generic;

namespace ET.Server
{
    [FriendOf(typeof(AOIEntity))]
    public static class AOIHelper
    {
        public static long CreateCellId(int x, int y)
        {
            //Log.Console($"x={x} y={y}");
            var x1 = (ulong)Math.Abs(x);
            var y1 = (uint)Math.Abs(y);
            var x2 = (long)(x1 << 32);
            return x2 | y1;
        }

        public static void CalcEnterAndLeaveCell(AOIEntity aoiEntity, int cellX, int cellY, HashSet<long> enterCell, HashSet<long> leaveCell)
        {
            enterCell.Clear();
            leaveCell.Clear();
            int r = (aoiEntity.ViewDistance - 1) / AOIManagerComponent.CellSize + 1;
            int leaveR = r;
            if (aoiEntity.Unit.IsPlayerActor())
            {
                leaveR += 1;
            }

            for (int i = cellX - leaveR; i <= cellX + leaveR; ++i)
            {
                for (int j = cellY - leaveR; j <= cellY + leaveR; ++j)
                {
                    long cellId = CreateCellId(i, j);
                    leaveCell.Add(cellId);

                    if (i > cellX + r || i < cellX - r || j > cellY + r || j < cellY - r)
                    {
                        continue;
                    }

                    enterCell.Add(cellId);
                }
            }
        }
    }
}