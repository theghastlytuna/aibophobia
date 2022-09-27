using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu menuInstance { get; private set; } //singleton
    private bool paused = false;
    private GameObject pauseCanvas;

    private void Start()
    {
        pauseCanvas = transform.GetChild(0).gameObject;
    }
    private void Awake()
    {
        //singleton check
        if (menuInstance != null && menuInstance != this)
        {
            Destroy(this);
        }
        else
        {
            menuInstance = this;
        }

    }
    public void ToggleActive()
    {
        if (paused)
        {
            Time.timeScale = 1;
            pauseCanvas.SetActive(false);
            PlayerController.PlayerInstance.ToggleControl();
            CameraController.CameraInstance.ToggleControl();
            paused = false;
        }
        else
        {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            PlayerController.PlayerInstance.ToggleControl();
            CameraController.CameraInstance.ToggleControl();
            paused = true;
        }
    }
}
