using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceen : MonoBehaviour
{
    public GameObject Screens;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Screens!=null)
        transform.position = new Vector3(Mathf.Lerp(transform.position.x, Screens.transform.position.x, 0.01f), 0, transform.position.z);
    }
}
