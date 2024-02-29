using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FileButtonController : ButtonBase
{
    static public string ActiveFileData;

    public string filePath;
    public bool Override;
    string[] newFileContent = new string[4] { "1", "1", "0;0-0;0;0-0;0;0" , "0"};

    SpriteRenderer spriteRenderer;
    public Sprite[] FileStates = new Sprite[5];
    bool[] CompletionRecord = new bool[3];

    private void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        filePath = Application.dataPath + "/Resources/SaveFiles/" + this.gameObject.name + ".txt"; //the file path to this menu items associated save file
        Override = false;

        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fileStream);

        string newFile = streamReader.ReadLine();
        streamReader.ReadLine();
        string[] FileProgress = streamReader.ReadLine().Split("-");

        streamReader.Close();
        fileStream.Close();

        if (newFile == "1")
        {
            for (int i = 0; i < FileProgress.Length; i++)
            {
                string[] AreaProgress = FileProgress[i].Split(";");
                int AreaCompletion = 0;

                for (int j = 0; j < AreaProgress.Length; j++)
                {
                    if (AreaProgress[j] == "1")
                    {
                        AreaCompletion++;
                    }
                }

                CompletionRecord[i] = (AreaCompletion == AreaProgress.Length) ? true : false;
            }


            if (CompletionRecord[0])
            {
                if (CompletionRecord[1])
                {
                    if (CompletionRecord[2])
                    {
                        spriteRenderer.sprite = FileStates[4];
                    }

                    else
                    {
                        spriteRenderer.sprite = FileStates[2];
                    }
                }

                else if (CompletionRecord[2])
                {
                    spriteRenderer.sprite = FileStates[3];
                }

                else
                {
                    spriteRenderer.sprite = FileStates[1];
                }
            }

            else
            {
                spriteRenderer.sprite = FileStates[1];
            }
        }

        else
        {
            spriteRenderer.sprite = FileStates[0];
        }
    }

    void Update()
    {
        if (MouseController()) //If clicked on...
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine(newFileContent[0]);

            if (Override) //If there are no empty save files, the player will have the choice to rest one save file
            {
                streamWriter.WriteLine(newFileContent[1]);
                streamWriter.WriteLine(newFileContent[2]);
                streamWriter.WriteLine(newFileContent[3]);
            }

            streamWriter.Close();
            fileStream.Close();

            FileStream GetfileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            StreamReader GetstreamReader = new StreamReader(GetfileStream);

            GetstreamReader.ReadLine();
            string ToScene = GetstreamReader.ReadLine();

            GetstreamReader.Close();
            GetfileStream.Close();

            ActiveFileData = filePath;
            SceneManager.LoadScene(int.Parse(ToScene), LoadSceneMode.Single);
        }
    }
}
