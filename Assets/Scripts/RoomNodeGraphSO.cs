using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RoomNodeGraph",menuName ="ScriptableObjects/Dungeon/Room Node Graph")]
public class RoomNodeGraphSO : ScriptableObject
{
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
    [HideInInspector] public List<RoomNodeTypeListSO> roomNodeList=new List<RoomNodeTypeListSO>();
    //The string in the dictionary represents GUID(globally unique identifier),also it is a struct.
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();




}
