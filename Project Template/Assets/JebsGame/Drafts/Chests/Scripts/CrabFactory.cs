using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class CrabFactory : MonoBehaviour
{
    public static CrabFactory factory;

    public int numOfCrabs = 20;

    public List<CrabBrain> crabs = new List<CrabBrain>();
    public List<CrabHome> homes = new List<CrabHome>();

    public GameObject crabPrefab;

    [Range(0.0f, 1.0f)]
    public float chestProbability;

    public Transform chestDestination;

    bool crabsAreRunning = true;

    void Awake()
    {
        if (!factory) factory = this;
    }

    void Update()
    {
        if (crabsAreRunning)
            EvaluatePopulation();
    }

    void EvaluatePopulation()
    {
        for (int i = 0; i < crabs.Count; i++)
        {
            if (!crabs[i])
            {
                crabs.RemoveAt(i);
                i--;
            }
        }

        if (crabs.Count < numOfCrabs)
        {
            Debug.Log("Total: " + homes.Count);

            int doorNumber = 0;
            CrabHome spawnPoint = homes[doorNumber];
            GameObject newBornCrab = Instantiate(crabPrefab,spawnPoint.transform.position,Quaternion.identity);
            newBornCrab.transform.parent = transform;
            CrabBrain smolBrain = newBornCrab.GetComponent<CrabBrain>();
            crabs.Add(smolBrain);

            List<CrabHome> availableNewDestinations = new List<CrabHome>(homes);

            availableNewDestinations.Remove(spawnPoint);
            smolBrain.homeDestination = homes[1];
            Debug.Log("Available: " + availableNewDestinations.Count);

            Debug.Log("Bron at " + spawnPoint.name + ", goes to " + smolBrain.homeDestination.name);

            if (Random.value < chestProbability)
                smolBrain.CarryChest();
        }
    }

    public void ToggleCrabs(bool b)
    {
        crabsAreRunning = b;
    }

    public void Restart()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        ToggleCrabs(true);
    }
}
