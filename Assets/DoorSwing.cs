using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DoorSwing : MonoBehaviour
{

    public Vector3 closeRotation;
    public Vector3 openRotation;
    public float rotationPerSec;
    public float targetRotationMargin;
    public bool closed;
    public float playerDetectionRadius;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool closedthisframe = false;

        if (!Physics.OverlapSphere(transform.position, playerDetectionRadius).Any(col => col.gameObject.name == "Crate"))
        {
            if (Physics.OverlapSphere(transform.position, playerDetectionRadius).Any(col => col.GetComponentInParent<PlayerController>()))
            {
                closed = false;
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(openRotation), rotationPerSec * Time.deltaTime);
                closedthisframe = true;
            /*if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(openRotation)) < targetRotationMargin) 
            { 
                closed = false;
            }*/

            } 
        }

        if (!closedthisframe)
        {
            closed = true;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(closeRotation), rotationPerSec * Time.deltaTime);

            /*if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(closeRotation)) < targetRotationMargin)
            {
                closed = true;
            }*/

        }

    }
}
