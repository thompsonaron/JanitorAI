using System.Collections.Generic;
using UnityEngine;

public abstract class AssetBaseProvider : MonoBehaviour, IPool
{
    protected GameObject poolObject;
    
    public Dictionary<GameObject, Stack<GameObject>> pool = new Dictionary<GameObject, Stack<GameObject>>();
    
    protected abstract AssetBaseProvider GetInstance();
    
    protected GameObject GetObjectFromPool(GameObject requiredObject)
    {
        if (GetInstance().pool[requiredObject].Count > 0)
        {
            var obj = GetInstance().pool[requiredObject].Pop();
            obj.SetActive(true);
            obj.transform.SetParent(null);

            return obj;
        }
        else
        {
            return CreatePoolableObject(requiredObject);
        }
    }

    protected GameObject CreatePoolableObject(GameObject poolableObject)
    {
        var newObject = GameObject.Instantiate(poolableObject);

        var poolable = newObject.GetComponent<PoolableObject>();
        if (poolable == null)
        {
            poolable = newObject.AddComponent<PoolableObject>();
        }

        poolable.SetPool(poolableObject, GetInstance());

        return newObject;
    }

    protected void InstatiatePool(GameObject gameObject, int poolSize)
    {
        GetInstance().pool[gameObject] = new Stack<GameObject>();
        FillPool(gameObject, poolSize);
    }

    protected void FillPool(GameObject templateObject, int numberOfInstances)
    {
        var pool = GetInstance().pool[templateObject];
        for (int i = 0; i < numberOfInstances; i++)
        {
            GameObject newObject = CreatePoolableObject(templateObject);
            newObject.transform.SetParent(GetInstance().poolObject.transform);
            newObject.SetActive(false);
            pool.Push(newObject);
        }
    }


    //IPool interface implementation
    public void ReturnToPool(GameObject objectToReturn, GameObject key)
    {
        objectToReturn.SetActive(false);
        objectToReturn.transform.SetParent(poolObject.transform);
        pool[key].Push(objectToReturn);
    }
}

public interface IPool
{
    void ReturnToPool(GameObject objectToReturn, GameObject key);
}

public class PoolableObject : MonoBehaviour
{
    private IPool pool;
    private GameObject key;

    public void SetPool(GameObject key, IPool pool)
    {
        this.key = key;
        this.pool = pool;
    }

    public void ReturnToPool()
    {
        pool.ReturnToPool(gameObject, key);
    }
}
