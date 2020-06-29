using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    public Transform head;
    public Transform floor;
    public Transform moveThis;

    Vector3 correctPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (!moveThis) moveThis = transform; 
    }

    // Update is called once per frame
    void Update()
    {
        correctPosition = head.transform.position;
        correctPosition.y = floor.position.y;
        moveThis.position = correctPosition;
    }
}
