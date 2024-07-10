using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPic : MonoBehaviour
{
    public Image image;
    public List<Sprite> sprites;
    public int Count = 0;
    private void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(Loop());
    }
    IEnumerator Loop() 
    {
        while (true)
        {
            image.sprite = sprites[Count];
            Count++;

            if (Count == 7)
                Count = 0;
            yield return new WaitForSeconds(0.08f);
        }
    }
   
}
