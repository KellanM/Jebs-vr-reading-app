using UnityEngine;

public class PosterBoyOfLetters : MonoBehaviour
{
    public AK.Wwise.Event[] akLetters;

    public void PlayLetter(int index)
    {
        akLetters[index].Post(gameObject);
    }
}
