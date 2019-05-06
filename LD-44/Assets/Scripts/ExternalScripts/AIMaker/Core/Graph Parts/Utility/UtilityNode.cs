using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using aim.FullSerializer;

namespace aim
{
    [NodeAIAttribute(CreatePath = "Utility Node", DefaultTypeName = "Utility Node", Description = "Utility node for BT")]
    public class UtilityNode : ACompositeNode
    {
        [fsProperty] List<UtilityDecider> m_deciders = new List<UtilityDecider>();
        public List<UtilityDecider> Deciders
        {
            get { return m_deciders; }
        }

        public UtilityDecider AddDecider()
        {
            UtilityDecider newDecider = new UtilityDecider();

            m_deciders.Add(newDecider);
            Children.Add(null);
            newDecider.UtilityNode = this;
            return newDecider;
        }
        public bool TryRemoveDecider(UtilityDecider deciderToRemove)
        {
            if (deciderToRemove != null)
            {
                int index = m_deciders.IndexOf(deciderToRemove);
                if (index >= 0)
                {
                    Children.RemoveAt(index);
                    m_deciders.RemoveAt(index);

                    return true;
                }
            }
            return false;
        }

        //public override bool ConnectChild(ANodeBT childToConnect, int index)
        //{
        //    if (childToConnect == null || childToConnect.Parent != null || index < 0 || index >= m_children.Count || m_children[index] != null)
        //    {
        //        Debug.Log("Connection failed!");
        //        return false;
        //    }

        //    m_children[index] = childToConnect;
        //    childToConnect.Parent = this;

        //    return true;
        //}
        //public override bool ConnectChild(ANodeBT childToConnect)
        //{
        //    return ConnectChild(childToConnect, Children.Count - 1);
        //}

        //public sealed override bool TryDisconnectChild(ANodeBT childToDisconnect)
        //{
        //    if (childToDisconnect != null)
        //    {
        //        int index = Children.IndexOf(childToDisconnect);
        //        if (index >= 0)
        //        {
        //            Children[index] = null;
        //            childToDisconnect.Parent = null;
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        //public override bool TryDisconnectChild(int index)
        //{
        //    if (index >= 0 && index < Children.Count && Children[index] != null)
        //    {
        //        Children[index] = null;
        //        Children[index].Parent = null;

        //        return true;
        //    }
        //    return false;
        //}
        //public override void DisconnectAll()
        //{
        //    while (m_deciders.Count > 0)
        //        TryRemoveDecider(m_deciders[m_deciders.Count - 1]);

        //    if (Parent != null)
        //        Parent.TryDisconnectChild(this);
        //}

        protected override EStatusAI OnUpdate()
        {
            float bestUtility = 0.0f;

            ANodeAI bestChild = null;
            for (int i = 0; i < m_deciders.Count; i++)
            {
                UtilityDecider currDecider = m_deciders[i];
                ANodeAI currChild = Children[i];

                if (currChild != null)
                {
                    float currUtility = currDecider.CalculateTotalUtility();
                    if (currUtility > bestUtility)
                    {
                        bestUtility = currUtility;
                        bestChild = currChild;
                    }
                }
            }

            return (bestChild != null) ? bestChild.Tick() : EStatusAI.FAILURE;
        }

#if UNITY_EDITOR

        public override Rect[] GetOutPointScreenRects_E(Vector2 windowSize)
        {
            Vector2 connectionPointSize = new Vector2(10, 10); //TODO: parameter!

            Rect[] outPoints = new Rect[m_deciders.Count];
            for (int i = 0; i < m_deciders.Count; i++)
            {
                UtilityDecider decider = m_deciders[i];
                Vector2 topLeft = new Vector2(decider.Position_E.x - connectionPointSize.x / 2, decider.Position_E.y + decider.Size_E.y / 2);
                topLeft = GraphAI.WorldToScreenPoint_E(topLeft, windowSize);
                outPoints[i] = new Rect(topLeft, connectionPointSize * GraphAI.Zoom_E);
            }
            return outPoints;
        }
        public override void ShowElementMenu_E()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () =>
            {

                AIAgent aiAgent = AgentAI;
                Instantiator.DestroyNode(this);
                if (AIEditorWindow.SelectedDrawable_E == this)
                    AIEditorWindow.SelectedDrawable_E = null;
                aiAgent.IsDirty_E = true;


            });
            menu.AddItem(new GUIContent("Set as Root"), false, () => { GraphAI.RootNode = this; });
            menu.AddItem(new GUIContent("Add Utility Decider"), false, () =>
            {
                UtilityDecider newDecider = AddDecider();
                newDecider.Position_E = Position_E + new Vector2(0, Size_E.y * 2);
                GraphAI.AgentAI.IsDirty_E = true;
            }
            );
            menu.ShowAsContext();
        }

        public override void OnGUIEarly_E(Vector2 windowSize)
        {
            base.OnGUIEarly_E(windowSize);
            Rect[] outPoints = GetOutPointScreenRects_E(windowSize);

            for (int i = 0; i < m_deciders.Count; i++)
            {
                UtilityDecider decider = m_deciders[i];
                ANodeAI child = Children[i];

                Vector2 nodeScreenPos = GetScreenRect_E(windowSize).center;
                Vector2 deciderScreenPos = decider.GetScreenRect_E(windowSize).center;

                //inner connection

                Handles.DrawBezier(
                    nodeScreenPos, deciderScreenPos,
                    nodeScreenPos + Vector2.up * 40, deciderScreenPos + Vector2.down * 40,
                    Color.blue, null, 5
                    );

                //out connection
                if (child != null)
                {
                    Rect inPoint = child.GetInPointScreenRect_E(windowSize);
                    Handles.DrawBezier(
                        outPoints[i].center, inPoint.center,
                        outPoints[i].center + Vector2.up * 40, inPoint.center + Vector2.down * 40,
                        Color.yellow, null, 5);
                }
            }
        }
        public override void OnGUILate_E(Vector2 windowSize)
        {
            base.OnGUILate_E(windowSize);

            foreach (UtilityDecider decider in m_deciders)
                decider.OnGUILate_E(windowSize);
        }

        public override bool ConnectToOutPoint_E(ANodeAI nodeToConnect, int outPointIndex)
        {
            return ConnectChild(nodeToConnect, outPointIndex);
        }

#endif




    }
}