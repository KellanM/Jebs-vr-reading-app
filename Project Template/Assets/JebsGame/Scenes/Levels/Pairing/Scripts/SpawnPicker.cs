using System.Collections.Generic;
using UnityEngine;

public class SpawnPicker : MonoBehaviour
{
    public bool random;
    [Space(10)]
    public Transform[] spawnPoints;
    private List<int> pickedPoints = new List<int>();

    public Transform GetSpawnPoint()
    {
        Transform spawnPoint;

        if (random)
            spawnPoint = GetRandomSpwanPoint();
        else
            spawnPoint = GetSequentialSpawnPoint();

        return spawnPoint;
    }
    private Transform GetSequentialSpawnPoint()
    {
        Transform spawnPoint = null;
        if (pickedPoints.Count != spawnPoints.Length)
        {
            int actualIndex = pickedPoints.Count;
            spawnPoint = spawnPoints[actualIndex];
            pickedPoints.Add(actualIndex);
        }

        return spawnPoint;
    }
    private Transform GetRandomSpwanPoint()
    {
        Transform spawnPoint = null;
        if (pickedPoints.Count != spawnPoints.Length)
        {
            List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);
            pickedPoints.Sort();
            for (int i = pickedPoints.Count - 1; i >= 0; i--)
                availableSpawnPoints.RemoveAt(pickedPoints[i]);
            
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            spawnPoint = availableSpawnPoints[randomIndex];
            int actualIndex = System.Array.IndexOf(spawnPoints, spawnPoint);
            pickedPoints.Add(actualIndex);
        }

        return spawnPoint;
    }
    public void Clear()
    {
        pickedPoints.Clear();
    }
}
