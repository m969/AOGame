using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FileGroup", menuName = "BundleFile/FileGroup")]
public class FileGroup : ScriptableObject
{
    public List<Object> GroupTargets;
    public GroupType GroupType;
    public string Filter;
    public List<Object> Assets { get; set; } = new List<Object>();
}

public enum GroupType
{
    BundleByFile,
    BundleInOne,
    BundleBySameBeginWithSplit,
    BundleBySameEndWithSplit,
    RawFile,
}