using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFadeIn : MonoBehaviour
{
    public TMP_Text text;
    public float Alaph;
    public bool CanAddAlaph;
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.transform.CompareTag("Player"))
        {
            CanAddAlaph = true;
        }
    }
    void AddAlaph()
    {
        if (CanAddAlaph && Alaph < 1) 
        {
            Alaph += 0.02f;
            text.color = new Color(text.color.r, text.color.g, text.color.b, Alaph);
        }
    }

    private void FixedUpdate()
    {
        AddAlaph();
    }
}
