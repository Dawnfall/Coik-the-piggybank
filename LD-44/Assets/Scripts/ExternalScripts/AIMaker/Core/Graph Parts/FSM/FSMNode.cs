using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using aim.FullSerializer;

namespace aim
{
    [GraphAIAttribute(CreatePath = "FSM", DefaultName = "FSM", Description = "Finite state machine")]
    public class FSMNode : ALeafNode
    {
        [fsProperty] public StateNode RootState { get; set; }

        [fsProperty] private StateNode m_currentState = null;
        public StateNode CurrentState
        {
            get { return m_currentState; }
            set
            {
                if (m_currentState != null)
                    foreach (AActionTaskAI action in m_currentState.Actions)
                        action.OnEndAct();

                m_currentState = value;

                if (m_currentState != null)
                    foreach (AActionTaskAI action in m_currentState.Actions)
                        action.OnBeginAct();
            }
        }

        protected override EStatusAI OnUpdate() //TODO maybe redo this
        {
            foreach (AActionTaskAI action in CurrentState.Actions)
                action.OnAct();

            foreach (TransitionNode transition in CurrentState.Transitions)
            {
                if (transition.DoTransit())
                {
                    CurrentState = transition.ChangeToState;
                    break;
                }
            }
            return EStatusAI.RUNNING;
        }


#if UNITY_EDITOR



        //public override void Draw_E(Vector2 windowSize)
        //{
        //    //base.Draw_E(windowSize);

        //    //foreach (StateFSM state in m_states)
        //    //    state.Draw_E(windowSize);
        //}

        public override void ShowElementMenu_E()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () =>
            {
                GraphAI parentGraph = GraphAI;
                Instantiator.DestroyNode(this);
                parentGraph.AgentAI.IsDirty_E = true;
            });
            menu.AddItem(new GUIContent("Set as Root"), false, () => { GraphAI.RootNode = this; });
            //menu.AddItem(new GUIContent("Add state"), false, () => { }) //TODO select to create default root node
            menu.ShowAsContext();
        }

#endif



    }
}