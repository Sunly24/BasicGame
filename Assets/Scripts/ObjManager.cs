using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjManager : MonoBehaviour
{
    public ObjPool Pool;

    public float speed = 10f;
    public float spawnInterval = 1f; // Time between spawns in seconds
    public float lifetime = 10f; // Time before object returns to pool

    void CreateObj()
    {
        GameObject obj = Pool.GetObj();
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Make sure the rigidbody is not kinematic
            rb.isKinematic = false;
            // Set the velocity directly in world space
            Vector3 direction = transform.forward.normalized;
            rb.velocity = direction * speed;
            Debug.Log($"Object created with velocity: {rb.velocity}, Speed: {speed}, Direction: {direction}");
        }
        else
        {
            Debug.LogWarning("No Rigidbody found on the spawned object!");
        }

        // Start coroutine to return object to pool after lifetime
        StartCoroutine(ReturnToPool(obj));
    }

    private IEnumerator ReturnToPool(GameObject obj)
    {
        yield return new WaitForSeconds(lifetime);
        if (obj != null && obj.activeInHierarchy)
        {
            Pool.ReturnObj(obj);
        }
    }

    private void Start()
    {
        // Start the automatic spawning coroutine
        StartCoroutine(AutoSpawnObjects());
    }

    private IEnumerator AutoSpawnObjects()
    {
        while (true)
        {
            CreateObj();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void Update()
    {
        // Removed automatic creation and space key input
    }
}