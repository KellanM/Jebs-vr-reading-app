using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class CrabBrain : MonoBehaviour
{
    NavMeshAgent agent;
    public CrabHome homeDestination;
    public GameObject carriedChest;
    public Transform chestSocket;
    public GameObject chestPrefab;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(homeDestination.transform.position);
    }

    void Update()
    {
        
    }

    public void CarryChest()
    {
        carriedChest = Instantiate(chestPrefab,chestSocket.position,chestSocket.rotation);
        carriedChest.transform.parent = chestSocket;
    }


}
