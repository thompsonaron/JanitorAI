using System.Collections;
using UnityEngine;

public class AssetProvider : AssetBaseProvider
{
    [Header("Boxes")]
    public int defaultPoolSize = 10;
    public GameObject blueBox;
    public GameObject redBox;

    private static AssetProvider _instance;
    public static AssetProvider Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<AssetProvider>("AssetProvider");
            }

            return _instance;
        }
    }

    protected override AssetBaseProvider GetInstance()
    {
        return Instance;
    }

    public static GameObject GetAsset(GameAsset asset)
    {
        return Instance.GetObjectFromPool(GameObjectForType(asset));
    }

    public static void Preload()
    {
        if (_instance == null)
        {
            _instance = Resources.Load<AssetProvider>("AssetProvider");

            _instance.poolObject = new GameObject();
            _instance.poolObject.name = "Pool";
            GameObject.DontDestroyOnLoad(_instance.poolObject);

            // box pool
            Instance.InstatiatePool(_instance.blueBox, _instance.defaultPoolSize);
            Instance.InstatiatePool(_instance.redBox, _instance.defaultPoolSize);
        }
    }

    public PoolableObject RegisterObjectAsPoolable(GameObject gameObject, GameAsset type)
    {
        var poolable = gameObject.GetComponent<PoolableObject>();
        if (poolable == null)
        {
            poolable = gameObject.AddComponent<PoolableObject>();
        }

        poolable.SetPool(GameObjectForType(type), Instance);

        return poolable;
    }

    public static GameObject GameObjectForType(GameAsset type)
    {
        switch (type)
        {
            case GameAsset.RedBox:
                return Instance.redBox;
            case GameAsset.BlueBox:
                return Instance.blueBox;
            default:
                return null;
        }
    }
}

public enum GameAsset
{
    RedBox, BlueBox
}
