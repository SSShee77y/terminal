using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hurtPlayer : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {   
        if (other.gameObject.tag == "Player"){
            Physics.IgnoreCollision(other.gameObject.GetComponent<CapsuleCollider>(), GetComponent<Collider>(), true);
        }
    }
}
