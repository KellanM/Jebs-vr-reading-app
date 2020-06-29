using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightControl : MonoBehaviour
{
    public Transform head;
    public Transform holsters;
    public Transform feet;
    public Transform scaleThis;
    public float topOffset = 0.2f;

    Vector3 posHead, posFeet, scaleBody, posBody;

    void Start()
    {
        
    }

    void Update()
    {
        // IMPORTANT: We are assuming parents scale is (1,1,1)

        posHead = head.position;
        posFeet = feet.position;

        scaleBody = scaleThis.localScale;
        scaleBody.y = posHead.y - posFeet.y + topOffset;
        scaleThis.localScale = scaleBody;

        posBody = scaleThis.position;
        posBody.y = posFeet.y + (scaleBody.y / 2);
        scaleThis.position = posBody;
        holsters.position = posBody;
    }
}
