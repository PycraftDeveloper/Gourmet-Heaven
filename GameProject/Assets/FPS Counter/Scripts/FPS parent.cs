using UnityEngine;

public class FPSparent : MonoBehaviour
{
    private void Awake()
    {
        if (FPS_Counter_Registry.FPS_Counter_Exists == false)
        {
            DontDestroyOnLoad(gameObject);
            FPS_Counter_Registry.FPS_Counter_Exists = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}