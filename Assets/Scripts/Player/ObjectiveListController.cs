using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class ObjectiveListController : ButtonBase
{
    float RestingY;
    float destinationDif;
    float TransitionSpeed;

    Vector4 DefaultColour;
    Vector4 CompleteColour;

    RectTransform Trans;
    Vector3 RestingPos;
    Vector3 ActivePos;

    int AreaIndex;
    string[] SaveProgress;
    string[] LevelProgress = new string[3];
    string[] Objectives = new string[3];
    public bool[] ObjectiveCompletion = new bool[3];
    public bool ObjectiveUpdate;

    TextMeshProUGUI[] Readouts;

    void Start()
    {
        RestingY = -6.7f;
        destinationDif = 0.01f;
        TransitionSpeed = 30f;

        DefaultColour = new Vector4(0.3137255f, 0.3137255f, 0.3137255f, 1f);
        CompleteColour = new Vector4(0.6666667f, 0.6666667f, 0.6666667f, 1f);

        Trans = GetComponent<RectTransform>();
        RestingPos = Trans.localPosition;
        ActivePos = new Vector3(RestingPos.x, RestingY, RestingPos.z);

        Readouts = GetComponentsInChildren<TextMeshProUGUI>();

        AreaIndex = SceneManager.GetActiveScene().buildIndex;
        DisplayText();
    }

    void Update()
    {
        Movement();

        if (ObjectiveUpdate && 0 < AreaIndex && AreaIndex < 4)
        {
            ObjectiveUpdate = false;
            UpdateText();
        }
    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Trans.localPosition = new Vector3(0f, Mathf.Lerp(Trans.localPosition.y, ActivePos.y, Time.deltaTime * TransitionSpeed), 0f);

            if ((ActivePos.y - destinationDif) < Trans.position.y && Trans.position.y < (ActivePos.y + destinationDif))
            {
                Trans.localPosition = ActivePos;
            }
        }

        else if (Trans.localPosition != RestingPos)
        {
            Trans.localPosition = new Vector3(0f, Mathf.Lerp(Trans.localPosition.y, RestingPos.y, Time.deltaTime * TransitionSpeed), 0f);

            if ((RestingPos.y - destinationDif) < Trans.position.y && Trans.position.y < (RestingPos.y + destinationDif))
            {
                Trans.localPosition = RestingPos;
            }
        }
    }

    void DisplayText()
    {
        //FileButtonController.ActiveFileData = Application.dataPath + "/Resources/SaveFiles/SaveFile1.txt";

        FileStream SaveFileStream = new FileStream(FileButtonController.ActiveFileData, FileMode.Open, FileAccess.Read);
        StreamReader SaveFileReader = new StreamReader(SaveFileStream);

        SaveFileReader.ReadLine();
        SaveFileReader.ReadLine();
        SaveProgress = SaveFileReader.ReadLine().Split("-");

        SaveFileReader.Close();
        SaveFileStream.Close();

        if (0 < AreaIndex && AreaIndex < 4)
        {
            LevelProgress = SaveProgress[AreaIndex - 1].Split(";");
            int LastObjectiveHide = Objectives.Length - 1;

            for (int i = 0; i < LevelProgress.Length; i++)
            {
                ObjectiveCompletion[i] = LevelProgress[i] == "1" ? true : false;

                if (ObjectiveCompletion[i])
                {
                    Readouts[i].color = CompleteColour;
                }

                else
                {
                    Readouts[i].color = DefaultColour;
                    LastObjectiveHide--;
                }
            }

            FileStream ObjectiveFileStream = new FileStream(Application.dataPath + "/Resources/Objectives/ObjectiveDatabase.txt", FileMode.Open, FileAccess.Read);
            StreamReader ObjectiveFileReader = new StreamReader(ObjectiveFileStream);

            string LevelObjectivesRaw = "null";
            for (int i = 0; i < AreaIndex; i++)
            {
                LevelObjectivesRaw = ObjectiveFileReader.ReadLine();
            }

            Objectives = LevelObjectivesRaw.Split(";");

            for (int i = 0; i < Objectives.Length; i++)
            {
                Readouts[i].text = Objectives[i];
                if (i != 0 && (i + 1) == Objectives.Length && LastObjectiveHide == 0)
                {
                    Readouts[i].text = "? ? ?";
                }
            }

            ObjectiveFileReader.Close();
            ObjectiveFileStream.Close();
        }

        else
        {
            Readouts[0].text = "Knock out the competition";
            Readouts[1].text = "to ensure that you win";
            Readouts[2].text = "the big horse race!";
        }
    }

    void UpdateText()
    {
        string[] ProtoSaveFileData = new string[3];
        int LastObjectiveHide = Objectives.Length - 1;

        for (int i = 0; i <= Objectives.Length - 1; i++) 
        {
            switch (ObjectiveCompletion[i])
            {
                case true:
                    ProtoSaveFileData[i] = "1";
                    LastObjectiveHide--;
                    Readouts[i].color = CompleteColour;
                    break;
                case false:
                    ProtoSaveFileData[i] = "0";
                    break;
            }

            if (LastObjectiveHide == 0)
            {
                Readouts[i].text = Objectives[i];
            }
        }

        SaveProgress[AreaIndex-1] = string.Join(";", ProtoSaveFileData);
        string NewSaveProgress = string.Join("-", SaveProgress);

        //FileButtonController.ActiveFileData = Application.dataPath + "/Resources/SaveFiles/SaveFile1.txt";

        FileStream OverwriteFileStream = new FileStream(FileButtonController.ActiveFileData, FileMode.Open, FileAccess.Write);
        StreamWriter OverwriteFileWriter = new StreamWriter(OverwriteFileStream);

        OverwriteFileWriter.WriteLine("1");
        OverwriteFileWriter.WriteLine(AreaIndex);
        OverwriteFileWriter.Write(NewSaveProgress);

        OverwriteFileWriter.Close();
        OverwriteFileStream.Close();
    }
}
