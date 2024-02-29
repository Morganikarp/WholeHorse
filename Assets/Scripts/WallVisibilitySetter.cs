using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;

public class WallVisibilitySetter : MonoBehaviour
{
    
    public Transform PosXWall;
    public Transform NegXWall;
    public Transform PosZWall;
    public Transform NegZWall;
    public Transform ceiling;
    public Transform floor;
    public BoxCollider boxCollider;
    public Mesh cubeMesh;

    public float defaultWallHeight;

    public WallVisibilityModes posXWallState;
    public WallVisibilityModes negXWallState;
    public WallVisibilityModes posZWallState;
    public WallVisibilityModes negZWallState;
    public WallVisibilityModes ceilState;
    public WallVisibilityModes floorState;

    //public float cameraObscuringScanSteps; //what's the spacing like on the intervals between camera scans, EG if this wall is nothing blocking at the bottom or top, great, but what about halfway through? (set to 1 for that) - what about the quarters (set to 2), etc
    public float cameraObscurationAllowance; //how many obscured objects should the camera check allow before it shortens the p, useful if there's a background it's going to catch, etc
    public string obscuringTag; //what's the name of the tag that the objects being hidden (usually the floor) has?
    public bool useCameraForwardDir; //uses the direction of the camera instead of the respective distance between here and there. activate for ortho cameras, deactivate for perspective

    public enum WallVisibilityModes { alwaysVisible, onlyVisibleAtFront, neverVisible };


    public void UpdateWallVisuals()
    {

        Vector3 LeftBottomBackCorner = new Vector3(-0.5f, 0, -0.5f);
        Vector3 LeftBottomFrontCorner = new Vector3(-0.5f, 0, 0.5f);
        Vector3 LeftTopBackCorner = new Vector3(-0.5f, defaultWallHeight, -0.5f);
        Vector3 LeftTopFrontCorner = new Vector3(-0.5f, defaultWallHeight, 0.5f);
        Vector3 RightBottomBackCorner = new Vector3(0.5f, 0, -0.5f);
        Vector3 RightBottomFrontCorner = new Vector3(0.5f, 0, 0.5f);
        Vector3 RightTopBackCorner = new Vector3(0.5f, defaultWallHeight, -0.5f);
        Vector3 RightTopFrontCorner = new Vector3(0.5f, defaultWallHeight, 0.5f);

        /*if (FindObjectsOfType<WallVisibilitySetter>().Any(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.position.x == wvs.transform.position.x - 1 || transform.position.z == wvs.transform.position.z - 1)))
        {
            LeftTopBackCorner = new Vector3(-0.5f, FindObjectsOfType<WallVisibilitySetter>().First(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.position.x == wvs.transform.position.x - 1 || transform.position.z == wvs.transform.position.z - 1)).defaultWallHeight, -0.5f);
        }

        if (FindObjectsOfType<WallVisibilitySetter>().Any(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.position.x == wvs.transform.position.x - 1 || transform.position.z == wvs.transform.position.z + 1)))
        {
            LeftTopFrontCorner = new Vector3(-0.5f, FindObjectsOfType<WallVisibilitySetter>().First(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.position.x == wvs.transform.position.x - 1 || transform.position.z == wvs.transform.position.z + 1)).defaultWallHeight, 0.5f);
        }

        if (FindObjectsOfType<WallVisibilitySetter>().Any(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.position.x == wvs.transform.position.x + 1 || transform.position.z == wvs.transform.position.z - 1)))
        {
            RightTopBackCorner = new Vector3(0.5f, FindObjectsOfType<WallVisibilitySetter>().First(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.position.x == wvs.transform.position.x + 1|| transform.position.z == wvs.transform.position.z - 1)).defaultWallHeight, -0.5f);
        }

        if (FindObjectsOfType<WallVisibilitySetter>().Any(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.position.x == wvs.transform.position.x + 1 || transform.position.z == wvs.transform.position.z + 1)))
        {
            RightTopFrontCorner = new Vector3(0.5f, FindObjectsOfType<WallVisibilitySetter>().First(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.position.x == wvs.transform.position.x + 1 || transform.position.z == wvs.transform.position.z + 1)).defaultWallHeight, 0.5f);
        }*/

        List<WallVisibilitySetter> lowerLeftPillars = FindObjectsOfType<WallVisibilitySetter>().Where(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.localPosition.x == wvs.transform.localPosition.x + 1 && transform.localPosition.z == wvs.transform.localPosition.z)).ToList();
        List<WallVisibilitySetter> lowerRightPillars = FindObjectsOfType<WallVisibilitySetter>().Where(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.localPosition.x == wvs.transform.localPosition.x - 1 && transform.localPosition.z == wvs.transform.localPosition.z)).ToList();
        List<WallVisibilitySetter> lowerBackPillars = FindObjectsOfType<WallVisibilitySetter>().Where(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.localPosition.x == wvs.transform.localPosition.x && transform.localPosition.z == wvs.transform.localPosition.z + 1)).ToList();
        List<WallVisibilitySetter> lowerFrontPillars = FindObjectsOfType<WallVisibilitySetter>().Where(wvs => defaultWallHeight > wvs.defaultWallHeight && (transform.localPosition.x == wvs.transform.localPosition.x && transform.localPosition.z == wvs.transform.localPosition.z - 1)).ToList();

        List<WallVisibilitySetter> lowerLeftBackPillars = new List<WallVisibilitySetter>(); lowerLeftBackPillars.AddRange(lowerLeftPillars); lowerLeftBackPillars.AddRange(lowerBackPillars);
        List<WallVisibilitySetter> lowerLeftFrontPillars = new List<WallVisibilitySetter>(); lowerLeftFrontPillars.AddRange(lowerLeftPillars); lowerLeftFrontPillars.AddRange(lowerFrontPillars);
        List<WallVisibilitySetter> lowerRightBackPillars = new List<WallVisibilitySetter>(); lowerRightBackPillars.AddRange(lowerRightPillars); lowerRightBackPillars.AddRange(lowerBackPillars);
        List<WallVisibilitySetter> lowerRightFrontPillars = new List<WallVisibilitySetter>(); lowerRightFrontPillars.AddRange(lowerRightPillars); lowerRightFrontPillars.AddRange(lowerFrontPillars);

        if (lowerLeftBackPillars.Count > 0) { LeftTopBackCorner = new Vector3(-0.5f, lowerLeftBackPillars.OrderBy(p => p.defaultWallHeight).ToList()[0].defaultWallHeight, -0.5f); }
        if (lowerLeftFrontPillars.Count > 0) { LeftTopFrontCorner = new Vector3(-0.5f, lowerLeftFrontPillars.OrderBy(p => p.defaultWallHeight).ToList()[0].defaultWallHeight, 0.5f); }
        if (lowerRightBackPillars.Count > 0) { RightTopBackCorner = new Vector3(0.5f, lowerRightBackPillars.OrderBy(p => p.defaultWallHeight).ToList()[0].defaultWallHeight, -0.5f); }
        if (lowerRightFrontPillars.Count > 0) { RightTopFrontCorner = new Vector3(0.5f, lowerRightFrontPillars.OrderBy(p => p.defaultWallHeight).ToList()[0].defaultWallHeight, 0.5f); }

        Mesh generatedMesh = new Mesh();// cubeMesh;

        Vector3[] translatedVertices = new Vector3[cubeMesh.vertices.Length];

        for (int i = 0; i < cubeMesh.vertices.Length; i++) {
            if (cubeMesh.vertices[i].x < 0) {

                if (cubeMesh.vertices[i].y < 0)
                {
                    if (cubeMesh.vertices[i].z < 0)
                    {
                        translatedVertices[i] = LeftBottomBackCorner;
                    } 
                
                    else
                    {
                        translatedVertices[i] = LeftBottomFrontCorner;
                    }
                } 
                
                else
                {
                    if (cubeMesh.vertices[i].z < 0)
                    {
                        translatedVertices[i] = LeftTopBackCorner;
                    } 
                
                    else
                    {
                        translatedVertices[i] = LeftTopFrontCorner;
                    }
                }

            }

            else
            {
                if (cubeMesh.vertices[i].y < 0)
                {
                    if (cubeMesh.vertices[i].z < 0)
                    {
                        translatedVertices[i] = RightBottomBackCorner;
                    } 
                
                    else
                    {
                        translatedVertices[i] = RightBottomFrontCorner;
                    }
                } 
                
                else
                {
                    if (cubeMesh.vertices[i].z < 0)
                    {
                        translatedVertices[i] = RightTopBackCorner;
                    } 
                
                    else
                    {
                        translatedVertices[i] = RightTopFrontCorner;
                    }
                }
            }
            
        }

        generatedMesh.SetVertices(translatedVertices);
        generatedMesh.triangles = cubeMesh.triangles;
        generatedMesh.uv = cubeMesh.uv;
        generatedMesh.normals = cubeMesh.normals;
        GetComponent<MeshFilter>().mesh = generatedMesh;
        boxCollider.center = new Vector3(boxCollider.center.x, defaultWallHeight / 2f, boxCollider.center.z);
        boxCollider.size = new Vector3(boxCollider.size.x, defaultWallHeight, boxCollider.size.z);



        /*Camera obscuraCheckingCamera = FindObjectOfType<Camera>();

        //p
        if (obscuraCheckingCamera != null)
        {
            if (useCameraForwardDir) { //for orthographic cameras
                bool foundBlockedTile = false;

                for (int i = 0; i <= defaultWallHeight; i++)
                {
                    if (Physics.RaycastAll(transform.position + new Vector3(0, i * transform.lossyScale.y, 0), obscuraCheckingCamera.transform.forward).Where(hit => hit.collider.gameObject.tag == obscuringTag).ToArray().Length > cameraObscurationAllowance)
                    {
                        foundBlockedTile = true;
                        Debug.DrawRay(transform.position + new Vector3(0, i * transform.lossyScale.y, 0), obscuraCheckingCamera.transform.forward, Color.red, 10);
                    }
                    
                    else
                    {
                        Debug.DrawRay(transform.position + new Vector3(0, i * transform.lossyScale.y, 0), obscuraCheckingCamera.transform.forward, Color.green, 10);
                    }
                }

                
                if (foundBlockedTile) {
                    SetWallHeight(obscuringWallHeight);
                }

                else { 
                    SetWallHeight(defaultWallHeight); 
                }
            }



            else //for perspective cameras
            {
                if (Physics.RaycastAll(obscuraCheckingCamera.transform.position, (floor.transform.position - obscuraCheckingCamera.transform.position).normalized).Where(hit => hit.collider.gameObject.tag == obscuringTag).ToArray().Length > cameraObscurationAllowance) {
                    SetWallHeight(obscuringWallHeight);
                }

                else { 
                    SetWallHeight(defaultWallHeight); 
                }
            }
        }

        else
        {
            SetWallHeight(defaultWallHeight);
        }

        //visibility modes
        SetSpecificVisibilityMode(posXWallState, PosXWall, new Vector3(1, 0, 0));
        SetSpecificVisibilityMode(negXWallState, NegXWall, new Vector3(-1, 0, 0));
        SetSpecificVisibilityMode(posZWallState, PosZWall, new Vector3(0, 0, 1));
        SetSpecificVisibilityMode(negZWallState, NegZWall, new Vector3(0, 0, -1));
        SetSpecificVisibilityMode(ceilState, ceiling, new Vector3(0, 1, 0));
        SetSpecificVisibilityMode(floorState, floor, new Vector3(0, -1, 0));*/

    }

    /*public void SetSpecificVisibilityMode(WallVisibilityModes specificMode, Transform renderingObject, Vector3 addedPosition)
    {
        if (specificMode == WallVisibilityModes.alwaysVisible || (specificMode == WallVisibilityModes.onlyVisibleAtFront && !FindObjectsOfType<WallVisibilitySetter>().Any(wall => wall.transform.position == transform.position + Vector3.Scale(addedPosition, transform.localScale))))
        {
            renderingObject.gameObject.SetActive(true);
        }

        else
        {
            renderingObject.gameObject.SetActive(false);
        }
    }

    public void SetWallHeight(float settingHeight)
    {
        PosXWall.localScale = new Vector3(PosXWall.localScale.x, settingHeight, PosXWall.localScale.z);
        NegXWall.localScale = new Vector3(NegXWall.localScale.x, settingHeight, NegXWall.localScale.z);
        PosZWall.localScale = new Vector3(PosZWall.localScale.x, settingHeight, PosZWall.localScale.z);
        NegZWall.localScale = new Vector3(NegZWall.localScale.x, settingHeight, NegZWall.localScale.z);
        boxCollider.size = new Vector3(boxCollider.size.x, settingHeight, boxCollider.size.z);

        PosXWall.localPosition = new Vector3(PosXWall.localPosition.x, settingHeight / 2f, PosXWall.localPosition.z);
        NegXWall.localPosition = new Vector3(NegXWall.localPosition.x, settingHeight / 2f, NegXWall.localPosition.z);
        PosZWall.localPosition = new Vector3(PosZWall.localPosition.x, settingHeight / 2f, PosZWall.localPosition.z);
        NegZWall.localPosition = new Vector3(NegZWall.localPosition.x, settingHeight / 2f, NegZWall.localPosition.z);
        boxCollider.center = new Vector3(boxCollider.center.x, settingHeight / 2f, boxCollider.center.z);
        ceiling.localPosition = new Vector3(ceiling.localPosition.x, settingHeight, ceiling.localPosition.z);
    }*/


}
