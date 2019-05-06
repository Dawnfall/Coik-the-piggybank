using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aim
{
    public class VariableAIPropertyAIDrawer : APropertyAIDrawer<AVariableAI>
    {
        public override AVariableAI Draw(AVariableAI instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            if (instance == null)
            {
                instance = System.Activator.CreateInstance(instanceType) as AVariableAI;
                return instance;
            }

            EditorGUILayout.BeginVertical(GUI.skin.box);

            GUILayout.BeginHorizontal();

            if (parentAIAgent != null && GUILayout.Button("BBEntry: "))
            {
                GenericMenu menu = new GenericMenu();
                foreach (ABBEntry e in parentAIAgent.Blackboard.AllEntriesList)
                {
                    ABBEntry bbEntry = e as ABBEntry;

                    if (bbEntry != null && bbEntry.GetValueType() == instance.GetValueType())//TODO: prolly add method on blackboard
                        menu.AddItem(new GUIContent(bbEntry.Key), false, () => { instance.Bind(bbEntry); });
                }
                menu.ShowAsContext();
            }

            ABBEntry boundEntry = instance.BBEntry;
            EditorGUILayout.LabelField((boundEntry != null) ? boundEntry.Key : "NONE");
            if (GUILayout.Button("X"))
            {
                instance.UnBind();
            }
            GUILayout.EndHorizontal();

            instance.ValueAsObject = PropertyAISerializer.DrawInstance(instance.ValueAsObject, instance.GetValueType(), "", parentAIAgent);

            EditorGUILayout.EndVertical();

            return instance;

        }
    }
}