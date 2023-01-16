using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    public static enemyScript instance;
    public AudioSource hitEnemy;
    public Rigidbody particles;
    
    public int lives;

    private Color originalColor;
    private int damaged;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        originalColor = GetComponent<MeshRenderer>().material.color;
    }

    void FixedUpdate()
    {
        if(GetComponent<MeshRenderer>().material.color != originalColor) damaged--;
        if(GetComponent<MeshRenderer>().material.color != originalColor && damaged % 4 == 0) GetComponent<MeshRenderer>().material.color = originalColor;
        else if (damaged != 0 && damaged % 2 == 0) GetComponent<MeshRenderer>().material.color = Color.white;
    }

    public void changeLives(int i)
    {
        lives += i;
        GetComponent<MeshRenderer>().material.color = Color.white;
        damaged = 15;
        checkLives();
    }

    public void setLives(int i)
    {
        lives = i;
        checkLives();
    }

    public int getLives()
    {
        return lives;
    }

    public void checkLives()
    {
        if (lives == 0) {
            scoreUI.instance.addHits();
            hitEnemy.Play();
            if (gameObject.tag == "Finish") {
                scoreUI.instance.onFinish();
                Debug.Log("Finished!");
            }
            if(gameObject.tag != "Destructable") createParticles();
            Destroy(gameObject);
        }
    }

    public void createParticles() {
        for(int i = 0; i<10; i++) {
            Rigidbody projectileClone = (Rigidbody) Instantiate(particles, transform.position, transform.rotation);
            projectileClone.constraints = 0;
            projectileClone.GetComponent<particle>().randomLifetime(6f);
            projectileClone.velocity = new Vector3  (Random.Range(-4f, 4f),
                                                    Random.Range(-2f, 8f),
                                                    Random.Range(-4f, 4f));
            projectileClone.transform.localEulerAngles = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
        }
    }
}
