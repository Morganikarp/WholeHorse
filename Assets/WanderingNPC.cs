using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingNPC : MonoBehaviour
{

    public GameObject actualNPC;
    public Vector3 currentDestination = Vector3.zero;
    public float destinationMargin;
    public float walkSpeed_LERPSTYLE;
    public float additionalWalkSpeed;
    public float lookSpeed_LERPSTYLE;
    public Vector2 animationDistanceSpeeds;
    public Vector2 animationSpeeds;

    public Vector2 stillnessCD;
    float internalStillnessCD;

    public bool horseTrauma;
    public bool horseNPC;
    public wetsignhazard slippinJimmySign;
    public Vector3 slippinJimmyDestination;
    float slippinJimmyRotation;
    public bool slippinJimmySlipped;

    public GameObject ObjectiveList;
    ObjectiveListController ObjectiveListCon;

    float usingWalkSpeed;

    // Start is called before the first frame update
    void Start()
    {
        currentDestination = actualNPC.transform.position;

        if (slippinJimmySign)
        {
            slippinJimmyDestination = slippinJimmySign.transform.position;
            ObjectiveListCon = ObjectiveList.GetComponent<ObjectiveListController>();
        }

        usingWalkSpeed = walkSpeed_LERPSTYLE;
    }

    // Update is called once per frame
    void Update()
    {





        if (slippinJimmySign)
        {

            //transform.eulerAngles += new Vector3(Random.Range(0, slippinJimmyRotation), Random.Range(0, slippinJimmyRotation), Random.Range(0, slippinJimmyRotation) * Time.deltaTime);
            GetComponentInChildren<Rigidbody>().velocity += new Vector3(0, slippinJimmyRotation * Time.deltaTime, 0);

            if (slippinJimmySign.readyForSlip)
            {
                currentDestination = slippinJimmyDestination;
            }
        }


        if (internalStillnessCD > 0)
        {
            internalStillnessCD = Mathf.Clamp(internalStillnessCD - Time.deltaTime, 0, Mathf.Infinity);
        }


        if (internalStillnessCD == 0)
        {

            Vector3 lastPosition = actualNPC.transform.position;

            actualNPC.transform.position += (currentDestination - actualNPC.transform.position).normalized * usingWalkSpeed * Time.deltaTime; //= Vector3.Lerp(actualNPC.transform.position, currentDestination, walkSpeed_LERPSTYLE * Time.deltaTime);
            actualNPC.transform.forward = Vector3.Slerp(actualNPC.transform.forward, (actualNPC.transform.position - currentDestination).normalized, lookSpeed_LERPSTYLE * Time.deltaTime);

            if (!horseNPC) { GetComponentInChildren<Animator>().SetBool("Walking", true); } else
            {
                GetComponentInChildren<Animator>().SetBool("FrontWalking", true);
                GetComponentInChildren<Animator>().SetBool("BackWalking", true);
            }


            if ((actualNPC.transform.position - currentDestination).magnitude < destinationMargin)
            {

                if (currentDestination == slippinJimmyDestination && slippinJimmySign != null)
                {
                    slippinJimmyRotation = 4.2f;
                    GetComponentInChildren<Rigidbody>().velocity += new Vector3(0, Time.deltaTime, 0);
                    GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.None;
                    GetComponentInChildren<Rigidbody>().useGravity = false;
                    GetComponentInChildren<Rigidbody>().freezeRotation = false;
                    GetComponentInChildren<Animator>().SetBool("Walking", false);
                    internalStillnessCD = Mathf.Infinity;
                    slippinJimmySlipped = true;
                }



                if (!horseNPC) { GetComponentInChildren<Animator>().SetBool("Walking", false); } else
                {
                    GetComponentInChildren<Animator>().SetBool("FrontWalking", false);
                    GetComponentInChildren<Animator>().SetBool("BackWalking", false);
                }

                internalStillnessCD = Random.Range(stillnessCD.x, stillnessCD.y);

                if (!FindObjectOfType<RaceManager>())
                {
                    Vector2 circleSample = Random.insideUnitCircle;
                    currentDestination = (new Vector3(circleSample.x, 0, circleSample.y) * GetComponent<SphereCollider>().radius) + transform.position;
                } else
                {
                    currentDestination = FindObjectOfType<RaceManager>().enemycheckpoints[FindObjectOfType<RaceManager>().nextEnemyCheckpoint].transform.position;
                    usingWalkSpeed = Random.Range(walkSpeed_LERPSTYLE, additionalWalkSpeed);
                }

            }

            GetComponentInChildren<Animator>().speed = Mathf.Lerp(animationSpeeds.x, animationSpeeds.y, Mathf.InverseLerp(animationDistanceSpeeds.x, animationDistanceSpeeds.y, (transform.position - lastPosition).magnitude));

        }





    }

    private void OnTriggerEnter(Collider other)
    {
        if (horseTrauma && other.GetComponentInParent<PlayerController>())
        {
            GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.None;
            GetComponentInChildren<Rigidbody>().freezeRotation = false;
            GetComponentInChildren<Animator>().SetBool("Walking", false);
            enabled = false;
        }
    }


}
