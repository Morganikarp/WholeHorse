using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnButtonController : ButtonBase
{
    public bool TransMenu; //Represents when the menu is transitioning
    public bool rootOnScreen; //Represents if the root menu is on screen of not
    float destinationDif; // The difference that a menu must be within to have made it to the final location

    public Transform RootMenuTrans; //The transform component of the root menu

    Transform SubMenuTrans; //The transform component of the file menu

    float OffScreenXPos;
    float TransitionSpeed;

    private void Start()
    {
        //RootMenuTrans = GameObject.Find("RootMenu").transform;

        SubMenuTrans = transform.parent.gameObject.transform;

        TransMenu = false;
        rootOnScreen = true;
        destinationDif = 0.01f;
        OffScreenXPos = 200f;
        TransitionSpeed = 30f;
    }

    private void Update()
    {
        if (MouseController() && !TransMenu) //If the return button is clicked on...
        {
            TransMenu = true;
            rootOnScreen = true;
        }

        if (TransMenu)
        {
            Move();
        }
    }

    void Move() //Moves the root menu off screen and the file menu on screen, and vice versa
    {

        switch (rootOnScreen)
        {
            case true:
                SubMenuTrans.position = new Vector3(Mathf.Lerp(SubMenuTrans.position.x, OffScreenXPos, Time.deltaTime * TransitionSpeed), SubMenuTrans.position.y, SubMenuTrans.position.z); //set the file menu's position to a linearly interpolated vector at some point between its on screen and off screen locations, based on the time between frames multipled by a speed value
                RootMenuTrans.position = new Vector3(Mathf.Lerp(RootMenuTrans.position.x, 0f, Time.deltaTime * TransitionSpeed), RootMenuTrans.position.y, SubMenuTrans.position.z); //set the root menu's position to a linearly interpolated vector at some point between its off screen and on screen locations, based on the time between frames multipled by a speed value
                if ((OffScreenXPos - destinationDif) < SubMenuTrans.position.x && SubMenuTrans.position.x < (OffScreenXPos + destinationDif)) //if the menu's x position within a certain distance to its final location...
                {
                    SubMenuTrans.position = new Vector3(OffScreenXPos, SubMenuTrans.position.y, SubMenuTrans.position.z); //set the file menu's position to its off screen vector
                    RootMenuTrans.transform.position = new Vector3(0f, RootMenuTrans.position.y, SubMenuTrans.position.z); //set the root menu's position to its on screen vector
                    TransMenu = false; //The menu's have finished transitioning
                }
                break;
            case false:
                SubMenuTrans.position = new Vector3(Mathf.Lerp(SubMenuTrans.position.x, 0f, Time.deltaTime * TransitionSpeed), SubMenuTrans.position.y, SubMenuTrans.position.z); //set the file menu's position to a linearly interpolated vector at some point between its off screen and on screen locations, based on the time between frames multipled by a speed value
                RootMenuTrans.position = new Vector3(Mathf.Lerp(RootMenuTrans.position.x, -OffScreenXPos, Time.deltaTime * TransitionSpeed), RootMenuTrans.position.y, SubMenuTrans.position.z); //set the root menu's position to a linearly interpolated vector at some point between its off screen and on screen locations, based on the time between frames multipled by a speed value

                if ((0f - destinationDif) < SubMenuTrans.position.x && SubMenuTrans.position.x < (0f + destinationDif)) //if the menu's x position within a certain distance to its final location...
                {
                    SubMenuTrans.transform.position = new Vector3(0f, SubMenuTrans.position.y, SubMenuTrans.position.z); //set the file menu's position to its on screen vector
                    RootMenuTrans.transform.position = new Vector3(-OffScreenXPos, RootMenuTrans.position.y, SubMenuTrans.position.z);//set the root menu's position to its off screen vector
                    TransMenu = false; //The menu's have finished transitioning
                }
                break;

        }
    }
}