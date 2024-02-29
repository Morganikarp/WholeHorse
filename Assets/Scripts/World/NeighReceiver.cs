using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighReceiver : MonoBehaviour
{

    public bool signal;
    bool singleUse;

    public GameObject ObjectiveList;
    public int ObjectiveNum;
    ObjectiveListController ObjectiveListCon;

    // Start is called before the first frame update
    void Start()
    {
        signal = false;
        singleUse = false;
        ObjectiveListCon = ObjectiveList.GetComponent<ObjectiveListController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (signal && !singleUse)
        {
            singleUse = true;
            ObjectiveListCon.ObjectiveCompletion[ObjectiveNum - 1] = true;
            ObjectiveListCon.ObjectiveUpdate = true;
        }
    }
}
