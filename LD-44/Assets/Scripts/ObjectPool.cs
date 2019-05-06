using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: this should be improved

//*****************
// this class just handles object by adding/removing them from pool instead of Instatiate/Destroy

public class ObjectPool<T> where T : MonoBehaviour
{
    Transform m_poolTransform;

    private T m_prefab = null;
    private List<T> m_objectPool = null;

    public ObjectPool(T prefab, int startPoolSize, Transform poolTransform)
    {
        if (prefab == null)
            throw new System.Exception("null prefab when creating object pool!");
        if (startPoolSize < 0)
            startPoolSize = 0;

        m_poolTransform = poolTransform;

        m_prefab = prefab;
        m_objectPool = new List<T>();
        InstatiateObjects(startPoolSize);
    }
    public T GetFromPool()
    {
        if (m_objectPool.Count == 0)
            InstatiateObjects(1);

        T objectToReturn = m_objectPool[m_objectPool.Count - 1];
        m_objectPool.RemoveAt(m_objectPool.Count - 1);

        objectToReturn.transform.SetParent(null);
        return objectToReturn;
    }
    public void AddToPool(T objectToRemove)
    {
        objectToRemove.transform.SetParent(m_poolTransform);
        m_objectPool.Add(objectToRemove);
    }
    private void InstatiateObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T newGO = GameObject.Instantiate(m_prefab);

            newGO.gameObject.SetActive(false);
            AddToPool(newGO);
        }
    }
}

public class ObjectPool
{
    private GameObject m_prefab = null;
    private List<GameObject> m_unusedObjects = null;

    public ObjectPool(GameObject prefab)
    {
        if (prefab == null)
            throw new System.Exception("null prefab when creating object pool!");

        m_prefab = prefab;
        m_unusedObjects = new List<GameObject>();
    }
    public GameObject GetFromPool()
    {
        if (m_unusedObjects.Count == 0)
            InstantiateObjects(1);

        GameObject newGO = m_unusedObjects[m_unusedObjects.Count - 1];
        m_unusedObjects.RemoveAt(m_unusedObjects.Count - 1);

        newGO.SetActive(true);

        return newGO;
    }
    public void AddToPool(GameObject objectToRemove)
    {
        objectToRemove.transform.parent = null;
        objectToRemove.SetActive(false);

        m_unusedObjects.Add(objectToRemove);
    }
    public void InstantiateObjects(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newGO = GameObject.Instantiate(m_prefab);
            newGO.name = m_prefab.name;

            newGO.SetActive(false);
            m_unusedObjects.Add(newGO);
        }
    }

}

