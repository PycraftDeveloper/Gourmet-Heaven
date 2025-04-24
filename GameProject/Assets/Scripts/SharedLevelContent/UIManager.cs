using UnityEngine;

public class UIManager : MonoBehaviour
{
    private void Awake()
    {
        if (Registry.UIManagerObject == null)
        {
            DontDestroyOnLoad(gameObject);
            Registry.UIManagerObject = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnPauseButtonClick()
    {
        Registry.GameManagerObject.ChangeScene(Constants.PAUSE_MENU);
    }

    public void OnSceneChanged()
    {
        Registry.UIManagerObject.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}