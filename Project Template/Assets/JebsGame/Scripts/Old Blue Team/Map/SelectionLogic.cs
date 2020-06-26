using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Haptics;
using JorgeJGnz;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public enum SelectionState
{
    Unselectable,
    Selectable,
    LoadingSelection
}

public class SelectionLogic : MonoBehaviour
{
    public SelectionState initialState = SelectionState.Unselectable;

    [Header("Objects")]
    public GameObject selectionGroupNotMe;
    public GameObject teleportationReceiver;
    public LoadingBar loadingBar;
    public Collider pokeTrigger;
    public Collider pokeL;
    public Collider pokeR;

    List<GameObject> enteredAreas = new List<GameObject>();

    [Header("Events")]
    public UnityEvent onUnselectable;
    public UnityEvent onSelectable;
    public UnityEvent onLoadingSelection;
    public UnityEvent onTeleport;

    bool triggerIsEnabled
    {
        get { return _triggerIsEnabled; }
        set
        {
            _triggerIsEnabled = value;
            if (!value)
            {
                LinTrigger = false;
                RinTrigger = false;
            }
        }
    }
    bool LinTrigger
    {
        get { return _LinTrigger; }
        set
        {
            _LinTrigger = value;
            if (currentPoke == pokeL.gameObject) StopLoadingBar();
        }
    }
    bool RinTrigger
    {
        get { return _RinTrigger; }
        set
        {
            _RinTrigger = value;
            if (currentPoke == pokeR.gameObject) StopLoadingBar();
        }
    }
    bool areaEntered
    {
        get { return _areaEntered; }
        set
        {
            // For the first/last area touched/untouched
            _areaEntered = value;
            if (!value) StopLoadingBar();
        }
    }

    [Header("Debugging")]
    public string stringCurrentState;
    public bool _triggerIsEnabled;
    public bool _LinTrigger;
    public bool _RinTrigger;
    public bool _areaEntered;
    public GameObject currentPoke;
    public SelectableArea currentArea;

    public SelectionState currentState
    {
        get { return _currentState; }
        set
        {
            _currentState = value;
            switch (value)
            {
                case SelectionState.Unselectable:
                    
                    //Objects
                    selectionGroupNotMe.SetActive(false);

                    //Variables
                    triggerIsEnabled = false;
                    areaEntered = false;

                    // Functions
                    StopLoadingBar();

                    onUnselectable.Invoke();

                    Debug.Log("Switched to UNSELECTABLE!");

                    break;
                case SelectionState.Selectable:

                    // Objects
                    selectionGroupNotMe.SetActive(true);

                    // Variables
                    triggerIsEnabled = true;
                    areaEntered = false;

                    onSelectable.Invoke();

                    Debug.Log("Switched to SELECTABLE!");

                    break;
                case SelectionState.LoadingSelection:

                    onLoadingSelection.Invoke();

                    break;
            }

            //Debugging
            stringCurrentState = currentState.ToString();
        }
    }
    SelectionState _currentState;

    private void Start()
    {
        currentState = initialState;
    }

    private void Update()
    {
        if (currentState == SelectionState.LoadingSelection)
        {
            if (!currentPoke.activeSelf)
            {
                enteredAreas = new List<GameObject>();
                areaEntered = false;
                currentState = SelectionState.Selectable;
            }
        }
    }

    // Only area triggers should be able to call onAreaEntered
    public void onAreaEntered(GameObject objArea, GameObject pokeVisitor)
    {
        // For every new area touched
        loadingBar.transform.position = objArea.transform.position;
        
        currentPoke = pokeVisitor;

        SelectableArea a = objArea.GetComponent<SelectableArea>();
        if (a != null) currentArea = a;

        if (currentState != SelectionState.LoadingSelection) currentState = SelectionState.LoadingSelection;

        if (!enteredAreas.Contains(objArea)) enteredAreas.Add(objArea);

        RestartLoadingBar();

        if (!areaEntered) areaEntered = true;

        Debug.Log("AREA ENTERED!");
    }

    public void onAreaExited(GameObject objArea, GameObject pokeVisitor)
    {
        if (enteredAreas.Contains(objArea)) enteredAreas.Remove(objArea);

        if (enteredAreas.Count == 0)
        {
            areaEntered = false;
            if (currentState == SelectionState.LoadingSelection) currentState = SelectionState.Selectable;
        }

        // For now, there is no haptic feedback from exiting poke so pokeVisitor is not used here
        // But we can use it if needed

        Debug.Log("AREA EXITED!");

    }

    public void SetLinTrigger(bool b)
    {
        LinTrigger = b;
    }

    public void SetRinTrigger(bool b)
    {
        RinTrigger = b;
    }

    public void StopLoadingBar()
    {
        loadingBar.gameObject.SetActive(false);
        loadingBar.canAnimate = false;
    }

    public void RestartLoadingBar()
    {
        loadingBar.gameObject.SetActive(true);
        loadingBar.canAnimate = true;
        loadingBar.ResetTimer();
    }

    // Only called when the map is rolled/unrolled
    public void SetSelectable(bool b)
    {
        if (b)
        {
            if (currentState != SelectionState.Selectable) currentState = SelectionState.Selectable;
        }
        else
        {
            if (currentState != SelectionState.Unselectable) currentState = SelectionState.Unselectable;
        }
    }

    public void Teleport()
    {
        if (currentArea != null)
        {
            teleportationReceiver.transform.position = currentArea.destination.position;
            teleportationReceiver.transform.rotation = currentArea.destination.rotation;
            onTeleport.Invoke();
        }
    }

    public void SetCurrentPoke(GameObject poke)
    {
        currentPoke = poke;
    }

    public void SetCurrentArea(GameObject area)
    {
        SelectableArea a = area.GetComponent<SelectableArea>();
        if (a != null) currentArea = a;
    }

    public void GetHapticFromCurrentPoke()
    {
        if (currentPoke != null)
        {
            XRNodeHapticPulser haptic = currentPoke.GetComponent<XRNodeHapticPulser>();

            if (haptic != null)
            {
                haptic.Begin();
            }
        }
    }
}
