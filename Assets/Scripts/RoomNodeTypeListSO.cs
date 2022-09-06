using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="RoomNodeTypeListSO",menuName ="ScriptableObjects/Dungeon/Room Node Type List")]
public class RoomNodeTypeListSO : ScriptableObject
{
   
    #region Header ROOM NODE Type LIST
    [Space(10)]
    [Header("ROOM NODE TYPE LIST")]
    #endregion
    #region ToolTip
    [Space(10)]
    [Header("This should be populated with all the RoomNodeTypeSO for the game - it is used instead of an enum")]
    #endregion
    public List<RoomNodeTypeListSO> list;
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumarableValues(this, nameof(list), list);
    }
#endif
    #endregion
}
