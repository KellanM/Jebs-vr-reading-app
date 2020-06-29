using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CrabHome : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        CrabBrain brain = other.GetComponent<CrabBrain>();
        if (brain && brain.homeDestination == this)
        {
            Destroy(other.gameObject);
        }
    }
}
