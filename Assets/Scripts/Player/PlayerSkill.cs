using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public Player player;
    public Vector2[] skill_position;
    public float[] anim_Movement;
    public bool[] anim_isGround;
    public bool[] anim_Jump;
    public Vector3[] skill_scale;
    private int recordCount = -1;

    void Record_Input()
    {

        if (player.skill_RecordStart)
        {        
            recordCount++;
            skill_position[recordCount] = player.transform.position;
            anim_Movement[recordCount] = player.animator.GetFloat("Movement");
            anim_isGround[recordCount] = player.animator.GetBool("isGround");
            anim_Jump[recordCount] = player.animator.GetBool("Jump");
            skill_scale[recordCount] = player.transform.localScale;
        }
    }
    void Start_Input()
    {
        if (player.skill_Start) 
        { 
            StartCoroutine(Play());
            player.skill_Start = false;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        Record_Input();
        
    }
    private void FixedUpdate()
    {
        Start_Input();
    }

    IEnumerator Play()
    {
        FindObjectOfType<SkillCountdown>().gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -95, 0);
        for (int i = 0; i < recordCount; i++)
        {
            transform.position = skill_position[i];
            animator.SetFloat("Movement", anim_Movement[i]);
            animator.SetBool("isGround", anim_isGround[i]);
            animator.SetBool("Jump", anim_Jump[i]);
            transform.localScale = skill_scale[i];
            yield return null;
        }
        StartCoroutine(Delete());
    }
    public IEnumerator Delete()
    {
        
        for (float i = 0.4f; i >0; i= i - 0.005f) 
        { 
            GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, i);
            FindObjectOfType<SkillCountdown>().countdown.color = new Color(FindObjectOfType<SkillCountdown>().countdown.color.r,
                                                               FindObjectOfType<SkillCountdown>().countdown.color.g,
                                                               FindObjectOfType<SkillCountdown>().countdown.color.b, i);
            yield return null;
        }
        FindObjectOfType<SkillCountdown>().countdown.fontSize = 100; 
        FindObjectOfType<SkillCountdown>().gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -95, 0);
        if (this.gameObject != null)
            Destroy(this.gameObject);
    }
}
