using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;

[CreateAssetMenu(fileName = "FileGroup", menuName = "BundleFile/FileGroup")]
public class FileGroup : ScriptableObject
{
    public List<Object> GroupTargets;
    public GroupType GroupType;
    public string Filter;
    public List<Object> Assets { get; set; } = new List<Object>();


    public string[] GetAssetPaths()
    {
        //Assets.Clear();
        var paths = new List<string>();
        foreach (var target in GroupTargets)
        {
            var folderPath = AssetDatabase.GetAssetPath(target);
            paths.Add(folderPath);
        }
        var assets = AssetDatabase.FindAssets(Filter, paths.ToArray());
        var projectPath = Application.dataPath.TrimEnd("/Assets".ToCharArray());
        var assetPaths = assets.Select(x => AssetDatabase.GUIDToAssetPath(x)).Where(x => File.Exists(projectPath + "/" + x));
        return assetPaths.ToArray();
    }
}

public enum GroupType
{
    BundleByFile,
    BundleInOne,
    BundleBySameBeginWithSplit,
    BundleBySameEndWithSplit,
    RawFile,
}