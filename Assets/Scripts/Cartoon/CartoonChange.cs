using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CartoonChange : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public Sprite[] Cartoon= new Sprite[7];
    public int Count = 1;
    public float timer;
    public int target;
    public bool is_end;
    void start_cartoon()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse0) && timer <= 0)
        {
            timer = 1.5f;
            image2.color = new Color(image1.color.r, image1.color.g, image1.color.b, 1);
            image2.color = new Color(image1.color.r, image1.color.g, image1.color.b, 0);

            image1.sprite = image2.sprite;
            if (Count >= target && !is_end)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            if (Count >= target && is_end)
                return;
            else
            {
                image2.sprite = Cartoon[Count];
                StartCoroutine(Change());
                Count++;
            }
        }
        
    }
    IEnumerator Change()
    {
        for (float i = 0; i < 1; i = i + 0.005f)
        {
            image2.color = new Color(image2.color.r, image2.color.g, image2.color.b, i);
            yield return null;
        }
    }

    void FixedUpdate()
    {
        start_cartoon();
    }
}
