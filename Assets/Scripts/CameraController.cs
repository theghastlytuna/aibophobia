using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    public static CameraController CameraInstance { get; private set; }
    private PostProcessVolume m_Volume;
    private ColorGrading m_Grading;
    private bool controlLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_Grading = ScriptableObject.CreateInstance<ColorGrading>();
        m_Grading.enabled.Override(true);
        m_Grading.saturation.Override(-100f);
    }
    private void Awake()
    {
        if (CameraInstance != null && CameraInstance != this)
        {
            Destroy(this);
        }
        else
        {
            CameraInstance = this;
        }
    }

    void OnLook(InputValue mouse)
    {
        if (!controlLocked)
        {
            Vector2 test = mouse.Get<Vector2>();
            Vector3 rotation = new Vector3(-test.y * 0.2f, 0.0f, 0.0f);
            transform.Rotate(rotation);
        }
    }

    public void PlayerDeath()
    {
        m_Volume = PostProcessManager.instance.QuickVolume(8, 10f, m_Grading);
        
        transform.parent = null;
    }

    private void OnDestroy()
    {
        if (m_Volume != null)
        {
            RuntimeUtilities.DestroyVolume(m_Volume, true, true);
        }
    }
    public void ToggleControl()
    {
        controlLocked = !controlLocked;
    }

}
