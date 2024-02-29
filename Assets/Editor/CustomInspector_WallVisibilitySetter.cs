using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WallVisibilitySetter))]
[CanEditMultipleObjects]

public class CustomInspector_WallVisibilitySetter : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Bake Walls NOW!"))
        {
            foreach (Transform selectedTransform in Selection.transforms) { 
            
                if (selectedTransform.GetComponent<WallVisibilitySetter>())
                {
                    
                    selectedTransform.GetComponent<WallVisibilitySetter>().UpdateWallVisuals();

                }
                
            }

        }

    }
}