using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseChest : MonoBehaviour
{
    public GameObject chest;
    public AK.Wwise.Event ChestHit;
    public AK.Wwise.Event ChestOpen;
    public AK.Wwise.Event ChestClose;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void PlayChestOpen(bool open)
    {
        if (open) ChestOpen.Post(chest);
        else ChestClose.Post(chest);
    }

    public void PlayHit()
    {
        ChestHit.Post(chest);
    }
}
