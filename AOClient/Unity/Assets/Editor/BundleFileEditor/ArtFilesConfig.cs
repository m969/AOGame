using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;
using UnityEditor.Build.Pipeline;
using System.Web.UI.WebControls;
using BundleFile;

[CreateAssetMenu(fileName = "ArtFilesConfig", menuName = "BundleFile/ArtFilesConfig")]
public class ArtFilesConfig : ScriptableObject
{
    public UnityEngine.Object GroupsFolder;

    public FileGroup[] GetFileGroups()
    {
        var artFilesConfig = this;
        var targetPath = AssetDatabase.GetAssetPath(artFilesConfig.GroupsFolder);
        var groupGUIDs = AssetDatabase.FindAssets("t:FileGroup", new string[] { targetPath });
        var groupPaths = groupGUIDs.Select(x => AssetDatabase.GUIDToAssetPath(x)).ToList();
        var groupAssets = groupPaths.Select(x => AssetDatabase.LoadAssetAtPath<FileGroup>(x)).ToArray();
        return groupAssets;
    }

    public FileGroup[] GetFileGroupsByType(GroupType groupType)
    {
        var groupAssets = GetFileGroups().Where(x => x.GroupType == groupType).ToArray();
        return groupAssets;
    }

    public string[] GetBundleByFileAssetPaths()
    {
        return GetGroupTypeAssetPaths(GroupType.BundleByFile);
    }

    public string[] GetGroupTypeAssetPaths(GroupType groupType)
    {
        var groupAssets = GetFileGroupsByType(groupType);
        var allAssetPaths = new List<string>();
        foreach (var fileGroup in groupAssets)
        {
            var assetPaths = fileGroup.GetAssetPaths();
            allAssetPaths.AddRange(assetPaths);
        }
        return allAssetPaths.ToArray();
    }

    public string[] GetAllAssetPaths()
    {
        var groupAssets = GetFileGroups();
        var allAssetPaths = new List<string>();
        foreach (var fileGroup in groupAssets)
        {
            var assetPaths = fileGroup.GetAssetPaths();
            allAssetPaths.AddRange(assetPaths);
        }
        return allAssetPaths.ToArray();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ArtFilesConfig))]
public class ArtFilesConfigEditor : Editor
{
    public static string FilePrefix => "_art_files";
    public static string BuildBundlesFolder => Application.dataPath + "/../Bundles";
    public static string StreamBundlesFolder => Application.streamingAssetsPath + "/Bundles";
    public static string PlatformName
    {
        get
        {
            var platformName = "Window";
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) platformName = "Android";
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS) platformName = "iOS";
            return platformName;
        }
    }

    public int GetVersion()
    {
        var platformBuildFolder = BuildBundlesFolder + "/" + PlatformName;
        var fileListVersionPath = platformBuildFolder + $"/{FilePrefix}_version.txt";
        var version = 0;
        if (File.Exists(fileListVersionPath))
        {
            version = int.Parse(File.ReadAllText(fileListVersionPath).Trim());
        }
        return version;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(20);

        if (GUILayout.Button("Zip ArtData To StreamingFolder"))
        {
            ZipArtDataToStreamingFolder();
        }

        if (GUILayout.Button("Set Streaming Version 0"))
        {
            if (!Directory.Exists(StreamBundlesFolder))
            {
                Directory.CreateDirectory(StreamBundlesFolder);
            }
            else
            {
                var oldFiles = Directory.GetFiles(StreamBundlesFolder);
                foreach (var item in oldFiles)
                {
                    File.Delete(item);
                }
            }
            var streamingArtVersionTxtPath = StreamBundlesFolder + $"/_art_files_version.txt";
            File.WriteAllText(streamingArtVersionTxtPath, "0");
            Debug.Log("Set Streaming Version 0 Finish.");
        }

        GUILayout.Space(20);

        if (GUILayout.Button("Start Build Bundle File"))
        {
            StartBuildBundleFile();
        }

        if (GUILayout.Button("Open Persistent Folder"))
        {
            OpenPersistentFolder();
        }
    }

    public void ZipArtDataToStreamingFolder()
    {
        if (!Directory.Exists(StreamBundlesFolder))
        {
            Directory.CreateDirectory(StreamBundlesFolder);
        }
        else
        {
            var oldFiles = Directory.GetFiles(StreamBundlesFolder);
            foreach (var item in oldFiles)
            {
                File.Delete(item);
            }
        }

        var fileList = new BinaryFileList() { FileList = new List<BinaryFileData>() };
        var platformBuildFolder = BuildBundlesFolder + "/" + PlatformName;
        var files = Directory.GetFiles(platformBuildFolder + "/artdata");

        var fileName = StreamBundlesFolder + "/artdata.bin";
        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }
        long offset = 0;
        using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
        {
            foreach (var item in files)
            {
                var bytes = File.ReadAllBytes(item);
                writer.Write(bytes);
                fileList.FileList.Add(new BinaryFileData() { FileName = Path.GetFileNameWithoutExtension(item), Offset = offset });
                offset += bytes.Length;
            }
        }
        var version = GetVersion();
        var filesListTxtPath = platformBuildFolder + $"/{FilePrefix}_{version}.txt";
        var destPath = StreamBundlesFolder + $"/{FilePrefix}_{version}.txt";
        if (File.Exists(destPath))
        {
            File.Delete(destPath);
        }
        File.Copy(filesListTxtPath, destPath);

        var versionTxtPath = platformBuildFolder + "/_art_files_version.txt";
        destPath = StreamBundlesFolder + "/_art_files_version.txt";
        if (File.Exists(destPath))
        {
            File.Delete(destPath);
        }
        File.Copy(versionTxtPath, destPath);

        var text = JsonUtility.ToJson(fileList);
        File.WriteAllText(Application.dataPath + "/Resources/BinaryFileList.txt", text);
    }

    public void StartBuildBundleFile()
    {
        var artFilesConfig = target as ArtFilesConfig;
        if (artFilesConfig.GroupsFolder == null)
        {
            throw new Exception("GroupsTarget == null");
        }
        // 获取需要构建Bundle的Asset信息 BundleByFile
        var assetPaths = artFilesConfig.GetBundleByFileAssetPaths();
        var assetBuilds = new List<AssetBundleBuild>();
        foreach (var assetPath in assetPaths)
        {
            var assetBuild = new AssetBundleBuild();
            assetBuild.assetBundleName = $"{ToMD5(assetPath)}";
            assetBuild.assetNames = new string[] { assetPath };
            assetBuilds.Add(assetBuild);
        }

        // 获取需要构建Bundle的Asset信息 BundleInOne
        var bundleInOneGroups = artFilesConfig.GetFileGroupsByType(GroupType.BundleInOne);
        foreach (var fileGroup in bundleInOneGroups)
        {
            var assetBuild = new AssetBundleBuild();
            assetBuild.assetBundleName = $"{ToMD5(AssetDatabase.GetAssetPath(fileGroup))}";
            var assetpaths = fileGroup.GetAssetPaths();
            assetBuild.assetNames = assetpaths;
            assetBuilds.Add(assetBuild);
        }

        if (!Directory.Exists(BuildBundlesFolder))
        {
            Directory.CreateDirectory(BuildBundlesFolder);
        }

        // 开始构建AssetBundle
        BuildAssetBundles(assetBuilds.ToArray(), "Bundles/" + PlatformName, false, true, EditorUserBuildSettings.activeBuildTarget);
        Debug.Log("Build Bundles Finish.");

        var platformBuildFolder = BuildBundlesFolder + "/" + PlatformName;

        // 获取上一次资源构建的版本
        var fileListVersionPath = platformBuildFolder + $"/{FilePrefix}_version.txt";
        var version = GetVersion();
        version++;// 提升资源构建版本

        var artDataFolder = platformBuildFolder + $"/artdata";
        if (!Directory.Exists(artDataFolder))
        {
            Directory.CreateDirectory(artDataFolder);
        }

        // 拷贝新增的bundle文件到artdata文件夹，添加md5后缀
        var file2bundleDict = new Dictionary<string, string>();
        var addedBundleFiles = new Dictionary<string, string>();
        foreach (var assetBuild in assetBuilds)
        {
            var bundleFilePath = platformBuildFolder + "/" + assetBuild.assetBundleName;
            var md5Value = ComputeMD5(bundleFilePath);
            var uploadBundleName = $"{assetBuild.assetBundleName}_{md5Value}";
            foreach (var assetName in assetBuild.assetNames)
            {
                file2bundleDict.Add(assetName, uploadBundleName);
            }

            var uploadBundlePath = artDataFolder + "/" + uploadBundleName;
            if (File.Exists(uploadBundlePath))
            {
                continue;
            }
            File.Copy(bundleFilePath, uploadBundlePath, false);
            addedBundleFiles.Add(bundleFilePath, uploadBundleName);
        }
        if (addedBundleFiles.Count == 0)
        {
            Debug.Log("No Added Files.");
            return;
        }

        // 拷贝新增的bundle文件到added文件夹，添加md5后缀
        var versionArtDataAddedFolder = platformBuildFolder + $"/artdata_{version}_added";
        if (!Directory.Exists(versionArtDataAddedFolder))
        {
            Directory.CreateDirectory(versionArtDataAddedFolder);
        }
        foreach (var item in addedBundleFiles)
        {
            var uploadBundlePath = versionArtDataAddedFolder + "/" + item.Value;
            var bundleFilePath = item.Key;
            File.Copy(bundleFilePath, uploadBundlePath, false);
        }

        Debug.Log("Copy Added Files Finish.");

        // 更新资源构建版本信息，添加新版本新增bundle信息
        File.WriteAllText(fileListVersionPath, version.ToString());
        var versionListPath = platformBuildFolder + $"/{FilePrefix}_{version}.txt";
        var versionAddedListPath = versionArtDataAddedFolder + $"/{FilePrefix}_added.txt";
        var versionListText = new StringBuilder();
        var versionAddedListText = new StringBuilder();
        foreach (var item in file2bundleDict)
        {
            versionListText.AppendLine($"{item.Value}={item.Key}");
        }
        //versionListText.AppendLine("added:");
        foreach (var item in addedBundleFiles)
        {
            var bundleFilePath = item.Key;
            var length = new FileInfo(bundleFilePath).Length;
            versionAddedListText.AppendLine(item.Value + $":{length}");
        }
        File.WriteAllText(versionListPath, versionListText.ToString());
        File.WriteAllText(versionAddedListPath, versionAddedListText.ToString());
        Debug.Log($"{FilePrefix}_{version}.txt {file2bundleDict.Values.Distinct().Count()} ab");
        Debug.Log($"artdata_{version}_added/{FilePrefix}_added.txt {addedBundleFiles.Count} ab");
    }

    public static void OpenPersistentFolder()
    {
        EditorUtility.OpenWithDefaultApp(Application.persistentDataPath);
    }

    public static bool BuildAssetBundles(AssetBundleBuild[] builds, string outputPath, bool forceRebuild, bool useChunkBasedCompression, BuildTarget buildTarget)
    {
        var options = BuildAssetBundleOptions.None;
        if (useChunkBasedCompression)
            options |= BuildAssetBundleOptions.ChunkBasedCompression;

        if (forceRebuild)
            options |= BuildAssetBundleOptions.ForceRebuildAssetBundle;

        Directory.CreateDirectory(outputPath);
        // Replaced BuildPipeline.BuildAssetBundles with CompatibilityBuildPipeline.BuildAssetBundles here
        var manifest = CompatibilityBuildPipeline.BuildAssetBundles(outputPath, builds, options, buildTarget);
        return manifest != null;
    }

    public static string ComputeMD5(string fileName)
    {
        var hashMD5 = string.Empty;
        //检查文件是否存在，如果文件存在则进行计算，否则返回空值
        if (File.Exists(fileName))
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                //计算文件的MD5值
                var buffer = md5.ComputeHash(fs);
                hashMD5 = ToHex(buffer, "x2");
            }//关闭文件流
        }//结束计算
        return hashMD5;
    }//ComputeMD5

    public static MD5 md5 = MD5.Create();
    public static string ToMD5(string content)
    {
        var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
        return ToHex(bytes, "x2");
    }

    public static string ToHex(byte[] bytes, string format)
    {
        var sb = new StringBuilder();
        foreach (var t in bytes)
        {
            sb.Append(t.ToString(format));
        }
        return sb.ToString();
    }

    [RuntimeInitializeOnLoadMethod]
    public static void Startup()
    {
        var artFilesConfig = AssetDatabase.LoadAssetAtPath<ArtFilesConfig>("Assets/Settings/ArtFilesConfig.asset");
        var assetPaths = artFilesConfig.GetAllAssetPaths();
        foreach (var assetPath in assetPaths)
        {
            AssetFile.Asset.AssetName2Paths.Add(Path.GetFileName(assetPath), assetPath);
            AssetFile.Asset.Path2BundleNames.Add(assetPath, ToMD5(assetPath));
            //Debug.Log($"{Path.GetFileName(assetPath)} {assetPath} {ToMD5(assetPath)}");
        }
    }
}
#endif
