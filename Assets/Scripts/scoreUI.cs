using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreUI : MonoBehaviour
{
    public static scoreUI instance;
    
    public Text timerText;
    // public Text scoreText;
    public Text informationText;
    public Text interactionText;

    public float timer;
    private float secs;
    private int mins, hits;
    private bool hasFinished;
    public bool playerDied;
    public bool selfDestruct;

    public void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (hasFinished == false)
        {
            timer = Mathf.Round(Time.timeSinceLevelLoad * 100f) / 100f;
            mins = (int)(timer / 60);
            secs = timer % 60;

            if (playerDied != true && selfDestruct != true) timerText.text = string.Format("{0}:{1:00.00}", mins, secs);
            // scoreText.text = string.Format("Score: {0}", hits);

            if (playerDied == true) {
                informationText.text = string.Format("Y O U   W E R E   K I L L E D");
                interactionText.text = string.Format("P R E S S   F   T O   R E S T A R T");
            }
            if (playerScript.instance.getSelfDestructCount() < 150 && playerScript.instance.getSelfDestructCount() != 0) {
                informationText.text = string.Format("{0}", ((float)playerScript.instance.getSelfDestructCount()/50));
            }

            if (playerScript.instance.getSelfDestructCount() >= 150 || playerScript.instance.getSelfDestructCount() <= 0) {
                informationText.text = string.Format("");
            }

            if (selfDestruct == true) {
                informationText.text = string.Format("S E L F   D E S T R U C T E D");
                interactionText.text = string.Format("P R E S S   F   T O   R E S T A R T");
            }
        }
        else if (hasFinished == true) {
                informationText.text = string.Format("L E V E L   C O M P L E T E");
                interactionText.text = string.Format("P R E S S   F   T O   C O N T I N U E");
            }
    }

    public void addHits()
    {
        hits++;
    }

    public int getHits()
    {
        return hits;
    }

    public void onFinish()
    {
        hasFinished = true;
    }

    public bool getFinish()
    {
        return hasFinished;
    }

    public void resetInformationText()
    {
        informationText.text = "";
    }

    public void resetInteractionText()
    {
        interactionText.text = "";
    }
}
