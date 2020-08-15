using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class LetterGenerator : MonoBehaviour
{
    public static LetterGenerator letterGen;

    public Transform lookAt;
    public char[] letters = {'a','b','c'};
    public GameObject[] letterPrefabs;

    [Range(3,10)]
    public int maxToCorrect;
    int actualMax;
    int incorrectCounter = 0;

    void Awake()
    {
        if (!letterGen) letterGen = this;
        actualMax = Random.Range(3, letters.Length);
    }

    void Update()
    {
        
    }

    public ChestLetter Generate(Transform destination)
    {
        int index = Random.Range(0, letters.Length - 1);
        if (incorrectCounter < actualMax)
        {
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

        if (index != -1)
        {
            ChestLetter letter = Instantiate(letterPrefabs[index], destination.position, destination.rotation).GetComponent<ChestLetter>();
            letter.value = letters[index];

            ConstraintSource source = new ConstraintSource();
            source.sourceTransform = lookAt;
            source.weight = 1.0f;

            letter.GetComponent<LookAtConstraint>().SetSource(0, source);

            return letter;
        }

        return null;
    }
}
