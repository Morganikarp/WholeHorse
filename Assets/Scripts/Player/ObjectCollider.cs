using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectCollider : MonoBehaviour
{
    List<GameObject> nearestPickupItems;
    List<GameObject> nearestNeighItems;

    GameObject chosenPickupItem;
    GameObject chosenNeighItem;

    PlayerController MyPC;

    void Start()
    {
        nearestPickupItems = new List<GameObject>();
        nearestNeighItems = new List<GameObject>();
        MyPC = transform.parent.transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickupItem")
        {
            nearestPickupItems.Add(other.gameObject);
        }

        if (other.gameObject.tag == "NeighItem")
        {
            nearestNeighItems.Add(other.gameObject);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "MergeArea")
        {
            MyPC.HeadPlayer = other.transform.parent.transform.parent.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PickupItem")
        {
            nearestPickupItems.Remove(other.gameObject);
        }

        if (other.gameObject.tag == "NeighOItem")
        {
            nearestNeighItems.Remove(other.gameObject);
        }

        if (other.gameObject.tag == "MergeArea")
        {
            MyPC.HeadPlayer = null;
        }
    }
    public void HoldObject()
    {
        
        Transform centerChild = null;

        foreach (Transform child in chosenPickupItem.transform)
        {
            if (child.name == "ManualCenter") {
                centerChild = child;
            }
        }

        if (centerChild != null)
        {
            chosenPickupItem.transform.position = transform.position - (Vector3.Scale(chosenPickupItem.transform.localScale, centerChild.localPosition));
        }

        else
        {
            chosenPickupItem.transform.position = transform.position;
        }

    }

    public GameObject ChoosePickUpObject()
    {
        if (nearestPickupItems.Count != 0)
        {
            chosenPickupItem = nearestPickupItems[Random.Range(0, nearestPickupItems.Count)];
            return chosenPickupItem;
        }
        else { return null; }
    }
    public GameObject ChooseNeighObject()
    {
        if (nearestNeighItems.Count != 0)
        {
            chosenNeighItem = nearestNeighItems[Random.Range(0, nearestNeighItems.Count)];
            return chosenNeighItem;
        }
        else { return null; }
    }

    private void OnDisable()
    {
        if (chosenPickupItem != null)
        {
            nearestPickupItems.Clear();
        }
        if (chosenNeighItem != null)
        {
            nearestNeighItems.Clear();
        }
    }
}
