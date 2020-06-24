using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;


[System.Serializable]
public class PluggableGap
{
    public GameObject gap;
    public char characterNeeded;
    public bool filled;
    public bool filledCorrectly;
    public GameObject filler;

    public PluggableGap(GameObject _gap, char _characterNeeded)
    {
        gap = _gap;
        characterNeeded = _characterNeeded;
    }
}

[System.Serializable]
public class PluggableLetter
{
    public GameObject letter;
    public char characterValue;

    public PluggableLetter(GameObject _letter, char _characterValue)
    {
        letter = _letter;
        characterValue = _characterValue;
    }
}

public class PluggablesController : MonoBehaviour
{
    [Header("References")]
    public TMP_Text counter;
    [SerializeField] public List<PluggableLetter> rigidAlphaList = new List<PluggableLetter>();
    public List<bool> displayLetters = new List<bool>();

    public GameObject feedbackSkybox;

    [Header("Script Data")]
    //Lists of objects to destroy
    [SerializeField] public List<PluggableGap> gaps = new List<PluggableGap>();
    [SerializeField] public List<PluggableLetter> letters = new List<PluggableLetter>();
    public List<GameObject> displayLetterObjects = new List<GameObject>();

    public GameObject highlightPrefab;

    [Header("PositionTransforms")]
    public Transform rigidSpawnLeft;
    public Transform rigidSpawnRight;

    [Header("Settings")]
    public int timeAllowed;
    public string displayString;
    public int amountNotDisplay;
    public float distanceToCountAsFilled;

    [Header("DisplayPositionalReferences")]
    public float displayDownY;
    public float displayUpY;
    public float xDistanceBetween;
    public float displayZ;

    int currentTime;
    bool congratulated;

    [Header("Debug")]
    public bool greenSkybox;
    public bool redSkybox;

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

        CheckGaps();

        //Blend Skybox Colors
        if (greenSkybox)
        {
            feedbackSkybox.GetComponent<MeshRenderer>().material.color = Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.color, Color.green, 9f * Time.deltaTime);
            float factor = Mathf.Pow(2, 1f);
            Color color = new Color(0f * factor, 1f * factor, 0f * factor);
            feedbackSkybox.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), color, Time.deltaTime * 9f));
        } else
        {
            if (redSkybox)
            {
                feedbackSkybox.GetComponent<MeshRenderer>().material.color = Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.color, Color.red, 9f * Time.deltaTime);
                float factor = Mathf.Pow(2, 1f);
                Color color = new Color(1f * factor, 0f * factor, 0f * factor);
                feedbackSkybox.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), color, Time.deltaTime * 9f));
            } else
            {
                feedbackSkybox.GetComponent<MeshRenderer>().material.color = Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.color, new Color(0,0,0,0), 3f * Time.deltaTime);
                float factor = Mathf.Pow(2, -10f);
                Color color = new Color(0f * factor, 0f * factor, 0f * factor);
                feedbackSkybox.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), color, Time.deltaTime * 3f));
            }
        }
    }

    void ResetEverything()
    {
        currentTime = timeAllowed;

        //Spawn The Physics Alphabet Prefabs
        StartCoroutine(SpawnRigids());

        //Display the chosen letters
        StartCoroutine(DisplayLetters());

        congratulated = false;
    }

    public void SeeIfCorrect()
    {
        bool completed = true;
        foreach(PluggableGap gap in gaps)
        {
            if(gap.filledCorrectly == false)
            {
                completed = false;
            }
        }

        if (completed)
        {
            if (!congratulated)
            {
                congratulated = true;
                StartCoroutine(PositiveFeedback());
            }
        } else
        {
            if (!congratulated)
            {
                congratulated = true;
                StartCoroutine(NegativeFeedback());
            }
        }
    }

    void DeleteLetters()
    {
        foreach(PluggableGap gap in gaps)
        {
            Destroy(gap.gap);
        }
        gaps = new List<PluggableGap>();

        foreach(PluggableLetter letter in letters)
        {
            //Check if object is being held

            if (letter.letter.GetComponent<XRGrabInteractable>().m_beingHeld)
            {
                //Hide Object
                letter.letter.transform.localScale = Vector3.zero;
            } else
            {
                //Destroy Object
                Destroy(letter.letter);
            }
        }
        letters = new List<PluggableLetter>();

        foreach (GameObject displayLetter in displayLetterObjects)
        {
            Destroy(displayLetter);
        }
        displayLetterObjects = new List<GameObject>();
    }

    IEnumerator NegativeFeedback()
    {
        DeleteLetters();
        redSkybox = true;
        yield return new WaitForSeconds(1f);
        redSkybox = false;
        ResetEverything();
    }

    IEnumerator PositiveFeedback()
    {
        DeleteLetters();
        greenSkybox = true;
        yield return new WaitForSeconds(1f);
        greenSkybox = false;
        ResetEverything();
    }

    void TimerIterate()
    {
        //Minus 1 from current time every second
        if (currentTime > 0)
        {
            currentTime -= 1;
        } else
        {
            SeeIfCorrect();
        }
    }

    IEnumerator DisplayLetters()
    {
        //Set all letters do display initially
        for (int s = 0; s < displayLetters.Count; s++)
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
                GameObject newLetter = Instantiate(rigidAlphaList[index - 1].letter, newPos, Quaternion.Euler(-90f, 180f, 0f), null);
                newLetter.GetComponent<Rigidbody>().isKinematic = true;

                //Give The letter the lerp component if decided to display
                newLetter.AddComponent<LetterDisplayLerp>().toPos = new Vector3(x, displayUpY, displayZ);

                Destroy(newLetter.GetComponent<XRGrabInteractable>());

                displayLetterObjects.Add(newLetter);
            } else
            {
                //Instantiate Placeholder if decided to hide
                GameObject newLetter = Instantiate(highlightPrefab, newPos, Quaternion.Euler(-90f, 180f, 0f), null);
                newLetter.GetComponent<Rigidbody>().isKinematic = true;

                //Give The placeholder the lerp component if decided to display
                newLetter.AddComponent<LetterDisplayLerp>().toPos = new Vector3(x, displayUpY, displayZ);

                //Add gap to list
                gaps.Add(new PluggableGap(newLetter, c));
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator SpawnRigids()
    {
        foreach (PluggableLetter alphaRigidData in rigidAlphaList)
        {
            GameObject alphaRigid = alphaRigidData.letter;
            //Get New Position somewhere between rigidSpawnLeft and rigidSpawnRight
            Vector3 spawnPos = Vector3.Lerp(rigidSpawnLeft.position, rigidSpawnRight.position, Random.Range(0f, 1f));

            //Instantiate the letter at that position
            GameObject newAlpha = Instantiate(alphaRigid, spawnPos, Quaternion.Euler(-110f, 180f, 0f), null);
            newAlpha.AddComponent<LetterTouchFloor>().pluggablesController = this;

            //Add letter to list
            letters.Add(new PluggableLetter(newAlpha, alphaRigidData.characterValue));

            yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }
    }


    //Teleport Letter back to machine if touch floor
    public void TouchedFloor(GameObject letter)
    {
        letter.transform.position = Vector3.Lerp(rigidSpawnLeft.position, rigidSpawnRight.position, Random.Range(0f, 1f));
    }

    //Check for letters in gaps
    public void CheckGaps()
    {
        //Check each gap for overlapping colliders
        foreach(PluggableGap gap in gaps)
        {
            foreach (PluggableLetter letter in letters)
            {
                //see if the original filler is still there
                if (letter.letter == gap.filler)
                {
                    if (Vector3.Distance(letter.letter.transform.position, gap.gap.transform.position) < distanceToCountAsFilled)
                    {
                        gap.filled = true;
                        gap.filler.GetComponent<Rigidbody>().useGravity = false;
                        gap.filler.transform.position = (gap.gap.transform.position);
                        gap.filler.transform.rotation = (gap.gap.transform.rotation);
                    } else
                    {
                        gap.filled = false;
                        gap.filler.GetComponent<Rigidbody>().useGravity = true;
                        gap.filler = null;
                        gap.filledCorrectly = false;
                    }
                } else
                {
                    //check if the gap isnt already filled
                    if (gap.filled == false)
                    {
                        if (Vector3.Distance(letter.letter.transform.position, gap.gap.transform.position) < distanceToCountAsFilled) {
                            gap.filler = letter.letter;
                            gap.filled = true;
                            gap.filler.GetComponent<Rigidbody>().useGravity = false;
                            gap.filler.transform.position = (gap.gap.transform.position);
                            gap.filler.transform.rotation = (gap.gap.transform.rotation);
                            if(gap.characterNeeded == letter.characterValue)
                            {
                                gap.filledCorrectly = true;
                            }
                        }
                    }
                }

                //Sloppy code butcant think of any other way to do it
                //If distance is too large and is ot filling any other gaps, enable gravity
                bool isFilling = false;
                foreach (PluggableGap gapCheck in gaps)
                {
                    if (gapCheck.filler == letter.letter)
                    {
                        isFilling = true;
                    }
                }
                if(!isFilling && Vector3.Distance(letter.letter.transform.position, gap.gap.transform.position) > distanceToCountAsFilled)
                {
                    letter.letter.GetComponent<Rigidbody>().useGravity = true;
                }
            }
        }
    }
}
