//================================
//===Author: easy
//===Email: gopiny@live.com
//===Date: 2017-10-19 12:13
//================================

using UnityEditor;
using UnityEngine;
public class GameConfig : EditorWindow
{
    public enum OSType
    {
        Windows,
        Linux
    }

    private bool _isEditorMode;
    private bool _isHotfixEnable;
    private OSType _oSType;

    [MenuItem("GameConfig/Open")]
    public static void ShowWindow()
    {
        var window = (GameConfig)EditorWindow.GetWindow(typeof(GameConfig));
        window.LoadConfig();
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("Base Settings:", EditorStyles.boldLabel);

        _isEditorMode = EditorGUILayout.Toggle("Is Editor Mode", _isEditorMode);
        _isHotfixEnable = EditorGUILayout.Toggle("Is Hotfix Enable", _isHotfixEnable);
        _oSType = (OSType)EditorGUILayout.EnumPopup("OS", _oSType);

        if(GUILayout.Button("Save"))
        {
            this.SaveConfig();
            this.SetSymbols();
        }
    }

    private void LoadConfig()
    {
        _oSType = (OSType)EditorPrefs.GetInt("GameConfig.OSType", (int)OSType.Windows);
        _isEditorMode = EditorPrefs.GetBool("GameConfig.IsEditorMode", false);
        _isHotfixEnable = EditorPrefs.GetBool("GameConfig.IsHotfixEnable", false);
    }

    private void SaveConfig()
    {
        EditorPrefs.SetInt("GameConfig.OSType", (int)_oSType);
        EditorPrefs.SetBool("GameConfig.IsEditorMode", _isEditorMode);
        EditorPrefs.SetBool("GameConfig.IsHotfixEnable", _isHotfixEnable);
    }

    private void SetSymbols()
    {
        string symbols = string.Empty;
        symbols += _isEditorMode == true ? "EDITOR_MODE;" : "";
        symbols += _isHotfixEnable == true ? "HOTFIX_ENABLE;INJECT_WITHOUT_TOOL;" : "";

        switch(_oSType)
        {
            case OSType.Windows:
                symbols += "WIN_EDITOR;";
                break;
            case OSType.Linux:
                symbols += "LINUX_EDITOR;";
                break;
        }

        this.SetDefineSymbols(BuildTargetGroup.Android, symbols);
        this.SetDefineSymbols(BuildTargetGroup.Standalone, symbols);
        this.SetDefineSymbols(BuildTargetGroup.iOS, symbols);
    }

    private void SetDefineSymbols(BuildTargetGroup target, string symbols)
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(target, symbols);
    }
}
