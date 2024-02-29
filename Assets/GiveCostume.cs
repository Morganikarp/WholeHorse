using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class GiveCostume : MonoBehaviour
{
    public List<GameObject> costumesToTakeDown;
    public bool head;

    public GameObject ObjectiveList;
    ObjectiveListController ObjectiveListCon;

    // Start is called before the first frame update
    void Start()
    {
        ObjectiveListCon = ObjectiveList.GetComponent<ObjectiveListController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>())
        {
            if (other.GetComponentInParent<PlayerController>().horselessMode)
            {
                ObjectiveListCon.ObjectiveCompletion[0] = true;
                ObjectiveListCon.ObjectiveUpdate = true;

                other.GetComponentInParent<PlayerController>().AmHeadPlayer = head;
                other.GetComponentInParent<PlayerController>().horselessMode = false;
                other.GetComponentInParent<PlayerController>().SetCostumeModel();

                foreach (GameObject costume in costumesToTakeDown) { 
                    costume.SetActive(false);
                }

                FileStream fileStream = new FileStream(Application.dataPath + "/Resources/SaveFiles/SaveFile1.txt", FileMode.Open, FileAccess.ReadWrite);
                StreamWriter streamWriter = new StreamWriter(fileStream);

                streamWriter.WriteLine("1");
                streamWriter.WriteLine("1");
                streamWriter.WriteLine("1;0-0;0;0-0;0;0");

                if (head)
                {
                    switch (other.GetComponentInParent<PlayerController>().Player1)
                    {
                        case true:
                            streamWriter.WriteLine("1");
                            break;
                        case false:
                            streamWriter.WriteLine("2");
                            break;
                    }
                }

                streamWriter.Close();
                fileStream.Close();

                Destroy(gameObject);
            }
        }
    }
}
