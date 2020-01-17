using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField] private GameObject prefabsClock;
    public Stack<GameObject> objectPool = new Stack<GameObject>();

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
        return Instantiate(GetClock());
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
            GameObject insObject = Instantiate(GetClock());
            AddPool(insObject);
        }
    }
    
    GameObject GetClock()
    {
        if (!prefabsClock)
        {
            prefabsClock = GameManager.Instance.Clock;
        }
        return prefabsClock;
    }

}
