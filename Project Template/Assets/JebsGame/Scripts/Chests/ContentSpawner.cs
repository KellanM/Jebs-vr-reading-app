using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ContentSpawner : MonoBehaviour
{
    public static ContentSpawner conentGen;

    public Transform lookAt;
    public char[] letters = {'a','b','c','d','e','f','g'};
    public GameObject[] letterPrefabs;
    public GameObject[] prizePrefabs;

    [Header("Prize spawn")]
    public bool willSpawnPrize = false;
    [Range(0.0f, 1.0f)]
    public float prizeProbability;

    [Header("Font variety")]
    public Color[] fontColors;

    [Header("Streak control")]
    [Range(3,10)]
    public int maxToCorrect;
    int actualMax;
    int incorrectCounter = 0;

    void Awake()
    {
        if (!conentGen) conentGen = this;
        actualMax = Random.Range(3, letters.Length);
    }

    void Update()
    {
        
    }

    public Spawnable Generate(Transform destination)
    {
        int index;

        Spawnable returnThis = null;

        if (willSpawnPrize || Random.value < prizeProbability)
        {
            // Spawn prize
            index = Random.Range(0, prizePrefabs.Length - 1);
            returnThis = Instantiate(prizePrefabs[index], destination.position, destination.rotation).GetComponent<Prize>();
            willSpawnPrize = false;
        }
        else
        {
            // Spawn letter
            index = Random.Range(0, letters.Length - 1);

            if (incorrectCounter < actualMax)
            {
                // Increase/Reset counter of incorrect letters spawned
                if (letterPrefabs[index].GetComponent<ChestLetter>().value == BagController.bag.searchForLetter)
                {
                    actualMax = Random.Range(3, letters.Length);
                    incorrectCounter = 0;
                }
                else
                    incorrectCounter++;
            }
            else
            {
                // Make index to point to the correct letter
                for (int i = 0; i < letterPrefabs.Length; i++)
                {
                    if (letterPrefabs[i].GetComponent<ChestLetter>().value == BagController.bag.searchForLetter)
                    {
                        index = i;

                        actualMax = Random.Range(3, letters.Length);
                        incorrectCounter = 0;

                        break;
                    }
                }
            }

            // Spawn the letter
            ChestLetter letter = Instantiate(letterPrefabs[index], destination.position, destination.rotation).GetComponent<ChestLetter>();
            returnThis = letter;
                
            letter.value = letters[index];

            ConstraintSource source = new ConstraintSource();
            source.sourceTransform = lookAt;
            source.weight = 1.0f;

            letter.GetComponent<LookAtConstraint>().SetSource(0, source);

            letter.UpdateFont();

            letter.UpdateColor(fontColors[Random.Range(0, fontColors.Length - 1)]);
        }

        return returnThis;
    }
}
