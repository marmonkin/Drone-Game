using System.Collections;
using System.Collections.Generic;
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

    public List<GameObject> emotions = new List<GameObject>();

    private GameManager gManager;

    private Vector3 StartingPosition;

    private Transform ResourceTarget;
    private NavMeshAgent Agent;

    private bool HoldingResource = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartingPosition = this.transform.position;

        gManager = FindFirstObjectByType<GameManager>();

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
            //If have no purpose, find it
            FindResource();
        }

        if (HoldingResource)
        {
            //If have resource, bring it back
            BringResource();
        }
    }

    /// <summary>
    /// Checks whether drone entered the base or grabbed the resource
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Resource") && !HoldingResource)
        {
            StartCoroutine(GrabResource(other.gameObject));
        }

        if (other.CompareTag("Base") && HoldingResource)
        {
            HoldingResource = false;
            gManager.GainResource(faction);
        }
    }

    /// <summary>
    /// Pings every second to find the closest resource
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Finds the closest resource, and targets it
    /// </summary>
    private void FindResource()
    {
        SwitchEmotion(2);
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
        {
            ResourceTarget = _resourceToGrab.transform;
            SwitchEmotion(0);
        }
    }

    /// <summary>
    /// If reached the initial resource, wait a little and destroy it
    /// If found a better resource on the way, change course and do the same
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Pathfinds towards the base if has a resource
    /// </summary>
    private void BringResource()
    {
        SwitchEmotion(1);
        Agent.SetDestination(StartingPosition);
    }

    /// <summary>
    /// 0 == Found, moving
    /// 1 == Grabbed, bringing
    /// 2 == Searching
    /// </summary>
    /// <param name="param"></param>
    private void SwitchEmotion(int param)
    {
        emotions.ForEach(e => { e.SetActive(false); });
        emotions[param].SetActive(true);
    }
}