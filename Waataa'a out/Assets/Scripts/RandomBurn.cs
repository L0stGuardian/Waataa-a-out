using System;
using System.Collections.Generic;
using UnityEngine;


public class RandomBurn : MonoBehaviour
{
    public GameObject ObjectParent; 

    public GameObject[] ChildrenObject;

    [SerializeField]private int _maximimAllowedOfBurnedObjects; 
    

    private void Awake()
    {
        CheckChildren();
        
    }

    void Start()
    {
        
        int burnedObjects = UnityEngine.Random.Range(1, _maximimAllowedOfBurnedObjects);
        for (int i = 0; i <= burnedObjects; i++)
        {
            int burn = UnityEngine.Random.Range(0, ChildrenObject.Length);
            GameObject objectToBurn = ChildrenObject[burn]; 
            objectToBurn.GetComponent<IIgnitable>().ChangeStatus("onfire");
            Debug.Log("Objeto quemado: "+ ChildrenObject[burn]);
        }
        

        
    }

  
    void Update()
    {

    }

    public void CheckChildren()
    {
        ChildrenObject = new GameObject[ObjectParent.transform.childCount];

        for (int i = 0; i < ChildrenObject.Length; i++)
        {
            ChildrenObject[i] = ObjectParent.transform.GetChild(i).gameObject;
        }
    }

}
