using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesShoot : MonoBehaviour
{
    [Header("References")]
    public BubblesController controller;

    public Transform shootPlace;

    [Header("Settings")]
    public float shootThreshold;
    public float shootTimeout;


    bool timedout;
    bool shot;

    private void Update()
    {
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
