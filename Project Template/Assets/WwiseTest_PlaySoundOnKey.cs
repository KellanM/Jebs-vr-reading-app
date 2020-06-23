using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseTest_PlaySoundOnKey : MonoBehaviour
{
    public AK.Wwise.Event TestSound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestSound.Post(gameObject);
        }
    }
}
