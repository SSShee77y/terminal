using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossScript : MonoBehaviour
{
    // This object
    public Rigidbody enemyObject;
    [SerializeField] float enemyObjectSpeed = 2f;
    
    public Rigidbody player;

    // Projectile Variables
    private enum projectileFaces {Single, Binary, Tri, Quad, Penta, None};
    [SerializeField] projectileFaces faceSelection = projectileFaces.Single;
    private int numberOfShots;
    [SerializeField] bool idleSpin;
    [SerializeField] float spinSpeed;
    
    public Rigidbody projectileDefault;
    public Rigidbody projectileImmune;
    [SerializeField] int immuneIter = 2;
    private int iterCountdown;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] int projectileCD = 100;
    private int projectileCooldown = 30;
    private float angle;

    void FixedUpdate()
    {   
        if (faceSelection != projectileFaces.None) {
            // Find angle between player and current enemy
            if (idleSpin != true) angle = Mathf.Atan2(gameObject.transform.position.x - player.transform.position.x, gameObject.transform.position.z - player.transform.position.z);
            else angle += spinSpeed;

            if (idleSpin != true) enemyObject.velocity = new Vector3 (enemyObjectSpeed * -Mathf.Sin(angle), 0, enemyObjectSpeed * -Mathf.Cos(angle));

            if (projectileCooldown <= 0) FireProjectile();
            if (projectileCooldown > 0) projectileCooldown--;

        }
    }

    public void FireProjectile() {
        float tempAngle = angle;
        
        if (faceSelection == projectileFaces.Single) numberOfShots = 1;
        else if (faceSelection == projectileFaces.Binary) numberOfShots = 2;
        else if (faceSelection == projectileFaces.Tri) {numberOfShots = 3; tempAngle -= Mathf.Deg2Rad*40f;}
        else if (faceSelection == projectileFaces.Quad) numberOfShots = 4;
        else if (faceSelection == projectileFaces.Penta) {numberOfShots = 5; tempAngle -= Mathf.Deg2Rad*90f;}
        
        for(int i = 0; i < numberOfShots; i++) {
            Rigidbody projectileClone;
            if (iterCountdown > 0) {
                projectileClone = (Rigidbody) Instantiate(projectileDefault, gameObject.transform.position, gameObject.transform.rotation);
            }
            else {
                projectileClone = (Rigidbody) Instantiate(projectileImmune, gameObject.transform.position, gameObject.transform.rotation);
                iterCountdown = immuneIter;
            }
            iterCountdown--;

            projectileClone.velocity = new Vector3 (projectileSpeed * -Mathf.Sin(tempAngle), 0, projectileSpeed * -Mathf.Cos(tempAngle));
            if (faceSelection == projectileFaces.Binary) tempAngle += Mathf.Deg2Rad*180f;
            else if (faceSelection == projectileFaces.Tri) tempAngle += Mathf.Deg2Rad*40f;
            else if (faceSelection == projectileFaces.Quad) tempAngle += Mathf.Deg2Rad*90f;
            else if (faceSelection == projectileFaces.Penta) tempAngle += Mathf.Deg2Rad*45f;
        }
        
        projectileCooldown = projectileCD;
        
    }

    public void setFaces(int i) {
        if (i == 1) faceSelection = projectileFaces.Single;
        else if (i == 2) faceSelection = projectileFaces.Binary;
        else if (i == 3) faceSelection = projectileFaces.Tri;
        else if (i == 4) faceSelection = projectileFaces.Quad;
        else if (i == 5) faceSelection = projectileFaces.Penta;
        
        else if (i == 0) faceSelection = projectileFaces.None;
    }
}
