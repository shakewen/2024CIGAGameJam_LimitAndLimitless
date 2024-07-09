using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//将次脚本拖拽到Canvas下，然后再在UI读秒中进行设置隐形，如果时间为0秒则显示按钮，点击按钮退出游戏
public class UI_QuitApplication : MonoBehaviour
{

    
    public Button gameOverButton;



    void Start()
    {
        gameOverButton = transform.Find("GameQuitButton").GetComponent<Button>();
        gameOverButton.onClick.AddListener(Gameover);

    }

    public void Gameover()
    {
        Debug.Log("退出程序成功");
        Application.Quit();

        Resources.UnloadUnusedAssets();
    }
}
