using TMPro;
using UnityEngine;

public class Pho_MiniGameManager : MonoBehaviour
{
    public GameObject NoodleIngredientPrefab;
    public GameObject BeefIngredientPrefab;
    public GameObject CillantroIngredientPrefab;
    public GameObject ParsleyIngredientPrefab;

    public GameObject NoodleContainerObject;
    public GameObject BeefContainerObject;
    public GameObject CillantroContainerObject;
    public GameObject ParsleyContainerObject;

    public Sprite[] TargetBowlSprites = new Sprite[5];
    public Sprite[] TippedBowlSprites = new Sprite[2];

    private GameObject CurrentIngredientObject;
    private Pho_Ingredient CurrentIngredient;

    private GameObject CurrentContainerObject;
    private SpriteRenderer CurrentContainerSpriteRenderer;

    public GameObject TargetBowlObject;
    private Pho_IngredientTarget IngredientTarget;
    private SpriteRenderer IngredientTargetRenderer;

    public GameObject MiniGameFailedPopUp;
    public GameObject MiniGameWinPopUp;
    public GameObject PhoMiniGameTutorial;

    public TextMeshProUGUI TimerText;

    private bool MiniGameLocked = false;
    private bool UserInput = false;

    private int CurrentIngredientIndex = 0;

    private float MiniGameTimer = 15;

    private string[] Ingredients = new string[] {
        Constants.PHO_MG_POT_NOODLES,
        Constants.PHO_MG_POT_BEEF,
        Constants.PHO_MG_BOWL_CILLANTRO,
        Constants.PHO_MG_BOWL_PARSLEY };

    private void ReturnToKitchen()
    {
        Registry.GameManagerObject.ChangeScene();
    }

    private void OnMiniGameFailed()
    {
        MiniGameLocked = true;
        MiniGameFailedPopUp.SetActive(true);
        Invoke("ReturnToKitchen", 2f);
    }

    private void ShowMiniGameSucsess()
    {
        MiniGameWinPopUp.SetActive(true);
        Registry.PlayerObject.GetComponent<Player>().HoldingMeal = Constants.PHO;
        Invoke("ReturnToKitchen", 2.0f);
    }

    private void OnMiniGameWin()
    {
        MiniGameLocked = true;

        ShowMiniGameSucsess();
    }

    public void OnContinueButtonClicked()
    {
        PhoMiniGameTutorial.SetActive(false);
        MiniGameLocked = false;
        Registry.NotInTutorialScreenTimeModifier = 1;
    }

    private void Start()
    {
        IngredientTarget = TargetBowlObject.GetComponent<Pho_IngredientTarget>();
        IngredientTargetRenderer = TargetBowlObject.GetComponent<SpriteRenderer>();

        if (!Registry.PhoMGTutorialShown)
        {
            PhoMiniGameTutorial.SetActive(true);
            MiniGameLocked = true;
            Registry.SushiMGTutorialShown = true;
            Registry.NotInTutorialScreenTimeModifier = 0;
        }
    }

    private void SetupIngredientSource()
    {
        if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_POT_NOODLES)
        {
            CurrentContainerObject = NoodleContainerObject;
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_POT_BEEF)
        {
            CurrentContainerObject = BeefContainerObject;
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_BOWL_CILLANTRO)
        {
            CurrentContainerObject = CillantroContainerObject;
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_BOWL_PARSLEY)
        {
            CurrentContainerObject = ParsleyContainerObject;
        }
        CurrentContainerSpriteRenderer = CurrentContainerObject.GetComponent<SpriteRenderer>();

        CurrentContainerObject.SetActive(true);
    }

    private void SetupIngredientDrop()
    {
        if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_POT_NOODLES)
        {
            CurrentIngredientObject = Instantiate(NoodleIngredientPrefab);
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_POT_BEEF)
        {
            CurrentIngredientObject = Instantiate(BeefIngredientPrefab);
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_BOWL_CILLANTRO)
        {
            CurrentIngredientObject = Instantiate(CillantroIngredientPrefab);
        }
        else if (Ingredients[CurrentIngredientIndex] == Constants.PHO_MG_BOWL_PARSLEY)
        {
            CurrentIngredientObject = Instantiate(ParsleyIngredientPrefab);
        }
        CurrentIngredient = CurrentIngredientObject.GetComponent<Pho_Ingredient>();
        Rigidbody2D CurrentDisplayedIngredientObjectRigidBody = CurrentContainerObject.GetComponent<Rigidbody2D>();
        CurrentIngredient.transform.position = CurrentDisplayedIngredientObjectRigidBody.position;

        CurrentContainerSpriteRenderer.sprite = TippedBowlSprites[1];
        if (CurrentIngredientIndex < 2)
        {
            CurrentContainerSpriteRenderer.sprite = TippedBowlSprites[0];
        }
    }

    private void Update()
    {
        if (!MiniGameLocked)
        {
            if (MiniGameTimer < 10)
            {
                TimerText.text = "00:0" + (int)MiniGameTimer;
            }
            else
            {
                TimerText.text = "00:" + (int)MiniGameTimer;
            }
            MiniGameTimer -= Time.deltaTime;

            if (CurrentContainerObject == null)
            {
                SetupIngredientSource();
            }

            if (CurrentIngredientObject == null)
            {
                if (Input.touchCount > 0) // if touch input is used
                {
                    if (!UserInput)
                    {
                        UserInput = true;
                        SetupIngredientDrop();
                    }
                }
                else if (Input.GetMouseButton(0)) // if mouse click is used
                {
                    if (!UserInput)
                    {
                        UserInput = true;
                        SetupIngredientDrop();
                    }
                }
                else
                {
                    UserInput = false;
                }
            }

            if (IngredientTarget.IngredientInTargetToggle)
            {
                Destroy(CurrentIngredientObject);
                CurrentIngredientObject = null;
                CurrentIngredientIndex++;
                IngredientTarget.IngredientInTargetToggle = false;
                CurrentContainerObject.SetActive(false);
                CurrentContainerObject = null;

                int IngredientSound = Random.Range(0, 2);
                if (IngredientSound == 0)
                {
                    Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.SoupSplash1);
                }
                if (IngredientSound == 1)
                {
                    Registry.GameManagerObject.SFXSource.PlayOneShot(Registry.GameManagerObject.SoupSplash2);
                }


                if (CurrentIngredientIndex >= Ingredients.Length)
                {
                    OnMiniGameWin();
                }
                else
                {
                    IngredientTargetRenderer.sprite = TargetBowlSprites[CurrentIngredientIndex];
                }
            }
            else if ((CurrentIngredientObject != null && CurrentIngredient.MissedTarget) || MiniGameTimer <= 0)
            {
                OnMiniGameFailed();
            }
        }
    }
}