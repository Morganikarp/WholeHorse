using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SceneChildSorter : MonoBehaviour
{

    public enum sortMode { Alphabetical, Positional, Random };
    public enum positionalOrder { xyz, xzy, yxz, yzx, zxy, zyx };
    public enum positivesSetup { ppp, ppn, pnp, npp, nnp, npn, pnn, nnn };

    public void SortChildren(sortMode currentMode, positionalOrder positionOrder = positionalOrder.xyz, positivesSetup positiveSetup = positivesSetup.ppp)
    {

        switch (currentMode)
        {
            case sortMode.Alphabetical:

                List<TransformAndString> nameEntrylist = new List<TransformAndString>();

                foreach (Transform child in transform) {

                    nameEntrylist.Add(new TransformAndString { myTransform = child, myString = child.gameObject.name } );

                }

                nameEntrylist = nameEntrylist.OrderBy(entry => entry.myString.Length).ThenBy(entry => entry.myString).ToList();

                for (int t = 0; t < nameEntrylist.Count; t++) {

                    nameEntrylist[t].myTransform.SetAsLastSibling();

                }

            break;

            case sortMode.Positional:

                List<TransformAndVector> positionEntryList = new List<TransformAndVector>();

                foreach (Transform child in transform) {

                    positionEntryList.Add(new TransformAndVector { myTransform = child, myVector = child.position } );

                }


                positionEntryList = ComputePositionalListSorting(positionEntryList, positionOrder, positiveSetup);



                for (int t = 0; t < positionEntryList.Count; t++) {

                    positionEntryList[t].myTransform.SetAsLastSibling();

                }

            break;

            case sortMode.Random:

                List<TransformAndInt> randomEntryList = new List<TransformAndInt>();

                foreach (Transform child in transform) {

                    int chosenRandomIndex = Random.Range(0, transform.childCount + 1);

                    while (randomEntryList.Any(entry => entry.myInt == chosenRandomIndex)) {

                        chosenRandomIndex = Random.Range(0, transform.childCount + 1);

                    }

                    randomEntryList.Add(new TransformAndInt { myTransform = child, myInt = chosenRandomIndex });

                }
         
                randomEntryList = randomEntryList.OrderBy(entry => entry.myInt).ToList();

                for (int t = 0; t < randomEntryList.Count; t++) {

                    randomEntryList[t].myTransform.SetAsLastSibling();

                }


            break;

        }
    }

    public void RenameChildren(string namingScheme)
    {
        if (namingScheme.Where(character => character == '*').Count() == 1)
        {

            string[] namingSchemeFragments = namingScheme.Split('*');

            if (namingSchemeFragments.Length == 2)
            {
                foreach (Transform child in transform)
                {
                    child.gameObject.name = namingSchemeFragments[0] + child.transform.GetSiblingIndex() + namingSchemeFragments[1];
                }
            }

        }

        else
        {
            Debug.LogError("Naming Scheme requires one * for index placement, to prevent data corruption");
        }


    }

    private class TransformAndString
    {
        public Transform myTransform;
        public string myString; 

    }

    private class TransformAndInt
    {
        public Transform myTransform;
        public int myInt;

    }

    private class TransformAndVector
    {
        public Transform myTransform;
        public Vector3 myVector;

    }

    private List<TransformAndVector> ComputePositionalListSorting(List<TransformAndVector> passedInEntryList, positionalOrder passedInPositionalOrder, positivesSetup passedInPositivesSetup)
    {

        switch (passedInPositionalOrder, passedInPositivesSetup)
        {
            //xyz
            case (positionalOrder.xyz, positivesSetup.ppp): return passedInEntryList.OrderBy(entry => entry.myVector.x).ThenBy(entry => entry.myVector.y).ThenBy(entry => entry.myVector.z).ToList();  
            case (positionalOrder.xyz, positivesSetup.ppn): return passedInEntryList.OrderBy(entry => entry.myVector.x).ThenBy(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.z).ToList();  
            case (positionalOrder.xyz, positivesSetup.pnp): return passedInEntryList.OrderBy(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.y).ThenBy(entry => entry.myVector.z).ToList();  
            case (positionalOrder.xyz, positivesSetup.npp): return passedInEntryList.OrderByDescending(entry => entry.myVector.x).ThenBy(entry => entry.myVector.y).ThenBy(entry => entry.myVector.z).ToList();  
            case (positionalOrder.xyz, positivesSetup.nnp): return passedInEntryList.OrderByDescending(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.y).ThenBy(entry => entry.myVector.z).ToList();  
            case (positionalOrder.xyz, positivesSetup.npn): return passedInEntryList.OrderByDescending(entry => entry.myVector.x).ThenBy(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.z).ToList();  
            case (positionalOrder.xyz, positivesSetup.pnn): return passedInEntryList.OrderBy(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.z).ToList();  
            case (positionalOrder.xyz, positivesSetup.nnn): return passedInEntryList.OrderByDescending(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.z).ToList();  

            //xzy
            case (positionalOrder.xzy, positivesSetup.ppp): return passedInEntryList.OrderBy(entry => entry.myVector.x).ThenBy(entry => entry.myVector.z).ThenBy(entry => entry.myVector.y).ToList();  
            case (positionalOrder.xzy, positivesSetup.ppn): return passedInEntryList.OrderBy(entry => entry.myVector.x).ThenBy(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.y).ToList();  
            case (positionalOrder.xzy, positivesSetup.pnp): return passedInEntryList.OrderBy(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.z).ThenBy(entry => entry.myVector.y).ToList();  
            case (positionalOrder.xzy, positivesSetup.npp): return passedInEntryList.OrderByDescending(entry => entry.myVector.x).ThenBy(entry => entry.myVector.z).ThenBy(entry => entry.myVector.y).ToList();  
            case (positionalOrder.xzy, positivesSetup.nnp): return passedInEntryList.OrderByDescending(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.z).ThenBy(entry => entry.myVector.y).ToList();  
            case (positionalOrder.xzy, positivesSetup.npn): return passedInEntryList.OrderByDescending(entry => entry.myVector.x).ThenBy(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.y).ToList();  
            case (positionalOrder.xzy, positivesSetup.pnn): return passedInEntryList.OrderBy(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.y).ToList();  
            case (positionalOrder.xzy, positivesSetup.nnn): return passedInEntryList.OrderByDescending(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.y).ToList();  

            //yxz
            case (positionalOrder.yxz, positivesSetup.ppp): return passedInEntryList.OrderBy(entry => entry.myVector.y).ThenBy(entry => entry.myVector.x).ThenBy(entry => entry.myVector.z).ToList();  
            case (positionalOrder.yxz, positivesSetup.ppn): return passedInEntryList.OrderBy(entry => entry.myVector.y).ThenBy(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.z).ToList();  
            case (positionalOrder.yxz, positivesSetup.pnp): return passedInEntryList.OrderBy(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.x).ThenBy(entry => entry.myVector.z).ToList();  
            case (positionalOrder.yxz, positivesSetup.npp): return passedInEntryList.OrderByDescending(entry => entry.myVector.y).ThenBy(entry => entry.myVector.x).ThenBy(entry => entry.myVector.z).ToList();  
            case (positionalOrder.yxz, positivesSetup.nnp): return passedInEntryList.OrderByDescending(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.x).ThenBy(entry => entry.myVector.z).ToList();  
            case (positionalOrder.yxz, positivesSetup.npn): return passedInEntryList.OrderByDescending(entry => entry.myVector.y).ThenBy(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.z).ToList();  
            case (positionalOrder.yxz, positivesSetup.pnn): return passedInEntryList.OrderBy(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.z).ToList();  
            case (positionalOrder.yxz, positivesSetup.nnn): return passedInEntryList.OrderByDescending(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.z).ToList();  

            //yzx
            case (positionalOrder.yzx, positivesSetup.ppp): return passedInEntryList.OrderBy(entry => entry.myVector.y).ThenBy(entry => entry.myVector.z).ThenBy(entry => entry.myVector.x).ToList();  
            case (positionalOrder.yzx, positivesSetup.ppn): return passedInEntryList.OrderBy(entry => entry.myVector.y).ThenBy(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.x).ToList();  
            case (positionalOrder.yzx, positivesSetup.pnp): return passedInEntryList.OrderBy(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.z).ThenBy(entry => entry.myVector.x).ToList();  
            case (positionalOrder.yzx, positivesSetup.npp): return passedInEntryList.OrderByDescending(entry => entry.myVector.y).ThenBy(entry => entry.myVector.z).ThenBy(entry => entry.myVector.x).ToList();  
            case (positionalOrder.yzx, positivesSetup.nnp): return passedInEntryList.OrderByDescending(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.z).ThenBy(entry => entry.myVector.x).ToList();  
            case (positionalOrder.yzx, positivesSetup.npn): return passedInEntryList.OrderByDescending(entry => entry.myVector.y).ThenBy(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.x).ToList();  
            case (positionalOrder.yzx, positivesSetup.pnn): return passedInEntryList.OrderBy(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.x).ToList();  
            case (positionalOrder.yzx, positivesSetup.nnn): return passedInEntryList.OrderByDescending(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.x).ToList();  

            //zxy    
            case (positionalOrder.zxy, positivesSetup.ppp): return passedInEntryList.OrderBy(entry => entry.myVector.z).ThenBy(entry => entry.myVector.x).ThenBy(entry => entry.myVector.y).ToList();  
            case (positionalOrder.zxy, positivesSetup.ppn): return passedInEntryList.OrderBy(entry => entry.myVector.z).ThenBy(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.y).ToList();  
            case (positionalOrder.zxy, positivesSetup.pnp): return passedInEntryList.OrderBy(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.x).ThenBy(entry => entry.myVector.y).ToList();  
            case (positionalOrder.zxy, positivesSetup.npp): return passedInEntryList.OrderByDescending(entry => entry.myVector.z).ThenBy(entry => entry.myVector.x).ThenBy(entry => entry.myVector.y).ToList();  
            case (positionalOrder.zxy, positivesSetup.nnp): return passedInEntryList.OrderByDescending(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.x).ThenBy(entry => entry.myVector.y).ToList();  
            case (positionalOrder.zxy, positivesSetup.npn): return passedInEntryList.OrderByDescending(entry => entry.myVector.z).ThenBy(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.y).ToList();  
            case (positionalOrder.zxy, positivesSetup.pnn): return passedInEntryList.OrderBy(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.y).ToList();  
            case (positionalOrder.zxy, positivesSetup.nnn): return passedInEntryList.OrderByDescending(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.x).ThenByDescending(entry => entry.myVector.y).ToList();  

            //zyx
            case (positionalOrder.zyx, positivesSetup.ppp): return passedInEntryList.OrderBy(entry => entry.myVector.z).ThenBy(entry => entry.myVector.y).ThenBy(entry => entry.myVector.x).ToList();  
            case (positionalOrder.zyx, positivesSetup.ppn): return passedInEntryList.OrderBy(entry => entry.myVector.z).ThenBy(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.x).ToList();  
            case (positionalOrder.zyx, positivesSetup.pnp): return passedInEntryList.OrderBy(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.y).ThenBy(entry => entry.myVector.x).ToList();  
            case (positionalOrder.zyx, positivesSetup.npp): return passedInEntryList.OrderByDescending(entry => entry.myVector.z).ThenBy(entry => entry.myVector.y).ThenBy(entry => entry.myVector.x).ToList();  
            case (positionalOrder.zyx, positivesSetup.nnp): return passedInEntryList.OrderByDescending(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.y).ThenBy(entry => entry.myVector.x).ToList();  
            case (positionalOrder.zyx, positivesSetup.npn): return passedInEntryList.OrderByDescending(entry => entry.myVector.z).ThenBy(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.x).ToList();  
            case (positionalOrder.zyx, positivesSetup.pnn): return passedInEntryList.OrderBy(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.x).ToList();  
            case (positionalOrder.zyx, positivesSetup.nnn): return passedInEntryList.OrderByDescending(entry => entry.myVector.z).ThenByDescending(entry => entry.myVector.y).ThenByDescending(entry => entry.myVector.x).ToList();  

        }

        Debug.LogError("Should have returned at this point!");
        return null;

    }
}
