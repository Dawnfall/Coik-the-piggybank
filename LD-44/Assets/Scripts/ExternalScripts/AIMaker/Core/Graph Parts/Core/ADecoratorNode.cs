using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using aim.FullSerializer;

namespace aim
{
    public abstract class ADecoratorNode : ANodeAI
    {
        [fsProperty] public ANodeAI Child { get; private set; }

        public bool ConnectChild(ANodeAI childToConnect)
        {
            if (childToConnect == null || childToConnect.Parent != null || Child == null)
            {
                Debug.Log("Connection failed!");
                return false;
            }
            //TODO:circular

            Child = childToConnect;
            childToConnect.Parent = this;
            return true;
        }

        public sealed override bool TryDisconnectChild(ANodeAI childToDisconnect)
        {
            if (childToDisconnect != null && Child == childToDisconnect)
            {
                Child.Parent = null;
                Child = null;
                return true;
            }
            return false;
        }

        public sealed override void DisconnectAll()
        {
            TryDisconnectChild(Child);
            if (Parent != null)
                Parent.TryDisconnectChild(this);
        }

        public sealed override void Terminate()
        {
            if (Child != null && Child.Status == EStatusAI.RUNNING)
                Child.Terminate();
            OnTerminate();

            Status = EStatusAI.INVALID;
        }

#if UNITY_EDITOR

        public override List<ANodeAI> GetAllChildren_E()
        {
            return (Child == null) ? new List<ANodeAI>() : new List<ANodeAI> { Child };
        }

        public override Rect[] GetOutPointScreenRects_E(Vector2 windowSize)
        {
            Vector2 topLeft = Position_E + new Vector2(-ConnectionPointSize_E.x / 2f, Size_E.y / 2f);
            topLeft = GraphAI.WorldToScreenPoint_E(topLeft, windowSize);

            return new Rect[] { new Rect(topLeft, ConnectionPointSize_E * GraphAI.Zoom_E) };

        }

        public override void OnGUIEarly_E(Vector2 windowSize)
        {
            base.OnGUIEarly_E(windowSize);

            Rect[] outPoints = GetOutPointScreenRects_E(windowSize);

            if (Child != null)
            {
                Rect childInPoint = Child.GetInPointScreenRect_E(windowSize);

                Handles.DrawBezier(
                    outPoints[0].center, childInPoint.center,
                    outPoints[0].center + Vector2.up * 40, childInPoint.center + Vector2.down * 40,
                    Color.yellow, null, 5);
            }
        }

        public override bool ConnectToOutPoint_E(ANodeAI nodeToConnect, int outPointIndex)
        {
            return ConnectChild(nodeToConnect);
        }

#endif
    }

}