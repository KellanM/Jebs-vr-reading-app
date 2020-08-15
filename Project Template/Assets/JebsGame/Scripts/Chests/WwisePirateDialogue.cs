using System.Collections;
using System.Collections.Generic;
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

    [Header("Request")]
    public string requestStateGroup;
    public string a_request;
    public string b_request;
    public string c_request;
    public string d_request;
    public string e_request;
    public string f_request;
    public string g_request;

    [Header("State")]
    public string stateStateGroup;
    public string correct;
    public string incorrect;

    public void PlayDialogue(char desiredLetter, char currentLetter, bool correctAction, char nextDesiredLetter)
    {
        if (correctAction)
            AkSoundEngine.SetState(stateStateGroup, correct);
        else
            AkSoundEngine.SetState(stateStateGroup, incorrect);

        AkSoundEngine.SetState(identifyStateGroup, GetState(identifyStateGroup, currentLetter));

        if (!(currentLetter == desiredLetter && correctAction) || desiredLetter != nextDesiredLetter)
            AkSoundEngine.SetState(requestStateGroup, GetState(requestStateGroup, nextDesiredLetter));

        playEvent.Post(mouth);
    }

    public void RequestDialogue(char nextDesiredLetter)
    {
        AkSoundEngine.SetState(requestStateGroup, GetState(requestStateGroup, nextDesiredLetter));
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
