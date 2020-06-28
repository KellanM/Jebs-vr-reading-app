using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblesController : MonoBehaviour
{
    [Header("References")]
    public GameObject bubbleGun;
    public Transform gunRestPlace;

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
