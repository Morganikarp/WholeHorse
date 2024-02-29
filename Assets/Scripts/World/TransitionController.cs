using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
    public GameObject ObjectiveList;
    ObjectiveListController ObjectiveListCon;
    public bool CostumeShop;

    public int NextAreaIndex;
    bool allowRace;
    bool allowLeaveCostume;

    public GameObject Player1;
    public GameObject Player2;

    private void Start()
    {
        ObjectiveListCon = ObjectiveList.GetComponent<ObjectiveListController>();
    }

    private void Update()
    {
        if (NextAreaIndex == 4 || CostumeShop)
        {
            FileStream fileStream = new FileStream(FileButtonController.ActiveFileData, FileMode.Open, FileAccess.Read);
            StreamReader fileReader = new StreamReader(fileStream);

            fileReader.ReadLine();
            fileReader.ReadLine();
            string[] progressCheck = fileReader.ReadLine().Split("-");
            string[] costumeProgress = progressCheck[0].Split(";");
            string[] barProgress = progressCheck[1].Split(";");
            string[] hotelProgress = progressCheck[2].Split(";");

            if ((int.Parse(barProgress[2]) + int.Parse(hotelProgress[2])) == 2)
            {
                allowRace = true;
            }

            else
            {
                allowRace = false;
            }

            if (costumeProgress[0] == "1")
            {
                allowLeaveCostume = true;
            }

            else
            {
                allowLeaveCostume = false;
            }

            fileReader.Close();
            fileStream.Close();
        }
    }

    private void OnTriggerEnter(Collider subject)
    {
        if (subject.gameObject.tag == "Player")
        {
            if (NextAreaIndex == 4)
            {
                if (allowRace)
                {
                    //FileButtonController.ActiveFileData = Application.dataPath + "/Resources/SaveFiles/SaveFile1.txt";

                    FileStream fileStream = new FileStream(FileButtonController.ActiveFileData, FileMode.Open, FileAccess.Write);
                    StreamWriter fileWriter = new StreamWriter(fileStream);

                    fileWriter.WriteLine("1");
                    fileWriter.WriteLine(NextAreaIndex);

                    fileWriter.Close();
                    fileWriter.Close();

                    Player1.transform.GetChild(1).gameObject.SetActive(true);
                    Player2.transform.GetChild(1).gameObject.SetActive(true);

                    SceneManager.LoadScene(NextAreaIndex, LoadSceneMode.Single);
                }
            }

            else if (CostumeShop)
            {
                if (allowLeaveCostume)
                {
                    //FileButtonController.ActiveFileData = Application.dataPath + "/Resources/SaveFiles/SaveFile1.txt";

                    FileStream fileStream = new FileStream(FileButtonController.ActiveFileData, FileMode.Open, FileAccess.Write);
                    StreamWriter fileWriter = new StreamWriter(fileStream);

                    fileWriter.WriteLine("1");
                    fileWriter.WriteLine(NextAreaIndex);

                    fileWriter.Close();
                    fileWriter.Close();

                    Player1.transform.GetChild(1).gameObject.SetActive(true);
                    Player2.transform.GetChild(1).gameObject.SetActive(true);

                    SceneManager.LoadScene(NextAreaIndex, LoadSceneMode.Single);
                }
            }

            else
            {
                //FileButtonController.ActiveFileData = Application.dataPath + "/Resources/SaveFiles/SaveFile1.txt";

                FileStream fileStream = new FileStream(FileButtonController.ActiveFileData, FileMode.Open, FileAccess.Write);
                StreamWriter fileWriter = new StreamWriter(fileStream);

                fileWriter.WriteLine("1");
                fileWriter.WriteLine(NextAreaIndex);

                fileWriter.Close();
                fileWriter.Close();

                Player1.transform.GetChild(1).gameObject.SetActive(true);
                Player2.transform.GetChild(1).gameObject.SetActive(true);

                if (CostumeShop)
                {
                    ObjectiveListCon.ObjectiveCompletion[1] = true;
                    ObjectiveListCon.ObjectiveUpdate = true;
                }

                SceneManager.LoadScene(NextAreaIndex, LoadSceneMode.Single);
            }
        }
    }
}
