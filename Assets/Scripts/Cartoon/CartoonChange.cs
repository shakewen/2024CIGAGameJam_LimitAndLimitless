using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CartoonChange : MonoBehaviour
{
    public Image CartoonImage;
    public Image CartoonImage2;
    public Sprite[] Cartoon= new Sprite[7];
    public int Count = 0;

    public bool canClick = true;

    public bool isFadeColor = false;

    private float AlphaValue = 0;

    public float fadeSpeed = 1f;


    private void Start()
    {
        CartoonImage =transform.Find("Play_Cartoon_BackGround").GetComponent<Image>();

        CartoonImage2 =transform.Find("Play_Cartoon").GetComponent<Image>();


        CartoonImage2.color = new Color(1, 1, 1, 0);
        AlphaValue =Mathf.Clamp(CartoonImage2.color.a, 0, 1);
    }
    void start_cartoon()
    {
        

          if (Input.GetMouseButtonDown(0)&&canClick)
          {
            
            Count++;

            CartoonImage.sprite =Cartoon[Count-1];

            CartoonImage2.sprite = Cartoon[Count];
            //设置为透明
            AlphaValue = 0;
            //开始淡入
            isFadeColor = true;
            //禁止点击
            canClick = false;
          }

        if (isFadeColor)
        {
            AlphaValue += Time.deltaTime * fadeSpeed;
            CartoonImage2.color = new Color(CartoonImage2.color.r, CartoonImage2.color.g, CartoonImage2.color.b, AlphaValue);

            //如果要设置单次时间设置的再在这里设置一个计时器即可
            if (CartoonImage2.color.a >= 1)//如果图片完全显示
            {
                //退出渐变
                isFadeColor = false;
                //允许点击
                canClick = true;
            }
        }
      
       
        
    }
   

    void Update()
    {
        start_cartoon();
    }
}
