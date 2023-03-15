using AssetFile;
using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace BundleFile
{
    public class FilePullTask
    {
        public UnityWebRequest WebRequest;
        public string FileSavePath;
        public long FileSize;
    }

    public class AssetTextTask
    {
        public UnityWebRequestAsyncOperation WebRequestOperation;
        public string FileText;

        public IEnumerator GetAssetText(string url)
        {
            FileText = string.Empty;
            var webRequest = UnityWebRequest.Get(url);
            WebRequestOperation = webRequest.SendWebRequest();
            while (!WebRequestOperation.isDone)
            {
                yield return null;
            }
            FileText = WebRequestOperation.webRequest.downloadHandler.text;
        }
    }

    public class ArtFilesUpdater : MonoBehaviour
    {
        public const string Bundles_art_files = "Bundles/_art_files";
        public string CdnVersionTxtUrl = "http://127.0.0.1:8080/Bundles/{0}/_art_files_version.txt";
        public string NextScene = "Init";
        [Space(10)]
        public Text InfoText;
        public Button ContinueBtn;
        public Button CancelBtn;
        public Slider ProgressSlider;

        public string CdnPlatformFolderPath { get; set; }
        public string PersistentArtVersionTxtPath { get; set; }
        public string LocalFilesListTxtPath { get; set; }
        public int CdnArtVersion { get; set; } = 0;
        public int LocalArtVersion { get; set; } = 0;
        public int StreamingArtVersion { get; set; } = 0;

        /// <summary> 资源下载队列 </summary>
        public Queue<FilePullTask> FilePullQueue { get; set; } = new();
        /// <summary> 下载中的资源 </summary>
        public List<FilePullTask> FileDownloadingList { get; set; } = new();
        /// <summary> 下载完成的资源 </summary>
        public List<FilePullTask> FileAddedList { get; set; } = new();
        public bool ContinueDownload { get; set; }
        public AssetTextTask GetTextTask { get; set; } = new();


        public string InfoLog
        {
            set
            {
                if (InfoText != null)
                {
                    InfoText.text = value;
                }
            }
        }

        WaitForSeconds waitForSeconds = new WaitForSeconds(0.1f);

        private void SetBtnsActive(bool value)
        {
            CancelBtn.gameObject.SetActive(value);
            ContinueBtn.gameObject.SetActive(value);
        }

        IEnumerator Start()
        {
            SetBtnsActive(false);

            if (Define.IsEditor)
            {
                StartCoroutine(UpdateComplete());
                yield break;
            }

            const string filesVersionTxt = "_art_files_version.txt";
            PersistentArtVersionTxtPath = Application.persistentDataPath + $"/{Bundles_art_files}_version.txt";
            var platformName = "Window";
            if (Application.platform == RuntimePlatform.Android) platformName = "Android";
            if (Application.platform == RuntimePlatform.IPhonePlayer) platformName = "iOS";
#if UNITY_EDITOR
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android) platformName = "Android";
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS) platformName = "iOS";
#endif
            CdnVersionTxtUrl = string.Format(CdnVersionTxtUrl, platformName);
            Debug.Log(CdnVersionTxtUrl);
            CdnPlatformFolderPath = CdnVersionTxtUrl.Replace($"/{filesVersionTxt}", "");

            // 获取包内Streaming资源版本
            var streamingArtVersionTxtPath = Application.streamingAssetsPath + $"/{Bundles_art_files}_version.txt";
            yield return StartCoroutine(GetTextTask.GetAssetText(streamingArtVersionTxtPath));
            var streamingVersionStr = GetTextTask.FileText.Trim();
            StreamingArtVersion = int.Parse(streamingVersionStr);

            // 获取本地的资源版本
            if (File.Exists(PersistentArtVersionTxtPath))
            {
                yield return StartCoroutine(GetTextTask.GetAssetText(PersistentArtVersionTxtPath));
                var versionStr = GetTextTask.FileText.Trim();
                LocalArtVersion = int.Parse(versionStr);
            }
            else
            {
                LocalArtVersion = StreamingArtVersion;
            }

            // 获取cdn上的资源版本
            InfoLog = "正在检查资源版本";
            yield return StartCoroutine(GetTextTask.GetAssetText(CdnVersionTxtUrl));
            var fileContent = GetTextTask.FileText;
            CdnArtVersion = int.Parse(fileContent.Trim());

            // 如果cdn和本地的资源版本一样就直接更新完毕
            if (CdnArtVersion == LocalArtVersion)
            {
                InfoLog = "本地资源已是最新";
                StartCoroutine(UpdateComplete());
                yield break;
            }

            if (!Directory.Exists(Application.persistentDataPath + "/Bundles"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Bundles");
            }
            if (!Directory.Exists(Application.persistentDataPath + "/Bundles/artdata"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/Bundles/artdata");
            }

            // cdn和本地的资源版本不一样，开始更新资源文件
            // 更新最新版本的资源列表txt
            InfoLog = $"最新资源版本{CdnArtVersion}(本地资源版本{LocalArtVersion})，正在获取新增资源信息";
            var filesListTxt = Application.persistentDataPath + $"/{Bundles_art_files}_{CdnArtVersion}.txt";
            if (!File.Exists(filesListTxt))
            {
                var cdnFilesListTxtUrl = CdnPlatformFolderPath + $"/_art_files_{CdnArtVersion}.txt";
                yield return StartCoroutine(GetTextTask.GetAssetText(cdnFilesListTxtUrl));
                fileContent = GetTextTask.FileText;
                File.WriteAllText(filesListTxt, fileContent);
            }

            // 下载版本资源新增列表txt
            var webDict = new Dictionary<int, UnityWebRequest>();
            for (int i = LocalArtVersion + 1; i <= CdnArtVersion; i++)
            {
                var versionAddedFilesTxt = Application.persistentDataPath + $"/{Bundles_art_files}_{i}_added.txt";
                if (!File.Exists(versionAddedFilesTxt))
                {
                    var cdnVersionAddedFilesTxt = CdnPlatformFolderPath + $"/artdata_{i}_added/_art_files_added.txt";
                    var request = UnityWebRequest.Get(cdnVersionAddedFilesTxt);
                    webDict.Add(i, request);
                    var op = request.SendWebRequest();
                }
            }

            // 等待版本资源新增列表txt下载完
            var webList = webDict.Values.ToList();
            while (webList.Exists(x => !x.isDone))
            {
                yield return waitForSeconds;
            }

            // 遍历检出本地没有的资源文件放到下载队列
            long size = 0;
            foreach (var item in webDict.Values)
            {
                fileContent = item.downloadHandler.text;
                var cdnFolderPath = item.url.Replace("_art_files_added.txt", "");
                var addedFileNames = fileContent.Split('\n');
                foreach (var addedFileName_Size in addedFileNames)
                {
                    if (string.IsNullOrEmpty(addedFileName_Size))
                    {
                        continue;
                    }
                    var arr = addedFileName_Size.Split(':');
                    var addedFileName = arr[0];
                    var length= long.Parse(arr[1]);
                    size += length;
                    var filePath = Application.persistentDataPath + $"/Bundles/artdata/{addedFileName}".Trim();
                    if (File.Exists(filePath))
                    {
                        continue;
                    }
                    Debug.Log($"FilePullQueue Enqueue {cdnFolderPath + addedFileName} {filePath}");
                    FilePullQueue.Enqueue(new FilePullTask() { WebRequest = UnityWebRequest.Get(cdnFolderPath + addedFileName), FileSavePath = filePath, FileSize = length });
                }
            }
            InfoLog = $"最新资源版本{CdnArtVersion}(本地资源版本{LocalArtVersion})，所需更新资源{FilePullQueue.Count}个，总计{ size / 1024f / 1024f : 0.00}MB";

            // 弹窗询问是否继续下载资源文件
            if (FilePullQueue.Count > 0)
            {
                SetBtnsActive(true);
                CancelBtn.onClick.AddListener(() => { Application.Quit(); });
                ContinueBtn.onClick.AddListener(() => {
                    ContinueDownload = true;
                    SetBtnsActive(false);
                    InfoLog = $"正在下载资源文件，请保持网络通畅";
                    ProgressSlider.gameObject.SetActive(true);
                    ProgressSlider.maxValue = FilePullQueue.Count;
                    ProgressSlider.value = 0;
                });
            }

            // 等待下载队列下载完
            while (FilePullQueue.Count > 0 || FileDownloadingList.Count > 0)
            {
                ProgressSlider.value = FilePullQueue.Count;
                yield return waitForSeconds;
            }

            // 下载完后
            // 将版本资源新增列表txt写到本地
            foreach (var item in webDict)
            {
                var versionAddedFilesTxt = Application.persistentDataPath + $"/{Bundles_art_files}_{item.Key}_added.txt";
                File.WriteAllText(versionAddedFilesTxt, item.Value.downloadHandler.text);
            }

            // 如果有下载新的资源文件，就重新检查一遍资源版本，重新走一遍资源更新流程
            // 验证有没有漏掉的和损坏的文件，同时也验证下载的时候cdn有没有再次更新版本
            if (FileAddedList.Count > 0)
            {
                FileAddedList.Clear();
                yield return StartCoroutine(Start());
                yield break;
            }

            // 如果没有下载新的资源文件
            // 将最新的资源版本号写到本地，更新完成
            InfoLog = $"更新完成";
            LocalArtVersion = CdnArtVersion;
            File.WriteAllText(PersistentArtVersionTxtPath, LocalArtVersion.ToString());

            StartCoroutine(UpdateComplete());
        }

        /// <summary> 更新完成 </summary>
        private IEnumerator UpdateComplete()
        {
            // 开始读取本地资源信息
            InfoLog = $"开始读取本地资源信息";
            string[] artFilesList;
            if (File.Exists(PersistentArtVersionTxtPath))
            {
                LocalFilesListTxtPath = Application.persistentDataPath + $"/{Bundles_art_files}_{LocalArtVersion}.txt";
                artFilesList = File.ReadAllLines(LocalFilesListTxtPath);
            }
            else
            {
                LocalFilesListTxtPath = Application.streamingAssetsPath + $"/{Bundles_art_files}_{LocalArtVersion}.txt";
                var request = UnityWebRequest.Get(LocalFilesListTxtPath);
                var op = request.SendWebRequest();
                while (!op.isDone)
                {
                    yield return waitForSeconds;
                }
                //Debug.Log(op.webRequest.downloadHandler.text);
                artFilesList = op.webRequest.downloadHandler.text.Split('\n');
            }
            //Debug.Log(LocalFilesListTxtPath);

            Asset.AssetName2Paths.Clear();
            Asset.Path2BundleNames.Clear();
            foreach (var item in artFilesList)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                var arr = item.Split('=');
                var bundleName = arr[0].Trim();
                var assetPath = arr[1].Trim();
                var assetName = Path.GetFileName(assetPath);
                //Debug.Log($"{assetName} {assetPath} {bundleName}");
                Asset.AssetName2Paths[assetName] = assetPath;
                Asset.Path2BundleNames[assetPath] = bundleName;
            }

            InfoLog = $"更新完成，即将进入游戏世界";
            LoadNextScene();
        }

        public async void LoadNextScene()
        {
            var asset = await Asset.LoadSceneAsync(NextScene + ".unity");
        }

        void Update()
        {
            if (!ContinueDownload)
            {
                return;
            }
            if (FilePullQueue.Count > 0 && FileDownloadingList.Count < 10)
            {
                var task = FilePullQueue.Peek();
                FileDownloadingList.Add(task);
                var op = task.WebRequest.SendWebRequest();
                op.completed += (x) =>
                {
                    File.WriteAllBytes(task.FileSavePath, task.WebRequest.downloadHandler.data);
                    FileDownloadingList.Remove(task);
                    FileAddedList.Add(task);
                    Debug.Log($"FilePullQueue completed {task.WebRequest.url} {task.FileSavePath}");
                };
                FilePullQueue.Dequeue();
            }
        }
    }
}