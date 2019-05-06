using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using aim.FullSerializer;
using System;

namespace aim
{
    public abstract class ANodeAI : AElementAI
    {
        [fsProperty] [HideInInspector] public EStatusAI Status { get; protected set; }
        [fsProperty] [HideInInspector] protected ANodeAI m_parent = null;

        public ANodeAI Parent
        {
            get
            {
                return m_parent;
            }
            set
            {
                m_parent = value; //TODO: do some checking;
            }
        }

        public abstract bool TryDisconnectChild(ANodeAI childToDisconnect);

        public EStatusAI Tick()
        {
            if (Status == EStatusAI.INVALID)
            {
                Status = EStatusAI.RUNNING;
                OnInit();
            }

            Status = OnUpdate();

            if (Status != EStatusAI.RUNNING)
            {
                OnTerminate();
                Status = EStatusAI.INVALID;
            }

            return Status;
        }

        protected virtual void OnInit() { }
        protected abstract EStatusAI OnUpdate();
        protected virtual void OnTerminate() { }

        public virtual void Terminate() { }

        public abstract void DisconnectAll();

#if UNITY_EDITOR
        public abstract List<ANodeAI> GetAllChildren_E();

        public virtual Vector2 ConnectionPointSize_E
        {
            get { return new Vector2(10, 10); }
        }

        private Vector2 descScroller;
        public override void OnInspectorGUI(Rect inspectorRect)
        {
            descScroller = GUILayout.BeginScrollView(descScroller);

            PropertyAISerializer.DrawDefaultPropertiesAI(this, AgentAI);

            GUILayout.EndScrollView();
        }

        public virtual Rect GetInPointScreenRect_E(Vector2 windowSize)
        {
            Vector2 topLeft = new Vector2(
                -ConnectionPointSize_E.x / 2f,
                -Size_E.y / 2f - ConnectionPointSize_E.y
                ) +
                Position_E;

            topLeft = GraphAI.WorldToScreenPoint_E(topLeft, windowSize);

            return new Rect(topLeft, ConnectionPointSize_E * GraphAI.Zoom_E);
        }
        public virtual Rect[] GetOutPointScreenRects_E(Vector2 windowSize)
        {
            return new Rect[0];
        }
        public virtual Rect GetNodeStatusScreenRect_E(Vector2 windowSize)
        {
            Vector2 screenSize = (Size_E + new Vector2(12, 12)) * GraphAI.Zoom_E; //TODO: parameter!
            Vector2 screenPos = GraphAI.WorldToScreenPoint_E(Position_E, windowSize);
            return new Rect(screenPos - screenSize / 2, screenSize);

        }

        public override void OnGUILate_E(Vector2 windowSize)
        {
            Rect nodeScreenRect = GetScreenRect_E(windowSize);

            if (AIEditorWindow.IsMousePressed(0) && AIEditorWindow.DetectInRect(nodeScreenRect))
            {
                AIEditorWindow.CurrDraggedDrawable_E = this;
            }
            else if (AIEditorWindow.IsMouseReleased(1) && AIEditorWindow.DetectInRect(nodeScreenRect))
            {
                ShowElementMenu_E();
            }
            else if (AIEditorWindow.IsMousePressed(2) && AIEditorWindow.DetectInRect(nodeScreenRect))
            {
                AIEditorWindow.SelectedDrawable_E = this;
            }

            ConfigAI_E config = ConfigAI_E.Instance;

            //status 
            Rect statusRect = GetNodeStatusScreenRect_E(windowSize);
            GUILayout.BeginArea(statusRect, config.GetBTStatusStyle(Status));
            GUILayout.EndArea();

            //selected node

            if (AIEditorWindow.SelectedDrawable_E == this)
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

            // in point
            Texture conPointTexture = config.GetTexture(config.connectionPointImage);
            if (GUI.Button(GetInPointScreenRect_E(windowSize), conPointTexture, new GUIStyle()))
            {
                SelectPoint_E(-1);
            }

            // out points
            Rect[] outRects = GetOutPointScreenRects_E(windowSize);
            for (int i = 0; i < outRects.Length; i++)
            {
                if (GUI.Button(outRects[i], conPointTexture, new GUIStyle()))
                {
                    SelectPoint_E(i);
                }
            }


        }
        public override void OnGUIEarly_E(Vector2 windowSize)
        {
            if (GraphAI.PrevSelectedBTPoint_E != null && GraphAI.PrevSelectedBTPoint_E.Item1 == this)
            {
                if (GraphAI.PrevSelectedBTPoint_E.Item2 == -1)
                {
                    Rect inPoint = GetInPointScreenRect_E(windowSize);
                    Handles.DrawBezier(AIEditorWindow.MousePosition, inPoint.center, AIEditorWindow.MousePosition,
                        inPoint.center, Color.red, null, 5);
                }
                else
                {
                    Rect[] outPoints = GetOutPointScreenRects_E(windowSize);
                    Handles.DrawBezier(outPoints[GraphAI.PrevSelectedBTPoint_E.Item2].center, AIEditorWindow.MousePosition,
                                        outPoints[GraphAI.PrevSelectedBTPoint_E.Item2].center, AIEditorWindow.MousePosition, Color.red, null, 5);
                }
            }
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

        public abstract bool ConnectToOutPoint_E(ANodeAI nodeToConnect, int outPointIndex);

        public virtual void SelectPoint_E(int connectionPointIndex)
        {
            Tuple<ANodeAI, int> currSelection = new Tuple<ANodeAI, int>(this, connectionPointIndex);

            if (GraphAI.PrevSelectedBTPoint_E == currSelection)
            {
                GraphAI.PrevSelectedBTPoint_E = null;
            }

            else if (GraphAI.PrevSelectedBTPoint_E == null)
            {
                Tuple<ANodeAI, int> newSelection;
                if (DisconnectSelection_E(currSelection, out newSelection))
                    GraphAI.PrevSelectedBTPoint_E = newSelection;
                else
                    GraphAI.PrevSelectedBTPoint_E = currSelection;
            }

            else if (GraphAI.PrevSelectedBTPoint_E.Item1 != currSelection.Item1 && ((GraphAI.PrevSelectedBTPoint_E.Item2 == -1 && currSelection.Item2 >= 0) || (GraphAI.PrevSelectedBTPoint_E.Item2 >= 0 && currSelection.Item2 == -1)))
            {
                Tuple<ANodeAI, int> newSelection;
                DisconnectSelection_E(currSelection, out newSelection);
                ConnectSelection_E(currSelection);

                GraphAI.PrevSelectedBTPoint_E = newSelection;
            }
        }
        private bool ConnectSelection_E(Tuple<ANodeAI, int> currSelection)
        {
            AgentAI.IsDirty_E = true;

            Tuple<ANodeAI, int> from = (GraphAI.PrevSelectedBTPoint_E.Item2 != -1) ? GraphAI.PrevSelectedBTPoint_E : (currSelection.Item2 != -1) ? currSelection : null;
            Tuple<ANodeAI, int> to = (GraphAI.PrevSelectedBTPoint_E.Item2 == -1) ? GraphAI.PrevSelectedBTPoint_E : (currSelection.Item2 == -1) ? currSelection : null;

            if (from != null && to != null)
            {
                from.Item1.ConnectToOutPoint_E(to.Item1, from.Item2);
            }
            return false;
        }
        private bool DisconnectSelection_E(Tuple<ANodeAI, int> currSelection, out Tuple<ANodeAI, int> newSelection)
        {
            AgentAI.IsDirty_E = true;

            //in point and has parent 
            if (currSelection.Item2 == -1 && currSelection.Item1.Parent != null)
            {
                //disonnect parent and return parent in point selection
                newSelection = new Tuple<ANodeAI, int>(currSelection.Item1.Parent, currSelection.Item1.Parent.GetAllChildren_E().IndexOf(currSelection.Item1) * 2);
                currSelection.Item1.Parent.TryDisconnectChild(currSelection.Item1);
                return true;
            }

            //out point
            if (currSelection.Item2 != -1)
            {
                List<ANodeAI> children = currSelection.Item1.GetAllChildren_E();
                //decorator has child
                if (currSelection.Item1 is ADecoratorNode && children.Count == 1)
                {
                    newSelection = new Tuple<ANodeAI, int>(children[0], -1);
                    currSelection.Item1.TryDisconnectChild(children[0]);
                    return true;
                }
                //composite connected point
                else if (currSelection.Item1 is ACompositeNode && currSelection.Item2 % 2 != 0)
                {
                    int childIndex = currSelection.Item2 / 2;
                    newSelection = new Tuple<ANodeAI, int>(children[childIndex], -1);
                    currSelection.Item1.TryDisconnectChild(children[childIndex]);
                    return true;
                }
            }
            newSelection = null;
            return false;
        }

        public override void ShowElementMenu_E()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, () =>
            {
                Instantiator.DestroyNode(this);
                if (AIEditorWindow.SelectedDrawable_E == this)
                    AIEditorWindow.SelectedDrawable_E = null;
                AgentAI.IsDirty_E = true;
            });
            menu.AddItem(new GUIContent("Set as Root"), false, () => { GraphAI.RootNode = this; });
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