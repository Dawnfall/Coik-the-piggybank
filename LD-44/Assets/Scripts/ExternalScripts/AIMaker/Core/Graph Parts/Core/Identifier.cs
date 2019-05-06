using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aim
{
    [System.Serializable]
    public class Identifier 
    {
        AIAgent m_aiAgent;
        [SerializeField] [HideInInspector] int m_nextID = 0;
        [SerializeField] [HideInInspector] List<int> m_missedIDs = new List<int>();
        public Dictionary<int, IIdable> IDs { get; private set; }

        public Identifier(AIAgent aiAgent)
        {
            m_aiAgent = aiAgent;
            IDs = new Dictionary<int, IIdable>();
        }

        private int GetNextID()
        {
            if (m_missedIDs.Count > 0)
            {
                int reUsedID = m_missedIDs[m_missedIDs.Count - 1];
                m_missedIDs.RemoveAt(m_missedIDs.Count - 1);
                return reUsedID;
            }
            return m_nextID++;
        }
        private void RemoveID(int id)
        {
            m_missedIDs.Add(id);
        }

        /// <summary>
        /// Use this method to create any IIdable object; AImaker system manages these objects internally; 
        /// every object created by this method should be destroyed with DestroyIdable()
        /// </summary>
        /// <typeparam name="T"> Idable type that you want to instantiate; must have empty constructor </typeparam>
        /// <returns> newly created Idable object </returns>
        public T CreateIdable<T>() where T : IIdable, new()
        {
            T newIdable = new T();

            newIdable.ID = GetNextID();
            IDs[newIdable.ID] = newIdable;

            newIdable.AgentAI = m_aiAgent;
            newIdable.OnCreate();

            return newIdable;
        } //TODO: maybe no aiAgent parameter
        public IIdable CreateIdable(System.Type type)
        {
            if (typeof(IIdable).IsAssignableFrom(type))
            {
                IIdable newIdable = System.Activator.CreateInstance(type) as IIdable;//TODO: check for empty constructor

                newIdable.ID = GetNextID();
                IDs[newIdable.ID] = newIdable;

                newIdable.AgentAI = m_aiAgent;
                newIdable.OnCreate();

                return newIdable;
            }
            return null;
        }
        public bool DestroyID(IIdable idable)
        {
            if (idable != null && idable.ID >= 0)
            {
                idable.OnDestroy();
                IDs.Remove(idable.ID);

                RemoveID(idable.ID);
                idable.ID = -1;
                idable.AgentAI = null;

                return true;
            }
            return false;
        }

        public T GetAIObject<T>(int id) where T : class, IIdable //TODO maybe put in Identifier
        {
            IIdable o;
            if (IDs.TryGetValue(id, out o))
                return o as T;
            return null;
        }
    }
}