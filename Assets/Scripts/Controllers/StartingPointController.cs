using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPointController : MonoBehaviour
{
    [SerializeField] GameObject playerCar;
    [SerializeField] Transform spawnPoint;
    private void OnEnable()
    {
        EventManager.onGameStarted += SpawnPlayerCar;
        EventManager.onRetry += SpawnPlayerCar;
    }

    private void OnDisable()
    {
        EventManager.onGameStarted -= SpawnPlayerCar;
        EventManager.onRetry -= SpawnPlayerCar;
    }
    private void SpawnPlayerCar()
    {
        GameObject player = Instantiate(playerCar, spawnPoint.position, spawnPoint.rotation);
    }
}
