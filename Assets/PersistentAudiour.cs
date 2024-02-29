using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAudiour : MonoBehaviour
{
    //public static PersistentAudiour instance;
    public AudioClip sceneClip;
    public float volumeDecrease_PERSEC;

    public bool shuttingDown;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        GetComponent<AudioSource>().clip = sceneClip;
        GetComponent<AudioSource>().Play();

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        shuttingDown = true;

    }

    private void Update()
    {
        if (shuttingDown)
        {
            GetComponent<AudioSource>().volume -= volumeDecrease_PERSEC * Time.deltaTime;
            if (GetComponent<AudioSource>().volume <= 0)
            {
                Destroy(gameObject);
            }

        }
    }
}
