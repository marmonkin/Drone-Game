using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Drone : MonoBehaviour
{
    public Faction faction;

    [SerializeField]
    private Material blueMat;
    [SerializeField]
    private Material redMat;
    [SerializeField]
    private Renderer body;


    private Transform ResourceTarget;
    private NavMeshAgent Agent;

    private bool HoldingResource = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Ping());

        if (faction == Faction.Blue)
        {
            body.material = blueMat;
        }
        else body.material = redMat;
    }

    private void Update()
    {
        if (ResourceTarget == null)
        {
            FindResource();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Resource" && !HoldingResource)
        {
            StartCoroutine(GrabResource(other.gameObject));
        }
    }

    private IEnumerator Ping()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (ResourceTarget != null && !HoldingResource)
            {
                Agent.SetDestination(ResourceTarget.position);
            }
            else
            {
                FindResource();
            }
        }
    }

    private void FindResource()
    {
        GameObject[] _resources = GameObject.FindGameObjectsWithTag("Resource");
        GameObject _resourceToGrab = null;
        float _minDist = Mathf.Infinity;

        foreach (GameObject res in _resources)
        {
            float dist = Vector3.Distance(transform.position, res.transform.position);
            if (dist < _minDist)
            {
                _minDist = dist;
                _resourceToGrab = res;
            }
        }

        if (_resourceToGrab != null)
            ResourceTarget = _resourceToGrab.transform;
    }

    private IEnumerator GrabResource(GameObject res)
    {
        if (ResourceTarget == res)
        {
            yield return new WaitForSeconds(2);
            Destroy(res);
            HoldingResource = true;
            ResourceTarget = null;
        }
        else
        {
            ResourceTarget = res.transform;
            Agent.SetDestination(ResourceTarget.position);
            yield return new WaitForSeconds(2);
            Destroy(res);
            HoldingResource = true;
            ResourceTarget = null;
        }
    }
}