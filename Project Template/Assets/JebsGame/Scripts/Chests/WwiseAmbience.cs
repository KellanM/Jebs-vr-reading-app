using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseAmbience : MonoBehaviour
{
    public AK.Wwise.Event Ambience;

    void Start()
    {
        Ambience.Post(gameObject);
    }

    void Update()
    {
        
    }
}
