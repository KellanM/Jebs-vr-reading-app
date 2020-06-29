using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

public class XRAxisToFloat : FloatAction
{
    [Header("Listen to")]
    public UnityEngine.XR.XRNode node = UnityEngine.XR.XRNode.LeftHand;
    public Usage usage = Usage.Grip;

    float retrievedState;
    bool successfulReading;

    UnityEngine.XR.InputDevice device;

    [Header("Debugging")]
    public float currentValue;

    void Update()
    {
        var devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(node, devices);

        // If there is only one instance for this controller
        if (devices.Count == 1)
        {
            device = devices[0];

            // We read different attributes of the class CommonUsages according to "usage" desired
            switch (usage)
            {
                case Usage.Trigger:
                    successfulReading = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out retrievedState);
                    break;
                case Usage.Grip:
                    successfulReading = device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out retrievedState);
                    break;
                default:
                    successfulReading = false;
                    break;
            }

            // If we can get the buttons state succesfully
            if (successfulReading)
            {
                Receive(retrievedState);
            }
        }
        else if (devices.Count > 1)
        {
            Debug.Log("Found more than one of that hand!");
        }

        currentValue = Value;
    }
}
