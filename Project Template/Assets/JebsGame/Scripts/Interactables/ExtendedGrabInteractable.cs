using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ExtendedGrabInteractable : XRBaseInteractable
{
    // The default interaction events are displayed in the inspector for any inherited class, even if it is empty

    // If this stops working, we can use the commented code

    /*
    public delegate void GrabEvent(XRBaseInteractable interactable, XRBaseInteractor interactor);

    public event GrabEvent onHover;
    public event GrabEvent onGrab;
    public event GrabEvent onUngrab;
    public event GrabEvent onUnhover;

    void Start()
    {
        onHoverEnter.AddListener(Hover);
        onSelectEnter.AddListener(Grab);
        onSelectExit.AddListener(Ungrab);
        onHoverExit.AddListener(Unhover);
    }

    void Hover(XRBaseInteractor interactor)
    {
        onHover?.Invoke(this, interactor);
    }

    void Grab(XRBaseInteractor interactor)
    {
        onGrab?.Invoke(this, interactor);
    }

    void Ungrab(XRBaseInteractor interactor)
    {
        onUngrab?.Invoke(this, interactor);
    }

    void Unhover(XRBaseInteractor interactor)
    {
        onUnhover?.Invoke(this, interactor);
    }
    */
}
