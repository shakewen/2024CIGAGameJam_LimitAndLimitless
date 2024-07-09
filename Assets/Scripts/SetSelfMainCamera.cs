using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSelfMainCamera : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FindObjectOfType<ChangeSceen>().Screens = gameObject;
        }
    }
}

