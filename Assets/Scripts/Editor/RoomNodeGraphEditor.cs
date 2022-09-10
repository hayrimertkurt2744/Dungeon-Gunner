using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.MPE;

//Callbacks namespace helps you to detect certain things happened in the editor.


public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeSO currentRoomNode = null;
    private RoomNodeTypeListSO roomNodeTypeList;

    //Node Layout Values
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    //connecting line values
    private const float connectingLineWidth = 3f;
    private const float connectingLineArrowSize = 6f;


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
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeStyle.normal.textColor = Color.white;
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        //Load Room node types
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    //Open the room node graph editor window if a room node graph scriptable object asset is double clicked in the inspector
    //Need the namespace UnityEditor.Callbacks
    // "0" indicates the count of functions to be called 
    //Adding this attribute to a static method will make the method be called when Unity is about to open an asset
    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceID,int line)
    {
        RoomNodeGraphSO roomNodeGraph=EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;

        if (roomNodeGraph!=null)
        {
            OpenWindow();

            currentRoomNodeGraph = roomNodeGraph;

            return true;
        }
        return false;
    }
   
    private void OnGUI()
    {
        //if a scriptable object of type RoomNodeGraphSO has been selected then process
        if (currentRoomNodeGraph!=null)
        {
            //Draw line if being dragged
            DrawDraggedLine();

            //Process Events
            ProcessEvents(Event.current);

            //Draw Connections Between Room Nodes
            DrawRoomConnections();

            //Draw Room Nodes
            DrawRoomNodes();
        }
        if(GUI.changed)
            Repaint();
        
    }
    private void DrawDraggedLine()
    {
        if (currentRoomNodeGraph.linePosition!=Vector2.zero)
        {
            //Draw Line From node to line position
            Handles.DrawBezier(currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition,
                currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePosition, Color.yellow, null, connectingLineWidth);
        }
    }

    private void ProcessEvents(Event currentEvent) 
    {
        //Get room node that mouse is over if it's null or not currently being dragged
        if (currentRoomNode==null|| currentRoomNode.isLeftClickDragging==false)
        {
            currentRoomNode= IsMouseOverRoomNode(currentEvent);
        }
        //If mouse isnt over a room node or we are currently dragging a line from the room node then process graph events
        if (currentRoomNode==null||currentRoomNodeGraph.roomNodeToDrawLineFrom!=null)
        {
            ProcessRoomNodeGraphEvents(currentEvent);
        }
        //Else process room node events
        else
        {
            currentRoomNode.ProcessEvents(currentEvent);
        }
        
    }


    private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
    {
        for (int i = currentRoomNodeGraph.roomNodeList.Count - 1; i >=0 ; i--)
        {
            if (currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition))
            {
                return currentRoomNodeGraph.roomNodeList[i];
            }
        }
        return null;
    }

    //Process Room Node Graph Events
    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;



            default:
                break;
        }
    }
    //Process mouse down events on the room node graph (not over a node)
    private void ProcessMouseDownEvent(Event currentEvent)
    {   
        //Process right click mouse down on graph event (show context menu)
        if (currentEvent.button==1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
    }
    //Show the context menu
    //Generic menus helps you to create custom context and dropdown menus
    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu=new GenericMenu();

        menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);

        menu.ShowAsContext();
    }
    //Create a room node at the mouse position
    private void CreateRoomNode(object mousePositionObject)
    {
        CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone));
    }
    //Create a room node at the mouse position - overloaded version
    private void CreateRoomNode(object mousePositionObject,RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePosition=(Vector2) mousePositionObject;

        //create room node scriptable object asset
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

        //add room node to current room node graph node list
        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        //set room node values
        roomNode.Initialise(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph,roomNodeType);

        //add room node to room node graph scriptable object assets database
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);

        AssetDatabase.SaveAssets();

        //Refresh graph node dictionary
        currentRoomNodeGraph.OnValidate();

        //    GUILayout.BeginArea(new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);
        //EditorGUILayout.LabelField("Node 1");
        //GUILayout.EndArea();

        //GUILayout.BeginArea(new Rect(new Vector2(300f, 300f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);
        //EditorGUILayout.LabelField("Node 1");
        //GUILayout.EndArea();
    }

    //Process Mouse Up Event
    private void ProcessMouseUpEvent(Event currentEvent)
    {
        //if releasing the right mouse button and currently dragging a line
        if (currentEvent.button==1 && currentRoomNodeGraph.roomNodeToDrawLineFrom!=null)
        {
            //Check if over a room node
            RoomNodeSO roomNode = IsMouseOverRoomNode(currentEvent);
            if (roomNode!=null)
            {
                // if so set it as a child of the parent room nose if it can be added
                if (currentRoomNodeGraph.roomNodeToDrawLineFrom.AddChildRoomNodeIDToRoomNode(roomNode.id))
                {
                    //Set parent id in child room node
                    roomNode.AddParentRoomNodeIDToRoomNode(currentRoomNodeGraph.roomNodeToDrawLineFrom.id);
                }
            }


            ClearLineDrag();

        }
    }


    //Process Mouse Drag Event
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        //process right click drag event - draw line
        if (currentEvent.button==1)
        {
            ProcessRightMouseDragEvent(currentEvent);
        }
    }

    //Process Right Mouse Drag Event
    private void ProcessRightMouseDragEvent(Event currentEvent)
    {
        //process right click drag event - draw line
        if (currentRoomNodeGraph.roomNodeToDrawLineFrom!=null)
        {
            DragConnectingLine(currentEvent.delta);
            GUI.changed = true; 
        }
    }
    //Drag Connecting Line From Room Node
    public void DragConnectingLine(Vector2 delta)
    {
        currentRoomNodeGraph.linePosition += delta;
    }
    //Clear line drag from a room node
    private void ClearLineDrag()
    {
        currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
        currentRoomNodeGraph.linePosition = Vector2.zero;
        GUI.changed = true;
    }

    //Draw connections in the graph window between nodes
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private void DrawRoomConnections()
    {
        //Loop through all nodes
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.childRoomNodeIDList.Count>0)
            {
                //loop through child room nodes
                foreach (string childRoomNodeID in roomNode.childRoomNodeIDList)
                {
                    if (currentRoomNodeGraph.roomNodeDictionary.ContainsKey(childRoomNodeID))
                    {
                        DrawConnectionLine(roomNode, currentRoomNodeGraph.roomNodeDictionary[childRoomNodeID]);
                        GUI.changed = true;
                    }
                }
            }
        }
    }
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    //Draw connection line between the parent room node and child room node
    private void DrawConnectionLine(RoomNodeSO parentRoomNode,RoomNodeSO childRoomNode)
    {
        //get line start and end position
        Vector2 startPosition = parentRoomNode.rect.center;
        Vector2 endPosition= childRoomNode.rect.center;

        //calculate midway point
        Vector2 midPosition = (endPosition + startPosition) / 2f;

        //vector from start to end position of line
        Vector2 direction=endPosition - startPosition;

        //Calculate normalised prependicular positions from the mid point
        Vector2 arrowTailPoint1= midPosition - new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;
        Vector2 arrowTailPoint2= midPosition + new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;

        //Calculate mid point offset position for arrow head
        Vector2 arrowHeadPoint = midPosition + direction.normalized * connectingLineArrowSize;

        //Draw Arrow
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint1, arrowHeadPoint, arrowTailPoint1, Color.white, null, connectingLineWidth);
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint2, arrowHeadPoint, arrowTailPoint2, Color.white, null, connectingLineWidth);

        //Draw line
        Handles.DrawBezier(startPosition, endPosition, startPosition, endPosition, Color.white, null, connectingLineWidth);

        GUI.changed = true;
    }


    //Draw the room nodes in the graph window
    private void DrawRoomNodes()
    {
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            roomNode.Draw(roomNodeStyle);
        }
        GUI.changed = true;
    }


}
