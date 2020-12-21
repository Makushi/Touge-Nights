using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private Transform target = null;

    [SerializeField] private Vector3 offset;
    [SerializeField] private string targetTag = null;
    [SerializeField] private float translateSpeed;
    [SerializeField] private float rotationSpeed;

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

    private void FixedUpdate()
    {
        if (target != null)
        { 
            HandleTranslation();
            HandleRotation();
        }
        else
        {
            if (GameController.Instance.isGameRunning) //SUPER HACKY AND AWFUL FIX
            {
                SetTarget();
            }    
        }
    }

    private void HandleTranslation()
    {
        var targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
