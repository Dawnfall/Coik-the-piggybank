using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : AMonoSingleton<ObjectPoolManager> //TODO:clear
{
    private Dictionary<string, ObjectPool> m_objectPools = new Dictionary<string, ObjectPool>();

    public bool AddObjectsToPool(GameObject prefab, int size)
    {
        if (prefab == null || prefab.name == "null")
        {
            Debug.Log("Adding invalid gameobject to pool!");
            return false;
        }
        ObjectPool objPool;
        if (!m_objectPools.TryGetValue(prefab.name, out objPool))
        {
            objPool = new ObjectPool(prefab);
            m_objectPools.Add(prefab.name, objPool);
        }

        objPool.InstantiateObjects(size);
        return true;
    }
    public GameObject GetObject(GameObject prefab)
    {
        ObjectPool objPool;
        if (!m_objectPools.TryGetValue(prefab.name, out objPool))
        {
            objPool = new ObjectPool(prefab);
            m_objectPools.Add(prefab.name, objPool);
        }
        return objPool.GetFromPool();
    }
    public void RemoveObject(GameObject objToRemove)
    {
        if (objToRemove == null)
            return;

        ObjectPool objPool;
        if (!m_objectPools.TryGetValue(objToRemove.name, out objPool))
            Debug.Log("This object didnt come from any pool!");
        else
            objPool.AddToPool(objToRemove);

    }
}


//public class ObjectPoolManager : AMonoSingleton<ObjectPoolManager>
//{
//private Dictionary<ActorData, ObjectPool<Actor>> m_actorPools = new Dictionary<ActorData, ObjectPool<Actor>>();

////********************
//// USE
////********************
//public bool CreateNewActorPool(ActorData actorData, int startPoolSize)
//{
//    if (actorData == null)
//    {
//        Debug.Log("Adding null actor to pool!");
//        return false;
//    }
//    if (m_actorPools.ContainsKey(actorData))
//    {
//        Debug.Log("Gameobject pool with this actorData alredy exists");
//        return false;
//    }
//    ObjectPool<Actor> newObjPool = new ObjectPool<Actor>(actorData.ActorPrefab, startPoolSize, transform);
//    m_actorPools[actorData] = newObjPool;
//    return true;
//}

//public Actor GetActorFromPool(ActorData actorData)
//{
//    ObjectPool<Actor> actorPool;
//    if (!m_actorPools.TryGetValue(actorData, out actorPool))
//    {
//        Debug.Log("No actor pool with this actorData exists!"); //TODO: can be created here instead?!?
//        return null;
//    }
//    return actorPool.GetFromPool();
//}

//public bool ReturnActorToPool(Actor actorToRemove)
//{
//    ObjectPool<Actor> actorPool;
//    if (!m_actorPools.TryGetValue(actorToRemove.ActorData, out actorPool))
//        return false;

//    actorPool.AddToPool(actorToRemove);
//    return true;
//}

////******************
//// UNITY
////******************

//protected override void OnAwake()
//{
//    base.OnAwake();

//    foreach (var actorData in ExternalMaster.Instance.GetActorData())
//    {
//        CreateNewActorPool(actorData, 10);
//    }
//}



//public bool CreateNewPool<T, U>(T prefab, U key, int startPoolSize) where T : MonoBehaviour
//{
//    if (prefab == null)
//    {
//        Debug.Log("Adding null gameobject to pool!");
//        return false;
//    }
//    if (prefab.name == "" || m_objectPools.ContainsKey(prefab.name))
//    {
//        Debug.Log("Gameobject pool with this name alredy exists");
//        return false;
//    }
//    ObjectPool<MonoBehaviour> newObjPool = new ObjectPool<MonoBehaviour>(prefab, startPoolSize);
//    m_objectPools[prefab.name] = newObjPool;
//    return true;
//}
//public T GetFromPool<T>(string name) where T:MonoBehaviour
//{
//    ObjectPool<MonoBehaviour> res;
//    if(!m_objectPools.TryGetValue(name,out res)) //TODO: now T is assumed to be of correct type
//    {
//        Debug.Log("No prefab with name: " + name + " is in the pool!");
//        return null;
//    }
//    T newObj = res.GetFromPool() as T;
//    return newObj;

//}
//public bool AddActorToPool(AActor actorToRemove)
//{
//    //ObjectPool<AActor> objPool;
//    //if (!m_actorPools.TryGetValue(actorToRemove.ActorData, out objPool))
//    //    return false;
//    //objPool.AddToPool(actorToRemove);
//    return true;
//}
