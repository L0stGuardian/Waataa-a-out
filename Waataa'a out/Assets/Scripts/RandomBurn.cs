using System;
using System.Collections.Generic;
using UnityEngine;


public class RandomBurn : MonoBehaviour
{
    public GameObject ObjetParent;

    public GameObject[] ChildrenObject;

    [SerializeField]private int _maxOfBurned;
    

    private void Awake()
    {
        CheckChildren();
        
    }

    void Start()
    {
        
        int burnedObjects = UnityEngine.Random.Range(1, _maxOfBurned);
        for (int i = 0; i <= burnedObjects; i++)
        {
            int burn = UnityEngine.Random.Range(0, ChildrenObject.Length);
            GameObject ObjectToBurn = ChildrenObject[burn];
            ObjectToBurn.GetComponent<IIgnitable>().ChangeStatus("onfire");
            Debug.Log("Objeto quemado: "+ ChildrenObject[burn]);
        }
        

        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckChildren()
    {
        ChildrenObject = new GameObject[ObjetParent.transform.childCount];

        for (int i = 0; i < ChildrenObject.Length; i++)
        {
            ChildrenObject[i] = ObjetParent.transform.GetChild(i).gameObject;
        }
    }

}
