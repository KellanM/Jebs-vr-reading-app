using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGenerator : MonoBehaviour
{
    public static LetterGenerator letterGen;

    public char[] letters = {'a','b','c'};
    public GameObject[] letterPrefabs;

    void Awake()
    {
        if (!letterGen) letterGen = this;
    }

    void Update()
    {
        
    }

    public ChestLetter Generate(Transform destination)
    {
        int index = Random.Range(0,letters.Length);
        ChestLetter letter = Instantiate(letterPrefabs[index],destination.position,destination.rotation).GetComponent<ChestLetter>();
        letter.value = letters[index];
        letter.transform.parent = destination;

        return letter;
    }
}
