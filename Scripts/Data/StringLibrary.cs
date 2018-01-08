using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class StringLibrary : ScriptableObject
{
    public string ResourcesDirectory;
    public string[] Strings;

    public string[] StringsWithResourcesDirectory => Strings.Select(s => $"{ResourcesDirectory}/{s}").ToArray();
}