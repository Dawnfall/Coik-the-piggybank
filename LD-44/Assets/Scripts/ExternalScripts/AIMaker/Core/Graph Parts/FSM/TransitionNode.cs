using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using aim.FullSerializer;

namespace aim
{
    [NodeAIAttribute(CreatePath = "Transition Node", DefaultTypeName = "Transition Node", Description = "Represent transition node of FSM")]
    public class TransitionNode : AElementAI,IDrawable
    {
        [fsProperty]
        private StateNode m_parentState = null;
        [fsProperty]
        private List<ADecision> m_decisions = new List<ADecision>();
        [fsProperty]
        private StateNode m_changeState = null;

        public StateNode ParentState
        {
            get { return m_parentState; }
            set { m_parentState = value; }
        }
        public List<ADecision> Decisions
        {
            get { return m_decisions; }
        }
        public StateNode ChangeToState
        {
            get { return m_changeState; }
        }

        public T CreateDecision<T>() where T : ADecision, new()
        {
            T newDecision = new T();
            m_decisions.Add(newDecision);
            return newDecision;
        }
        public ADecision CreateDecision(System.Type decisionType)
        {
            if (decisionType != null && typeof(ADecision).IsAssignableFrom(decisionType))
            {
                ADecision newDecision = Activator.CreateInstance(decisionType) as ADecision;
                m_decisions.Add(newDecision);
                return newDecision;
            }
            return null;
        }

        public bool RemoveDecision(ADecision decisionToRemove)
        {
            return m_decisions.Remove(decisionToRemove);
        }

        public bool SetChangeToState(StateNode changeToState)
        {
            if (changeToState == null && m_changeState != null)
            {
                m_changeState.ParentTransitions.Remove(this);
                m_changeState = null;
                return true;
            }
            else if (changeToState != null && m_changeState != changeToState)
            {
                m_changeState = changeToState;
                changeToState.ParentTransitions.Add(this);
                return true;
            }
            return false;
        }

        public bool DoTransit()
        {
            foreach (ADecision decision in m_decisions)
            {
                if (!decision.Decide())
                    return false;
            }
            return true;
        }
        //public override void DisconnectAll()
        //{
        //    while (m_parentStates.Count > 0)
        //    {
        //        m_parentStates[m_parentStates.Count - 1].DisconnectTransition(this);
        //    }
        //    SetChangeToState(null);
        //}

        public void DisconnectAll()
        {
            //TODO:...
        }
#if UNITY_EDITOR

        public override void ShowElementMenu_E()
        {
            //GenericMenu menu = new GenericMenu();
            //menu.AddItem(new GUIContent("Delete"), false, () =>
            //{
            //    AIAgent aiAgent = Identifier.AgentAI;
            //    if (AIAgent.DestroyNode(this))
            //    {
            //        if (AIEditorWindow.SelectedToConnectNode == this)
            //            AIEditorWindow.SelectedToConnectNode = null;
            //        aiAgent.IsDirty_E = true;
            //    }
            //});
            //menu.AddItem(new GUIContent("Select to connect"), false, () => { AIEditorWindow.SelectedToConnectNode = this; });
            //menu.ShowAsContext();
        }

        //public override void OnGUIEarly_E(Vector2 windowSize)
        //{
        //    base.OnGUIEarly_E(windowSize);

        //    if (m_changeState != null)
        //    {
        //        Handles.DrawBezier(
        //            GraphAI.WorldToScreenPoint_E(Position_E, windowSize), GraphAI.WorldToScreenPoint_E(m_changeState.Position_E, windowSize),
        //            GraphAI.WorldToScreenPoint_E(Position_E, windowSize), GraphAI.WorldToScreenPoint_E(m_changeState.Position_E, windowSize),
        //            Color.yellow, null, 5
        //            );
        //    }
        //}

        //public override void SelectToConnect()
        //{
        //    if (AIEditorWindow.SelectedToConnectNode != null && typeof(StateNode).IsAssignableFrom(AIEditorWindow.SelectedToConnectNode.GetType()))
        //    {
        //        if ((AIEditorWindow.SelectedToConnectNode as StateNode).ConnectTransition(this))
        //            AIEditorWindow.SelectedToConnectNode = null;
        //    }
        //}

        public override Rect GetScreenRect_E(Vector2 windowSize)
        {
            Vector2 screenCenter = ParentState.GraphAI.WorldToScreenPoint_E(Position_E, windowSize);
            Vector2 screenSize = Size_E * ParentState.GraphAI.Zoom_E;

            return new Rect(screenCenter - screenSize / 2, screenSize);
        }

        private Vector2 descScroller;
        public override void OnInspectorGUI(Rect inspectorRect)
        {
            descScroller = GUILayout.BeginScrollView(descScroller);
            PropertyAISerializer.DrawDefaultPropertiesAI(this, AgentAI);
            GUILayout.EndScrollView();
        }

        public override void OnGUIEarly_E(Vector2 windowSize)
        {

        }

        public override void OnGUILate_E(Vector2 windowSize)
        {
            //ConfigAI_E config = ConfigAI_E.Instance;

            // current state TODO:... 
            //if (GraphAI.RootNode == this)
            //{
            //    Rect statusRect = GetNodeStatusScreenRect_E(windowSize);
            //    GUILayout.BeginArea(statusRect, config.GetBTStatusStyle(EStatusAI.RUNNING));
            //    GUILayout.EndArea();
            //}
            //selected node    

            // node 

            //Rect nodeRect = GetScreenRect_E(windowSize);

            //GUILayout.BeginArea(GetScreenRect_E(windowSize), config.GetNodeRectStyle());
            //GUILayout.BeginVertical();

            //// node content
            //OnDrawContent_E();

            //GUILayout.EndVertical();
            //GUILayout.EndArea();
            ////***********

            //if (AIEditorWindow.IsMouseReleased(0) && AIEditorWindow.DetectInRect(GetScreenRect_E(windowSize)))
            //{
            //    SelectToConnect();
            //}
        }
#endif
    }
}