using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string id;
    [HideInInspector] public List<string> parentRoomNodeIDList=new List<string>();
    [HideInInspector] public List<string> childRoomNodeIDList=new List<string>();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
    public RoomNodeTypeSO roomNodeType;

    #region Editor Code

    //the following code should only be run in the Unity Editor
#if UNITY_EDITOR
    [HideInInspector] public Rect rect;
    public void Initialise(Rect rect,RoomNodeGraphSO nodeGraph,RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id=Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph; 
        this.roomNodeType = roomNodeType;

        //load room node type list
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;

    }
    public void Draw(GUIStyle nodeStyle)
    {
        //Draw node box using begin area
        //put other things between begin and end area
        GUILayout.BeginArea(rect,nodeStyle);

        //Start region to detect popup selection changes
        EditorGUI.BeginChangeCheck();

        //Display a popup using the RoomNodeType name values that can be selected from (default to the currently set roomNodeType)

        int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);

        int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());
        
        roomNodeType=roomNodeTypeList.list[selection];

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(this);
        GUILayout.EndArea();

    }
#endif
    #endregion Editor Code

    //Populate a string array with the room node types to display that can be selected

    public string[] GetRoomNodeTypesToDisplay()
    {
        string[] roomArray = new string[roomNodeTypeList.list.Count];

        for (int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
            }
        }
        return roomArray;
    }

}
