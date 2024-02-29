using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BasePlayer : MonoBehaviour
{

    public float moveSpeed_SCALARSTYLE = 1;
    public Transform movementIndex;
    public float rotationSpeed_LERPSTYLE;
    public bool inputsLocked;
    private Quaternion lookingRotation;


    // Update is called once per frame
    public virtual void Update()
    {

        DoPlayerMovement();

    }



    public virtual void DoPlayerMovement()
    {
        //Start Movement Section
        if (!inputsLocked)
        {
            Vector3 movementDirection = new Vector3();

            if (Input.GetKey("w"))
            {
                GetComponent<CharacterController>().Move(movementIndex.forward * moveSpeed_SCALARSTYLE * Time.deltaTime);
                movementDirection += new Vector3(0.5f, 0, -0.5f);
            }

            if (Input.GetKey("s"))
            {
                GetComponent<CharacterController>().Move(-movementIndex.forward * moveSpeed_SCALARSTYLE * Time.deltaTime);
                movementDirection += new Vector3(-0.5f, 0, 0.5f);
            }

            if (Input.GetKey("a"))
            {
                GetComponent<CharacterController>().Move(-movementIndex.right * moveSpeed_SCALARSTYLE * Time.deltaTime);
                movementDirection += new Vector3(0.5f, 0, 0.5f);
            }

            if (Input.GetKey("d"))
            {
                GetComponent<CharacterController>().Move(movementIndex.right * moveSpeed_SCALARSTYLE * Time.deltaTime);
                movementDirection += new Vector3(-0.5f, 0, -0.5f);
            }


            if (movementDirection.magnitude != 0)
            {
                lookingRotation = Quaternion.LookRotation(movementDirection.normalized);
            } 
            
            else {

                Vector3 targetEuler = transform.rotation.eulerAngles;
                Vector3 roundedEuler = new Vector3((Mathf.RoundToInt(targetEuler.x / 45f)) * 45f, (Mathf.RoundToInt(targetEuler.y / 45f)) * 45f, (Mathf.RoundToInt(targetEuler.z / 45f)) * 45f);
                //Debug.Log(targetEuler + " " + roundedEuler);
                lookingRotation = Quaternion.Euler(roundedEuler);

            }

            transform.rotation = Quaternion.Slerp(transform.rotation, lookingRotation, rotationSpeed_LERPSTYLE * Time.deltaTime);

        }


        //End Movement Section
    }


  
    
}
