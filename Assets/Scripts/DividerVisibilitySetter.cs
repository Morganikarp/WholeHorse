using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DividerVisibilitySetter : MonoBehaviour
{
    public Transform PosXDiv;
    public Transform NegXDiv;
    public Transform PosZDiv;
    public Transform NegZDiv;

    public DividerVisibilityModes posXWallState;
    public DividerVisibilityModes negXWallState;
    public DividerVisibilityModes posZWallState;
    public DividerVisibilityModes negZWallState;

    public enum DividerVisibilityModes { alwaysVisible, onlyVisibleBetweenCounters, neverVisible };

    public void UpdateDividerVisuals()
    {

        SetSpecificVisibilityMode(posXWallState, PosXDiv, new Vector3(1, 0, 0));
        SetSpecificVisibilityMode(negXWallState, NegXDiv, new Vector3(-1, 0, 0));
        SetSpecificVisibilityMode(posZWallState, PosZDiv, new Vector3(0, 0, 1));
        SetSpecificVisibilityMode(negZWallState, NegZDiv, new Vector3(0, 0, -1));

    }

    public void SetSpecificVisibilityMode(DividerVisibilityModes specificMode, Transform renderingObject, Vector3 addedPosition)
    {
        if (specificMode == DividerVisibilityModes.alwaysVisible || (specificMode == DividerVisibilityModes.onlyVisibleBetweenCounters && GameObject.FindGameObjectsWithTag("Counter").Any(counter => counter.transform.position == transform.position) && GameObject.FindGameObjectsWithTag("Counter").Any(counter => counter.transform.position == transform.position + Vector3.Scale(addedPosition, transform.localScale))))
        {
            renderingObject.gameObject.SetActive(true);
        } 
        
        else
        {
            renderingObject.gameObject.SetActive(false);
        }
    }
}
