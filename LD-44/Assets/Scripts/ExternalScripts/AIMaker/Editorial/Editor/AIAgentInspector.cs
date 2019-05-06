using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aim
{

    [CustomEditor(typeof(AIAgent))]
    public class AIAgentInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            AIAgent agentAI = target as AIAgent;

            if (GUILayout.Button("Edit graph"))
            {
                EditorWindow.GetWindow<AIEditorWindow>().ActiveAgent = agentAI;
            }
        }
    }
}