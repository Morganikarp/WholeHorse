using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SceneChildSorter))]

public class CustomInpsector_SceneChildSorter : Editor
{

    string enteredNamingScheme = "Object (*)";
    int selectedGridButton;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var scs = (target as SceneChildSorter);

        if (GUILayout.Button("Sort Children Alphabetically"))
        {
            scs.SortChildren(SceneChildSorter.sortMode.Alphabetical);
        }

        if (GUILayout.Button("Sort Children Randomly"))
        {
            scs.SortChildren(SceneChildSorter.sortMode.Random);
        }


        EditorGUILayout.Space();

        

        selectedGridButton = GUILayout.SelectionGrid(selectedGridButton, new string[48] { 
            "+X +Y +Z", "+X +Z +Y", "+Y +X +Z", "+Y +Z +X", "+Z +X +Y", "+Z +Y +X", //PPP
            "+X +Y -Z", "+X +Z -Y", "+Y +X -Z", "+Y +Z -X", "+Z +X -Y", "+Z +Y -X", //PPN
            "+X -Y +Z", "+X -Z +Y", "+Y -X +Z", "+Y -Z +X", "+Z -X +Y", "+Z -Y +X", //PNP
            "-X +Y +Z", "-X +Z +Y", "-Y +X +Z", "-Y +Z +X", "-Z +X +Y", "-Z +Y +X", //NPP
            "-X -Y +Z", "-X -Z +Y", "-Y -X +Z", "-Y -Z +X", "-Z -X +Y", "-Z -Y +X", //NNP
            "-X +Y -Z", "-X +Z -Y", "-Y +X -Z", "-Y +Z -X", "-Z +X -Y", "-Z +Y -X", //NPN
            "+X -Y -Z", "+X -Z -Y", "+Y -X -Z", "+Y -Z -X", "+Z -X -Y", "+Z -Y -X", //PNN
            "-X -Y -Z", "-X -Z -Y", "-Y -X -Z", "-Y -Z -X", "-Z -X -Y", "-Z -Y -X", //NNN
            }, 6);

        if (GUILayout.Button("Sort Children Positionally"))
        {
            switch (selectedGridButton)
            {
                case 0: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xyz, SceneChildSorter.positivesSetup.ppp); break;
                case 1: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xzy, SceneChildSorter.positivesSetup.ppp); break;
                case 2: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yxz, SceneChildSorter.positivesSetup.ppp); break;
                case 3: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yzx, SceneChildSorter.positivesSetup.ppp); break;
                case 4: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zxy, SceneChildSorter.positivesSetup.ppp); break;
                case 5: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zyx, SceneChildSorter.positivesSetup.ppp); break;

                case 6: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xyz, SceneChildSorter.positivesSetup.ppn); break;
                case 7: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xzy, SceneChildSorter.positivesSetup.ppn); break;
                case 8: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yxz, SceneChildSorter.positivesSetup.ppn); break;
                case 9: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yzx, SceneChildSorter.positivesSetup.ppn); break;
                case 10: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zxy, SceneChildSorter.positivesSetup.ppn); break;
                case 11: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zyx, SceneChildSorter.positivesSetup.ppn); break;

                case 12: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xyz, SceneChildSorter.positivesSetup.pnp); break;
                case 13: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xzy, SceneChildSorter.positivesSetup.pnp); break;
                case 14: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yxz, SceneChildSorter.positivesSetup.pnp); break;
                case 15: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yzx, SceneChildSorter.positivesSetup.pnp); break;
                case 16: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zxy, SceneChildSorter.positivesSetup.pnp); break;
                case 17: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zyx, SceneChildSorter.positivesSetup.pnp); break;

                case 18: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xyz, SceneChildSorter.positivesSetup.npp); break;
                case 19: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xzy, SceneChildSorter.positivesSetup.npp); break;
                case 20: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yxz, SceneChildSorter.positivesSetup.npp); break;
                case 21: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yzx, SceneChildSorter.positivesSetup.npp); break;
                case 22: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zxy, SceneChildSorter.positivesSetup.npp); break;
                case 23: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zyx, SceneChildSorter.positivesSetup.npp); break;

                case 24: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xyz, SceneChildSorter.positivesSetup.nnp); break;
                case 25: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xzy, SceneChildSorter.positivesSetup.nnp); break;
                case 26: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yxz, SceneChildSorter.positivesSetup.nnp); break;
                case 27: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yzx, SceneChildSorter.positivesSetup.nnp); break;
                case 28: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zxy, SceneChildSorter.positivesSetup.nnp); break;
                case 29: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zyx, SceneChildSorter.positivesSetup.nnp); break;

                case 30: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xyz, SceneChildSorter.positivesSetup.npn); break;
                case 31: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xzy, SceneChildSorter.positivesSetup.npn); break;
                case 32: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yxz, SceneChildSorter.positivesSetup.npn); break;
                case 33: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yzx, SceneChildSorter.positivesSetup.npn); break;
                case 34: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zxy, SceneChildSorter.positivesSetup.npn); break;
                case 35: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zyx, SceneChildSorter.positivesSetup.npn); break;

                case 36: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xyz, SceneChildSorter.positivesSetup.pnn); break;
                case 37: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xzy, SceneChildSorter.positivesSetup.pnn); break;
                case 38: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yxz, SceneChildSorter.positivesSetup.pnn); break;
                case 39: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yzx, SceneChildSorter.positivesSetup.pnn); break;
                case 40: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zxy, SceneChildSorter.positivesSetup.pnn); break;
                case 41: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zyx, SceneChildSorter.positivesSetup.pnn); break;

                case 42: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xyz, SceneChildSorter.positivesSetup.nnn); break;
                case 43: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.xzy, SceneChildSorter.positivesSetup.nnn); break;
                case 44: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yxz, SceneChildSorter.positivesSetup.nnn); break;
                case 45: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.yzx, SceneChildSorter.positivesSetup.nnn); break;
                case 46: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zxy, SceneChildSorter.positivesSetup.nnn); break;
                case 47: scs.SortChildren(SceneChildSorter.sortMode.Positional, SceneChildSorter.positionalOrder.zyx, SceneChildSorter.positivesSetup.nnn); break;

            }

            
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        enteredNamingScheme = EditorGUILayout.TextField("Child Naming Scheme: ", enteredNamingScheme);

        if (GUILayout.Button("Rename Children"))
        {
            scs.RenameChildren(enteredNamingScheme);
        }
    }
}
