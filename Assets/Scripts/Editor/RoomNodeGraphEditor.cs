using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml.Serialization;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStyle;

    //Node Layout Values
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;


    [MenuItem("Room Node Graph Editor",menuItem="Window/DungeonEditor/Room Node Graph Editor")]

    //A MenuItem attribute allows you to add menu items to the main menu and inspector context menus.
    //MenuItem turns any static function into a menu command. Only static functions can use MenuItem attribute.
    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
        //Gets or creates a window 
    }
    private void OnEnable()
    {
        //Define node layout style 
        roomNodeStyle = new GUIStyle();
        roomNodeStyle.normal.background= EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeStyle.normal.textColor = Color.white;
        roomNodeStyle.padding=new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);
    }
    private void OnGUI()
    {

        GUILayout.BeginArea(new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);
        EditorGUILayout.LabelField("Node 1");
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(new Vector2(300f, 300f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);
        EditorGUILayout.LabelField("Node 1");
        GUILayout.EndArea();
    }
}
