using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aim
{
    public class HudAI
    {
        Rect GetTitleInspectorRect()
        {
            return new Rect(Vector2.zero, new Vector2(ConfigAI_E.Instance.m_hudWidth, 30));
        }
        Rect GetAIAgentInspectorRect()
        {
            return new Rect(new Vector2(0, 30), new Vector2(ConfigAI_E.Instance.m_hudWidth, 100));
        }
        Rect GetGraphInspectorRect()
        {
            return new Rect(new Vector2(0, 130), new Vector2(ConfigAI_E.Instance.m_hudWidth, 400));
        }
        Rect GetNodeInspectorRect()
        {
            return new Rect(new Vector2(0, 530), new Vector2(ConfigAI_E.Instance.m_hudWidth, 500));
        }

        Rect GetBBHeaderRect(Vector2 windowSize)
        {
            ConfigAI_E config = ConfigAI_E.Instance;
            Vector2 topLeft = new Vector2(windowSize.x - config.m_bbWidth, 0);
            Vector2 headerSize = new Vector2(config.m_bbWidth, 30);

            return new Rect(topLeft, headerSize);
        }
        Rect GetBBBodyRect(Vector2 windowSize)
        {
            ConfigAI_E config = ConfigAI_E.Instance;
            Vector2 topLeft = new Vector2(windowSize.x - config.m_bbWidth, 0);
            Vector2 bodySize = new Vector2(config.m_bbWidth, config.m_bbHeight);

            return new Rect(topLeft + new Vector2(0, 30), bodySize);
        }

        Vector2 bbScroller;

        public void DrawHUD(AIEditorWindow window)
        {
            DrawTitlePart(window.ActiveGraph);

            if (ConfigAI_E.Instance.m_showHUD)
            {
                DrawAIAgentInspectorPart(window.ActiveAgent);
                DrawGraphInspectorPart(window.ActiveGraph);
                DrawNodeInspecotrPart(AIEditorWindow.SelectedDrawable_E);

            }

            if (window.ActiveAgent != null)
            {
                DrawBBHeaderPart(window);
                DrawBBBodyPart(window, window.ActiveAgent);

            }
        }
        private void DrawTitlePart(GraphAI graph)
        {
            ConfigAI_E config = ConfigAI_E.Instance;
            GUILayout.BeginArea(GetTitleInspectorRect(), config.m_skin.box);
            GUILayout.BeginHorizontal();

            GUILayout.Label("AI Maker");

            if (GUILayout.Button("C", GUILayout.Width(30)))
            {
                if (graph != null)
                    graph.ViewOffset_E = Vector2.zero;
            }
            if (GUILayout.Button("H", GUILayout.Width(30)))
            {
                ConfigAI_E.Instance.m_showHUD = !ConfigAI_E.Instance.m_showHUD;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

        }
        private void DrawAIAgentInspectorPart(AIAgent aiAgent)
        {
            GUILayout.BeginArea(GetAIAgentInspectorRect(), ConfigAI_E.Instance.m_skin.box);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("AIAgent Inspector:");
            if (aiAgent != null)
            {
                string prefabType = (PrefabUtility.GetPrefabType(aiAgent) == PrefabType.Prefab) ? "Prefab" : " Scene object";
                EditorGUILayout.LabelField(aiAgent.gameObject.name + " (" + prefabType + ")");
                if (GUILayout.Button("Select"))
                {
                    Selection.activeObject = aiAgent;
                }
            }
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawGraphInspectorPart(GraphAI graph)
        {
            GUILayout.BeginArea(GetGraphInspectorRect(), ConfigAI_E.Instance.m_skin.box);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Graph Inspector:");
            if (graph != null)
            {
                graph.Name_E = EditorGUILayout.TextField("Name:", graph.Name_E);
                graph.Description_E = EditorGUILayout.TextArea(graph.Description_E, GUILayout.Height(70));
                graph.OnInspectorDraw_E(GetGraphInspectorRect());
            }
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawNodeInspecotrPart(IDrawable drawable)
        {
            GUILayout.BeginArea(GetNodeInspectorRect(), ConfigAI_E.Instance.m_skin.box);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Node Inspector:");
            if (drawable != null)
            {
                EditorGUILayout.LabelField("Type: " + drawable.Name_E);

                //EditorGUIUtility.labelWidth = 50;
                drawable.Name_E = EditorGUILayout.TextField("Name:", drawable.Name_E);

                drawable.Description_E = EditorGUILayout.TextArea(drawable.Description_E, GUILayout.Height(70));

                drawable.OnInspectorGUI(GetNodeInspectorRect());
            }
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
        }
        private void DrawBBHeaderPart(AIEditorWindow window)
        {
            GUILayout.BeginArea(GetBBHeaderRect(window.position.size), GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Blackboard");
            if (GUILayout.Button("_", GUILayout.Width(20)))
            {
                ConfigAI_E config = ConfigAI_E.Instance;
                config.m_showBB = !config.m_showBB;
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();
        }
        private void DrawBBBodyPart(AIEditorWindow window, AIAgent aiAgent)
        {
            if (ConfigAI_E.Instance.m_showBB)
            {
                Rect bbBodyRect = GetBBBodyRect(window.position.size);
                GUILayout.BeginArea(bbBodyRect, GUI.skin.box);
                bbScroller = GUILayout.BeginScrollView(bbScroller);
                GUILayout.BeginVertical();

                window.ActiveAgent.OnInspectorBlackboardDraw_E(GetBBBodyRect(window.position.size));

                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                GUILayout.EndArea();
            }
        }

        public bool IsMouseOverHud() //TODO change this
        {
            return (
                GetTitleInspectorRect().Contains(AIEditorWindow.MousePosition) ||
                (
                ConfigAI_E.Instance.m_showHUD && (
                    GetGraphInspectorRect().Contains(AIEditorWindow.MousePosition) ||
                    GetNodeInspectorRect().Contains(AIEditorWindow.MousePosition)
                ))
                );

        }
    }
}