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
        bubble.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
        bubble.transform.GetChild(0).parent = null;
        bubble.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(SpawnBubbles());
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
    }

}
