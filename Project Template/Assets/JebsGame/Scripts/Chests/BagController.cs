using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using Zinnia.Action;

public class BagController : MonoBehaviour
{
    public static BagController bag;

    bool followOrder = true;
    int letterIndex = 0;
    int letterGroupLength;
    public char searchForLetter;

    public WwisePirateDialogue pirate;
    public AK.Wwise.Event positiveEvent;
    public AK.Wwise.Event negativeEvent;

    public UnityEvent positiveFeedback;
    public UnityEvent negativeFeedback;

    CrabFactory factory;

    private void Awake()
    {
        if (!bag) bag = this;
    }

    private void Start()
    {
        letterGroupLength = LetterGenerator.letterGen.letters.Length;
        searchForLetter = LetterGenerator.letterGen.letters[letterIndex];

        factory = CrabFactory.factory;

        pirate.RequestDialogue(searchForLetter);
    }

    private void OnTriggerStay(Collider other)
    {
        ChestLetter letter = other.GetComponentInParent<ChestLetter>();

        if (letter && letter.currentInteractor == null)
        { 
            Evaluate(letter,true);

            letter.Destroy();
        }
    }

    public void Evaluate(ChestLetter letter, bool accepted)
    {
        if (letter.value == searchForLetter && accepted)
        {
            NextLetter();
            
            pirate.PlayDialogue(letter.value, letter.value,true,searchForLetter);

            positiveEvent.Post(gameObject);
            positiveFeedback.Invoke();
        }    
        else if (letter.value != searchForLetter && !accepted)
        {
            pirate.PlayDialogue(searchForLetter, letter.value, true, searchForLetter);

            positiveEvent.Post(gameObject);
            positiveFeedback.Invoke();
        }
        else
        {
            pirate.PlayDialogue(searchForLetter, letter.value, false, searchForLetter);

            negativeEvent.Post(gameObject);
            negativeFeedback.Invoke();
        }

        factory.Restart();
    }

    void NextLetter()
    {
        if (letterIndex < letterGroupLength - 1 && followOrder)
        {
            letterIndex++;
        }
        else
        {
            followOrder = false;
            letterIndex = Random.Range(0, letterGroupLength - 1);
        }

        searchForLetter = LetterGenerator.letterGen.letters[letterIndex];
    }

}
