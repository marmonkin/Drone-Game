using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float spawnInterval = 5f;
    public float objectRadius = 0.5f;

    public bool isResource;

    public List<GameObject> drones = new List<GameObject>();

    private List<GameObject> resourses = new List<GameObject>();

    private int MaxAttempts = 10;

    private BoxCollider BoxCollider;

    private void Awake()
    {
        BoxCollider = GetComponent<BoxCollider>();
        if (BoxCollider == null)
        {
            Debug.LogError("No BoxCollider found on the spawner object: " + this.name);
        }
    }

    private void Start()
    {
        if (isResource)
            StartCoroutine(SpawnResource());
    }

    /// <summary>
    /// Locate a good position to spawn in the area
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Starts spawning resources forever
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnResource()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPoint;
            bool found = FindFreePosition(out spawnPoint);

            if (found)
            {
                GameObject _resourse = Instantiate(objectToSpawn, spawnPoint, Quaternion.identity);
                resourses.Add(_resourse);
            }
            else
            {
                Debug.LogWarning("No free position found.");
            }
        }
    }

    /// <summary>
    /// Spawns a drone and assigns the correct faction to it
    /// </summary>
    /// <param name="faction"></param>
    public void SpawnDrone(Faction faction)
    {
        GameObject _newDrone = Instantiate(objectToSpawn, this.transform.position, Quaternion.identity);

        Drone _droneScript = _newDrone.GetComponent<Drone>();
        _droneScript.faction = faction;

        drones.Add(_newDrone);
    }
}