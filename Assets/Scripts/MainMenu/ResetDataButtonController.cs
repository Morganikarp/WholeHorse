using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetDataButtonController : ButtonBase
{

    string[] newFileContent = new string[4] { "0", "1", "0;0-0;0;0-0;0;0", "0" };
    int fileNum;

    void Start()
    {
        fileNum = 3;
    }

    void Update()
    {
        if (MouseController()) //If clicked on...
        {
            for (int i = 0; i < fileNum; i++)
            {
                string filePath = Application.dataPath + "/Resources/SaveFiles/SaveFile" + (i + 1) + ".txt";

                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write);
                StreamWriter streamWriter = new StreamWriter(fileStream);

                for (int j = 0; j < newFileContent.Length; j++)
                {
                    streamWriter.WriteLine(newFileContent[j]);
                }

                streamWriter.Close();
                fileStream.Close();
            }
        }
    }
}
