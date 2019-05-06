using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;
using UnityEditor;

namespace aim
{
    public abstract class AElementAI : IIdable, IDrawable
    {
        [fsProperty] [HideInInspector] public GraphAI GraphAI { get; set; }

        //*****************
        // IDable
        [FullSerializer.fsProperty] [HideInInspector] public AIAgent AgentAI { get; set; }
        [FullSerializer.fsProperty] [HideInInspector] public int ID { get; set; }
        [FullSerializer.fsProperty] [HideInInspector] public string Tag { get; set; }

        public BlackBoard BB { get { return AgentAI.Blackboard; } }

        public virtual void OnCreate() { }
        public virtual void OnDestroy() { }

        public virtual void OnAwake() { }
        public virtual void OnStart() { }
        public virtual void OnDisable() { }
        public virtual void OnEnable() { }

#if UNITY_EDITOR

        [HideInInspector]
        public Vector2 Position_E
        {
            get;
            set;
        }
        [HideInInspector]
        public string Name_E
        {
            get;
            set;
        }
        [HideInInspector]
        public Vector2 Size_E
        {
            get
            {
                return new Vector2(100, 100);
            }
        }
        [HideInInspector]
        public string Description_E
        {
            get;
            set;
        }

        private Vector2 descScroller;

        public abstract void OnInspectorGUI(Rect inspectorRect);
        public abstract void OnGUIEarly_E(Vector2 windowSize);
        public abstract void OnGUILate_E(Vector2 windowSize);
        public abstract void ShowElementMenu_E();
        public abstract Rect GetScreenRect_E(Vector2 windowSize);

#endif
    }
}
