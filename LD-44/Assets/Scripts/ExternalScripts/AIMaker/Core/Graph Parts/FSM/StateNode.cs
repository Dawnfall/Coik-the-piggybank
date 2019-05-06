using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using aim.FullSerializer;

/*
        [fsProperty]
        private List<AActionTaskAI> m_startActions = new List<AActionTaskAI>();
                [fsProperty]
        private List<AActionTaskAI> m_exitActions = new List<AActionTaskAI>();
 */
namespace aim
{
    [NodeAIAttribute(CreatePath = "State Node", DefaultTypeName = "State Node", Description = "Represents State node of FSM")]
    public class StateNode : AElementAI, IDrawable
    {
        [fsProperty] public List<TransitionNode> ParentTransitions { get; private set; }
        [fsProperty] public List<AActionTaskAI> Actions { get; private set; }
        [fsProperty] public List<TransitionNode> Transitions { get; private set; }

        public StateNode()
        {
            ParentTransitions = new List<TransitionNode>();
            Actions = new List<AActionTaskAI>();
            Transitions = new List<TransitionNode>();
        }

        public T CreateAction<T>() where T : AActionTaskAI, new() //TODO: must do with instantiator!
        {
            T newAction = new T();
            Actions.Add(newAction);
            return newAction;
        }
        public AActionTaskAI CreateAction(Type actionType) //TODO: same
        {
            if (!typeof(AActionTaskAI).IsAssignableFrom(actionType))
                return null;

            AActionTaskAI newAction = System.Activator.CreateInstance(actionType) as AActionTaskAI;
            Actions.Add(newAction);

            return newAction;
        }
        public bool RemoveAction(AActionTaskAI action) //same
        {
            return Actions.Remove(action);
        }

        public TransitionNode CreateTransition() //same
        {
            TransitionNode newTransition = AgentAI.Identifier.CreateIdable<TransitionNode>();

            newTransition.ParentState = this;
            Transitions.Add(newTransition);
            return newTransition;
        }
        public bool DestoryTransition(TransitionNode transition) //TODO: remove decisions, parentTransition from childState
        {
            if (transition != null && transition.ParentState == this)
            {
                transition.ParentState = null;
                Transitions.Remove(transition);
                AgentAI.Identifier.DestroyID(transition);
                return true;
            }
            return false;
        }

        public void DisconnectAll()
        {
            while (ParentTransitions.Count > 0)
            {
                ParentTransitions[ParentTransitions.Count - 1].SetChangeToState(null);
            }
            while (Transitions.Count > 0)
            {
                DestoryTransition(Transitions[Transitions.Count - 1]);
            }
        }

#if UNITY_EDITOR

        public override void OnGUIEarly_E(Vector2 windowSize)
        {
            if (GraphAI.SelectedState_E == this)
            {
                Handles.DrawBezier(AIEditorWindow.MousePosition, GraphAI.WorldToScreenPoint_E(Position_E, windowSize), AIEditorWindow.MousePosition,
                     GraphAI.WorldToScreenPoint_E(Position_E, windowSize), Color.red, null, 5);
            }

            foreach (TransitionNode transition in Transitions)
            {
                Handles.DrawBezier(
                    GraphAI.WorldToScreenPoint_E(Position_E, windowSize), GraphAI.WorldToScreenPoint_E(transition.Position_E, windowSize),
                    GraphAI.WorldToScreenPoint_E(Position_E, windowSize), GraphAI.WorldToScreenPoint_E(transition.Position_E, windowSize),
                    Color.yellow, null, 5
                    );
            }
        }
        public override void OnGUILate_E(Vector2 windowSize)
        {
            ConfigAI_E config = ConfigAI_E.Instance;

            // current state TODO:... 
            //if (GraphAI.RootNode == this)
            //{
            //    Rect statusRect = GetNodeStatusScreenRect_E(windowSize);
            //    GUILayout.BeginArea(statusRect, config.GetBTStatusStyle(EStatusAI.RUNNING));
            //    GUILayout.EndArea();
            //}
            //selected node

            if (GraphAI.SelectedState_E == this)
            {
                Rect rect = GetScreenRect_E(windowSize);
                Vector2 selectedSize = rect.size * (1.08f);//TODO size percent to parameter
                Rect selectedRect = new Rect(rect.center - selectedSize / 2f, selectedSize);

                GUILayout.BeginArea(selectedRect, config.GetSelectedStyle());
                GUILayout.EndArea();
            }

            // node 

            //Rect nodeRect = GetScreenRect_E(windowSize);

            GUILayout.BeginArea(GetScreenRect_E(windowSize), config.GetNodeRectStyle());
            GUILayout.BeginVertical();

            // node content
            OnDrawContent_E();

            GUILayout.EndVertical();
            GUILayout.EndArea();
            //***********

            if (AIEditorWindow.IsMouseReleased(0) && AIEditorWindow.DetectInRect(GetScreenRect_E(windowSize)))
            {
                SelectToConnect();
            }
        }

        public virtual void SelectToConnect()
        {
            //if (AIEditorWindow.SelectedToConnectNode != null && typeof(Transition).IsAssignableFrom(AIEditorWindow.SelectedToConnectNode.GetType()))
            //{
            //    if ((AIEditorWindow.SelectedToConnectNode as Transition).SetChangeToState(this))
            //        AIEditorWindow.SelectedToConnectNode = null;
            //}
        }
        public virtual void OnDrawContent_E()
        {
            //*********
            // Node name

            GUIStyle guiStyle = new GUIStyle();
            guiStyle.padding = new RectOffset(5, 5, 5, 5);

            GUIContent nodeNameContent = new GUIContent();
            nodeNameContent.text = Name_E;

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(nodeNameContent, GUILayout.Height(35));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        public virtual Rect GetNodeStatusScreenRect_E(Vector2 windowSize)
        {
            Vector2 screenSize = (Size_E + new Vector2(12, 12)) * GraphAI.Zoom_E; //TODO: parameter!
            Vector2 screenPos = GraphAI.WorldToScreenPoint_E(Position_E, windowSize);
            return new Rect(screenPos - screenSize / 2, screenSize);

        }

        private Vector2 descScroller;
        public override void OnInspectorGUI(Rect inspectorRect)
        {
            descScroller = GUILayout.BeginScrollView(descScroller);
            PropertyAISerializer.DrawDefaultPropertiesAI(this, AgentAI);
            GUILayout.EndScrollView();
        }

        public override void ShowElementMenu_E()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () =>
            {
                if (AIEditorWindow.SelectedDrawable_E == this)
                    AIEditorWindow.SelectedDrawable_E = null;
                AgentAI.IsDirty_E = true;
            });
            menu.ShowAsContext();
        }

        public override Rect GetScreenRect_E(Vector2 windowSize)
        {
            Vector2 screenCenter = GraphAI.WorldToScreenPoint_E(Position_E, windowSize);
            Vector2 screenSize = Size_E * GraphAI.Zoom_E;

            return new Rect(screenCenter - screenSize / 2, screenSize);
        }

#endif
    }
}