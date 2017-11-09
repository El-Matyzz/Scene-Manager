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

    public Vector2 scrollPos;

    public static string pathOfDeletion;

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


        #region MainLayout - Do Not Touch || Title || BeginScroll

        EditorGUILayout.Space();
        GUI.enabled = false;
        EditorGUILayout.TextArea("Scene Changer", _style02);
        GUI.enabled = true;
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, true, false); //Begin Scroll

        #endregion

        GetAllScenes();
        CreateScene();
        OpenBuilder();
        Repaint();

        #region MainLayout - Do Not Touch || Window Fix || EndScroll
        EditorGUILayout.EndScrollView(); //End Scroll
        GUILayoutUtility.GetRect(20, 20);

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

            #region Función para cambiar de escena
            sceneNameInspector = sceneName;
            EditorGUILayout.TextArea(sceneNameInspector, _style00);
            GUI.enabled = true;
            Rect buttonRect00 = EditorGUILayout.BeginHorizontal("Button");
            if (GUI.Button(buttonRect00, GUIContent.none))
            {
                //Función para cambiar la escena
                Main_Scene_Info.ChangeSceneTo_EditorMode(sceneNameInspector);
            }
            buttonRect00.height = 10;
            buttonRect00.width = 50;
            GUILayout.Label("Change Scene to " + sceneNameInspector, _style01);

            EditorGUILayout.EndHorizontal();
            #endregion

            EditorGUILayout.BeginHorizontal();

            #region Función para guardar la escena
            //Solo podes guardar la escena seleccionada si es la que está cargada ahora mismo
            #region Toggle Save Button

            //if (SceneManager.GetActiveScene().name != Path.GetFileNameWithoutExtension(info[i].Name))
            //{
            //    GUI.enabled = false;
            //    GUILayout.Button((Texture2D)Resources.Load("Lock"));
            //    GUI.enabled = true;
            //}
            if (SceneManager.GetActiveScene().name != Path.GetFileNameWithoutExtension(info[i].Name))
            {
                GUI.enabled = false;
                Rect dontSave = EditorGUILayout.BeginHorizontal("Button");
                if (GUI.Button(dontSave, GUIContent.none))
                {

                }
                
                GUI.DrawTexture(GUILayoutUtility.GetRect(16,16), (Texture2D)Resources.Load("Lock"), ScaleMode.ScaleToFit);
                GUI.enabled = true;

                dontSave.height = 10;
                dontSave.width = 50;
                //GUILayout.Label("Lala", _style01);
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                Rect saveButton = EditorGUILayout.BeginHorizontal("Button");
                if (GUI.Button(saveButton, GUIContent.none))
                {
                    //Funcion para guardar la escena
                    EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
                }
                GUI.enabled = true;

                saveButton.height = 10;
                saveButton.width = 50;
                GUILayout.Label("Save scene", _style01);
                EditorGUILayout.EndHorizontal();
            }


            #endregion

            #endregion

            #region Función para borrar la escena

            
        //((Custom_Build_Settings)GetWindow(typeof(SecureDeletion))).Show();

            Rect deleteButton = EditorGUILayout.BeginHorizontal("Button");
            if (GUI.Button(deleteButton, GUIContent.none))
            {
                pathOfDeletion = "Assets/Scenes/" + Path.GetFileNameWithoutExtension(info[i].Name) + ".unity";
                ((SecurityAdvice)GetWindow(typeof(SecurityAdvice))).Show();
            }

            deleteButton.height = 10;
            deleteButton.width = 50;
            GUILayout.Label("Delete scene", _style01);
            EditorGUILayout.EndHorizontal();
            #endregion

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayoutUtility.GetRect(30, 30);
        }
    }

    public void CreateScene()
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

        #region SceneCreator Locker p1

        for (int i = 0; i < info.Length; i++)
        {
            if (newSceneName == "" || newSceneName == Path.GetFileNameWithoutExtension(info[i].Name))
            {
                GUI.enabled = false;
            }
        }

        #endregion

        if (GUI.Button(buttonRect01, GUIContent.none))
        {
            EditorSceneManager.SaveScene(EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single), "Assets/Scenes/" + newSceneName + ".unity");
            newSceneName = "";
        }

        buttonRect01.height = 10;
        buttonRect01.width = 50;
        GUI.enabled = true;
        GUILayout.Label("Create new scene as: " + newSceneName, _style01);

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();


        #region SceneCreator Locker p2
        EditorGUILayout.BeginHorizontal();

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
        #endregion

        GUILayoutUtility.GetRect(30, 30);


    }
    public void OpenBuilder()
    {
        Rect openBuilder = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(openBuilder, GUIContent.none))
        {
            ((Custom_Build_Settings)GetWindow(typeof(Custom_Build_Settings))).Show();
        }

        GUILayout.Label("Open Builder Settings ", _style01);
        openBuilder.height = 10;
        openBuilder.width = 50;
        EditorGUILayout.EndHorizontal();
        GUILayoutUtility.GetRect(30, 30);
    }


    public class Main_Scene_Info : MonoBehaviour
    {

        public static void ChangeSceneTo_EditorMode(string SceneName)
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + SceneName + ".unity");
        }

    }


}



