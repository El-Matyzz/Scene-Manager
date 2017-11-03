using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Custom_Builder : EditorWindow
{
    public DirectoryInfo dir;
    public FileInfo[] info;
    public List<int> sceneIncluded = new List<int>();

    public GUIStyle _style00;
    public GUIStyle _style01;
    public GUIStyle _style02;

    [MenuItem("Custom Windows/Custom Builder")]
    static void CreateWindow()
    {
        ((Custom_Builder)GetWindow(typeof(Custom_Builder))).Show();
    }

    void OnGUI()
    {
        #region _style00 (Large + Middle + Bold + (Font = 15) )
        _style00 = EditorStyles.largeLabel;
        _style00.alignment = TextAnchor.UpperCenter;
        _style00.fontStyle = FontStyle.Bold;
        _style00.fontSize = 15;
        #endregion

        #region _style01(Normal + Center)
        _style01 = EditorStyles.label;
        _style01.fontStyle = FontStyle.Normal;
        _style01.alignment = TextAnchor.MiddleCenter;
        #endregion

        #region _style02 (Large + Middle + Bold + (Font = 15) )
        _style02 = EditorStyles.largeLabel;
        _style02.alignment = TextAnchor.UpperCenter;
        _style02.fontStyle = FontStyle.Bold;
        _style02.fontSize = 15;
        #endregion

        EditorGUILayout.Space();
        GUI.enabled = false;
        EditorGUILayout.TextArea("Custom Builder", _style02);
        GUI.enabled = true;

        EditorGUILayout.HelpBox("Select a value of 0 and up to set the order of the scenes in the build. If you do not want a scene to be included in the build, just use a value lower than 0.", MessageType.Info);
        EditorGUILayout.Space();

        GetAllScenes();

        #region Build For PC
        Rect pcButton = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(pcButton, GUIContent.none))
            BuildForPC();
        pcButton.height = 10;
        pcButton.width = 50;
        GUILayout.Label("Build for PC", _style01);
        EditorGUILayout.EndHorizontal();
        #endregion
        
        /*
        #region Build For Android
        Rect androidButton = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(androidButton, GUIContent.none))
            BuildForAndroid();
        androidButton.height = 10;
        androidButton.width = 50;
        GUILayout.Label("Build for Android", _style01);
        EditorGUILayout.EndHorizontal();
        #endregion
        */
    }

    public void GetAllScenes()
    {
        dir = new DirectoryInfo("Assets/Scenes/");
        info = dir.GetFiles("*.unity");

        for (int i = 0; i < info.Length; i++)
        {
            sceneIncluded[i] = EditorGUILayout.IntField(Path.GetFileNameWithoutExtension(info[i].Name), sceneIncluded[i]);
            EditorGUILayout.Space();
        }
    }

    public void BuildForPC()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        int max = -1;
        for (int i = 0; i < sceneIncluded.Count; i++)
        {
            if (sceneIncluded[i] > max)
                max = sceneIncluded[i];
        }

        string[] levels = new string[max];
        for (int i = 0; i < max; i++)
            levels[i] = "Assets/Scenes/" + Path.GetFileName(info[i].Name);
        options.scenes = levels;

        options.locationPathName = "PC_Build/" + PlayerSettings.productName + ".exe";
        options.target = BuildTarget.StandaloneWindows64;
        options.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(options);
    }

    /*
    public void BuildForAndroid()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        int max = -1;
        for (int i = 0; i < sceneIncluded.Count; i++)
        {
            if (sceneIncluded[i] > max)
                max = sceneIncluded[i];
        }

        string[] levels = new string[max];
        for (int i = 0; i < max; i++)
            levels[i] = "Assets/Scenes/" + Path.GetFileName(info[i].Name);
        options.scenes = levels;

        options.locationPathName = "Android_Build/" + PlayerSettings.productName + ".apk";
        options.target = BuildTarget.Android;
        options.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(options);
    }
    */
}
