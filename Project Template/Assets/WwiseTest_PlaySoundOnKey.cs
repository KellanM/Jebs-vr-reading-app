using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseTest_PlaySoundOnKey : MonoBehaviour
{
    public AK.Wwise.Event TestSound;
    void Start()
    {
        AkSoundEngine.SetState("letter_state", "correct");
        AkSoundEngine.SetState("letter_identify","a_identify");
        AkSoundEngine.SetState("letter_request", "b_request");
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
