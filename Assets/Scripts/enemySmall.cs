using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySmall : MonoBehaviour
{
    // This object
    public Rigidbody enemyObject;
    [SerializeField] float enemyObjectSpeed = 2f;
    [SerializeField] float enemyRotationSpeed = 2f;
    private float enemyRotationVelocity;
    
    public Rigidbody player;

    // Projectile Variables
    public Rigidbody projectileDefault;
    public Rigidbody projectileImmune;
    [SerializeField] int immuneIter = 2;
    private int iterCountdown;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] int projectileCD = 100;
    [SerializeField] bool passive;
    private int projectileCooldown = 30;
    private float angle;

    void FixedUpdate()
    {   
        // Find angle between player and current enemy
        angle = Mathf.Atan2(gameObject.transform.position.x - player.transform.position.x, gameObject.transform.position.z - player.transform.position.z);

        enemyObject.velocity = new Vector3 (enemyObjectSpeed * Mathf.Sin(Mathf.Deg2Rad*transform.localEulerAngles.y), 0, enemyObjectSpeed * Mathf.Cos(Mathf.Deg2Rad*transform.localEulerAngles.y));

        rotateEnemy();

        if (projectileCooldown <= 0 && passive == false) FireProjectile();
        if (projectileCooldown > 0) projectileCooldown--;
    }

    public void FireProjectile() {
        Rigidbody projectileClone;
        if (iterCountdown > 0) {
            projectileClone = (Rigidbody) Instantiate(projectileDefault, gameObject.transform.position, gameObject.transform.rotation);
        }
        else {
            projectileClone = (Rigidbody) Instantiate(projectileImmune, gameObject.transform.position, gameObject.transform.rotation);
            iterCountdown = immuneIter;
        }

        projectileClone.velocity = new Vector3 (projectileSpeed * Mathf.Sin(Mathf.Deg2Rad*transform.localEulerAngles.y), 0, projectileSpeed * Mathf.Cos(Mathf.Deg2Rad*transform.localEulerAngles.y));
        
        projectileCooldown = projectileCD;
        iterCountdown--;

        // Debug.Log(string.Format("{0}, {1}, {2}", player.transform.position.x, player.transform.position.y, player.transform.position.z));
    }

    public void rotateEnemy() {
        float currentAngle = transform.localEulerAngles.y-180;
        int sign = 1;
        if (currentAngle == angle*Mathf.Rad2Deg) enemyRotationVelocity = 0;
        else {
            if (Mathf.Abs(angle*Mathf.Rad2Deg-currentAngle) > 180) sign = -1;
            if (currentAngle <= angle*Mathf.Rad2Deg) enemyRotationVelocity = 0.2f * sign;
            else if (currentAngle >= angle*Mathf.Rad2Deg) enemyRotationVelocity = -0.2f * sign;
        }

        enemyObject.angularVelocity = new Vector3 (0, enemyRotationVelocity * enemyRotationSpeed, 0);
    }
}
