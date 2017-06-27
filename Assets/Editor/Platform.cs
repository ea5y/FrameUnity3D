using UnityEditor;
using UnityEngine;

public class Platform
{
    [MenuItem("Platform/Linux Editor")]
    public static void SwitchToLinuxEditor()
    {
        SetDefineSymbols(BuildTargetGroup.Standalone, "LINUX_EDITOR");
    }

    [MenuItem("Platform/Win Editor")]
    public static void SwitchToWinEditor()
    {
        SetDefineSymbols(BuildTargetGroup.Android, "WIN_EDITOR");
    }

    public static void SetDefineSymbols(BuildTargetGroup target, string symbols)
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(target, symbols);
    }
}

