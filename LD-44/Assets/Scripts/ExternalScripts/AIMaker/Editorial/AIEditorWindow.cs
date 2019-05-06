using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using helper;

namespace aim
{
    /// <summary>
    /// Window for AI editing
    /// </summary>
    public class AIEditorWindow : EditorWindow //TODO: remove statics
    {
        [SerializeField] private AIAgent m_activeAgent = null;
        public AIAgent ActiveAgent
        {
            get { return m_activeAgent; }
            set
            {
                m_activeAgent = value;
            }
        }
        public GraphAI ActiveGraph
        {
            get
            {
                if (ActiveAgent != null)
                {
                    return (ActiveAgent.ActiveGraphOwner_E != null) ? ActiveAgent.ActiveGraphOwner_E.ChildGraphAI : ActiveAgent.GraphAI;
                }
                return null;
            }
        }

        public static IDrawable CurrDraggedDrawable_E
        {
            get;
            set;
        }
        public static IDrawable SelectedDrawable_E
        {
            get;
            set;
        }

        HudAI m_hud = new HudAI();
        //********************
        // CREATION
        //********************
        /// <summary>
        /// Opens the window
        /// </summary>
        [MenuItem("Window/AI Editor")]
        public static void OpenEditorWindow()
        {
            GetWindow<AIEditorWindow>("AI Editor");
        }

        //**************************
        // INIT
        //**************************

        private void OnEnable()
        {
            Reset();
        }

        public void Reset()
        {
            string[] skinPath = AssetDatabase.FindAssets("Simplex t:" + typeof(GUISkin));
            if (skinPath != null && skinPath.Length != 0)
            {
                ConfigAI_E.Instance.m_skin = AssetDatabase.LoadAssetAtPath<GUISkin>(AssetDatabase.GUIDToAssetPath(skinPath[0]));
            }
            else
            {
                Debug.Log("No skin could be loaded default will be used!");
                ConfigAI_E.Instance.m_skin = GUI.skin;
            }
            RegisterAITypes();
        }

        public void RegisterAITypes()
        {
            System.Reflection.Assembly[] assemblies = ReflectionHelper.getAllAssemblies();
            List<Type> types = ReflectionHelper.getAllTypesInAssemblies(assemblies);
            foreach (Type type in types)
            {
                if (type.IsAbstract)
                    continue;
            }
        }


        //*****************
        // DATA
        //*****************




        //***********************
        // MAIN
        //***********************

        private void OnGUI()
        {
            GUI.skin = ConfigAI_E.Instance.m_skin;
            HandleDragAndDrop();

            DrawBackground();


            if (ActiveAgent != null)
            {
                GraphAI graph = ActiveGraph;
                if (graph != null)
                {
                    graph.OnGUIEarly_E(position.size);
                    graph.OnGUILate_E(position.size);
                }
            }

            m_hud.DrawHUD(this);
        }
        private void Update()
        {
            ValidateAgent();

            if (m_changeToAgent != null)
            {
                ActiveAgent = m_changeToAgent;
                m_changeToAgent = null;
            }

            GUI.changed = true;
            if (GUI.changed)
            {
                Repaint();
            }
        }

        private void ValidateAgent()
        {
            if (ActiveAgent != null && ActiveAgent.IsDirty_E)
            {
                if (PrefabUtility.GetPrefabType(ActiveAgent) == PrefabType.Prefab)
                {
                    Debug.Log("prefab saving!");
                    EditorUtility.SetDirty(ActiveAgent);
                }
                else
                {
                    Debug.Log("scene object saving");
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(ActiveAgent.gameObject.scene);
                }

                ActiveAgent.IsDirty_E = false;
            }
        }

        //***********************
        // Drag & Drop
        //***********************

        AIAgent m_changeToAgent = null;
        public void HandleDragAndDrop()
        {
            if (Event.current.type == EventType.DragPerform || Event.current.type == EventType.DragUpdated)
                if (DragAndDrop.objectReferences.Length == 1 && DetectInRect(new Rect(Vector2.zero, position.size), false))
                {
                    GameObject objAsGO = DragAndDrop.objectReferences[0] as GameObject;
                    AIAgent aiAgent = (objAsGO == null) ? null : objAsGO.GetComponent<AIAgent>();

                    if (aiAgent == null)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                        return;
                    }

                    DragAndDrop.visualMode = DragAndDropVisualMode.Link;

                    if (Event.current.type == EventType.DragPerform)
                    {
                        if (aiAgent != null)
                            m_changeToAgent = aiAgent;
                    }
                }
        }

        //***********************
        // Background
        //***********************

        public void DrawBackground()
        {
            //Rect windowArea = new Rect(Vector2.zero, position.size);

            GUIStyle backgroundStyle = new GUIStyle();
            backgroundStyle.normal.background = ConfigAI_E.Instance.GetBackgroundTexture();

            GUILayout.BeginArea(new Rect(Vector2.zero, position.size), backgroundStyle);
            GUILayout.EndArea();

            Vector2 offset = (ActiveGraph != null) ? ActiveGraph.ViewOffset_E : Vector2.zero;

            DrawGrid(20f, offset, new Color(0.4f, 0.4f, 0.4f, 0.4f));
            DrawGrid(100f, offset, new Color(0.6f, 0.6f, 0.6f, 0.6f));
        }

        /// <summary>
        /// Draws a grid with given offset and color 
        /// </summary>
        /// <param name="gridSpacing"></param>
        /// <param name="color"></param>
        protected void DrawGrid(float gridSpacing, Vector2 offset, Color color)
        {
            int widthGridCount = Mathf.CeilToInt(position.width / gridSpacing);
            int heightGridCount = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = color;

            Vector2 currOffset = new Vector2(offset.x % gridSpacing, offset.y % gridSpacing);

            for (int i = -1; i < widthGridCount + 1; i++)
            {
                Handles.DrawLine(new Vector2(gridSpacing * i + currOffset.x, 0), new Vector2(gridSpacing * i + currOffset.x, position.height));
            }
            for (int i = -1; i < heightGridCount + 1; i++)
            {
                Handles.DrawLine(new Vector2(0, gridSpacing * i + currOffset.y), new Vector2(position.width, gridSpacing * i + currOffset.y));
            }
            Handles.EndGUI();
        }




        //*****************
        // Event helpers
        //*****************

        public static bool IsUsedEvent
        {
            get { return Event.current.type == EventType.Used; }
            set { if (value == true) Event.current.Use(); }
        }
        public static bool IsMouseReleased(int button)
        {
            return Event.current.type == EventType.MouseUp && Event.current.button == button;
        }
        public static bool IsMousePressed(int button)
        {
            return Event.current.type == EventType.MouseDown && Event.current.button == button;
        }
        public static bool IsKeyPressed(KeyCode keyCode)
        {
            return Event.current.type == EventType.KeyDown && Event.current.keyCode == keyCode;
        }
        public static bool IsKeyReleased(KeyCode keyCode)
        {
            return (Event.current.type == EventType.KeyUp && Event.current.keyCode == keyCode);
        }
        public static Vector2 MousePosition
        {
            get
            {
                return Event.current.mousePosition;
            }
        }
        public static Vector2 GetMouseDragDelta(int button)
        {
            if (Event.current.type == EventType.MouseDrag && Event.current.button == button)
                return Event.current.delta;
            return Vector2.zero;
        }
        public static float GetMouseWheelDelta()
        {
            return (Event.current.type == EventType.ScrollWheel) ? Event.current.delta.y : 0;
        }

        public static bool DetectInRect(Rect rect, bool doUse = true)
        {
            if (rect.Contains(MousePosition))
            {
                if (doUse)
                    Event.current.Use();
                return true;
            }
            return false;
        }

    }
}