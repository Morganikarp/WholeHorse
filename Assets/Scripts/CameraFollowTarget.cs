using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    public float followDistance;
    public float followSpeed_HalfLife;

    Camera Cam;
    public Transform P1_costumeTrans;
    public Transform P1_horseTrans;
    public Transform P2_costumeTrans;
    public Transform P2_horseTrans;
    public Vector3 CamDistance;
    public float minZoom;
    public float maxZoom;
    RectTransform ObjectiveTrans;
    

private void Start() 
    {
        Cam = GetComponent<Camera>();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "ObjectiveCanvas")
            {
                ObjectiveTrans = transform.GetChild(i).GetComponent<RectTransform>();
            }
        }

        

    }

    private void Update() 
    {
        Vector3 playerMidpoint = new Vector3((P1_costumeTrans.position.x + P2_costumeTrans.position.x) / 2f, (P1_costumeTrans.position.y + P2_costumeTrans.position.y) / 2f, (P1_costumeTrans.position.z + P2_costumeTrans.position.z) / 2f);
        transform.position = Vector3.Lerp(transform.position, playerMidpoint + CamDistance, followSpeed_HalfLife * Time.deltaTime);
        
        if (!P1_costumeTrans.parent.transform.gameObject.activeSelf)
        {
            transform.position = P2_horseTrans.position + CamDistance;
        }

        else if (!P2_costumeTrans.parent.transform.gameObject.activeSelf)
        {
            transform.position = P1_horseTrans.position + CamDistance;
        }

        else 
        { 
            Vector3 playerDifference = P1_costumeTrans.position - P2_costumeTrans.position;
            Cam.orthographicSize = Mathf.Lerp(minZoom, maxZoom, playerDifference.magnitude / 50f);
            float ScaledSize = Cam.orthographicSize * (1 / minZoom);
            ObjectiveTrans.localScale = new Vector3(ScaledSize, ScaledSize, ScaledSize); 
        }
    }
}
