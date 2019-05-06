using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using aim.FullSerializer;

namespace aim
{
    public abstract class ACompositeNode : ANodeAI
    {
        [fsProperty] [HideInInspector] public List<ANodeAI> Children { get; private set; }

        public ACompositeNode()
        {
            Children = new List<ANodeAI>();
        }

        public bool ConnectChild(ANodeAI childToConnect)
        {
            return ConnectChild(childToConnect, Children.Count);
        }
        public bool ConnectChild(ANodeAI newChild, int index)
        {
            if (newChild == null || newChild.Parent != null)
            {
                Debug.Log("Connection failed!");
                return false;
            }

            if (index < 0)
                index = 0;
            else if (index > Children.Count)
                index = Children.Count;

            //HashSet<ANodeAI> outNodeChildren = new HashSet<ANodeAI>();
            //newChild.FindNodesInSubgraph(outNodeChildren);
            //if (outNodeChildren.Contains(this))
            //{
            //    Debug.Log("Connection failed! Circular graph would be created!");
            //    return false;
            //}

            Children.Insert(index, newChild);
            newChild.Parent = this;
            return true;
        }

        public sealed override bool TryDisconnectChild(ANodeAI childToDisconnect)
        {
            if (childToDisconnect != null && Children.Remove(childToDisconnect))
            {
                childToDisconnect.Parent = null;
                return true;
            }
            return false;
        }
        public bool TryDisconnectChild(int index)
        {
            if (index >= 0 && index < Children.Count)
            {
                ANodeAI childToDisconnect = Children[index];

                Children.RemoveAt(index);
                childToDisconnect.Parent = null;

                return true;
            }
            return false;
        }

        public sealed override void DisconnectAll()
        {
            while (Children.Count > 0)
                TryDisconnectChild(Children.Count - 1);

            if (m_parent != null)
                Parent.TryDisconnectChild(this);
        }

        public sealed override void Terminate()
        {
            foreach (ANodeAI child in Children)
                if (child != null && child.Status == EStatusAI.RUNNING)
                    child.Terminate();
            OnTerminate();

            Status = EStatusAI.INVALID;
        }

#if UNITY_EDITOR

        public override List<ANodeAI> GetAllChildren_E()
        {
            return new List<ANodeAI>(Children);
        }


        public override Rect[] GetOutPointScreenRects_E(Vector2 windowSize) //TODO: correct zooming!
        {
            Rect[] outRects = new Rect[Children.Count * 2 + 1];
            float totalWidth = outRects.Length * ConnectionPointSize_E.x;

            Vector2 topLeft = Position_E + new Vector2(-totalWidth / 2f, Size_E.y / 2f);

            for (int i = 0; i < outRects.Length; i++) //TODO
            {
                Vector2 currTopLeft = new Vector2(topLeft.x + i * ConnectionPointSize_E.x, topLeft.y);
                currTopLeft = GraphAI.WorldToScreenPoint_E(currTopLeft, windowSize);
                outRects[i] = new Rect(currTopLeft, ConnectionPointSize_E * GraphAI.Zoom_E);
            }
            return outRects;

        }

        public override bool ConnectToOutPoint_E(ANodeAI nodeToConnect, int outPointIndex)
        {
            return ConnectChild(nodeToConnect, outPointIndex / 2);
        }

        public override void OnGUIEarly_E(Vector2 windowSize)
        {
            base.OnGUIEarly_E(windowSize);

            Rect[] outPoints = GetOutPointScreenRects_E(windowSize);

            for (int i = 0; i < Children.Count; i++)
            {
                Rect inPoint = Children[i].GetInPointScreenRect_E(windowSize);
                Handles.DrawBezier(
                    outPoints[i * 2 + 1].center, inPoint.center,
                    outPoints[i * 2 + 1].center + Vector2.up * 40, inPoint.center + Vector2.down * 40,
                    Color.yellow, null, 5);
            }
        }
#endif
    }

}