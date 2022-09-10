using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RoomNodeGraph",menuName ="ScriptableObjects/Dungeon/Room Node Graph")]
public class RoomNodeGraphSO : ScriptableObject
{
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
    [HideInInspector] public List<RoomNodeSO> roomNodeList=new List<RoomNodeSO>();
    //The string in the dictionary represents GUID(globally unique identifier),also it is a struct.
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();

    private void Awake()
    {
        LoadRoomNodeDictionary();
    }

    //Load the room node dictionary from the room node list.
    private void LoadRoomNodeDictionary()
    {
        roomNodeDictionary.Clear();

        //Populate dictionary
        foreach (RoomNodeSO node in roomNodeList)
        {
            roomNodeDictionary[node.id] = node;
        }
    }
    #region Editor Code
#if UNITY_EDITOR

    [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom= null;
    [HideInInspector] public Vector2 linePosition;

    //Repopulate node dictionary every time a change is made in the editor.
    public void OnValidate()
    {
        LoadRoomNodeDictionary();
    }

    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node, Vector2 position)
     {
        roomNodeToDrawLineFrom = node;
        linePosition = position;
     }
    
#endif

    #endregion Editor Code
}
