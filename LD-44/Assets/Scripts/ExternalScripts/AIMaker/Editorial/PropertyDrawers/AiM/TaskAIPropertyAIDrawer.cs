using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aim
{
    public class TaskAIPropertyAIDrawer : APropertyAIDrawer<ATaskAI>
    {
        public override ATaskAI Draw(ATaskAI instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            Debug.Log("here");
            //name,type,description in ostalo
            EditorGUILayout.LabelField(label);
            PropertyAISerializer.DrawDefaultPropertiesAI(instance, parentAIAgent);

            return instance;
        }
    }
}