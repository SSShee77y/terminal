using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onShieldBreak : MonoBehaviour
{
    public Rigidbody bossEnemy;
    public int requiredDestroyed;
    public int faceChange = 1;

    void FixedUpdate()
    {
        if(scoreUI.instance.getHits() >= requiredDestroyed) {
            bossEnemy.GetComponent<bossScript>().setFaces(faceChange);
        }
    }
}
