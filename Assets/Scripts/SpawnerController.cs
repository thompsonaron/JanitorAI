using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [Header("Spawn")]
    public float xBoundaryL = -5.5f;
    public float xBoundaryR = 5.5f;
    public float yBoundaryT = 3f;
    public float yBoundaryB = 1.5f;

    [Range(3f, 5f)]
    public float delay = 2.9f;
    [Range(1, 10f)]
    public int trashAmount = 3;

    public static event Action<Transform[]> OnSpawn;

    private void Awake()
    {
        AssetProvider.Preload();
    }

    void Start()
    {
        StartCoroutine(Spawn(delay));
    }

    private IEnumerator Spawn(float endDelay)
    {
        SpawnTrash();
        yield return new WaitForSeconds(endDelay);
        StartCoroutine(Spawn(endDelay));
    }

    public void SpawnTrash()
    {
        Transform[] trash = new Transform[trashAmount];

        for (int i = 0; i < trash.Length; i++)
        {
            Vector2 randomPos = new Vector2(UnityEngine.Random.Range(xBoundaryL, xBoundaryR), UnityEngine.Random.Range(yBoundaryT, yBoundaryB));
            var randomBox = HelperFunctions.RandomEnumElement<GameAsset>();
            trash[i] = AssetProvider.GetAsset(randomBox).transform;
            trash[i].transform.position = randomPos;
        }
        OnSpawn?.Invoke(trash);
    }

    void OnDrawGizmos()
    {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(xBoundaryL, yBoundaryT, 0), new Vector3(xBoundaryR, yBoundaryT, 0));
            Gizmos.DrawLine(new Vector3(xBoundaryL, yBoundaryB, 0), new Vector3(xBoundaryR, yBoundaryB, 0));
    }
}