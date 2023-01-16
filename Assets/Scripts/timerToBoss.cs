using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timerToBoss : MonoBehaviour
{
    public int timeLimit = 30;
    public Transform enemyObject;
    private bool destroyed;
    private float timer;

    void Update()
    {
        if (destroyed == false) {
            if(timer < timeLimit) timer = Mathf.Round(Time.timeSinceLevelLoad * 100f) / 100f;
            else if(timer >= timeLimit) {
                enemyObject.GetComponent<shieldScript>().destroyShield();
                destroyed = true;
            }   
        }
    }
}
