using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    [Header("Script Data")]
    public List<GameObject> instantiatedBubbles = new List<GameObject>();
    [SerializeField] public List<GameObject> instantiateQueue = new List<GameObject>();

    public List<string> shotBubbles = new List<string>();

    [Header("Settings")]
    public int gridSize;

    public float baseY;

    public float xBetween;
    public float yBetween;

    [Header("Debug")]
    public bool greenSkybox;
    public bool redSkybox;

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

    public void ShotBubble(GameObject bubble)
    {
        //Called by gun when bubble shot
        shotBubbles.Add(bubble.transform.name);
        bubble.transform.GetChild(0).gameObject.AddComponent<DestroyAfterDelay>().StartDelay(2f);
        bubble.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
        bubble.transform.GetChild(0).parent = null;
        bubble.SetActive(false);
        CheckIfWin();
    }

    void CheckIfWin()
    {
        for(int i = 0; i < shotBubbles.Count; i++)
        {
            if(shotBubbles[i] == letterBubblePrefabs[i].transform.name)
            {
                if(i == shotBubbles.Count - 1)
                {
                    if(shotBubbles.Count == letterBubblePrefabs.Count)
                    {
                        StartCoroutine(PositiveFeedback());
                    }
                }
            } else
            {
                StartCoroutine(NegativeFeedback());
            }
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnBubbles());
    }

    void ResetEverything()
    {
        instantiateQueue = new List<GameObject>();
        shotBubbles = new List<string>();
        instantiatedBubbles = new List<GameObject>();
        StartCoroutine(SpawnBubbles());
    }

    void DeleteEverything()
    {
        foreach(GameObject instantiatedBubble in instantiatedBubbles)
        {
            if (instantiatedBubble.activeSelf == true)
            {
                instantiatedBubble.transform.GetChild(0).gameObject.AddComponent<DestroyAfterDelay>().StartDelay(2f);
                instantiatedBubble.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
                instantiatedBubble.transform.GetChild(0).parent = null;
                instantiatedBubble.SetActive(false);
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
    }

}
