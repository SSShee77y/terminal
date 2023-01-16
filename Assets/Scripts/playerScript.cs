using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class playerScript : MonoBehaviour {
    public static playerScript instance;
    
    // Audio
    public AudioSource playerDeath;
    public AudioSource playerShoot;

    // Player Movement Variables
    public MeshRenderer playerMR;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotationSpeed = 1f;
    private float rotationVelocity;
    public Rigidbody player;
    
    // Boundaries of Player
    [SerializeField] float xBorder = 50f;
    [SerializeField] float zBorder = 50f;

    // Projectile Variables
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] int projectileCD;
    private int projectileCooldown;
    public Rigidbody projectile;

    // Player Lives
    [SerializeField] int lives = 3;
    private bool death;
    public Rigidbody projectileParticles;

    // Post-Processing Adjustments
    public Volume volume;
    private Vignette vig;
    private DepthOfField dof;
    private ColorAdjustments colAdj;
    private float cVal = 1;
    private LensDistortion lensDis;

    private Color originalColor;
    private int damaged;
    private int selfDestructCount = 150;

    public void Awake() {
        instance = this;
    }

    void Start() {
        Debug.Log("Welcome to the game. Move your player with WASD or arrow keys. Rotate with J and L. GLHF! :)");
        volume.profile.TryGet(out vig);
        volume.profile.TryGet(out dof);
        volume.profile.TryGet(out colAdj);
        volume.profile.TryGet(out lensDis);
        vig.intensity.value = 1;
        vig.smoothness.value = 1;
        lensDis.intensity.value = -1;

        originalColor = GetComponent<MeshRenderer>().material.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hurter")
        {
            setPlayerLives(0);
            Debug.Log("Player has died to a hurtful cube");
        }
    }

    void FixedUpdate() {
        if (death != true && scoreUI.instance.getFinish() != true) {
            if(GetComponent<MeshRenderer>().material.color != originalColor) damaged--;
            if(GetComponent<MeshRenderer>().material.color != originalColor && damaged % 4 == 0) GetComponent<MeshRenderer>().material.color = originalColor;
            else if (damaged != 0 && damaged % 2 == 0) GetComponent<MeshRenderer>().material.color = Color.black;

            if(vig.intensity.value > .44) vig.intensity.value -= 0.0175f;
            if(vig.smoothness.value > .2) vig.smoothness.value -= 0.025f;
            if(lensDis.intensity.value < 0) lensDis.intensity.value += 0.03125f;

            MovePlayer();
            RotatePlayer();
            //MousePointPlayer();

            if (Input.GetKey(KeyCode.Space) && projectileCooldown <= 0) FireProjectile();
            if (projectileCooldown > 0) projectileCooldown--;

            if (Input.GetKey(KeyCode.Backspace)) selfDestructCount--;
            else selfDestructCount = 150;
            if (selfDestructCount <= 0) selfDestruct();
        }
        else if (scoreUI.instance.getFinish()) {
            resetPlayerTransforms();
            onPromptEffect();
            scoreUI.instance.onFinish();
            if (Input.GetKey(KeyCode.F)) nextLevel();
        }
        else if (death == true) {
            resetPlayerTransforms();
            onPromptEffect();
            playerMR.enabled = false;
            if (Input.GetKey(KeyCode.F)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void MovePlayer() {   
        float xValue = Input.GetAxis("Horizontal");
        float zValue = Input.GetAxis("Vertical");

        player.velocity = new Vector3 (xValue * moveSpeed, 0, zValue * moveSpeed);

        if (Mathf.Abs(player.position.x) > xBorder) {
            player.position = new Vector3 (Mathf.Sign(player.position.x) * (xBorder-0.05f), 0.1f, player.position.z);
            //if (player.position.z > 0) player.velocity = new Vector3 (0, 0, -player.velocity.z);
            //else player.velocity = new Vector3 (0, 0, player.velocity.z);
        }

        if (Mathf.Abs(player.position.z) > zBorder) {
            player.position = new Vector3 (player.position.x, 0.1f, Mathf.Sign(player.position.z) * (zBorder-0.05f));
            //if (player.position.x > 0) player.velocity = new Vector3 (-player.velocity.x, 0, 0);
            //else player.velocity = new Vector3 (player.velocity.x, 0, 0);
        }
    }

    public void MousePointPlayer() {    
        //[--- Work in Progress ---]
        Vector2 positionOnScreen = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        float angle = 0;
        if(mouseOnScreen.x < positionOnScreen.x) angle = -Vector2.Angle(positionOnScreen, mouseOnScreen);
        else if (mouseOnScreen.x >= positionOnScreen.x) angle = Vector2.Angle(positionOnScreen, mouseOnScreen);

        transform.rotation = Quaternion.Euler(90f,0f,angle);
        //[--- Work in Progress ---]
    }

    public void RotatePlayer() {
        if (rotationVelocity > 2 || rotationVelocity < -2) {;}
        else if (Input.GetKey(KeyCode.J)) rotationVelocity -= 0.2f;
        else if (Input.GetKey(KeyCode.L)) rotationVelocity += 0.2f;
        
        if (rotationVelocity < 0) rotationVelocity += 0.1f;
        else if (rotationVelocity > 0) rotationVelocity -= 0.1f;

        if (rotationVelocity <= 0.1 && rotationVelocity >= -0.1) rotationVelocity = 0;

        // if (rotationVelocity != 0) Debug.Log(rotationVelocity);

        player.angularVelocity = new Vector3 (0, rotationVelocity * rotationSpeed, 0);
        transform.localEulerAngles = new Vector3(-90, transform.localEulerAngles.y, 0);
    }

    public void FireProjectile() {
        playerShoot.Play();
        Rigidbody projectileClone = (Rigidbody) Instantiate(projectile, player.position + new Vector3(1 * -Mathf.Sin(Mathf.Deg2Rad*transform.localEulerAngles.y), 0, 1 * -Mathf.Cos(Mathf.Deg2Rad*transform.localEulerAngles.y)), player.rotation);
        projectileClone.velocity = new Vector3
            (projectileSpeed * -Mathf.Sin(Mathf.Deg2Rad*transform.localEulerAngles.y), 0, projectileSpeed * -Mathf.Cos(Mathf.Deg2Rad*transform.localEulerAngles.y));
        
        projectileCooldown = projectileCD;

        /*Debug.Log(string.Format("{0}: [{1} | {2}]",
            player.rotation.eulerAngles.y, Mathf.Sin(Mathf.Deg2Rad*player.rotation.eulerAngles.y), Mathf.Cos(Mathf.Deg2Rad*player.rotation.eulerAngles.y)));*/
    }

    public void nextLevel() {
        int nextScene = SceneManager.GetActiveScene().buildIndex;
        
        
        if (SceneManager.GetActiveScene().buildIndex >= 6 && SceneManager.GetActiveScene().buildIndex <= 13) {
            while(SceneManager.GetActiveScene().buildIndex == nextScene) nextScene = (int)Random.Range(6, 13);
            SceneManager.LoadScene(nextScene);
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 14 && SceneManager.GetActiveScene().buildIndex <= 21) {
            while(SceneManager.GetActiveScene().buildIndex == nextScene) nextScene = (int)Random.Range(14, 21);
            SceneManager.LoadScene(nextScene);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5) SceneManager.LoadScene(0);
        else if (scoreUI.instance.getFinish()) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void changePlayerLives(int i)
    {
        if (damaged <= 0) {
            lives += i;
            GetComponent<MeshRenderer>().material.color = Color.black;
            damaged = 15;
            checkLives();
        }
        
    }

    public void setPlayerLives(int i)
    {
        lives = i;
        checkLives();
    }

    public int getPlayerLives()
    {
        return lives;
    }

    public void checkLives()
    {
        if (lives == 0) {
            death = true;
            scoreUI.instance.playerDied = true;
            createParticles();
            playerDeath.Play();
        }
    }

    public void selfDestruct() {
        death = true;
        scoreUI.instance.selfDestruct = true;
        createParticles();
        playerDeath.Play();
    }

    public bool checkIfDead()
    {
        return death;
    }

    public void resetPlayerTransforms()
    {
        player.velocity = new Vector3 (0, 0, 0);
        player.angularVelocity = new Vector3 (0, 0, 0);
    }

    public void onPromptEffect()
    {
        dof.active = true;
        if (lensDis.intensity.value > -0.35) lensDis.intensity.value -= 0.025f;
        if (cVal > .35) cVal -= 0.025f;
        colAdj.colorFilter.value = new Color(cVal, cVal, cVal);
    }

    public void createParticles() {
        for(int i = 0; i<30; i++) {
            Rigidbody projectileClone = (Rigidbody) Instantiate(projectileParticles, transform.position, transform.rotation);
            projectileClone.constraints = 0;
            projectileClone.GetComponent<particle>().randomLifetime(6f);
            projectileClone.velocity = new Vector3  (Random.Range(-8f, 8f),
                                                    Random.Range(-2f, 14f),
                                                    Random.Range(-8f, 8f));
            projectileClone.transform.localEulerAngles = new Vector3(Random.Range(-180f, 180f), Random.Range(-180f, 180f), Random.Range(-180f, 180f));
        }
    }

    public int getSelfDestructCount() {
        return selfDestructCount;
    }
}
