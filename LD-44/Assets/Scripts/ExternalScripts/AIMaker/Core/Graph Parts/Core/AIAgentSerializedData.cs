using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using aim.FullSerializer;

namespace aim
{
    [System.Serializable]
    public class AIAgentSerializedData: IUnityObjectRegister
    {
        [SerializeField] [fsIgnore] [HideInInspector] public string m_serDataAsString = "";
        //[SerializeField] [fsIgnore] [HideInInspector] public bool m_isSerSuccess = false;

        [fsProperty] public Identifier Identifier { get; set; }
        [fsProperty] public BlackBoard BlackBoard { get; set; }
        [fsProperty] public GraphAI RootGraph { get; set; }

        [SerializeField] [fsIgnore] public List<UnityEngine.Object> m_unityObjectReferences = new List<UnityEngine.Object>();

        public int RegisterUnityObject(UnityEngine.Object uo)
        {
            m_unityObjectReferences.Add(uo);
            return m_unityObjectReferences.Count - 1;
        }
        public UnityEngine.Object GetRegisteredUO(int index)
        {
            if (index >= 0 && index < m_unityObjectReferences.Count)
                return m_unityObjectReferences[index];

            return null;
        }

        public bool fsSerialize(AIAgent aiAgent)
        {
            Identifier = aiAgent.Identifier;
            BlackBoard = aiAgent.Blackboard;
            RootGraph = aiAgent.GraphAI;

            FullSerializer.fsSerializer fsSerializer = new FullSerializer.fsSerializer();
            FullSerializer.fsData fsData;

            fsResult result = fsSerializer.TrySerialize(typeof(AIAgentSerializedData), this, out fsData, this);
            if (result.Succeeded)
            {
                m_serDataAsString = FullSerializer.fsJsonPrinter.CompressedJson(fsData);
            }
            else
            {
                Debug.Log("AIAgentSerializationData serialization failed!\n " + result.FormattedMessages);
                m_serDataAsString = "";

            }
            Identifier = null;
            BlackBoard = null;
            RootGraph = null;

            return result.Succeeded;
        }
        public bool fsDeserialize(AIAgent aiAgent)
        {
            if (aiAgent == null)
                return false;
            FullSerializer.fsSerializer fsSerializer = new FullSerializer.fsSerializer();
            FullSerializer.fsData fsData;

            FullSerializer.fsResult parseResult = FullSerializer.fsJsonParser.Parse(m_serDataAsString, out fsData);
            if (parseResult.Succeeded)
            {
                object deserializedObj = null;
                FullSerializer.fsResult deserResult = fsSerializer.TryDeserialize(fsData, typeof(AIAgentSerializedData), ref deserializedObj, this);
                if (deserResult.Succeeded)
                {
                    AIAgentSerializedData serData = deserializedObj as AIAgentSerializedData;
                    Identifier = serData.Identifier;
                    BlackBoard = serData.BlackBoard;
                    RootGraph = serData.RootGraph;

                    return true;
                }
            }
            else
            {
                Debug.Log("AIAgentSerializationData string to fsData parse failed!" + parseResult.RawMessages);
            }
            return false;
        }
    }
}