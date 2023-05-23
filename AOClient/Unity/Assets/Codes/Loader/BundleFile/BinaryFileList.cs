using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Unity.Plastic.Newtonsoft.Json;

namespace BundleFile
{
    [Serializable]
    public class BinaryFileData
    {
        public string FileName;
        public long Offset;
    }

    public class BinaryFileList
    {
        public List<BinaryFileData> FileList;
        //[JsonIgnore]
        public Dictionary<string, long> File2OffsetDict { get; set; }
    }
}
