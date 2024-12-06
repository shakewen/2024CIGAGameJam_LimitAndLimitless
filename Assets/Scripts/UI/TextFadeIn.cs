using DG.Tweening;
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
        WorldCanvas = GameObject.FindGameObjectWithTag("WorldCanvas");
        
        for (int i = 0; i < WorldCanvas.transform.childCount; i++)
        {
            TMP_Text text = WorldCanvas.transform.GetChild(i).GetComponent<TMP_Text>();
            text.DOFade(0, 0).SetEase(Ease.OutExpo);
            texts.Add(text);
            whatTextFadeIn[text] = false;
            
        }
       
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
       

        if (texts != null)
        {
            foreach (TMP_Text t in texts)
            {
                if (t != null && t.name == collider.name)
                {

                    currentText = t;
                    whatTextFadeIn[currentText] = true;

                    //防止连开
                    currentText.DOKill();
                    currentText.DOFade(1, 0.5f).SetEase(Ease.InOutQuart);
                }
            }
        }

        
    }
    void OnTriggerExit2D(Collider2D collider)
    {


        if (texts != null)
        {
            foreach (TMP_Text t in texts)
            {
                if (t!=null&&t.name == collider.name)
                {
                    currentText = t;
                    whatTextFadeIn[currentText] = false;

                    //防止连开
                    currentText.DOKill();
                    currentText.DOFade(0, 0.5f).SetEase(Ease.InOutQuart);

                }
            }
        }

        
    }

    private void OnDestroy()
    {
        currentText.DOKill();

        texts.Clear();
        // 清理 whatTextFadeIn 和 whatTextStartFade 字典中的对象
        whatTextFadeIn.Clear();
        whatTextStartFade.Clear();
    }
    

}
