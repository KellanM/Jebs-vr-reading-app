using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zinnia.Data.Type.Transformation.Conversion;
using TMPro;
using System.Reflection;

public class ListTools
{
    public static T RandomValueIn<T> (List<T> list)
    {
        int chosen = Random.Range(0, list.Count);
        return list[chosen];
    }
}

public class BubblesController : MonoBehaviour
{
    [Header("References")]
    public GameObject bubbleGun;
    public Transform gunRestPlace;
    public GameObject bubbleEmitter;

    public GameObject feedbackSkybox;

    public List<GameObject> letterBubblePrefabs = new List<GameObject>();

    public TMP_Text timerText;

    [Header("Script Data")]
    public List<GameObject> instantiatedBubbles = new List<GameObject>();
    [SerializeField] public List<GameObject> instantiateQueue = new List<GameObject>();

    public List<string> shotBubbles = new List<string>();

    [Header("Settings")]
    public int gridSize;

    public float baseY;

    public float xBetween;
    public float yBetween;

    public float timeAllowed;
    public bool timed;

    [Header("Debug")]
    public bool greenSkybox;
    public bool redSkybox;

    float timeLeft;

    IEnumerator SpawnBubbles()
    {
        yield return new WaitForSeconds(0.2f);
        instantiateQueue = new List<GameObject>(gridSize * gridSize);

        //Populate the list with nulls
        for(int i = 0; i < gridSize * gridSize; i++)
        {
            instantiateQueue.Add(null);
        }

        //Make sure atleast one of each letter is queued to spawn;
        foreach(GameObject letterBubblePrefab in letterBubblePrefabs)
        {
            //Make sure doesnt override other letter queued
            bool queued = false;
            while (!queued)
            {
                int chosenId = Random.Range(0, gridSize * gridSize);
                if(instantiateQueue[chosenId] == null)
                {
                    instantiateQueue[chosenId] = letterBubblePrefab;
                    queued = true;
                }
            }
        }

        //Populate unpopulated places in the grid
        for(int i = 0; i < gridSize * gridSize; i += 1)
        {
            if(instantiateQueue[i] == null)
            {
                instantiateQueue[i] = ListTools.RandomValueIn(letterBubblePrefabs);
            }
        }

        //Instantiate all queued Bubbles
        for(int i = 0; i < instantiateQueue.Count; i += 1)
        {
            GameObject newBubble = Instantiate(instantiateQueue[i], bubbleEmitter.transform.position, Quaternion.identity, null);
            instantiatedBubbles.Add(newBubble);

            newBubble.transform.name = instantiateQueue[i].name;

            //Calculate target position
            float x = (bubbleEmitter.transform.position.x - ((xBetween * gridSize) / 2)) + (xBetween * (i - (gridSize * (Mathf.Floor((i)/gridSize))))) + xBetween/2;
            float y = baseY + ((yBetween * gridSize) - ((Mathf.Floor((i) / gridSize)) * yBetween));

            newBubble.AddComponent<LetterDisplayLerp>().toPos = new Vector3(x, y, newBubble.transform.position.z);
            yield return new WaitForSeconds(0.02f);
        }

    }

    public void ShotBubble(GameObject bubble, Vector3 dir, Vector3 point)
    {
        //Called by gun when bubble shot
        shotBubbles.Add(bubble.transform.name);
        bubble.transform.GetChild(0).gameObject.AddComponent<DestroyAfterDelay>().StartDelayWithScale(2f);
        bubble.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
        bubble.transform.GetChild(0).gameObject.GetComponent<Rigidbody>().AddForceAtPosition(dir * 2f, point, ForceMode.Impulse);
        bubble.transform.GetChild(0).parent = null;
        Destroy(bubble);
        CheckIfWin();
    }

    void CheckIfWin(bool decisive = false)
    {
        if (!decisive)
        {
            for (int i = 0; i < shotBubbles.Count; i++)
            {
                if (shotBubbles[i] == letterBubblePrefabs[i].transform.name)
                {
                    if (i == shotBubbles.Count - 1)
                    {
                        if (shotBubbles.Count == letterBubblePrefabs.Count)
                        {
                            StartCoroutine(PositiveFeedback());
                        }
                    }
                }
                else
                {
                    StartCoroutine(NegativeFeedback());
                }
            }
        } else
        {
            List<string> compareList = new List<string>();
            foreach(GameObject letterBubblePrefab in letterBubblePrefabs)
            {
                compareList.Add(letterBubblePrefab.name);
            }

            if(shotBubbles == compareList)
            {
                StartCoroutine(PositiveFeedback());
            } else
            {
                StartCoroutine(NegativeFeedback());
            }
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnBubbles());

        if (timerText == null)
        {
            timed = false;
        }

        if (timed)
        {
            StartCoroutine("TimerControl");
        }
    }

    IEnumerator TimerControl()
    {
        timeLeft = timeAllowed;
        bool finished = false;
        while (!finished)
        {
            yield return new WaitForSeconds(1f);
            if (timeLeft > 0)
            {
                timeLeft -= 1;
            } else
            {
                finished = true;
                CheckIfWin(true);
            }
        }
    }

    void ResetEverything()
    {
        instantiateQueue = new List<GameObject>();
        shotBubbles = new List<string>();
        instantiatedBubbles = new List<GameObject>();
        StartCoroutine(SpawnBubbles());
        StopCoroutine("TimerControl");
        if (timed)
        {
            StartCoroutine("TimerControl");
        }
    }

    void DeleteEverything()
    {
        foreach(GameObject instantiatedBubble in instantiatedBubbles)
        {
            if (instantiatedBubble != null && instantiatedBubble.transform.childCount != 0)
            {
                instantiatedBubble.transform.GetChild(0).gameObject.AddComponent<DestroyAfterDelay>().StartDelayWithScale(2f);
                instantiatedBubble.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
                instantiatedBubble.transform.GetChild(0).parent = null;
                Destroy(instantiatedBubble);
            }
        }
    }

    IEnumerator NegativeFeedback()
    {
        DeleteEverything();
        redSkybox = true;
        yield return new WaitForSeconds(1f);
        redSkybox = false;
        ResetEverything();
    }

    IEnumerator PositiveFeedback()
    {
        DeleteEverything();
        greenSkybox = true;
        yield return new WaitForSeconds(1f);
        greenSkybox = false;
        ResetEverything();
    }

    private void Update()
    {
        //If gun is not held, lerp to gunRestPlace
        if(bubbleGun.GetComponent<GrabIsHeld>().isHeld == false)
        {
            bubbleGun.GetComponent<Rigidbody>().velocity /= 2;
            bubbleGun.GetComponent<Rigidbody>().angularVelocity /= 2;
            bubbleGun.transform.position = Vector3.Lerp(bubbleGun.transform.position, gunRestPlace.transform.position, 3f * Time.deltaTime);
            bubbleGun.transform.rotation = Quaternion.Lerp(bubbleGun.transform.rotation, gunRestPlace.transform.rotation, 3f * Time.deltaTime);
        }

        //Blend Skybox Colors
        if (greenSkybox)
        {
            feedbackSkybox.GetComponent<MeshRenderer>().material.color = Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.color, Color.green, 9f * Time.deltaTime);
            float factor = Mathf.Pow(2, 1f);
            Color color = new Color(0f * factor, 1f * factor, 0f * factor);
            feedbackSkybox.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), color, Time.deltaTime * 9f));
        }
        else
        {
            if (redSkybox)
            {
                feedbackSkybox.GetComponent<MeshRenderer>().material.color = Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.color, Color.red, 9f * Time.deltaTime);
                float factor = Mathf.Pow(2, 1f);
                Color color = new Color(1f * factor, 0f * factor, 0f * factor);
                feedbackSkybox.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), color, Time.deltaTime * 9f));
            }
            else
            {
                feedbackSkybox.GetComponent<MeshRenderer>().material.color = Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.color, new Color(0, 0, 0, 0), 3f * Time.deltaTime);
                float factor = Mathf.Pow(2, -10f);
                Color color = new Color(0f * factor, 0f * factor, 0f * factor);
                feedbackSkybox.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.Lerp(feedbackSkybox.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"), color, Time.deltaTime * 3f));
            }
        }

        if (timed)
        {
            timerText.text = timeLeft.ToString();
        }
    }

}
