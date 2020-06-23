using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.UI;

public class PluggablesController : MonoBehaviour
{
    [Header("References")]
    public TMP_Text counter;
    public List<GameObject> rigidAlphaList = new List<GameObject>();
    public List<bool> displayLetters = new List<bool>();

    public GameObject highlightPrefab;

    [Header("PositionTransforms")]
    public Transform rigidSpawnLeft;
    public Transform rigidSpawnRight;

    [Header("Settings")]
    public int timeAllowed;
    public string displayString;
    public int amountNotDisplay;

    [Header("DisplayPositionalReferences")]
    public float displayDownY;
    public float displayUpY;
    public float xDistanceBetween;
    public float displayZ;

    int currentTime;

    private void Start()
    {
        //Start Timer
        currentTime = timeAllowed;
        InvokeRepeating("TimerIterate", 1f, 1f);

        //Spawn The Physics Alphabet Prefabs
        StartCoroutine(SpawnRigids());

        //Display the chosen letters
        StartCoroutine(DisplayLetters());
    }

    private void Update()
    {
        //Update Text to have time left
        counter.text = currentTime.ToString();
    }

    void TimerIterate()
    {
        //Minus 1 from current time every second
        if (currentTime > 0)
        {
            currentTime -= 1;
        }
    }

    IEnumerator DisplayLetters()
    {
        //Set all letters do display initially
        for(int s = 0; s < displayLetters.Count; s++)
        {
            displayLetters[s] = true;
        }

        //Decide which letters not to display
        for (int hidden = 0; hidden < amountNotDisplay; hidden++)
        {
            //Loop until found letter not already hidden
            bool found = false;
            while (!found)
            {
                //choose a random letter and hide it if not already hidden
                int chosen = Random.Range(0, displayLetters.Count);
                if (displayLetters[chosen] == true)
                {
                    displayLetters[chosen] = false;
                    found = true;
                }
                //if already hidden, choose another one
            }
        }

        for (int i = 0; i < displayString.Length; i++)
        {
            char c = displayString[i];
            int index = char.ToUpper(c) - 64; //Get Alphabetical Index

            //Calculate position so it is always centered
            float x = (0 - (xDistanceBetween * ((float)displayString.Length / 2))) + (xDistanceBetween * (i)) + (xDistanceBetween / 2);
            Vector3 newPos = new Vector3(x, displayDownY, displayZ);

            if (displayLetters[i] == true)
            {
                //Instantiate a new letter if decided to display
                GameObject newLetter = Instantiate(rigidAlphaList[index - 1], newPos, Quaternion.Euler(-90f, 180f, 0f), null);
                newLetter.GetComponent<Rigidbody>().isKinematic = true;

                //Give The letter the lerp component if decided to display
                newLetter.AddComponent<LetterDisplayLerp>().toPos = new Vector3(x, displayUpY, displayZ);
            } else
            {
                //Instantiate Placeholder if decided to hide
                GameObject newLetter = Instantiate(highlightPrefab, newPos, Quaternion.Euler(-90f, 180f, 0f), null);
                newLetter.GetComponent<Rigidbody>().isKinematic = true;

                //Give The placeholder the lerp component if decided to display
                newLetter.AddComponent<LetterDisplayLerp>().toPos = new Vector3(x, displayUpY, displayZ);
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator SpawnRigids()
    {
        foreach(GameObject alphaRigid in rigidAlphaList)
        {
            //Get New Position somewhere between rigidSpawnLeft and rigidSpawnRight
            Vector3 spawnPos = Vector3.Lerp(rigidSpawnLeft.position, rigidSpawnRight.position, Random.Range(0f, 1f));

            //Instantiate the letter at that position
            GameObject newAlpha = Instantiate(alphaRigid, spawnPos, Quaternion.Euler(-110f, 180f, 0f), null);
            newAlpha.AddComponent<LetterTouchFloor>().pluggablesController = this;

            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
    }


    //Teleport Letter back to machine if touch floor
    public void TouchedFloor(GameObject letter)
    {
        letter.transform.position = Vector3.Lerp(rigidSpawnLeft.position, rigidSpawnRight.position, Random.Range(0f, 1f));
    }
}
