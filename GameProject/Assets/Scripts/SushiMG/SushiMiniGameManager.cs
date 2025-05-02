using UnityEngine;

public class SushiMiniGameManager : MonoBehaviour
{
    public GameObject RiceHitbox;
    public GameObject SeaweedHitbox;
    public GameObject WasabiHitbox;
    public GameObject TunaHitbox;

    private IngredientSpawn RiceIngredientSpawn;
    private IngredientSpawn SeaweedIngredientSpawn;
    private IngredientSpawn WasabiIngredientSpawn;
    private IngredientSpawn TunaIngredientSpawn;

    public GameObject DisplayedRiceObject;
    public GameObject DisplayedSeaweedObject;
    public GameObject DisplayedWasabiObject;
    public GameObject DisplayedTunaObject;

    public bool IngredientSpawned = false;

    private void Start()
    {
        RiceIngredientSpawn = RiceHitbox.GetComponent<IngredientSpawn>();
        SeaweedIngredientSpawn = SeaweedHitbox.GetComponent<IngredientSpawn>();
        WasabiIngredientSpawn = WasabiHitbox.GetComponent<IngredientSpawn>();
        TunaIngredientSpawn = TunaHitbox.GetComponent<IngredientSpawn>();
    }

    private void Update()
    {
        if (RiceIngredientSpawn.IngredientDraggedIntoTarget)
        {
            DisplayedRiceObject.SetActive(true);
        }

        if (SeaweedIngredientSpawn.IngredientDraggedIntoTarget)
        {
            DisplayedSeaweedObject.SetActive(true);
        }

        if (WasabiIngredientSpawn.IngredientDraggedIntoTarget)
        {
            DisplayedWasabiObject.SetActive(true);
        }

        if (TunaIngredientSpawn.IngredientDraggedIntoTarget)
        {
            DisplayedTunaObject.SetActive(true);
        }
    }
}