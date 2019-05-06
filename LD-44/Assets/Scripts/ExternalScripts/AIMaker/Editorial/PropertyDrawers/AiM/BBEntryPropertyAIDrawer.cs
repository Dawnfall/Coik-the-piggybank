using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aim
{
    public class BBEntryPropertyAIDrawer : APropertyAIDrawer<ABBEntry>
    {
        public override ABBEntry Draw(ABBEntry instance, System.Type instanceType, string label, AIAgent parentAIAgent)
        {
            EditorGUILayout.LabelField("BB Entry");
            if (instance == null)
            {
                EditorGUILayout.LabelField("NULL");
                return instance;
            }

            if (instance.IsValid)
            {
                instance.Key = EditorGUILayout.DelayedTextField(instance.Key);
                PropertyAISerializer.DrawPropertyAI(this, "m_value", parentAIAgent);
            }
            return instance;
        }
    }
}