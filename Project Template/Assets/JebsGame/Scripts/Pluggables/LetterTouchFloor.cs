using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterTouchFloor : MonoBehaviour
{
    public PluggablesController pluggablesController;
    public float killHeight = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            pluggablesController.TouchedFloor(gameObject);
        }
    }

    private void Update()
    {
        if(transform.position.y < killHeight)
        {
            pluggablesController.TouchedFloor(gameObject);
        }
    }
}
