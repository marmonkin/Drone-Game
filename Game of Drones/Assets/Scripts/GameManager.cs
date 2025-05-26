using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int bluePoints = 0;
    public int redPoints = 0;

    public GameObject blueBase;
    public GameObject redBase;

    [SerializeField]
    private GameObject Poof;

    private void Start()
    {
        
    }

    public void GainResource(Faction faction)
    {
        if (faction == Faction.Blue)
        {
            bluePoints++;
            GameObject poof = Instantiate(Poof, blueBase.transform.position, Quaternion.identity);
            Destroy(poof, 2f);
        }
        else if (faction == Faction.Red)
        {
            redPoints++;
            GameObject poof = Instantiate(Poof, redBase.transform.position, Quaternion.identity);
            Destroy(poof, 2f);
        }
    }
}
