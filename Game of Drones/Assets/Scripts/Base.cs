using UnityEngine;

public class Base : MonoBehaviour
{
    public Faction faction;

    private Spawner spawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawner = GetComponentInChildren<Spawner>();

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
