using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Custom_Build_Settings : EditorWindow
{
    public DirectoryInfo dir;
    public FileInfo[] info;
    public List<int> sceneIncluded = new List<int>();

    public Object sceneToAdd;
    public List<Object> scenes = new List<Object>();

    public GUIStyle _style00;
    public GUIStyle _style01;
    public GUIStyle _style02;

    public Vector2 scrollPos;

    public bool mustShowFolder;

    [MenuItem("Custom Windows/Custom Build Settings")]
    static void CreateBuildWindow()
    {
        ((Custom_Build_Settings)GetWindow(typeof(Custom_Build_Settings))).Show();
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

        GUI.enabled = false;
        EditorGUILayout.TextArea("Custom Build Settings", _style02);
        GUI.enabled = true;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, false); //Begin Scroll

        for (int i = 0; i < scenes.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(scenes[i].name, _style01);
            EditorGUILayout.BeginHorizontal();

            if (i == 0)
                GUI.enabled = false;
            Rect moveUp = EditorGUILayout.BeginHorizontal("Button");
            if (GUI.Button(moveUp, GUIContent.none))
            {
                Object temp = scenes[i];
                scenes[i] = scenes[i - 1];
                scenes[i - 1] = temp;
            }
            GUILayout.Label("↑", _style01);
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;

            if (i == scenes.Count - 1)
                GUI.enabled = false;
            Rect moveDown = EditorGUILayout.BeginHorizontal("Button");
            if (GUI.Button(moveDown, GUIContent.none))
            {
                Object temp = scenes[i];
                scenes[i] = scenes[i + 1];
                scenes[i + 1] = temp;
            }
            GUILayout.Label("↓", _style01);
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;

            Rect remove = EditorGUILayout.BeginHorizontal("Button");
            if (GUI.Button(remove, GUIContent.none))
                scenes.Remove(scenes[i]);
            GUILayout.Label("x", _style01);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        sceneToAdd = EditorGUILayout.ObjectField(sceneToAdd, typeof(SceneAsset), true);

        if (sceneToAdd == null || scenes.Contains(sceneToAdd))
            GUI.enabled = false;

        Rect addScene = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(addScene, GUIContent.none))
            AddScene();
        GUILayout.Label("+", _style01);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        GUI.enabled = true;

        if (scenes.Contains(sceneToAdd))
            EditorGUILayout.HelpBox("This scene is already in the list.", MessageType.Warning);
        EditorGUILayout.Space();

        if (scenes.Count == 0) GUI.enabled = false;

        EditorGUILayout.BeginHorizontal();
        Rect build = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(build, GUIContent.none))
            Build();
        GUILayout.Label("Build", _style01);
        EditorGUILayout.EndHorizontal();

        Rect buildAndRun = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(buildAndRun, GUIContent.none))
            BuildAndRun();
        GUILayout.Label("Build & Run", _style01);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();


        PrepareFolder();

        FixWindow();
        EditorGUILayout.EndScrollView();
    }

    public void AddScene()
    {
        scenes.Add(sceneToAdd);
        sceneToAdd = null;
    }

    public void Move(Object current, Object other)
    {
        Object temp = current;
        current = other;
        other = temp;
    }

    public void Build()
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

        if (mustShowFolder)
        {
            ShowFolder();
        }
    }

    public void BuildAndRun()
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
        options.options = BuildOptions.AutoRunPlayer;

        BuildPipeline.BuildPlayer(options);
        if (mustShowFolder)
        {
            ShowFolder();
        }
    }

    public void PrepareFolder()
    {
        GUILayoutUtility.GetRect(15, 15);

        #region Button & Toggle

        GUI.enabled = true;
        Rect toggleButton = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(toggleButton, GUIContent.none))
        {
            mustShowFolder = !mustShowFolder;
        }
        EditorGUILayout.Space();
        mustShowFolder = EditorGUILayout.Toggle("Open Folder at Finish", mustShowFolder);
        EditorGUILayout.EndHorizontal();

        #endregion
    }

    public void ShowFolder()
    {
        //Falta poner la ruta de guardado de los buildeos
        //Falta poner la ruta de guardado de los buildeos
        //Falta poner la ruta de guardado de los buildeos
        System.Diagnostics.Process.Start("C:\\oraclexe\\app");
        //Falta poner la ruta de guardado de los buildeos
        //Falta poner la ruta de guardado de los buildeos
        //Falta poner la ruta de guardado de los buildeos
    }

    public void FixWindow()
    {
        minSize = new Vector2(300, 500);
        maxSize = new Vector2(300, 500);
        Repaint();
    }

}
