using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CartoonEnd : MonoBehaviour
{
    public Image CartoonImage;
    public Image CartoonImage2;
    public Sprite[] Cartoon;
    public Sprite[] cartoonAnimation;
    public int Count = 0;
    public int animationCount = 0;
    public bool canClick = true;

    public bool isFadeColor = false;

    private float AlphaValue = 0;

    public float fadeSpeed = 1f;

    public float animationFallTime = 0.25f;

    private void Start()
    {
        CartoonImage = transform.GetChild(0).GetComponent<Image>();

        CartoonImage2 = transform.GetChild(1).GetComponent<Image>();

        transform.GetChild(2).gameObject.SetActive(false);


        CartoonImage2.color = new Color(1, 1, 1, 0);
        AlphaValue = Mathf.Clamp(CartoonImage2.color.a, 0, 1);
    }
    void start_cartoon()
    {


        if (Input.GetMouseButtonDown(0) && canClick)
        {

            Count++;

           

            if (Count ==5)
            {
                
                StartCoroutine(LoadScene());

                //防止协程多开
                canClick = false;
            }

            else
            {
                CartoonImage.sprite = Cartoon[Count - 1];

                CartoonImage2.sprite = Cartoon[Count];
                //设置为透明
                AlphaValue = 0;
                //开始淡入
                isFadeColor = true;
                //禁止点击
                canClick = false;
                print(Count);
                if (Count == Cartoon.Length-1)
                {
                    
                    //显示结束游戏按钮,
                    StartCoroutine(GameEnd());

                }
            }

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

                //如果是最后一张图片,则禁止点击
               if(Count == Cartoon.Length - 1)
                {
                    canClick = false;
                }
                else
                {
                    //允许点击
                    canClick = true;
                }
               
            }
        }



    }


    void Update()
    {
        start_cartoon();
    }

    IEnumerator LoadScene()
    {
        while (true)
        {
            animationCount++;
            CartoonImage2.color = new Color(CartoonImage2.color.r, CartoonImage2.color.g, CartoonImage2.color.b, 1);
            if (animationCount >= cartoonAnimation.Length-1)
            {
                yield return new WaitForSeconds(animationFallTime);
                CartoonImage2.sprite = cartoonAnimation[animationCount];

                canClick = true;
                break;
            }
            else
            {

                CartoonImage.sprite = cartoonAnimation[animationCount - 1];
                yield return new WaitForSeconds(0.1f);
                

                CartoonImage2.sprite = cartoonAnimation[animationCount];
               
            }
            


        }

    }

    IEnumerator GameEnd()
    {

        //canClick = false;
        yield return new WaitForSeconds(2f);

         transform.GetChild(2).gameObject.SetActive(true);
            
    }
}
