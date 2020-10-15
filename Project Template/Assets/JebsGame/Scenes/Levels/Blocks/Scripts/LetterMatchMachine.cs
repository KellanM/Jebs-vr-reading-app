using UnityEngine;

public class LetterMatchMachine : MonoBehaviour
{
    private PosterBoyOfLetters LetterPoster { get { if (_letterPoster == null) _letterPoster = GetComponent<PosterBoyOfLetters>(); return _letterPoster; } }
    private PosterBoyOfLetters _letterPoster;

    public UnifiedLetter shownLetter;

    [Tooltip("How often the machine repeats the spoken letter (0 for never)")]
    public float spokenFrequency = 5;
    private float lastSpoken;

    public UnityEngine.Events.UnityEvent onCorrect;
    public UnityEngine.Events.UnityEvent onIncorrect;

    private int spokenLetterIndex, shownLetterIndex;
    public int randomness = 1, otherLetterCount;

    void Update()
    {
        if (Time.time - lastSpoken >= spokenFrequency)
            PlaySpokenLetter();
    }

    public void ResetLetters()
    {
        SetSpokenLetter(Random.Range(0, 26));
        RandomizeShownLetter();
    }

    void PlaySpokenLetter()
    {
        LetterPoster.PlayLetter(spokenLetterIndex);
        lastSpoken = Time.time;
    }

    public void SetShownLetter(int index)
    {
        shownLetterIndex = index;
        shownLetter.SetCase(true);
        shownLetter.SetLetter(shownLetterIndex);
    }
    public void SetSpokenLetter(int index)
    {
        spokenLetterIndex = index;
        PlaySpokenLetter();
        randomness = 1;
    }

    public void RandomizeShownLetter()
    {
        otherLetterCount = Mathf.Clamp(24 / randomness, 2, 24);
        randomness++;
        int randomStepAmount = Random.Range(0, otherLetterCount + 1);
        int startIndex = ((spokenLetterIndex - otherLetterCount / 2) + 26) % 26;
        int index = (startIndex + randomStepAmount) % 26;

        if (index == spokenLetterIndex)
            randomness = 1;

        SetShownLetter(index);
    }
    public void Test()
    {
        bool correct = spokenLetterIndex == shownLetterIndex;
        if (correct)
            onCorrect?.Invoke();
        else
            onIncorrect?.Invoke();
    }
}
