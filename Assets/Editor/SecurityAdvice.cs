using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

        minSize = new Vector2(240, 50);
        maxSize = new Vector2(240, 50);
       
    }

    
    public void SecureDeletion()
    {
        string path = Scene_Manager_Editor.pathOfDeletion;
        Rect deleteButton = EditorGUILayout.BeginHorizontal("Button");
        if (GUI.Button(deleteButton, GUIContent.none))
        {
            AssetDatabase.MoveAssetToTrash(path);

            ((SecurityAdvice)GetWindow(typeof(SecurityAdvice))).Close();
        }

        deleteButton.height = 10;
        deleteButton.width = 50;
        EditorGUILayout.HelpBox("Are you sure you want to delete this?", MessageType.Error);

        EditorGUILayout.EndHorizontal();
    }
}
