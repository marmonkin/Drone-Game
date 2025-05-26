using System.Collections;
using System.Drawing;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 5f;
    public float objectRadius = 0.5f;

    public bool isResource;

    private int MaxAttempts = 10;

    private BoxCollider BoxCollider;



    void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        if (BoxCollider == null)
        {
            Debug.LogError("No BoxCollider found on the spawner object: " + this.name);
        }
    }

    void Start()
    {
        if(isResource)
            StartCoroutine(SpawnResource());
    }

    private IEnumerator SpawnResource()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPoint;
            bool found = FindFreePosition(out spawnPoint);

            if (found)
            {
                Instantiate(objectToSpawn, spawnPoint, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("No free position found.");
            }
        }
    }

    private bool FindFreePosition(out Vector3 position)
    {
        Vector3 size = Vector3.Scale(BoxCollider.size, transform.lossyScale);
        Vector3 center = transform.TransformPoint(BoxCollider.center);

        for (int i = 0; i < MaxAttempts; i++)
        {
            Vector3 randomPoint = center + new Vector3(
                Random.Range(-size.x / 2f, size.x / 2f),
                0.1f,
                Random.Range(-size.z / 2f, size.z / 2f)
            );

            Collider[] colliders = Physics.OverlapSphere(randomPoint, objectRadius);
            bool isFree = true;
            foreach (Collider col in colliders)
            {
                if (col.gameObject != this.gameObject)
                {
                    isFree = false;
                    break;
                }
            }

            if (isFree)
            {
                position = randomPoint;
                return true;
            }
        }

        position = Vector3.zero;
        return false;
    }

    public void SpawnDrone(Faction faction)
    {

    }
}
