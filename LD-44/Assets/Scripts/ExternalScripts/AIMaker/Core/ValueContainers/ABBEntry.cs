using System;
using aim.FullSerializer;
using System.Collections.Generic;

namespace aim
{

    public abstract class ABBEntry : IValueContainerAI
    {
        public ABBEntry(BlackBoard blackboard, string key)
        {
            m_bb = blackboard;
            m_key = key;
        }

        [fsProperty] private BlackBoard m_bb = null;
        [fsProperty] private string m_key = "";

        public BlackBoard Blackboard
        {
            get { return m_bb; }
            set
            {
                if (value == null && m_bb != null)
                {
                    Dictionary<string, ABBEntry> allEntries = m_bb.AllEntriesSource;
                    allEntries.Remove(m_key);
                    m_bb = null;
                }
            }
        }
        public string Key
        {
            get { return m_key; }
            set
            {
                if (m_bb == null || m_key == value)
                    return;

                Dictionary<string, ABBEntry> m_allEntries = m_bb.AllEntriesSource;
                if (m_allEntries.ContainsKey(value))
                    return;

                m_allEntries.Remove(m_key);
                m_key = value;
                m_allEntries.Add(value, this);
            }
        }
        public bool IsValid
        {
            get { return m_bb != null; }
        }

        public abstract Type GetValueType();
        public abstract object ValueAsObject { get; set; }
    }
}