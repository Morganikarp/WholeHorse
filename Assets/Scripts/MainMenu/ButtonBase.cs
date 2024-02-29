using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ButtonBase : MonoBehaviour
{
    protected bool MouseController()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(Ray, out RaycastHit Subject, 200) && Subject.collider != null && Subject.collider.gameObject == this.gameObject)
            {
                //MARV: Audio code goes between here...



                //...and here
                return true;
            }

            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }
}
