using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;

public class Scene_Manager_Editor : EditorWindow
{
    public Object mainScene;
    public string sceneNameInspector;
    public string newSceneName;

    public GUIStyle _style00;
    public GUIStyle _style01;
    public GUIStyle _style02;

    public DirectoryInfo dir;
    public FileInfo[] info;

    [MenuItem("Custom Windows/Scene Info")]
    static void CreateWindow()
    {
        ((Scene_Manager_Editor)GetWindow(typeof(Scene_Manager_Editor))).Show();
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


        #region Title - || Scene Manager ||

        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        GUI.enabled = false;
        EditorGUILayout.TextArea("Scene Changer", _style02);
        GUI.enabled = true;

        #endregion


        /*
        EditorGUILayout.Space();
        //Main Menu
        EditorGUILayout.BeginHorizontal();
        GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50), (Texture2D)Resources.Load("Unity Icon"), ScaleMode.ScaleToFit);
        EditorGUILayout.BeginVertical();
        SceneNameInspector = "Main Menu";
        EditorGUILayout.TextArea(SceneNameInspector, _style00);
        GUI.enabled = true;
        Rect buttonRect00 = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(buttonRect00, GUIContent.none))
        {
            Main_Scene_Info.ChangeSceneTo_EditorMode(SceneNameInspector);
        }
        buttonRect00.height = 10;
        buttonRect00.width = 50;
        GUILayout.Label("Change Scene to " + SceneNameInspector, _style01);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();
        */
        GetAllScenes();
        CreateScene();

        GUILayoutUtility.GetRect(20, 20);

        #region MainLayout - Do Not Touch
        EditorGUILayout.EndVertical();
        minSize = new Vector2(300, 500);
        maxSize = new Vector2(300, 500);
        #endregion

    }

    public void GetAllScenes()
    {
        dir = new DirectoryInfo("Assets/Scenes/");
        info = dir.GetFiles("*.unity");

        for (int i = 0; i < info.Length; i++)
        {
            string sceneName = Path.GetFileNameWithoutExtension(info[i].Name);
            GUI.enabled = false;
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50), (Texture2D)Resources.Load("Unity Icon"), ScaleMode.ScaleToFit);
            EditorGUILayout.BeginVertical();
            sceneNameInspector = sceneName;
            EditorGUILayout.TextArea(sceneNameInspector, _style00);
            GUI.enabled = true;
            Rect buttonRect00 = EditorGUILayout.BeginHorizontal("Button");
            if (GUI.Button(buttonRect00, GUIContent.none))
            {
                Main_Scene_Info.ChangeSceneTo_EditorMode(sceneNameInspector);
            }
            buttonRect00.height = 10;
            buttonRect00.width = 50;
            GUILayout.Label("Change Scene to " + sceneNameInspector, _style01);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
        }
    }

    void CreateScene()
    {

        #region Title || Create Scene ||

        string SceneNameInspector = "Create Scene";
        GUILayoutUtility.GetRect(20, 50);
        GUI.enabled = false;
        EditorGUILayout.TextArea(SceneNameInspector, _style00);
        GUI.enabled = true;
        EditorGUILayout.Space();

        #endregion

        EditorGUILayout.BeginHorizontal();
        GUI.DrawTexture(GUILayoutUtility.GetRect(50, 50), (Texture2D)Resources.Load("Unity Icon"), ScaleMode.ScaleToFit);
        EditorGUILayout.BeginVertical();

        newSceneName = EditorGUILayout.TextField("New scene name: ", newSceneName);
        Rect buttonRect01 = EditorGUILayout.BeginHorizontal("Button");

        GUI.enabled = true;
        for (int i = 0; i < info.Length; i++)
        {
            if (newSceneName == "" || newSceneName == Path.GetFileNameWithoutExtension(info[i].Name))
            {
                GUI.enabled = false;
            }
        }

        if (GUI.Button(buttonRect01, GUIContent.none))
        {
            EditorSceneManager.SaveScene(EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single), "Assets/Scenes/" + newSceneName + ".unity");
        }

        buttonRect01.height = 10;
        buttonRect01.width = 50;
        GUI.enabled = true;
        GUILayout.Label("Create new scene as: " + newSceneName, _style01);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayoutUtility.GetRect(50, 50);

        for (int i = 0; i < info.Length; i++)
        {
            if (newSceneName == Path.GetFileNameWithoutExtension(info[i].Name))
            {
                EditorGUILayout.HelpBox("Why are you using the same scene name again? ヽ(ﾟДﾟ)ﾉ", MessageType.Warning);
            }
        }

        if (newSceneName == "")
        {
            EditorGUILayout.HelpBox("You should put something up here or it won't work (>_<)", MessageType.Warning);
        }


        EditorGUILayout.EndHorizontal();

    }

}

public class Main_Scene_Info : MonoBehaviour
{

    public static void ChangeSceneTo_EditorMode(string SceneName)
    {
        EditorSceneManager.OpenScene("Assets/Scenes/" + SceneName + ".unity");
    }

}

