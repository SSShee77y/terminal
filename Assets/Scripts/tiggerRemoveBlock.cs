using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tiggerRemoveBlock : MonoBehaviour
{
    public Rigidbody changeBlock;
    public Rigidbody particles;
    public AudioSource hitEnemy;

     private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Something moved!");
            createParticles();
            hitEnemy.Play();
            Destroy(changeBlock.gameObject);
            Destroy(gameObject);
        }
    }

    public void createParticles() {
        for(int i = 0; i<10; i++) {
            Rigidbody projectileClone = (Rigidbody) Instantiate(particles, changeBlock.position, changeBlock.rotation);
            projectileClone.constraints = 0;
            projectileClone.GetComponent<particle>().randomLifetime(6f);
            projectileClone.velocity = new Vector3  (Random.Range(-4f, 4f),
                                                    Random.Range(-2f, 8f),
                                                    Random.Range(-4f, 4f));
            projectileClone.transform.localEulerAngles = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
        }
    }
}
