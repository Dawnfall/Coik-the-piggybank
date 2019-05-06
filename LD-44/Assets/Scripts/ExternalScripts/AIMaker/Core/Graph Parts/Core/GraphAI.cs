using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;
using UnityEditor;

namespace aim
{
    public sealed class GraphAI : AElementAI
    {
        [fsProperty] public IGraphOwner GraphOwner { get; set; }
        [fsProperty] public EStatusAI Status { get; private set; }
        [fsProperty] public List<ANodeAI> Nodes { get; private set; }
        [fsProperty] private ANodeAI m_rootNode;
        public ANodeAI RootNode
        {
            get { return m_rootNode; }
            set
            {
                if (value == null || Nodes.Contains(value))
                    m_rootNode = value;
            }
        }

        public GraphAI()
        {
            Nodes = new List<ANodeAI>();
        }

        //**************
        // Usable

        public T CreateNode<T>() where T : ANodeAI, new()
        {
            T newNode = AgentAI.Identifier.CreateIdable<T>();

            newNode.GraphAI = this;
            Nodes.Add(newNode);

            return newNode;
        }
        public ANodeAI CreateNode(System.Type nodeType)
        {
            if (nodeType != null && typeof(ANodeAI).IsAssignableFrom(nodeType) && !nodeType.IsAbstract)
            {
                ANodeAI newNode = AgentAI.Identifier.CreateIdable(nodeType) as ANodeAI; //TODO: check for constructor
                newNode.GraphAI = this;
                Nodes.Add(newNode);

                return newNode;
            }
            return null;
        }
        public void DestroyNode(ANodeAI nodeToRemove)
        {
            if (nodeToRemove != null && nodeToRemove.GraphAI == this)
            {
                Nodes.Remove(nodeToRemove);
                nodeToRemove.GraphAI = null;
                if (RootNode == nodeToRemove)
                    RootNode = null;

                nodeToRemove.AgentAI.Identifier.DestroyID(nodeToRemove);
            }
        }

        public void Clear()
        {
            //TODO: remove all nodes
        }

        //****************
        // Pipeline

        public EStatusAI OnTick()
        {
            Status = (RootNode != null) ? RootNode.Tick() : EStatusAI.INVALID;
            return Status;
        }
        public override void OnDestroy()
        {
            while (Nodes.Count > 0)
                DestroyNode(Nodes[Nodes.Count - 1]);
        }

#if UNITY_EDITOR

        [fsIgnore] public Tuple<ANodeAI, int> PrevSelectedBTPoint_E { get; set; }
        [fsIgnore] public StateNode SelectedState_E { get; set; }
        [fsIgnore] public TransitionNode SelectedTransition_E { get; set; }

        //*********************
        // EDITOR
        //*********************
        [fsProperty] public Vector2 ViewOffset_E { get; set; }
        [fsProperty] public float Zoom_E { get; set; }

        public override void OnGUIEarly_E(Vector2 windowSize)
        {
            if (RootNode != null)
            {
                Handles.color = Color.green;

                Vector2 graphZero = WorldToScreenPoint_E(Vector2.zero, windowSize);
                Vector2 rootNodeCenter = WorldToScreenPoint_E(RootNode.Position_E, windowSize);
                Handles.DrawLine(graphZero, rootNodeCenter);
            }

            foreach (ANodeAI node in Nodes)
            {
                node.OnGUIEarly_E(windowSize);
            }
        }
        public override void OnGUILate_E(Vector2 windowSize)
        {
            ConfigAI_E config = ConfigAI_E.Instance;
            Rect windowArea = new Rect(Vector2.zero, windowSize);

            // Root rect

            Vector2 rootSize = new Vector2(70, 30);//TODO parameter
            Rect screenRootRect = new Rect(WorldToScreenPoint_E(Vector2.zero - rootSize / 2f, windowSize), rootSize * Zoom_E);
            GUIContent rootContent = new GUIContent("ROOT");

            GUILayout.BeginArea(screenRootRect, rootContent, config.GetNodeRectStyle());
            GUILayout.EndArea();

            foreach (ANodeAI node in Nodes)
            {
                node.OnGUILate_E(windowSize);
            }


            if (AIEditorWindow.IsMouseReleased(1) && AIEditorWindow.DetectInRect(windowArea))
            {
                ShowCreateMenu_E(windowSize);
            }
            else if (AIEditorWindow.GetMouseWheelDelta() != 0 && AIEditorWindow.DetectInRect(windowArea, false))
            {
                Zoom_E += AIEditorWindow.GetMouseWheelDelta() * config.m_zoomSpeed;
                Zoom_E = Mathf.Clamp(Zoom_E, config.m_minZoom, config.m_maxZoom);
            }
            else if (AIEditorWindow.GetMouseDragDelta(0) != Vector2.zero)
            {
                if (AIEditorWindow.CurrDraggedDrawable_E == null)
                    ViewOffset_E += Event.current.delta;
                else
                    AIEditorWindow.CurrDraggedDrawable_E.Position_E += Event.current.delta;
            }
            else if (AIEditorWindow.IsMouseReleased(0))
                AIEditorWindow.CurrDraggedDrawable_E = null;


        }
        public void OnEditorUpdate_E(Vector2 windowSize)
        {
        }
        public void OnInspectorDraw_E(Rect inspectorRect)
        {
            foreach (ANodeAI node in new List<ANodeAI>(Nodes))
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(node.Name_E);
                if (GUILayout.Button("C"))
                {
                    CenterOnDrawable(node);
                }
                if (GUILayout.Button("X"))
                {
                    Instantiator.DestroyNode(node);
                    //TODO...remove selected

                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Clear"))
            {
                Clear();
                AIEditorWindow.SelectedDrawable_E = null;
            }
        }

        public void CenterOnDrawable(IDrawable drawable)
        {
            Vector2 nodeToScreenCenter = ViewOffset_E - drawable.Position_E;
            ViewOffset_E -= nodeToScreenCenter;
        }
        public Vector2 WorldToScreenPoint_E(Vector2 point, Vector2 windowSize) //TODO: maybe put this somewhere else
        {
            return (point - ViewOffset_E + windowSize / (2f * Zoom_E)) * Zoom_E;
        }

        public void ShowCreateMenu_E(Vector2 windowSize)
        {
            List<Type> allNodeTypes = ConfigAI_E.Instance.GetDerivedTypes(typeof(ANodeAI), false);// GetNodeTypesForGraphType(GetType());
            if (allNodeTypes == null)
                return;

            Vector2 mousePosition = AIEditorWindow.MousePosition;
            //Type thisGraphType = GetType();

            GenericMenu menu = new GenericMenu();
            foreach (Type nodeType in allNodeTypes)
            {
                string createPath = nodeType.ToString();
                object[] attributes = nodeType.GetCustomAttributes(typeof(NodeAIAttribute), true);
                if (attributes != null && attributes.Length != 0)
                {
                    NodeAIAttribute nodeAtt = attributes[0] as NodeAIAttribute;
                    createPath = nodeAtt.CreatePath;
                }
                menu.AddItem(new GUIContent(createPath), false, () =>
                {
                    ANodeAI newNode = Instantiator.CreateNode(nodeType, this);
                    newNode.Position_E = mousePosition + ViewOffset_E - windowSize / 2f;
                    newNode.Name_E = nodeType.FullName;
                    AgentAI.IsDirty_E = true;
                });

            }
            menu.ShowAsContext();
        }



        public override void OnInspectorGUI(Rect inspectorRect)
        {
            throw new NotImplementedException();
        }

        public override void ShowElementMenu_E()
        {
            throw new NotImplementedException();
        }

        public override Rect GetScreenRect_E(Vector2 windowSize)
        {
            throw new NotImplementedException();
        }

#endif
    }




    //***************************************************** BT

    //*************************
    // MAIN FUNCTIONALITY
    //*************************

    //public override void Optimize()
    //{
    //    //if (m_rootNode != null)
    //    //{
    //    //    HashSet<ANodeAI> rootSubgraphNodes = new HashSet<ANodeAI>();
    //    //    m_rootNode.FindNodesInSubgraph(rootSubgraphNodes);

    //    //    int i = m_nodes.Count - 1;
    //    //    while (i >= 0)
    //    //    {
    //    //        ANodeAI currNode = m_nodes[i];
    //    //        if (!rootSubgraphNodes.Contains(currNode))
    //    //        {
    //    //            currNode.DisconnectAll();
    //    //            m_nodes.RemoveAt(i);
    //    //        }
    //    //        else
    //    //            i--;
    //    //    }
    //    //}
    //    //else
    //    //    Debug.Log("Cannot optimize since there is no root node!");
    //}


}