using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera tpCamera;
    [SerializeField] private Camera fpCamera;

    private void OnEnable()
    {
        EventManager.onCameraSwitch += SwitchCamera;
    }

    private void OnDisable()
    {
        EventManager.onCameraSwitch -= SwitchCamera;
    }

    private void Start()
    {
        tpCamera.enabled = true;
        fpCamera.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchCamera();
        }
    }
    private void SwitchCamera()
    {
        tpCamera.enabled = !tpCamera.enabled;
        fpCamera.enabled = !fpCamera.enabled;
    }
}
