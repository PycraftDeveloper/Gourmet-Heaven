using UnityEngine;


public class LevelSelectionMenuManagerScript : MonoBehaviour
{
    public Canvas TutorialCanvas;
    public Canvas LevelSelectCanvas;

    private void Start() 
    {
      LevelSelectCanvas.gameObject.SetActive(true);
    }
    

    public void OnLevelOneButtonClick()
    {
        LevelSelectCanvas.gameObject.SetActive(false);
        TutorialCanvas.gameObject.SetActive(true);
    }

    public void OnLevelTwoButtonClick()
    {
        Registry.LevelCustomerObject.SetupLevelTwo();
        Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
    }

   public void OnContinueButtonClick()
   {
     TutorialCanvas.gameObject.SetActive(false);
     Registry.LevelCustomerObject.SetupLevelOne();
     Registry.GameManagerObject.ChangeScene(Constants.KITCHEN);
   }
    
    public void OnBackButtonClick()
    {
        Registry.GameManagerObject.ChangeScene();
    }
}