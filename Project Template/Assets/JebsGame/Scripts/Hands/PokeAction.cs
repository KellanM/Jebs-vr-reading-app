using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

public class PokeAction : BooleanAction
{
    public BooleanAction isGripping, isGrabbing, isTouchingIndexTrigger;

    void Start()
    {
        
    }

    void Update()
    {
        Receive(isGripping.Value && !isGrabbing.Value && !isTouchingIndexTrigger.Value);
    }
}
