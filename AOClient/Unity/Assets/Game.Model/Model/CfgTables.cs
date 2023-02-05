using ET;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;

namespace AO
{
    public class CfgTables
    {
        public static cfg.Tables Tables { get; set; }
        public static cfg.Item.TbItems TbItems => Tables.TbItems;
        public static cfg.Unit.TbUnits TbUnits => Tables.TbUnits;
    }
}
