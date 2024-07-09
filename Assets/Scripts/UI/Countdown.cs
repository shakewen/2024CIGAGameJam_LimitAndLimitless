using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Countdown : MonoBehaviour
{
    public float Now_Time = 60;
    public int int_Now_Time;
    public TMP_Text countdown;
    public Player player;
    public Vector3 player_position;
    public Vector3 player_screen_position;
    // Start is called before the first frame update
    void Start()
    {
        countdown = GetComponent<TMP_Text>();
        countdown.text = "01:00";
        player = FindObjectOfType<Player>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!player.skill_RecordStart)
        {
            Now_Time -= Time.deltaTime;
            int_Now_Time = (int)Now_Time;
            if (int_Now_Time >= 10)
                countdown.text = "00:" + int_Now_Time.ToString();
            else if (int_Now_Time >= 0 && int_Now_Time < 10)
            {
                countdown.color = Color.red;
                countdown.text = "00:0" + int_Now_Time.ToString();
            }
        }
        if (int_Now_Time <= 0)
            SceneManager.LoadScene(2);
    }
}
