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

    public ParticleSystem bagParticles;
    public ParticleSystem chestParticles;

    public UnityEvent positiveFeedback;
    public UnityEvent negativeFeedback;

    CrabFactory factory;

    public int positiveStreak = 0;
    public int negativeStreak = 0;
    int streakState = 0;

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
        bool correctAction;
        char previousCorrectLetter = searchForLetter;

        ParticleSystem particleSys = null;

        if (letter.value == searchForLetter && accepted)
        {
            NextLetter();

            positiveStreak++;
            negativeStreak = 0;
            correctAction = true;

            particleSys = bagParticles;

            positiveFeedback.Invoke();
        }    
        else if (letter.value != searchForLetter && !accepted)
        {
            positiveStreak++;
            negativeStreak = 0;
            correctAction = true;

            particleSys = chestParticles;

            positiveFeedback.Invoke();
        }
        else
        {
            negativeStreak++;
            positiveStreak = 0;
            correctAction = false;

            negativeFeedback.Invoke();
        }

        streakState = 0;
        if (positiveStreak >= 3)
        {
            streakState = 1;
            positiveStreak = 0;

            CrabFactory.factory.crabsSpeed += CrabFactory.factory.speedIncrease;

            if (particleSys) {
                ParticleSystem.Burst burst = particleSys.emission.GetBurst(0);
                burst.count = 10;
                particleSys.emission.SetBurst(0,burst);
                particleSys.Play();
            }
        }
        else if (positiveStreak > 0)
        {
            if (particleSys)
            {
                ParticleSystem.Burst burst = particleSys.emission.GetBurst(0);
                burst.count = 1;
                particleSys.emission.SetBurst(0, burst);
                particleSys.Play();
            }
        }
        else if (negativeStreak >= 3)
        {
            streakState = -1;
            negativeStreak = 0;
        }
        else if (negativeStreak > 0)
        {

        }
            

        pirate.PlayDialogue(previousCorrectLetter, letter.value, correctAction, searchForLetter, streakState);


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
