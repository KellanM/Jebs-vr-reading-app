using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterDisplayLerp : MonoBehaviour
{
    public Vector3 toPos;
    public float speed = 3f;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, toPos, speed * Time.deltaTime);
    }
}
