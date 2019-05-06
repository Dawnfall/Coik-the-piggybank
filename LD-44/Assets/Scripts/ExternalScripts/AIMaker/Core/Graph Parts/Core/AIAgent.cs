using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System;
using aim.FullSerializer;
using aim.FullSerializer.Internal;

namespace aim
{ 
    public class AIAgent : MonoBehaviour, ISerializationCallbackReceiver
    {
        public Identifier Identifier;
        public GraphAI GraphAI;
        public BlackBoard Blackboard;

        private void Awake() //TODO: init out of awake
        {
            Identifier = new Identifier(this);
            GraphAI = Identifier.CreateIdable<GraphAI>();
            Blackboard = new BlackBoard();

            foreach (IIdable idable in Identifier.IDs.Values)
                idable.OnAwake();
        }

        //************************
        // MAIN FUNCTIONALITY
        //************************

        private void Start()
        {
            foreach (IIdable idable in Identifier.IDs.Values)
                idable.OnStart();
        }
        private void OnEnable()
        {
            foreach (IIdable idable in Identifier.IDs.Values)
                idable.OnEnable();
        }
        private void OnDisable()
        {
            foreach (IIdable idable in Identifier.IDs.Values)
                idable.OnDisable();
        }

        private void Update()
        {
            if (GraphAI != null)
                GraphAI.OnTick();
        }

        //***********************
        // SERIALIZATION
        //***********************

        [SerializeField] [HideInInspector] AIAgentSerializedData m_serData;

        public void OnBeforeSerialize()
        {
            m_serData = new AIAgentSerializedData();
            m_serData.fsSerialize(this);
        }
        public void OnAfterDeserialize()
        {
            if (m_serData != null)
            {
                if (m_serData.fsDeserialize(this))
                {
                    Identifier = m_serData.Identifier;
                    Blackboard = m_serData.BlackBoard;
                    GraphAI = m_serData.RootGraph;
                }
            }
        }


#if UNITY_EDITOR

        public bool IsDirty_E { get; set; }
        public IGraphOwner ActiveGraphOwner_E { get; set; }
        public void OnInspectorBlackboardDraw_E(Rect inspectorRect)
        {
            ConfigAI_E config = ConfigAI_E.Instance;

            //***********************
            // Add / Clear

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                List<RegisteredType> allRegTypes = ConfigAI_E.Instance.GetAllBBTypes();

                GenericMenu addMenu = new GenericMenu();
                foreach (RegisteredType regType in allRegTypes)
                {
                    addMenu.AddItem(new GUIContent(regType.CreatePath), false, () =>
                    {
                        Blackboard.CreateEntry(Blackboard.GetValidName(), regType.Type);
                        IsDirty_E = true;
                    });
                }
                addMenu.ShowAsContext();
            }
            if (GUILayout.Button("Clear"))
            {
                Blackboard.Clear();
                IsDirty_E = true;
            }
            EditorGUILayout.EndHorizontal();

            //**********************
            // All entries

            foreach (ABBEntry bbEntry in Blackboard.AllEntriesList)
            {
                EditorGUILayout.BeginHorizontal();

                bbEntry.Key = PropertyAISerializer.DrawInstance(bbEntry.Key, typeof(string), "", this) as string;
                bbEntry.ValueAsObject = PropertyAISerializer.DrawInstance(bbEntry.ValueAsObject, bbEntry.GetValueType(), "", this);

                if (GUILayout.Button("x"))
                {
                    Blackboard.RemoveEntry(bbEntry);
                    IsDirty_E = true;
                }

                EditorGUILayout.EndHorizontal();
            }
        }

#endif
    }




    //public void RegisterUnityObject(IAgentAIAsignable graphableAI, string propertyName, UnityEngine.Object unityObject)
    //{
    //    m_unityReferences.Add(unityObject);
    //    int index = m_unityReferences.Count - 1;

    //    if (!m_propertyToUnityReferences.ContainsKey(graphableAI))
    //        m_propertyToUnityReferences.Add(graphableAI, new Dictionary<string, int>());
    //    if (m_propertyToUnityReferences[graphableAI].ContainsKey(propertyName))
    //    {
    //        Debug.Log("Property name for this graphableAI already registered!");
    //        m_unityReferences.RemoveAt(index);
    //        return;
    //    }
    //    m_propertyToUnityReferences[graphableAI].Add(propertyName, index);
    //}
    //public UnityEngine.Object GetRegisteredUnityObject(IAgentAIAsignable graphableAI, string propertyName)
    //{
    //    Dictionary<string, int> currReference;
    //    if (!m_propertyToUnityReferences.TryGetValue(graphableAI, out currReference))
    //    {
    //        Debug.Log("This node has no serialized unity references registered!");
    //        return null;
    //    }

    //    int index;
    //    if (currReference.TryGetValue(propertyName, out index))
    //    {
    //        if (index >= 0 && index < m_unityReferences.Count)
    //            return m_unityReferences[index];
    //    }
    //    Debug.Log("Property is not registered!");
    //    return null;


    //}

}

