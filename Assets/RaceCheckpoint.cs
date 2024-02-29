using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCheckpoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>())
        {
            Debug.Log("seaioufrthjseriogjdrs");
            FindObjectOfType<RaceManager>().ReportCheckpoint(this, false);
        }

        if (other.GetComponentInParent<WanderingNPC>())
        {
            FindObjectOfType<RaceManager>().ReportCheckpoint(this, true);
        }
    }
}
