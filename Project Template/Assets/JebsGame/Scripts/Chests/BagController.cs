using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public AK.Wwise.Event positiveWwiseEvent;
    public AK.Wwise.Event negativeWwiseEvent;
    public AkGameObj bagSoundSource;

    public ParticleSystem bagParticles;
    public ParticleSystem chestParticles;

    public UnityEvent positiveFeedback;
    public UnityEvent negativeFeedback;

    CrabFactory factory;

    [Header("Streak")]
    public int streakNumber = 3;
    public int positiveStreak = 0;
    public int negativeStreak = 0;
    int streakState = 0;

    public ImageFiller streakBar;
    int goldBarCounter = 0;
    public TextMeshPro goldBarTmpro;

    private void Awake()
    {
        if (!bag) bag = this;
    }

    private void Start()
    {
        letterGroupLength = ContentSpawner.conentGen.letters.Length;
        searchForLetter = ContentSpawner.conentGen.letters[letterIndex];

        factory = CrabFactory.factory;

        pirate.RequestDialogue(searchForLetter);
    }

    private void OnTriggerStay(Collider other)
    {
        Spawnable spawnable = other.GetComponentInParent<Spawnable>();

        if (spawnable && spawnable.currentInteractor == null)
        { 
            Evaluate(spawnable, true);

            spawnable.Destroy();
        }
    }

    public void Evaluate(Spawnable spawnable, bool accepted)
    {
        bool correctAction;
        char previousCorrectLetter = searchForLetter;

        if (spawnable is ChestLetter)
        {
            ChestLetter letter = spawnable as ChestLetter;

            if (letter.value == searchForLetter && accepted)
            {
                NextLetter();

                positiveStreak++;
                negativeStreak = 0;
                correctAction = true;

                positiveFeedback.Invoke();
            }
            else if (letter.value != searchForLetter && !accepted)
            {
                positiveStreak++;
                negativeStreak = 0;
                correctAction = true;

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
            if (positiveStreak > 0)
            {
                if (positiveStreak >= streakNumber)
                {
                    streakState = 1;

                    CrabFactory.factory.crabsSpeed += CrabFactory.factory.speedIncrease;

                    PlayParticleBurst(10);

                    ContentSpawner.conentGen.willSpawnPrize = true;
                }
                else
                {
                    PlayParticleBurst(1);
                }
            }
            else if (negativeStreak > 0)
            {
                if (negativeStreak >= streakNumber)
                {
                    streakState = -1;
                    negativeStreak = 0;
                }
                else
                {

                }

                positiveStreak = 0;
            }

            pirate.PlayDialogue(previousCorrectLetter, letter.value, correctAction, searchForLetter, streakState);
        }
        else
        {
            if (accepted)
            {
                // Storing the prize is the correct choice
                positiveFeedback.Invoke();

                streakState = 1;
                positiveStreak = 0;

                PlayParticleBurst(5);

                goldBarCounter++;
                goldBarTmpro.text = "x" + goldBarCounter;

                
            }
        }

        streakBar.SetFillAmount((float)positiveStreak / (float)streakNumber);

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

        searchForLetter = ContentSpawner.conentGen.letters[letterIndex];
    }

    void PlayParticleBurst(int burstSize)
    {
        if (bagParticles)
        {
            ParticleSystem.Burst burst = bagParticles.emission.GetBurst(0);
            burst.count = burstSize;
            bagParticles.emission.SetBurst(0, burst);
            bagParticles.Play();
        }

    }

    public void PositiveSoundFeedback()
    {
        positiveWwiseEvent.Post(bagSoundSource.gameObject);
    }

    public void NegativeSoundFeedback()
    {
        negativeWwiseEvent.Post(bagSoundSource.gameObject);
    }

}
