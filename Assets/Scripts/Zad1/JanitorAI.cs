using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JanitorAI : MonoBehaviour
{
    public List<Transform> trashTransforms;
    public Rigidbody2D rb;
    public Animator animator;
    public Sprite sprite;
    public Transform blueGarbageTransform;
    public Transform redGarbageTransform;

    public Transform target;
    public Transform carryPosition;
    public float janitorVelocity = 3;
    public float janitorReach = 0.5f;

    void Start()
    {
        SpawnerController.OnSpawn += SpawnerController_OnSpawn;
        rb = gameObject.GetComponent<Rigidbody2D>();
        trashTransforms = new List<Transform>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (rb.velocity.x > 0f)
        {
            transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, 0f);
        }
        else if(rb.velocity.x < 0f)
        {
            transform.rotation = new Quaternion(transform.rotation.x , 180f, transform.rotation.z, 0f);
        }
        animator.SetFloat("movementVel", rb.velocity.x);
    }

    private void SpawnerController_OnSpawn(Transform[] obj)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            trashTransforms.Add(obj[i]);
        }
    }

    public void DestroyCarryingTrash()
    {
        target.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        target.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        target.gameObject.GetComponent<PoolableObject>().ReturnToPool();
    }
}
