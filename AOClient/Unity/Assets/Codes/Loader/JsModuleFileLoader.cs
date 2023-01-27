using System.IO;

namespace ET
{
    public class JsModuleFileLoader : Puerts.ILoader, Puerts.IModuleChecker
    {
        private string root = string.Empty;
        public string bootstrapScript = string.Empty;

        public JsModuleFileLoader()
        {
        }

        public JsModuleFileLoader(string root)
        {
            this.root = root;
        }

        private string PathToUse(string filepath)
        {
            if (!filepath.Contains("puerts/"))
            {
                filepath = Path.GetFileName(filepath);
            }
            return
            // .cjs asset is only supported in unity2018+
#if UNITY_2018_1_OR_NEWER
            filepath.EndsWith(".cjs") || filepath.EndsWith(".mjs") ?
                filepath.Substring(0, filepath.Length - 4) :
#endif
                filepath;
        }

        public bool FileExists(string filepath)
        {
#if PUERTS_GENERAL
            return File.Exists(Path.Combine(root, filepath));
#else 
            string pathToUse = this.PathToUse(filepath);
            bool exist = UnityEngine.Resources.Load(pathToUse) != null;
#if !PUERTS_GENERAL && UNITY_EDITOR && !UNITY_2018_1_OR_NEWER
            if (!exist) 
            {
                UnityEngine.Debug.LogWarning("¡¾Puerts¡¿unity 2018- is using, if you found some js is not exist, rename *.cjs,*.mjs in the resources dir with *.cjs.txt,*.mjs.txt");
            }
#endif
            return exist;
#endif
        }

        public string ReadFile(string filepath, out string debugpath)
        {
#if PUERTS_GENERAL
            debugpath = Path.Combine(root, filepath);
            return File.ReadAllText(debugpath);
#else 
            string pathToUse = this.PathToUse(filepath);
            UnityEngine.TextAsset file = (UnityEngine.TextAsset)UnityEngine.Resources.Load(pathToUse);

            debugpath = System.IO.Path.Combine(root, filepath);
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            debugpath = debugpath.Replace("/", "\\");
#endif
            if (pathToUse.EndsWith("bootstrap"))
            {
                var scripts = bootstrapScript + file.text;
                //UnityEngine.Debug.Log(scripts);
                return scripts;
            }
            return file == null ? null : file.text;
#endif
        }


        public bool IsESM(string filepath)
        {
            return filepath.Length >= 4 && filepath.EndsWith(".mjs");
        }
    }
}
