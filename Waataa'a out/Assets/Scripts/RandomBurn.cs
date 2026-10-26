using System;
using System.Collections.Generic;
using UnityEngine;


public class RandomBurn : MonoBehaviour
{
    public GameObject ObjetParent;

    public GameObject[] ChildrenObject;

    public int quemar;

    private void Awake()
    {
        CheckChildren();
        
    }

    void Start()
    {
        int quemar = UnityEngine.Random.Range(0, ChildrenObject.Length);
        Debug.Log("Objeto quemado:" + ChildrenObject[quemar]);
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

    public void RandomFire()
    {

    }

}
