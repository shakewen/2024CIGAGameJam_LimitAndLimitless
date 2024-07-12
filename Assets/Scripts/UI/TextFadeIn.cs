using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFadeIn : MonoBehaviour
{
    public GameObject WorldCanvas;
    public List<TMP_Text> texts;
    public Dictionary<TMP_Text, bool> whatTextFadeIn = new Dictionary<TMP_Text, bool>();
    public Dictionary<TMP_Text, bool> whatTextStartFade = new Dictionary<TMP_Text, bool>();
    public TMP_Text currentText;
    public float Alaph;
    public bool canAddAlaph = false;
    public bool canFade =true;
    public float fadeSpeed = 1;
    void Start()
    {
        WorldCanvas = GameObject.Find("World Canvas");
        for (int i = 0; i < WorldCanvas.transform.childCount; i++)
        {
            TMP_Text text = WorldCanvas.transform.GetChild(i).GetComponent<TMP_Text>();
            texts.Add(text);
            whatTextFadeIn[text] = false;
        }
      
    }
    void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.transform.CompareTag("WorldCanvas"))
        {
            
            foreach (TMP_Text t in texts)
            {
                if (t.name == collider.name)
                {
                   
                    currentText = collider.GetComponent<TMP_Text>();
                    whatTextFadeIn[currentText] = true;
                }
            }

        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("WorldCanvas"))
        {

            foreach (TMP_Text t in texts)
            {
                if (t.name == collider.name)
                {
                    currentText = collider.GetComponent<TMP_Text>();
                    whatTextFadeIn[currentText] = false;


                }
            }

        }
    }
    void AddAlaph(bool whatToFade)
    {

           

           
           
            if (whatToFade)
                {
                    whatTextStartFade[currentText] = true;
                    if (whatTextStartFade[currentText])
                    {
                        Alaph += Time.deltaTime * fadeSpeed;
                        currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, Alaph);
                        if (currentText.color.a >= 1)
                        {
                            whatTextStartFade[currentText] = false;
                        }
                    }

                }
                else
                {

                    if (!whatTextStartFade[currentText])
                    {
                        Alaph -= Time.deltaTime * fadeSpeed;
                        currentText.color = new Color(currentText.color.r, currentText.color.g, currentText.color.b, Alaph);
                        if (currentText.color.a <= 0)
                        {
                            whatTextStartFade[currentText] = true;
                        }
                    }
                }
            
        
        

    }


    private void FixedUpdate()
    {
        if (currentText == null && whatTextFadeIn != null)
            return;

        AddAlaph(whatTextFadeIn[currentText]);
    }
}
