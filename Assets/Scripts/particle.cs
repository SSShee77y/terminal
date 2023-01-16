using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle : MonoBehaviour
{   
    public static particle instance;

    public float lifetime = -999;

    public void Awake()
    {
        instance = this;
    }

    void FixedUpdate()
    {
        if (lifetime > -999) lifetime-= 0.1f;
        if (lifetime <= 0 && lifetime > -999) Destroy(gameObject);
    }

    public void randomLifetime() 
    {
        lifetime = Random.Range(6f, 10f);
    }

    public void randomLifetime(float addTime) 
    {
        lifetime = addTime + Random.Range(6f, 10f);
    }



}
