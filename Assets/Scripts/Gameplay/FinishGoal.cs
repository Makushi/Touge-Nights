using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            EventManager.LapCompleted();
        }
    }
}
