using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChestController : MonoBehaviour
{
    public Collider hitTrigger;
    public Collider interiorTrigger;

    [Header("Enable when ready")]
    public GameObject[] objects;
    public Collider[] colliders;
    public Rigidbody[] nonKinematicRbs;

    [Header("Other")]

    public float chestAnimationSpeed = 1.0f;

    public UnityEvent onHit;

    public int chestStrength = 3;

    public Transform letterSpawn;

    public Transform pivot;
    public Vector3 pivotRot;

    [Header("Open/Close animation")]
    float closedAngle = 359.0f;
    public float semiClosedAngle = 340.0f;
    float openAngle = 181.0f;

    public UnityEvent onOpen;
    public UnityEvent onClose;

    int chestHealth;
    bool animateChest = false;

    public bool chestIsOpen = false;

    CrabFactory factory;
    LetterGenerator letterGen;
    ChestLetter myLetter;

    float startTime, journeyLength, distCovered, fractionOfJourney;

    void Start()
    {
        chestHealth = chestStrength;
        SetInteractable(false);

        factory = CrabFactory.factory;
        letterGen = LetterGenerator.letterGen;
    }

    void Update()
    {
        if (animateChest)
        {
            distCovered = (Time.time - startTime) * chestAnimationSpeed;
            fractionOfJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(transform.position, factory.chestDestination.position, fractionOfJourney);

            transform.rotation = factory.chestDestination.rotation;

            if (transform.position == factory.chestDestination.position)
            {
                animateChest = false;
                ChestReady();
            }
        }
        // 359 a 330 cerrado, > 180 y < 330 cerrado, <180 indeterminado
        pivotRot = pivot.localRotation.eulerAngles;

        if (pivotRot.x <= closedAngle && pivotRot.x >= semiClosedAngle && chestIsOpen)
        {
            if (interiorTrigger.bounds.Contains(myLetter.transform.position))
            {
                chestIsOpen = false;

                BagController.bag.Evaluate(myLetter,false);

                /*
                factory.ToggleCrabs(true);
                Destroy(this.gameObject);
                */
            }
        }
        else if (pivotRot.x < semiClosedAngle && pivotRot.x >= openAngle && !chestIsOpen)
            chestIsOpen = true;
    }

    public void ChestHit()
    {
        onHit.Invoke();
        chestHealth--;
        if (chestHealth == 0)
            ReleaseChest();
    }

    public void ReleaseChest()
    {
        factory.ToggleCrabs(false);
        transform.parent = factory.transform;

        animateChest = true;
        startTime = Time.time;
        journeyLength = Vector3.Distance(transform.position, factory.chestDestination.position);
    }

    public void ChestReady()
    {
        SetInteractable(true);
        myLetter = letterGen.Generate(letterSpawn);
    }

    public void SetInteractable(bool b)
    {
        hitTrigger.enabled = !b;

        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(b);
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = b;
        }

        for (int i = 0; i < nonKinematicRbs.Length; i++)
        {
            nonKinematicRbs[i].isKinematic = !b;
        }
    }

    public void TogglePivot()
    {
        if (!chestIsOpen)
        {
            pivot.Rotate(new Vector3(150.0f, 0.0f, 0.0f), Space.Self);
            chestIsOpen = true;

            onOpen.Invoke();
        }
        else
        {
            pivot.Rotate(new Vector3(-150.0f, 0.0f, 0.0f), Space.Self);
            chestIsOpen = false;

            onClose.Invoke();

            if (interiorTrigger.bounds.Contains(myLetter.transform.position))
            {
                BagController.bag.Evaluate(myLetter, false);

                /*
                factory.ToggleCrabs(true);
                Destroy(this.gameObject);
                */
            }

        }

    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(ChestController))]
public class ChestControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChestController myScript = (ChestController)target;
        if (GUILayout.Button("Set chest ready"))
        {
            myScript.ChestReady();
        }

    }
}
#endif

