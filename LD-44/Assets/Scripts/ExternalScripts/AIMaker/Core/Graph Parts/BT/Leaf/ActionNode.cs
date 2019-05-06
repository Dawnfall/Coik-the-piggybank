using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;
using UnityEditor;

namespace aim
{
    [NodeAIAttribute(CreatePath = "Leaf/Action Node", DefaultTypeName = "Action Node", Description = "Represent action leaf node of BT")]
    public class ActionNode : ALeafNode, IActionOwner
    {
        [fsProperty] public AActionTaskAI ActionAI { get; set; }

        protected override void OnInit()
        {
            if (ActionAI != null)
                ActionAI.OnBeginAct();
        }
        protected override EStatusAI OnUpdate()
        {
            if (ActionAI != null)
                return ActionAI.OnAct();
            return EStatusAI.FAILURE;
        }
        protected override void OnTerminate()
        {
            if (ActionAI != null)
                ActionAI.OnEndAct();
        }

        public T CreateAction<T>() where T : AActionTaskAI, new()
        {
            T newAction = AgentAI.Identifier.CreateIdable<T>();
            newAction.ParentNode = this;
            ActionAI = newAction;

            return newAction;
        }
        public AActionTaskAI CreateAction(Type actionType)
        {
            if (actionType != null && typeof(AActionTaskAI).IsAssignableFrom(actionType) && !actionType.IsAbstract)
            {
                AActionTaskAI newAction = AgentAI.Identifier.CreateIdable(actionType) as AActionTaskAI;
                newAction.ParentNode = this;
                ActionAI = newAction;
            }
            return null;
        }
        public bool DestroyAction(AActionTaskAI actionToRemove)
        {
            if (actionToRemove != null && actionToRemove == ActionAI)
            {
                ActionAI.ParentNode = null;
                ActionAI = null;

                actionToRemove.AgentAI.Identifier.DestroyID(actionToRemove);

                return true;
            }
            return false;
        }

        public override void OnDestroy()
        {
            DestroyAction(ActionAI);
        }

#if UNITY_EDITOR

        public override void OnInspectorGUI(Rect inspectorRect)
        {
            PropertyAISerializer.DrawPropertyAI(this, "ActionAI", AgentAI);
            if (ActionAI != null)
            {
                if (GUILayout.Button("Remove"))
                {
                    ActionAI = null;
                }
            }
            else
            {
                if (GUILayout.Button("Create"))
                {
                    ConfigAI_E config = ConfigAI_E.Instance;

                    GenericMenu menu = new GenericMenu();
                    foreach (Type actionType in config.GetDerivedTypes(typeof(AActionTaskAI), false))
                    {
                        menu.AddItem(new GUIContent(actionType.ToString()), false, () => { CreateAction(actionType); });
                    }
                    menu.ShowAsContext();
                }
            }
        }

#endif

    }
}