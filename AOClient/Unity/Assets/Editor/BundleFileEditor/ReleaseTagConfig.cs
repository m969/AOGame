using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;

[CreateAssetMenu(fileName = "ReleaseTagConfig", menuName = "BundleFile/ReleaseTagConfig")]
public class ReleaseTagConfig : ScriptableObject
{
    public List<Object> GroupTargets;
    public GroupType GroupType;
    public string Filter;
    public List<Object> Assets { get; set; } = new List<Object>();
}