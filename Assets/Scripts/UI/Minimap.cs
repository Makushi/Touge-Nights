using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform target = null;

    [SerializeField] private string targetTag = null;
    [SerializeField] private GameObject MinimapCamera = null;
    [SerializeField] private GameObject MinimapIcon = null;

    private void OnEnable()
    {
        EventManager.onPlayerSpawned += SetTarget;
    }

    private void OnDisable()
    {
        EventManager.onPlayerSpawned -= SetTarget;
    }

    private void SetTarget()
    {
        target = GameObject.FindGameObjectWithTag(targetTag).transform;
    }

    void Update()
    {
        if (target != null)
        {
            MinimapCamera.transform.position = (new Vector3(target.transform.position.x, MinimapCamera.transform.position.y, target.transform.position.z));
            MinimapIcon.transform.position = (new Vector3(target.transform.position.x, MinimapIcon.transform.position.y, target.transform.position.z));

            Vector3 iconRotation = new Vector3(target.transform.eulerAngles.x, target.transform.eulerAngles.y - 180, target.transform.eulerAngles.z);
            Vector3 cameraRotation = new Vector3(MinimapCamera.transform.eulerAngles.x, target.transform.eulerAngles.y, MinimapCamera.transform.eulerAngles.z);

            MinimapIcon.transform.eulerAngles = iconRotation;
            MinimapCamera.transform.eulerAngles = cameraRotation;
        }
        else
        {
            if (GameController.Instance.isGameRunning) //SUPER HACKY AND AWFUL FIX
            {
                SetTarget();
            }
        }
    }
}
