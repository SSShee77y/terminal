using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float x = 0f;
    [SerializeField] float y = 2f;
    [SerializeField] float z = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(x*Time.deltaTime*200, y*Time.deltaTime*200, z*Time.deltaTime*200);
    }
}
