using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    public UnityEvent pause;
    public void ToggleActive()
    {
        if (gameObject.activeSelf)
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            pause.Invoke();
        }
        else
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
            pause.Invoke();
        }
    }
}
