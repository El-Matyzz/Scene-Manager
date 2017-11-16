using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class SecurityAdvice : EditorWindow
{

    public GUIStyle _style00;
    public GUIStyle _style01;
    public GUIStyle _style02;


    public void OnGUI()
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

        SecureDeletion();

        minSize = new Vector2(170, 95);
        maxSize = new Vector2(170, 95);

    }


    public void SecureDeletion()
    {
        if (Scene_Manager_Editor.toDelete)
        {
            EditorGUILayout.HelpBox("Are you sure you want to delete this?", MessageType.Info);

            EditorGUILayout.BeginHorizontal();

            #region Deletion YesButton
            Rect yesButton00 = EditorGUILayout.BeginHorizontal("Button");
            if (GUI.Button(yesButton00, GUIContent.none))
            {
                AssetDatabase.MoveAssetToTrash(Scene_Manager_Editor.pathOfDeletion);

                ((SecurityAdvice)GetWindow(typeof(SecurityAdvice))).Close();
            }
            EditorGUILayout.HelpBox("Yes", MessageType.Error);
            EditorGUILayout.EndHorizontal();
            #endregion
            #region Deletion NoButton
            Rect noButton00 = EditorGUILayout.BeginHorizontal("Button");

            if (GUI.Button(noButton00, GUIContent.none))
            {
                ((SecurityAdvice)GetWindow(typeof(SecurityAdvice))).Close();
            }
            EditorGUILayout.HelpBox("No", MessageType.Warning);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            #endregion
        }
        else if (Scene_Manager_Editor.toSave)
        {
            EditorGUILayout.HelpBox("Are you sure you want to overwrite the scene?", MessageType.Info);

            EditorGUILayout.BeginHorizontal();

            #region Save YesButton
            Rect yesButton01 = EditorGUILayout.BeginHorizontal("Button");

            if (GUI.Button(yesButton01, GUIContent.none))
            {
                EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

                ((SecurityAdvice)GetWindow(typeof(SecurityAdvice))).Close();
            }
            EditorGUILayout.HelpBox("Yes", MessageType.Error);
            EditorGUILayout.EndHorizontal();
            #endregion
            #region Save NoButton
            Rect noButton01 = EditorGUILayout.BeginHorizontal("Button");

            if (GUI.Button(noButton01, GUIContent.none))
            {
                ((SecurityAdvice)GetWindow(typeof(SecurityAdvice))).Close();
            }
            EditorGUILayout.HelpBox("No", MessageType.Warning);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            #endregion
        }
        else if (Scene_Manager_Editor.toChange)
        {
            EditorGUILayout.HelpBox("Are you sure you want to change the scene without saving?", MessageType.Info);

            EditorGUILayout.BeginHorizontal();

            #region Change YesButton
            Rect yesButton02 = EditorGUILayout.BeginHorizontal("Button");

            if (GUI.Button(yesButton02, GUIContent.none))
            {
                EditorSceneManager.OpenScene(Scene_Manager_Editor.pathOfChange);

                ((SecurityAdvice)GetWindow(typeof(SecurityAdvice))).Close();
            }
            EditorGUILayout.HelpBox("Yes", MessageType.Error);
            EditorGUILayout.EndHorizontal();
            #endregion
            #region Change NoButton
            Rect noButton02 = EditorGUILayout.BeginHorizontal("Button");

            if (GUI.Button(noButton02, GUIContent.none))
            {
                ((SecurityAdvice)GetWindow(typeof(SecurityAdvice))).Close();
            }
            EditorGUILayout.HelpBox("No", MessageType.Warning);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            #endregion
        }
    }
}
