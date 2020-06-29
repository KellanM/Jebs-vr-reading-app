using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class BubblesShoot : MonoBehaviour
{
    [Header("References")]
    public BubblesController controller;
    public GameObject laserPrefab;
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
            if (value > shootThreshold)
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
                    controller.ShotBubble(hit.transform.root.gameObject, shootPlace.transform.forward, hit.point);
                }
                Laser(shootPlace.transform.position, hit.point);
            } else
            {
                Laser(shootPlace.transform.position, shootPlace.transform.position + (shootPlace.transform.forward * 500f));
            }
            timedout = true;
            shot = true;
            CancelInvoke();
            Invoke("AllowShot", shootTimeout);
        }
    }

    public void Laser(Vector3 from, Vector3 to)
    {
        Vector3 scale = new Vector3(1, 1, 1);
        scale.z = Vector3.Distance(from, to)/2;

        Vector3 pos = Vector3.Lerp(from, to, 0.5f);

        GameObject newLaser = Instantiate(laserPrefab, pos, Quaternion.identity, null);
        newLaser.transform.localScale = scale;
        newLaser.transform.forward = (from - to).normalized;

        newLaser.AddComponent<LaserFadeOut>();
    }

    public void TriggerUp()
    {
        shot = false;
    }
}
