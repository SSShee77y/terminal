using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldScript : MonoBehaviour
{
    public Rigidbody particles;
    public AudioSource hitEnemy;
    public int requiredDestroyed;
    public static shieldScript instance;

    public void Awake()
    {
        instance = this;
    }

    void OnCollisionEnter(Collision other)
    {   
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Projectile" && other.gameObject.tag != "Enemy"){
            Physics.IgnoreCollision(other.collider, GetComponent<Collider>(), true);
        }
    }

    void FixedUpdate()
    {
        if(scoreUI.instance.getHits() >= requiredDestroyed) {
            destroyShield();
        }
    }

    public void createParticles() {
        for(int i = 0; i<15; i++) {
            Rigidbody projectileClone = (Rigidbody) Instantiate(particles, gameObject.transform.position, gameObject.transform.rotation);
            projectileClone.constraints = 0;
            projectileClone.GetComponent<particle>().randomLifetime(6f);
            projectileClone.velocity = new Vector3  (Random.Range(-6f, 6f),
                                                    Random.Range(-2f, 10f),
                                                    Random.Range(-6f, 6f));
            projectileClone.transform.localEulerAngles = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
        }
    }

    public void destroyShield() {
        createParticles();
        hitEnemy.Play();
        Destroy(gameObject);
    }
}
