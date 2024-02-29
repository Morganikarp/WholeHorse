using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class wetsignhazard : MonoBehaviour
{
    public GameObject wetSign;
    public bool readyForSlip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!Physics.OverlapBox(transform.position, GetComponent<BoxCollider>().size / 2f).Any(col => col.gameObject == wetSign))
        {
            readyForSlip = true;
        }
    }

    
}
