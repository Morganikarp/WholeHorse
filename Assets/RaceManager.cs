using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{

    public List<GameObject> playercheckpoints;
    public List<GameObject> enemycheckpoints;
    public int nextPlayerCheckpoint = 0;
    public int nextEnemyCheckpoint = 0;

    public Material opaqueBrown;
    public Material opaqueRed;
    public Material clearBrown;
    public Material clearRed;

    public bool raceStarted = false;

    public List<GameObject> setActiveLater;

    public Image fadeToBlack;
    public float alphaPerSec;
    public float waitBeforeLoading;
    public Sprite goodEndingImage;
    public Sprite badEndingImage;
    public bool ending;
    public float endingTimer;
    public AudioClip goodFanfare;
    public AudioClip badFanfare;
    public AudioClip bothFanfare;

    bool goodEnding;
    bool rdyToBlow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && rdyToBlow)
        {
            FindObjectOfType<PersistentAudiour>().enabled = true;
            Destroy(gameObject);
        }


        if (ending) {

            fadeToBlack.color += new Color(0,0,0,alphaPerSec * Time.deltaTime);
            endingTimer += Time.deltaTime;

            if (endingTimer > waitBeforeLoading)
            {
                endingTimer = -Mathf.Infinity;
                DontDestroyOnLoad(gameObject);
                SceneManager.LoadScene(0);
            }

        }
        
        else { 
            playercheckpoints[nextPlayerCheckpoint].GetComponentInChildren<MeshRenderer>().material = opaqueRed;
            enemycheckpoints[nextEnemyCheckpoint].GetComponentInChildren<MeshRenderer>().material = clearRed;

            if (nextPlayerCheckpoint > 0) { playercheckpoints[nextPlayerCheckpoint - 1].GetComponentInChildren<MeshRenderer>().material = opaqueBrown; }
            if (nextEnemyCheckpoint > 0) { enemycheckpoints[nextEnemyCheckpoint - 1].GetComponentInChildren<MeshRenderer>().material = clearBrown; }

            if (nextPlayerCheckpoint > 11)
            {
                foreach (GameObject activenNOW in setActiveLater)
                {
                    activenNOW.SetActive(true);
                }
            }
        }
    }

    public void ReportCheckpoint(RaceCheckpoint reportedCheckpoint, bool enemyActivated)
    {
        if (reportedCheckpoint.gameObject == playercheckpoints[nextPlayerCheckpoint] && enemyActivated == false)
        {
            nextPlayerCheckpoint++;
            raceStarted = true;
            foreach (WanderingNPC npc in FindObjectsOfType<WanderingNPC>(true)) {
                npc.enabled = true;    
                
            }

            if (nextPlayerCheckpoint == playercheckpoints.Count)
            {
                //good ending?
                SceneManager.sceneLoaded += OnSceneLoaded;
                goodEnding = true;
                FindObjectOfType<PersistentAudiour>().shuttingDown = true;
                ending = true;
                GetComponent<AudioSource>().PlayOneShot(bothFanfare);
            }

            
        }
        

        if (reportedCheckpoint.gameObject == enemycheckpoints[nextEnemyCheckpoint] && enemyActivated == true && raceStarted)
        {
            nextEnemyCheckpoint++;

            if (nextEnemyCheckpoint == enemycheckpoints.Count)
            {
                //bad ending?
                SceneManager.sceneLoaded += OnSceneLoaded;
                goodEnding = false;
                ending = true;
                FindObjectOfType<PersistentAudiour>().shuttingDown = true;
                GetComponent<AudioSource>().PlayOneShot(bothFanfare);

            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        rdyToBlow = true;
        fadeToBlack.color = Color.white;
        FindObjectOfType<PersistentAudiour>().enabled = false;

        if (goodEnding)
        {
            GetComponent<AudioSource>().PlayOneShot(goodFanfare);
            fadeToBlack.sprite = goodEndingImage;

        }

        else
        {
            GetComponent<AudioSource>().PlayOneShot(badFanfare);
            fadeToBlack.sprite = badEndingImage;
        }
        

    }
}
