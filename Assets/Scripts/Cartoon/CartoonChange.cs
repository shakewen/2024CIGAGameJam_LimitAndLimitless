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
            //����Ϊ͸��
            AlphaValue = 0;
            //��ʼ����
            isFadeColor = true;
            //��ֹ���
            canClick = false;
          }

        if (isFadeColor)
        {
            AlphaValue += Time.deltaTime * fadeSpeed;
            CartoonImage2.color = new Color(CartoonImage2.color.r, CartoonImage2.color.g, CartoonImage2.color.b, AlphaValue);

            //���Ҫ���õ���ʱ�����õ�������������һ����ʱ������
            if (CartoonImage2.color.a >= 1)//���ͼƬ��ȫ��ʾ
            {
                //�˳�����
                isFadeColor = false;
                //������
                canClick = true;
            }
        }
      
       
        
    }
   

    void Update()
    {
        start_cartoon();
    }
}
