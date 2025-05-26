using UnityEngine;

public class Base : MonoBehaviour
{
    public Faction faction;

    private GameManager gManager;

    private Spawner spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gManager = FindFirstObjectByType<GameManager>();

        spawner = GetComponentInChildren<Spawner>();
        spawner.SpawnDrone(faction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum Faction
{
    Blue,
    Red
}
