using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveController : MonoBehaviour
{
    public GameObject RequiredObject;
    public GameObject ObjectiveList;
    public int ObjectiveNum;
    ObjectiveListController ObjectiveListCon;

    void Start()
    {
        ObjectiveListCon = ObjectiveList.GetComponent<ObjectiveListController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider subject)
    {
        if (subject.gameObject == RequiredObject)
        {
            ObjectiveListCon.ObjectiveCompletion[ObjectiveNum-1] = true;
            ObjectiveListCon.ObjectiveUpdate = true;
        }
    }
}
