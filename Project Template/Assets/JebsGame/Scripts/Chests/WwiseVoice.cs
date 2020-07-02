using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseVoice : MonoBehaviour
{
    public GameObject characterMouth;

    public AK.Wwise.Event wellDone;
    public AK.Wwise.Event wrongAnswer;

    [Header("Thats The")]
    public AK.Wwise.Event TT_LetterA;
    public AK.Wwise.Event TT_LetterB;
    public AK.Wwise.Event TT_LetterC;

    [Header("Please Give Me")]
    public AK.Wwise.Event PGM_LetterA;
    public AK.Wwise.Event PGM_LetterB;
    public AK.Wwise.Event PGM_LetterC;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void Info(char c)
    {
        switch (c)
        {
            case 'a':
                TT_LetterA.Post(characterMouth);
                break;
            case 'b':
                TT_LetterB.Post(characterMouth);
                break;
            case 'c':
                TT_LetterC.Post(characterMouth);
                break;
        }
    }

    public void Reminder(char c)
    {
        switch (c)
        {
            case 'a':
                PGM_LetterA.Post(characterMouth);
                break;
            case 'b':
                PGM_LetterB.Post(characterMouth);
                break;
            case 'c':
                PGM_LetterC.Post(characterMouth);
                break;
        }
    }

    public void WellDone()
    {
        wellDone.Post(characterMouth);
    }

    public void Wrong()
    {
        wrongAnswer.Post(characterMouth);
    }
}
