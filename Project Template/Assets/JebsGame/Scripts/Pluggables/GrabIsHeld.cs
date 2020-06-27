using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabIsHeld : MonoBehaviour
{
    public bool isHeld;

    public void StartHeld()
    {
        isHeld = true;
    }

    public void EndHeld()
    {
        isHeld = false;
    }
}
