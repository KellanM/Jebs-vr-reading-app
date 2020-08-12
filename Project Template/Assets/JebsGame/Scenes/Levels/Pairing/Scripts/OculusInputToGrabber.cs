using UnityEngine;

public class OculusInputToGrabber : MonoBehaviour
{
    public UnityHelpers.Grabber grabber;
    public OVRHand.Hand handType;
    public float grabMinValue = 0.2f;
    private float gripValue;

    void Update()
    {
        if (handType == OVRHand.Hand.HandLeft)
            gripValue = OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger, OVRInput.Controller.All);
        else
            gripValue = OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger, OVRInput.Controller.All);

        grabber.grab = gripValue >= grabMinValue;
    }
}
