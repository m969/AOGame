using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace BundleFile
{
    public class ArtFilesUpdater : MonoBehaviour
    {
        public const string Bundles_filePrefix = "Bundles/_art_files";
        public string CdnVersionFileUri = "http://127.0.0.1:8080/Bundles/{0}/_art_files_version.txt";
        public string CdnPlatformFolderPath { get; set; }
        public string LocalArtVersionTxtPath { get; set; }
        public string LocalFilesListTxtPath { get; set; }
        public int CdnArtVersion { get; set; } = 0;
        public int LocalArtVersion { get; set; } = 0;
        public int StreamingArtVersion { get; set; } = 0;
        public Dictionary<string, string> Path2Bundles { get; set; }
        public Dictionary<string, string> Name2Paths { get; set; }
        public Queue<FileUpdateTask> FileUpdateQueue { get; set; } = new();
        public List<FileUpdateTask> FileDownloadingList { get; set; } = new();


        IEnumerator Start()
        {
            if (Define.IsEditor)
            {
                UpdateComplete();
                yield break;
            }

            var waitForSeconds = new WaitForSeconds(0.1f);

            const string filesVersionTxt = "_art_files_version.txt";
            var persistentArtVersionTxtPath = Application.persistentDataPath + $"/{Bundles_filePrefix}_version.txt";
            LocalArtVersionTxtPath = persistentArtVersionTxtPath;
            LocalFilesListTxtPath = Application.persistentDataPath + $"/{Bundles_filePrefix}_1.txt";
            CdnVersionFileUri = string.Format(CdnVersionFileUri, "Android");
            CdnPlatformFolderPath = CdnVersionFileUri.Replace($"/{filesVersionTxt}", "");

            var fileListUri = CdnVersionFileUri.Replace(filesVersionTxt, "_art_files_1.txt");

            // 获取包Streaming资源版本
            var streamingArtVersionTxtPath = Application.streamingAssetsPath + $"/{Bundles_filePrefix}_version.txt";
            var request = UnityWebRequest.Get(streamingArtVersionTxtPath);
            var op = request.SendWebRequest();
            while (!op.isDone)
            {
                yield return waitForSeconds;
            }
            var streamingVersionStr = op.webRequest.downloadHandler.text.Trim();
            StreamingArtVersion = int.Parse(streamingVersionStr);

            // 获取本地的资源版本
            //var hasPersistentFile = File.Exists(LocalArtVersionTxtPath);
            if (File.Exists(LocalArtVersionTxtPath))
            {
                request = UnityWebRequest.Get(LocalArtVersionTxtPath);
                op = request.SendWebRequest();
                while (!op.isDone)
                {
                    yield return waitForSeconds;
                }
                var versionStr = op.webRequest.downloadHandler.text.Trim();
                LocalArtVersion = int.Parse(versionStr);
            }
            else
            {
                LocalArtVersion = StreamingArtVersion;
            }

            // 获取cdn上的资源版本
            request = UnityWebRequest.Get(CdnVersionFileUri);
            op = request.SendWebRequest();
            while (!op.isDone)
            {
                yield return waitForSeconds;
            }
            var fileContent = op.webRequest.downloadHandler.text;
            CdnArtVersion = int.Parse(fileContent.Trim());

            // 如果cdn和本地的资源版本一样就直接更新完毕
            if (CdnArtVersion == LocalArtVersion)
            {
                UpdateComplete();
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
            // 下载更新资源文件列表txt
            var filesListTxt = Application.persistentDataPath + $"/{Bundles_filePrefix}_{CdnArtVersion}.txt";
            if (!File.Exists(filesListTxt))
            {
                var cdnFilesListTxt = CdnPlatformFolderPath + $"/_art_files_{CdnArtVersion}.txt";
                request = UnityWebRequest.Get(cdnFilesListTxt);
                op = request.SendWebRequest();
                while (!op.isDone)
                {
                    yield return waitForSeconds;
                }
                fileContent = op.webRequest.downloadHandler.text;
                File.WriteAllText(filesListTxt, fileContent);
            }

            // 下载版本新增资源文件列表txt
            var webDict = new Dictionary<int, UnityWebRequest>();
            for (int i = LocalArtVersion + 1; i <= CdnArtVersion; i++)
            {
                var versionAddedFilesTxt = Application.persistentDataPath + $"/{Bundles_filePrefix}_{i}_added.txt";
                if (!File.Exists(versionAddedFilesTxt))
                {
                    var cdnVersionAddedFilesTxt = CdnPlatformFolderPath + $"/artdata_{i}_added/_art_files_added.txt";
                    request = UnityWebRequest.Get(cdnVersionAddedFilesTxt);
                    webDict.Add(i, request);
                    op = request.SendWebRequest();
                }
            }

            // 等待版本新增资源文件列表txt下载完
            var webList = webDict.Values.ToList();
            while (webList.Exists(x => !x.isDone))
            {
                yield return waitForSeconds;
            }

            // 遍历检出本地没有的资源文件放到下载队列
            foreach (var item in webDict.Values)
            {
                fileContent = item.downloadHandler.text;
                var cdnFolderPath = item.uri.AbsoluteUri.Replace("_art_files_added.txt", "");
                var addedFileNames = fileContent.Split('\n');
                foreach (var addedFileName in addedFileNames)
                {
                    if (string.IsNullOrEmpty(addedFileName))
                    {
                        continue;
                    }
                    var filePath = Application.persistentDataPath + $"/Bundles/artdata/{addedFileName}".Trim();
                    var streamingFilePath = Application.streamingAssetsPath + $"/Bundles/artdata/{addedFileName}";
                    if (File.Exists(filePath) || File.Exists(streamingFilePath))
                    {
                        continue;
                    }
                    Debug.Log($"FileUpdateQueue {cdnFolderPath + addedFileName} {filePath}");
                    FileUpdateQueue.Enqueue(new FileUpdateTask() { UnityWeb = UnityWebRequest.Get(cdnFolderPath + addedFileName), FileSavePath = filePath });
                }
            }

            // 等待下载队列下载完
            while (FileUpdateQueue.Count > 0 || FileDownloadingList.Count > 0)
            {
                yield return waitForSeconds;
            }

            // 将版本新增资源文件列表txt写到本地
            foreach (var item in webDict)
            {
                var versionAddedFilesTxt = Application.persistentDataPath + $"/{Bundles_filePrefix}_{item.Key}_added.txt";
                File.WriteAllText(versionAddedFilesTxt, item.Value.downloadHandler.text);
            }

            // 将最新的资源版本号写到本地，更新完成
            File.WriteAllText(persistentArtVersionTxtPath, CdnArtVersion.ToString());

            // 开始读取本地资源信息
            if (File.Exists(LocalArtVersionTxtPath))
            {
                LocalFilesListTxtPath = Application.persistentDataPath + $"/{Bundles_filePrefix}_{LocalArtVersion}.txt";
            }
            else
            {
                LocalFilesListTxtPath = Application.streamingAssetsPath + $"/{Bundles_filePrefix}_{LocalArtVersion}.txt";
            }
            request = UnityWebRequest.Get(LocalFilesListTxtPath);
            op = request.SendWebRequest();
            while (!op.isDone)
            {
                yield return waitForSeconds;
            }
            var artFiles = op.webRequest.downloadHandler.text.Split('\n');
            foreach (var item in artFiles)
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }
                var arr = item.Split('=');
                var bundleName = arr[0].Trim();
                var assetPath = arr[1].Trim();
                var assetName = Path.GetFileName(assetPath);
                AssetFile.Asset.AssetName2Paths[assetName] = assetPath;
                AssetFile.Asset.Path2BundleNames[assetPath] = bundleName;
            }

            UpdateComplete();
        }

        private void UpdateComplete()
        {
            SceneManager.LoadScene("Init");
        }

        void Update()
        {
            if (FileUpdateQueue.Count > 0 && FileDownloadingList.Count < 10)
            {
                var task = FileUpdateQueue.Peek();
                FileDownloadingList.Add(task);
                var op = task.UnityWeb.SendWebRequest();
                op.completed += (x) =>
                {
                    File.WriteAllBytes(task.FileSavePath, task.UnityWeb.downloadHandler.data);
                    FileDownloadingList.Remove(task);
                };
                FileUpdateQueue.Dequeue();
            }
        }
    }

    public class FileUpdateTask
    {
        public UnityWebRequest UnityWeb;
        public string FileSavePath;
    }
}