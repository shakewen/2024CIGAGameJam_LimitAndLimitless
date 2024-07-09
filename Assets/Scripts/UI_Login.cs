using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Login : MonoBehaviour
{
    public Button button;
    void Start()
    {
        button =GetComponent<Button>();
        button.onClick.AddListener(Login);
    }
    

   public void Login()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
