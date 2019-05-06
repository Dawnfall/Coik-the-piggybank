using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aim
{
    public class ActionAIPropertyAIDrawer : APropertyAIDrawer<AActionTaskAI>
    {
        public override AActionTaskAI Draw(AActionTaskAI instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            if (instance == null)
            {
                EditorGUILayout.LabelField("No ActionAI");
            }
            else
            {
                EditorGUILayout.BeginVertical(GUI.skin.box);

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                GUILayout.Label(instance.GetType().ToString());

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();

                PropertyAISerializer.DrawDefaultPropertiesAI(instance, parentAIAgent);

                EditorGUILayout.EndVertical();
            }

            return instance;
        }
    }
}