using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BubblesShoot : MonoBehaviour
{
    [Header("References")]
    public BubblesController controller;

    public Transform shootPlace;

    [Header("Settings")]
    public float shootThreshold;
    public float shootTimeout;

    public UnityEngine.XR.XRNode node = UnityEngine.XR.XRNode.RightHand;

    bool timedout;
    bool shot;

    //Unity XR very complicated for getting values of Triggers

    private void Update()
    {
        //Get list of devices
        var devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(node, devices);

        //Loop through each device
        for (int i = 0; i < devices.Count; i++)
        {
            //Get the value of the trigger
            float value;
            devices[i].TryGetFeatureValue(CommonUsages.trigger, out value);
            print(value);

            //Shoot if value higher than 0.7
            if (value > 0.7f)
            {
                TriggerDown();
            } else
            {
                TriggerUp();
            }
        }
    }

    void AllowShot()
    {
        timedout = false;
    }

    public void TriggerDown()
    {
        if (!timedout && !shot)
        {
            Debug.DrawRay(shootPlace.transform.position, shootPlace.transform.forward * 500f, Color.red, 5f);
            RaycastHit hit;
            if (Physics.Raycast(shootPlace.transform.position, shootPlace.transform.forward, out hit))
            {
                if (hit.transform.root.gameObject.tag == "Bubble")
                {
                    controller.ShotBubble(hit.transform.root.gameObject);
                }
            }
            timedout = true;
            shot = true;
            CancelInvoke();
            Invoke("AllowShot", shootTimeout);
        }
    }

    public void TriggerUp()
    {
        shot = false;
    }
}
