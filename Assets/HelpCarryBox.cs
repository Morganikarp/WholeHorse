using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpCarryBox : MonoBehaviour
{

    public int objectCollidersOnMe;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objectCollidersOnMe > 1)
        {

            GetComponentInParent<Rigidbody>().mass = 500000;

        }

        else
        {

            GetComponentInParent<Rigidbody>().mass = 1000000;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ObjectCollider>())
        {

            objectCollidersOnMe++;


        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponentInParent<ObjectCollider>())
        {
            objectCollidersOnMe--;
        }
    }

}
