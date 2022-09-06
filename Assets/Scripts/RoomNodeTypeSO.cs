using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RoomNodeType_", menuName = "ScriptableObjects/Dungeon/Room Node Type")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string roomNodeTypeName;

    #region Header
    [Header("Only flag the RoomNodeTypes that should be cisible ib the editor")]
    #endregion Header
    public bool displayInNodeGraphEditor = true;

    #region Header
    [Header("One type should be a corridor")]
    #endregion Header
    public bool isCorridor;

    #region Header
    [Header("One type should be a corridorNS")]
    #endregion Header
    public bool isCorridorNS;

    #region Header
    [Header("One type should be a corridorEW")]
    #endregion Header
    public bool isCorridorEW;

    #region Header
    [Header("One type should be an entrance")]
    #endregion Header
    public bool isEntrance;

    #region Header
    [Header("One type should be an boss room")]
    #endregion Header
    public bool isBossRoom;

    #region Header
    [Header("One type should be an none(Unassigned)")]
    #endregion Header
    public bool isNone;

    #region Validation
    //Compiler directive.It makes the inner runs in unity editor.
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
    }
#endif
    #endregion


}
