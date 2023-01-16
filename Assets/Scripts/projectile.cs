using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public AudioSource hitWall;
    public AudioSource hitEnemy;

    public Rigidbody projectileParticles;

    void FixedUpdate()
    {
        if (Mathf.Abs(transform.position.x) >= 150) Destroy(gameObject);
        if (Mathf.Abs(transform.position.z) >= 150) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {   
        if(gameObject.tag == "Projectile")
        {
            if(other.gameObject.tag != "Player" && other.gameObject.tag != "ProjectileImmune")
            {      
                if(other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyBoss" || other.gameObject.tag == "Destructable" || other.gameObject.tag == "Finish") {
                    other.GetComponent<enemyScript>().changeLives(-1);
                }

                else if(other.gameObject.tag == "Untagged") {
                    hitWall.Play();
                }
                
                createParticles();
                Destroy(gameObject);
            }
        }
        
        if (gameObject.tag == "ProjectileDestructable")
        {   
            if(other.gameObject.tag != "SphereShield" && other.gameObject.tag != "Finish" && other.gameObject.tag != "Enemy" &&
               other.gameObject.tag != "ProjectileDestructable" && other.gameObject.tag != "ProjectileImmune" && other.gameObject.tag != "EnemyBoss")
            {
                if(other.gameObject.tag == "Player")
                {
                    createParticles();
                    playerScript.instance.changePlayerLives(-1);
                }
                
            Destroy(gameObject);
            }
        }

        if (gameObject.tag == "ProjectileImmune")
        {   
            if(other.gameObject.tag != "SphereShield" && other.gameObject.tag != "Finish" && other.gameObject.tag != "Enemy" &&
               other.gameObject.tag != "ProjectileDestructable" && other.gameObject.tag != "ProjectileImmune" &&
               other.gameObject.tag != "Projectile" && other.gameObject.tag != "EnemyBoss")
            {
                if(other.gameObject.tag == "Player")
                {
                    createParticles();
                    playerScript.instance.changePlayerLives(-1);
                }
                
            Destroy(gameObject);
            }
        }
    }

    public void createParticles() {
        for(int i = 0; i<3; i++) {
            Rigidbody projectileClone = (Rigidbody) Instantiate(projectileParticles, transform.position +
                new Vector3(2f * Mathf.Sin(Mathf.Deg2Rad*transform.localEulerAngles.y), 0, 2f * Mathf.Cos(Mathf.Deg2Rad*transform.localEulerAngles.y)), transform.rotation);
            projectileClone.constraints = 0;
            projectileClone.GetComponent<particle>().randomLifetime();
            projectileClone.velocity = new Vector3  (Random.Range(0f, Random.Range(-4f, 4f) + 8f * Mathf.Sin(Mathf.Deg2Rad*transform.localEulerAngles.y)),
                                                    Random.Range(-2f, 8f),
                                                    Random.Range(0f, Random.Range(-4f, 4f) + 8f * Mathf.Cos(Mathf.Deg2Rad*transform.localEulerAngles.y)));
            projectileClone.transform.localEulerAngles = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
        }
    }
}
