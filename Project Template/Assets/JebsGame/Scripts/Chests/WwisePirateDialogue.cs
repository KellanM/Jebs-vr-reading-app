using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WwisePirateDialogue : MonoBehaviour
{
    public AK.Wwise.Event playEvent;
    public GameObject mouth;

    [Header("Identify")]
    public string identifyStateGroup;
    public string a_identify;
    public string b_identify;
    public string c_identify;
    public string d_identify;
    public string e_identify;
    public string f_identify;
    public string g_identify;
    public string none_identify;

    [Header("Request")]
    public string requestStateGroup;
    public string a_request;
    public string b_request;
    public string c_request;
    public string d_request;
    public string e_request;
    public string f_request;
    public string g_request;
    public string none_request;

    [Header("State")]
    public string stateStateGroup;
    public string storedCorrect;
    public string storedIncorrect;
    public string discardedCorrect;
    public string discardedIncorrect;
    public string noneState;

    [Header("Strikes")]
    public string strikesStateGroup;
    public string positiveStreak;
    public string negativeStreak;
    public string noneStreak;

    [Header("Debug")]
    public TextMeshPro tmpro;

    public void PlayDialogue(char desiredLetter, char currentLetter, bool correctAction, char nextDesiredLetter, int streakState)
    {
        tmpro.text = "";

        string stateStateName;
        string identifyStateName;
        string streakStateName;
        string requestStateName;

        // Action result
        if (correctAction && desiredLetter != currentLetter)
        {
            stateStateName = discardedCorrect;
        }
        else if (correctAction && desiredLetter == currentLetter)
        {
            stateStateName = storedCorrect;
        }
        else if (!correctAction && desiredLetter != currentLetter)
        {
            stateStateName = storedIncorrect;
        }
        else if (!correctAction && desiredLetter == currentLetter)
        {
            stateStateName = discardedIncorrect;
        }
        else
        {
            stateStateName = noneState;
        }
        AkSoundEngine.SetState(stateStateGroup, stateStateName);
        tmpro.text += "State: " + stateStateName + "\n";

        // Identify used letter
        identifyStateName = GetState(identifyStateGroup, currentLetter);
        AkSoundEngine.SetState(identifyStateGroup, identifyStateName);
        tmpro.text += "State: " + identifyStateName + "\n";

        // Streak sentences
        if (streakState > 0)
        {
            streakStateName = positiveStreak;
        }
        else if (streakState < 0)
        {
            streakStateName = negativeStreak;
        }
        else
        {
            streakStateName = noneStreak;
        }
        AkSoundEngine.SetState(strikesStateGroup, streakStateName);
        tmpro.text += "Streak: " + streakStateName + "\n";

        // Request new letter if needed
        if (!(currentLetter == desiredLetter && correctAction) || desiredLetter != nextDesiredLetter)
        {
            requestStateName = GetState(requestStateGroup, nextDesiredLetter);
        }
        else
        {
            requestStateName = none_request;
        }
        AkSoundEngine.SetState(requestStateGroup, requestStateName);
        tmpro.text += "Request: " + requestStateName + "\n";

        playEvent.Post(mouth);
    }

    public void RequestDialogue(char nextDesiredLetter)
    {
        string requestStateName = GetState(requestStateGroup, nextDesiredLetter);
        AkSoundEngine.SetState(requestStateGroup, requestStateName);
        tmpro.text = "Request: " + requestStateName + "\n";
        playEvent.Post(mouth);
    }

    string GetState(string stateGroup, char relatedChar)
    {
        if (stateGroup == identifyStateGroup)
        {
            switch (relatedChar)
            {
                case 'a':
                    return a_identify;
                case 'b':
                    return b_identify;
                case 'c':
                    return c_identify;
                case 'd':
                    return d_identify;
                case 'e':
                    return e_identify;
                case 'f':
                    return f_identify;
                case 'g':
                    return g_identify;
            }
        }
        else if (stateGroup == requestStateGroup)
        {
            switch (relatedChar)
            {
                case 'a':
                    return a_request;
                case 'b':
                    return b_request;
                case 'c':
                    return c_request;
                case 'd':
                    return d_request;
                case 'e':
                    return e_request;
                case 'f':
                    return f_request;
                case 'g':
                    return g_request;
            }
        }

        return "None";
    }
}
