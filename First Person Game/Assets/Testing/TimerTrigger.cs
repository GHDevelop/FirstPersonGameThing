using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTrigger : MonoBehaviour
{
    private static UnityEngine.UI.Text[] timerText = new UnityEngine.UI.Text[0];
    private static bool timerOver = true;
    private static decimal numSeconds = 0;
    private static int numFrames = 0;

    private void Awake()
    {
        Application.targetFrameRate = -1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timerText.Length == 0 || timerText[0] == null)
        {
            timerText = other.GetComponentsInChildren<UnityEngine.UI.Text>();
        }

        if (timerText.Length != 0 && timerText[0] != null)
        {
            timerOver = !timerOver;
            if (timerOver == false)
            {
                numSeconds = 0;
                numFrames = 0;
                StartCoroutine(runTimer());
            }
        }
    }

    private IEnumerator runTimer()
    {
        while (timerOver == false)
        {
            IncrementTimer();
            yield return new WaitForFixedUpdate();
        }
    }

    private static void IncrementTimer()
    {
        numSeconds += (decimal)Time.fixedDeltaTime;
        numFrames++;
        timerText[0].text = (numSeconds).ToString();

        if (timerText.Length >= 1 && timerText[1] != null)
        {
            timerText[1].text = numFrames.ToString();
        }

        if (timerText.Length >= 2 && timerText[2] != null)
        {
            timerText[2].text = (numFrames / numSeconds).ToString();
        }
    }
}
