// Please note that the 'Adjust' method in the CameraAdjust class was sourced by Paddy Costelloe and therefore
// NOT written by any member in the group game project. All other code in this program remains our own work.

using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Adjust();
        if (Registry.GameManagerObject != null)
        {
            Registry.GameManagerObject.AfterSceneChange();
        }
        else
        {
            if (Registry.CurrentSceneName != Constants.MAIN_MENU)
            {
                Debug.LogWarning("You are not running the game from the main menu! This means the game hasn't been properly initialised and the game will not behave as expected.\nPlease stop running the game and start playing from the 'main menu' scene.");
            }
        }
    }

    // Update is called once per frame
    public void Adjust()
    {
        float targetaspect = 16.0f / 9.0f;

        float windowaspect = (float)Screen.width / (float)Screen.height;

        float scaleheight = windowaspect / targetaspect;

        Camera camera = GetComponent<Camera>();

        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}