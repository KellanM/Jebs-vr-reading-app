using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

public class FistAction : BooleanAction
{
    public BooleanAction isGripping, isGrabbing, isPressingIndexTrigger;

    void Start()
    {

    }

    void Update()
    {
        Receive(isGripping.Value && !isGrabbing.Value && isPressingIndexTrigger.Value);
    }
}
