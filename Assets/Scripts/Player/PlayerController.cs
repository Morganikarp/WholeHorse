using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool Player1;
    public string forward;
    public string backward;
    public string right;
    public string left;
    public string grab;
    public string neigh;

    public ObjectCollider costrumeObjectCollider;
    public ObjectCollider horseObjectCollider;

    float turnSpeed;

    GameObject costumeObject;
    Rigidbody costumeRB;
    Transform costumeTrans;
    GameObject attachArea;
    GameObject costumeGrab;

    public GameObject horseObject;
    Rigidbody horseRB;
    Transform horseTrans;

    int moveSpeed;
    public GameObject nearestPickupItem;
    GameObject nearestNeighItem;

    public bool AmHeadPlayer;
    public GameObject HeadPlayer;
    PlayerController HeadPlayerCon;

    public AudioClip[] NeighArray;
    AudioSource horseNeighSource;

    public bool grabToggle; 
    public bool currentlyGrabbingIfToggle;
    public GameObject frontCostumeBody;
    public GameObject backCostumeBody;
    public GameObject noCostumeBody;
    public bool horselessMode;

    public List<Animator> costumeAnimators;
    public List<Animator> horseAnimators;

    public float forceHeadDetachmentDistance;

    bool walkingThisFrame;
    bool otherWalkingThisFrame;

    string horselessCheck;
    string headPlayerCheck;

    void Start()
    {

        FileStream fileStream = new FileStream(Application.dataPath + "/Resources/SaveFiles/SaveFile1.txt", FileMode.Open, FileAccess.ReadWrite);
        StreamReader streamReader = new StreamReader(fileStream);

        streamReader.ReadLine();
        horselessCheck = streamReader.ReadLine();
        streamReader.ReadLine();
        headPlayerCheck = streamReader.ReadLine();

        streamReader.Close();
        fileStream.Close();

        if ( horselessCheck == "1" && headPlayerCheck == "0") 
        {
            horselessMode = true;
        }
        else
        {
            horselessMode = false;
        }

        if (headPlayerCheck == "1" && Player1)
        {
            AmHeadPlayer = true;
        }

        else if (headPlayerCheck == "2" && !Player1)
        {
            AmHeadPlayer = true;
        }

        else
        {
            AmHeadPlayer = false;
        }

        costumeObject = transform.GetChild(0).gameObject;
        costumeRB = costumeObject.GetComponent<Rigidbody>();
        costumeTrans = costumeObject.GetComponent<Transform>();

        for (int i = 0; i < costumeTrans.childCount; i++)
        {
            if (costumeTrans.GetChild(i).gameObject.tag == "MergeArea")
            {
                attachArea = costumeTrans.GetChild(i).gameObject;
            }
            else
            {
                costumeGrab = costumeTrans.GetChild(i).gameObject;
                //costrumeObjectCollider = costumeGrab.GetComponent<ObjectCollider>();
            }
        }

        attachArea.SetActive(AmHeadPlayer ? true : false);

        HeadPlayer = null;

        horseObject = transform.GetChild(1).gameObject;
        horseRB = horseObject.GetComponent<Rigidbody>();
        horseTrans = horseObject.GetComponent<Transform>();
        //horseObjectCollider = horseTrans.GetChild(0).GetComponent<ObjectCollider>();
        horseNeighSource = horseTrans.GetComponent<AudioSource>();

        horseObject.SetActive(false);

        moveSpeed = 5;
        turnSpeed = 10f;

        SetCostumeModel();



    }

    public void SetCostumeModel()
    {
        if (horselessMode) { noCostumeBody.SetActive(true); frontCostumeBody.SetActive(false); backCostumeBody.SetActive(false); } 
        else if (AmHeadPlayer) { noCostumeBody.SetActive(false); frontCostumeBody.SetActive(true); backCostumeBody.SetActive(false); } 
        else { noCostumeBody.SetActive(false); frontCostumeBody.SetActive(false); backCostumeBody.SetActive(true); }
    }

    void Update()
    {
        if (HeadPlayer != null) { 
            if (Vector3.Distance(transform.position, HeadPlayer.transform.position) > forceHeadDetachmentDistance) 
            {
                HeadPlayer = null; 
            }
        }

        walkingThisFrame = false;
        otherWalkingThisFrame = false;

        attachArea.SetActive(AmHeadPlayer ? true : false);
        costumeMovement();
        if (!horselessMode && !FindObjectsOfType<PlayerController>(true).First(pc => pc != this).horselessMode) { costumeGrabbing(); }

        /*if (Physics.OverlapSphere(transform.position, 5).Any(col => col.GetComponent<Rigidbody>()))
        {
            if (Physics.OverlapSphere(transform.position, 5).Where(col => col.GetComponent<Rigidbody>()).Any(col => col.GetComponent<Rigidbody>().mass > 200000))
            {
                transform.position = (transform.position - Physics.OverlapSphere(transform.position, 5).Where(col => col.GetComponent<Rigidbody>()).First(col => col.GetComponent<Rigidbody>().mass > 200000).transform.position).normalized * 5;
            }
        }*/

        moveSpeed = 5;
        costrumeObjectCollider.transform.localPosition = new Vector3(1.25f, 0, 0);
        horseObjectCollider.transform.localPosition = new Vector3(2.3f, 0, 0);

        if (Physics.OverlapBox(costrumeObjectCollider.transform.position, .5f * Vector3.one).Where(col => col.GetComponent<Rigidbody>()).Any(col => col.GetComponent<Rigidbody>().mass >= 200000))
        {
            moveSpeed = 2;
            turnSpeed = 6f;
            costrumeObjectCollider.transform.localPosition = new Vector3(2.5f, 0, 0);
            horseObjectCollider.transform.localPosition = new Vector3(2.3f+1.25f, 0, 0);
        }

        if (Physics.OverlapBox(costrumeObjectCollider.transform.position, .5f * Vector3.one).Where(col => col.GetComponent<Rigidbody>()).Any(col => col.GetComponent<Rigidbody>().mass >= 800000))
        {
            moveSpeed = 0;
            turnSpeed = 2f;

        }

        UpdateChildAnimators(walkingThisFrame, otherWalkingThisFrame);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void costumeMovement()
    {
        bool MovingUp = false;
        bool MovingRight = false;

        if (Input.GetKey(forward))
        {
            MovingUp = true;
            costumeRB.velocity = new Vector3(costumeRB.velocity.x, costumeRB.velocity.y, moveSpeed);
            costumeTrans.rotation = Quaternion.Euler(0, Mathf.LerpAngle(costumeTrans.eulerAngles.y, 270f, Time.deltaTime * turnSpeed), 0);

            walkingThisFrame = true;
        }

        if (Input.GetKey(backward))
        {
            if (MovingUp)
            {
                costumeRB.velocity = new Vector3(costumeRB.velocity.x, costumeRB.velocity.y, 0f);
            }

            else
            {
                costumeRB.velocity = new Vector3(costumeRB.velocity.x, costumeRB.velocity.y, -moveSpeed);
            }
            costumeTrans.rotation = Quaternion.Euler(0, Mathf.LerpAngle(costumeTrans.eulerAngles.y, 90f, Time.deltaTime * turnSpeed), 0);

            walkingThisFrame = true;
        }

        if (Input.GetKey(right))
        {
            MovingRight = true;
            costumeRB.velocity = new Vector3(moveSpeed, costumeRB.velocity.y, costumeRB.velocity.z);
            costumeTrans.rotation = Quaternion.Euler(0, Mathf.LerpAngle(costumeTrans.eulerAngles.y, 0f, Time.deltaTime * turnSpeed), 0);

            walkingThisFrame = true;
        }

        if (Input.GetKey(left))
        {
            if (MovingRight)
            {
                costumeRB.velocity = new Vector3(0f, costumeRB.velocity.y, costumeRB.velocity.z);
            }

            else
            {
                costumeRB.velocity = new Vector3(-moveSpeed, costumeRB.velocity.y, costumeRB.velocity.z);
            }
            costumeTrans.rotation = Quaternion.Euler(0, Mathf.LerpAngle(costumeTrans.eulerAngles.y, 180f, Time.deltaTime * turnSpeed), 0);

            walkingThisFrame = true;
        }
    }

    void costumeGrabbing()
    {
        if (Input.GetKeyDown(grab))
        {
            if (HeadPlayer != null && nearestPickupItem == null)
            {
                Merge(true);
            }

            else
            {
                nearestPickupItem = costrumeObjectCollider.ChoosePickUpObject();

                if (nearestPickupItem != null)
                {
                    if (nearestPickupItem.GetComponent<Rigidbody>())
                    {
                        nearestPickupItem.GetComponent<Rigidbody>().isKinematic = true;

                        //if (nearestPickupItem.GetComponent<Rigidbody>().mass != 999999)
                        //{
                            nearestPickupItem.layer = 2;
                        //}

                       
                    }
                }

            }
        }

        else if (Input.GetKeyUp(grab) && !grabToggle)
        {
            if (nearestPickupItem != null)
            {
                if (nearestPickupItem.GetComponent<Rigidbody>())
                {
                    nearestPickupItem.GetComponent<Rigidbody>().isKinematic = false;

                    //if (nearestPickupItem.GetComponent<Rigidbody>().mass != 999999)
                    //{
                    nearestPickupItem.layer = 0;
                    //}


                }
            }

            if (nearestPickupItem == null && HeadPlayer != null)
            {
                Merge(false);
            } 
            
            else if (nearestPickupItem != null)
            {

                Rigidbody nearestItemRB = nearestPickupItem.GetComponent<Rigidbody>();
                nearestItemRB.velocity = Vector3.zero;

            }
        }

        if (Input.GetKey(grab) && HeadPlayer != null)
        {
            HeadPlayerCon = HeadPlayer.GetComponent<PlayerController>();
            horseMovement();
            horseGrabbing();
            horseNeighing();
        }

        else if (Input.GetKey(grab) && nearestPickupItem != null)
        {
            costrumeObjectCollider.HoldObject();
        }

        
    }

    void Merge(bool Start)
    {
        switch (Start)
        {
            case true:

                HeadPlayerCon = HeadPlayer.GetComponent<PlayerController>();
                HeadPlayer.SetActive(false);
                horseObject.SetActive(true);
                horseTrans.position = costumeTrans.position;
                horseTrans.rotation = costumeTrans.rotation;
                costumeObject.SetActive(false);
                break;

            case false:
                HeadPlayer.SetActive(true);

                costumeTrans.position = horseTrans.position;
                HeadPlayer.transform.position = horseTrans.position;
                costumeTrans.rotation = horseTrans.rotation;
                HeadPlayer.transform.rotation = costumeTrans.rotation;
                
                
                Rigidbody HeadPlayerRB = transform.GetChild(0).GetComponent<Rigidbody>();
                HeadPlayerRB.velocity = Vector3.zero;
                costumeRB.velocity = Vector3.zero;

                horseObject.SetActive(false);
                costumeObject.SetActive(true);
            break;
        }
    }

    void horseMovement()
    {

        bool MovingUp = false;
        bool MovingRight = false;

        if (Input.GetKey(forward) || Input.GetKey(HeadPlayerCon.forward))
        {
            horseRB.velocity = new Vector3(horseRB.velocity.x, horseRB.velocity.y, moveSpeed * 2f);
            horseTrans.rotation = Quaternion.Euler(0, Mathf.LerpAngle(horseTrans.eulerAngles.y, 270f, Time.deltaTime * turnSpeed), 0);
            MovingUp = true;

        }

        if (Input.GetKey(backward) || Input.GetKey(HeadPlayerCon.backward))
        {
            if (MovingUp)
            {
                horseRB.velocity = new Vector3(horseRB.velocity.x, horseRB.velocity.y, 0f);
            }
            else
            {
                horseRB.velocity = new Vector3(horseRB.velocity.x, horseRB.velocity.y, -moveSpeed * 2f);
            }
            horseTrans.rotation = Quaternion.Euler(0, Mathf.LerpAngle(horseTrans.eulerAngles.y, 90f, Time.deltaTime * turnSpeed), 0);

        }

        if (Input.GetKey(right) || Input.GetKey(HeadPlayerCon.right))
        {
            horseRB.velocity = new Vector3(moveSpeed * 2f, horseRB.velocity.y, horseRB.velocity.z);
            horseTrans.rotation = Quaternion.Euler(0, Mathf.LerpAngle(horseTrans.eulerAngles.y, 0f, Time.deltaTime * turnSpeed), 0);
            MovingRight = true;

        }

        if (Input.GetKey(left) || Input.GetKey(HeadPlayerCon.left))
        {
            if (MovingRight)
            {
                horseRB.velocity = new Vector3(0f, horseRB.velocity.y, horseRB.velocity.z);
            }
            else
            {
                horseRB.velocity = new Vector3(-moveSpeed * 2f, horseRB.velocity.y, horseRB.velocity.z);
            }
            horseTrans.rotation = Quaternion.Euler(0, Mathf.LerpAngle(horseTrans.eulerAngles.y, 180f, Time.deltaTime * turnSpeed), 0);

        }

        if (Input.GetKey(forward) || Input.GetKey(backward) || Input.GetKey(right) || Input.GetKey(left)) { walkingThisFrame = true; }
        if (Input.GetKey(HeadPlayerCon.forward) || Input.GetKey(HeadPlayerCon.backward) || Input.GetKey(HeadPlayerCon.right) || Input.GetKey(HeadPlayerCon.left)) { otherWalkingThisFrame = true; }

    }

    void horseGrabbing()
    {
        if (Input.GetKeyDown(HeadPlayerCon.grab) && (!grabToggle || nearestPickupItem))
        {
            nearestPickupItem = horseObjectCollider.ChoosePickUpObject();

            if (nearestPickupItem != null)
            {
                if (nearestPickupItem.GetComponent<Rigidbody>())
                {
                    nearestPickupItem.GetComponent<Rigidbody>().isKinematic = true;

                    //if (nearestPickupItem.GetComponent<Rigidbody>().mass != 999999)
                    //{
                        nearestPickupItem.layer = 2;
                    //}


                }
            }

        }

        if (Input.GetKey(HeadPlayerCon.grab) && nearestPickupItem != null)
        {
            horseObjectCollider.HoldObject();
        }

        if ((Input.GetKeyUp(HeadPlayerCon.grab) && nearestPickupItem != null && !grabToggle) || (nearestPickupItem != null && Input.GetKeyDown(grab) && grabToggle))
        {

            if (nearestPickupItem.GetComponent<Rigidbody>())
            {
                nearestPickupItem.GetComponent<Rigidbody>().isKinematic = false;

                //if (nearestPickupItem.GetComponent<Rigidbody>().mass != 999999)
                //{
                    nearestPickupItem.layer = 0;
                //  }


            }

            Rigidbody nearestItemRB = nearestPickupItem.GetComponent<Rigidbody>();
            nearestItemRB.velocity = Vector3.zero;

            
        }
    }

    void horseNeighing()
    {
        if (Input.GetKeyDown(HeadPlayerCon.neigh))
        {
            horseNeighSource.clip = NeighArray[Random.Range(0, NeighArray.Length)];
            horseNeighSource.Play();

            nearestNeighItem = horseObjectCollider.ChooseNeighObject();

            if (nearestNeighItem != null)
            {
                NeighReceiver ItemNR = nearestNeighItem.GetComponent<NeighReceiver>();
                ItemNR.signal = true;
            }
        }
    }

    public void UpdateChildAnimators(bool walking, bool otherWalking)
    {

        foreach (Animator childAnimator in costumeAnimators) { 
            
            childAnimator.SetBool("Walking", walking);
            childAnimator.SetBool("Grabbing", Input.GetKey(grab));

        }

        if (AmHeadPlayer)
        {

            horseAnimators[0].SetBool("Grabbing", Input.GetKey(grab));
            horseAnimators[0].SetBool("FrontWalking", otherWalking);
            horseAnimators[0].SetBool("BackWalking", walking);

        }

        else if (HeadPlayerCon != null)
        {

            horseAnimators[0].SetBool("Grabbing", Input.GetKey(HeadPlayerCon.grab));
            horseAnimators[0].SetBool("BackWalking", walking);
            horseAnimators[0].SetBool("FrontWalking", otherWalking);

        }

    }


}
