using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefabsClock;
    public Stack<GameObject> objectPool = new Stack<GameObject>();

    private static ObjectPool _instance;
    public static ObjectPool Initializing 
    {
        get
        {
            if(_instance == null)
            {
                GameObject oPool = new GameObject("ObjectPool");
                oPool.AddComponent<ObjectPool>();
            }
            return _instance;
        }
    
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else
        {
            Debug.Log("Destroying other ObjectPools..");
            Destroy(gameObject);
        }

    }

    /// <summary>
    /// Why i use start instead of awake method ?
    /// -Because, if this object is a singleton, I want it to fill
    /// else if it will already be destroyed in awake.
    /// </summary>
    private void Start()
    {
        // fill the object pool with 35 clock
        FillPool(35);
    }

    public GameObject GetAtPool()
    {
        if(objectPool.Count > 0)
        {
            GameObject newClock = objectPool.Pop();
            newClock.SetActive(true);
            return newClock;
        }
        return Instantiate(prefabsClock);
    }

    public void AddPool(GameObject newObject)
    {
        newObject.SetActive(false);
        objectPool.Push(newObject);
    }

    public void FillPool(int value)
    {
        for (int i = 0; i < value; i++)
        {
            GameObject insObject = Instantiate(prefabsClock);
            AddPool(insObject);
        }
    }

}
