using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RootButtonController : ButtonBase
{

    public GameObject FileMenu;
    public FileButtonController[] FileItems = new FileButtonController[3];
    public ReturnButtonController FileReturn;

    public GameObject OptionsMenu;
    public ReturnButtonController OptionsReturn;

    private void Start()
    {
        for (int i = 0; i < FileMenu.transform.childCount; i++)
        {
            Transform subject = FileMenu.transform.GetChild(i);
            if (subject.tag == "File")
            {
                FileItems[i] = subject.GetComponent<FileButtonController>();
            }
            else //If the child lacks the correct tag, then it must be the return button
            {
                FileReturn = subject.GetComponent<ReturnButtonController>();
            }
        }

        OptionsReturn = OptionsMenu.transform.GetChild(0).GetComponent<ReturnButtonController>();
    }

    void Update()
    {
        if (MouseController())
        {
            ButtonFunctions();
        }
    }

    void ButtonFunctions()
    {
        switch (this.gameObject.name)
        {
            case "New":
                bool fileFree = false; //Creates a new bool to represent if there are any empty save files, and sets it as false by default

                for (int i = 0; i <= FileItems.Length - 1; i++)
                {
                    string currentPath = FileItems[i].filePath;

                    FileStream fileStream = new FileStream(currentPath, FileMode.Open, FileAccess.Read);
                    StreamReader streamReader = new StreamReader(fileStream);

                    if (streamReader.ReadLine() == "0") //If the first line of the subject save file is "false", it is empty
                    {
                        streamReader.Close();
                        fileStream.Close();

                        FileStream newFileStream = new FileStream(currentPath, FileMode.Open, FileAccess.Write); //Opens a file stream to wrrite to the active save file
                        StreamWriter streamWriter = new StreamWriter(newFileStream);

                        streamWriter.Write("1"); //Writes "true " to the active save file, as it is about to be used

                        streamWriter.Close();
                        newFileStream.Close();

                        fileFree = true; //A free file was found, so the variable changes to reflect that

                        FileButtonController.ActiveFileData = currentPath;
                        SceneManager.LoadScene(1, LoadSceneMode.Single); //Loads the level select scene
                        break;
                    }

                    else
                    {
                        streamReader.Close();
                        fileStream.Close();
                    }
                }

                if (!fileFree) //If no free files were found...
                {
                    for (int i = 0; i <= FileItems.Length - 1; i++)
                    {
                        FileItems[i].Override = true; //Every FileMenuItem component has their Override bool set to true, and they can now overwrite the data of their associate save files
                    }
                    FileReturn.TransMenu = true;
                    FileReturn.rootOnScreen = false;
                }
                break;
            case "Load":
                for (int i = 0; i <= FileItems.Length - 1; i++)
                {
                    FileItems[i].Override = false;
                }
                FileReturn.TransMenu = true;
                FileReturn.rootOnScreen = false;

                break;
            case "Options":
                OptionsReturn.TransMenu = true;
                OptionsReturn.rootOnScreen = false;
                break;
            case "Quit":
                Application.Quit();
                break;
        }
    }
}
