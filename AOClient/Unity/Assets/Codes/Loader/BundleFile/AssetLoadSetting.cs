using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BundleFile
{
    [CreateAssetMenu(fileName = "AssetLoadSetting", menuName = "BundleFile/AssetLoadSetting")]
    public class AssetLoadSetting : ScriptableObject
    {
        public bool StreamingBinaryData;


        private static AssetLoadSetting instance;
        public static AssetLoadSetting Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                instance = Resources.Load<AssetLoadSetting>("AssetLoadSetting");
                return instance;
            }
        }
    }
}
