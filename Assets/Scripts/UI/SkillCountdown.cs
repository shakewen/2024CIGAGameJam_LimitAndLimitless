using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillCountdown : MonoBehaviour
{
    public float Now_Time = 60;
    public int int_Now_Time;
    public TMP_Text countdown;
    public Player player;
    public Vector3 player_position;
    public Vector3 player_screen_position;
    public GameObject Target_player;
    public Camera camera;
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
        if (player.other != null)
        {
            player_position = new Vector3(Target_player.transform.position.x, Target_player.transform.position.y + 1);
            player_screen_position = camera.WorldToScreenPoint(player_position);

            countdown.color = new Color(countdown.color.r, countdown.color.g, countdown.color.b, 0.4f);
            if (Vector3.Distance(transform.position, player_screen_position) > 50f)
                transform.position = Vector3.Lerp(transform.position, player_screen_position, 10 * Time.deltaTime);
            else
                transform.position = Vector3.Lerp(transform.position, player_screen_position, 40 * Time.deltaTime);
            if (player.skill_RecordStart)
            {
                countdown.fontSize = Mathf.Lerp(countdown.fontSize, 50, 10 * Time.deltaTime);
            }
        }
        if (!player.skill_RecordStart)
        {

            Now_Time = FindObjectOfType<Countdown>().Now_Time;
            countdown.text = FindObjectOfType<Countdown>().countdown.text;
        }
        else
        {
            Now_Time -= Time.deltaTime;
            int_Now_Time = (int)Mathf.Clamp(Now_Time, 0, 60);
            if (int_Now_Time >= 10)
                countdown.text = "00:" + int_Now_Time.ToString();
            else if (int_Now_Time >= 0 && int_Now_Time < 10)
            {
                countdown.color = Color.red;
                countdown.text = "00:0" + int_Now_Time.ToString();
            }
        }
    }
}
