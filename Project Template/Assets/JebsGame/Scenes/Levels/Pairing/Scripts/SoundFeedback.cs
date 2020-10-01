using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFeedback : MonoBehaviour
{
    public AK.Wwise.Event positiveEvent;
    public AK.Wwise.Event negativeEvent;
    public AkGameObj soundSource;

    public void playPositiveFeedback()
    {
        positiveEvent.Post(soundSource.gameObject);
    }

    public void playNegativeFeedback()
    {
        negativeEvent.Post(soundSource.gameObject);
    }
}
