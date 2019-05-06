using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using aim.FullSerializer;

namespace aim
{
    public class BlackBoard
    {
        [fsProperty] public Dictionary<string, ABBEntry> AllEntriesSource { get; private set; }

        public BlackBoard()
        {
            AllEntriesSource = new Dictionary<string, ABBEntry>();
        }

        public List<ABBEntry> AllEntriesList
        {
            get
            {
                return new List<ABBEntry>(AllEntriesSource.Values);
            }
        }

        public BBEntry<T> CreateEntry<T>(string key)
        {
            if (AllEntriesSource.ContainsKey(key))
                return null;

            BBEntry<T> newEntry = new BBEntry<T>(this, key);
            AllEntriesSource.Add(key, newEntry);
            return newEntry;
        }
        public ABBEntry CreateEntry(string key, System.Type type)
        {
            if (AllEntriesSource.ContainsKey(key))
                return null;

            var newEntryGenericType = typeof(BBEntry<>).MakeGenericType(type);
            ABBEntry newEntry = System.Activator.CreateInstance(newEntryGenericType, this, key) as ABBEntry;

            if (newEntry != null)
                AllEntriesSource.Add(key, newEntry);
            return newEntry;
        }

        public BBEntry<T> GetEntry<T>(string key)
        {
            ABBEntry result;
            AllEntriesSource.TryGetValue(key, out result);

            return result as BBEntry<T>;
        }
        public ABBEntry GetEntry(string key)
        {
            ABBEntry result;
            AllEntriesSource.TryGetValue(key, out result);

            return result;
        }

        public bool RemoveEntry(ABBEntry entryToRemove)
        {
            if (entryToRemove != null && entryToRemove.Blackboard == this && AllEntriesSource.Remove(entryToRemove.Key))
            {
                entryToRemove.Blackboard = null;
                return true;
            }
            return false;
        }
        public bool RemoveEntry(string key)
        {
            ABBEntry entryToRemove = GetEntry(key);
            return RemoveEntry(entryToRemove);
        }

        public void Clear()
        {
            foreach (ABBEntry bbEntry in AllEntriesList)
            {
                RemoveEntry(bbEntry);
            }
        }

        public string GetValidName() //TODO: improve this
        {
            string name = "new Entry";
            int i = 1;
            while (true)
            {
                if (!AllEntriesSource.ContainsKey(name + i))
                    return name + i;
                i++;
            }
        }
    }
}