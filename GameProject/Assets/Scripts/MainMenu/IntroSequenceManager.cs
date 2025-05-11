using System.Collections;
using UnityEngine;

public class IntroSequenceManager : MonoBehaviour
{
    public GameObject[] IntroSequenceItems = new GameObject[8];

    public bool IsFinished = false;

    public IEnumerator Play()
    {
        for (int i = 0; i < IntroSequenceItems.Length; i++)
        {
            IntroSequenceItems[i].SetActive(true);
            yield return new WaitForSeconds(1.833f);
        }

        IsFinished = true;
    }
}