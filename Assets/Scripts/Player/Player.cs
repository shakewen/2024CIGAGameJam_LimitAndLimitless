using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator;
    public Object skill_perfab;
    public PlayerAudio playerAudio;

    //获取按键输入
    public float moveInput;
    //是否j键交互
    public bool isInteract;
    //是否跳跃，
    public bool isJump;

    public bool skill_Start;

    public bool skill_RecordStart;

    public float skill_Timer;
    //生成的次时空预制体
    public bool skill_hasPerfab;
    //记录玩家位置用于回放
    public Vector3 Record_Position;

    //是否可以移动
    public float speed = 2f;
    public bool canMove = true;
    public bool faceRight = true;
    public bool isGround = false;
    public Transform groundCheck;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;

    public float jumpForce = 5f;
    public Object other;
    //public float Now_Time;


   
    //主角摩擦力材质切换
    public PhysicsMaterial2D[] playerMaterials2D;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator =GetComponent<Animator>();
        groundCheck = transform.Find("GroundCheck");
        whatIsGround = LayerMask.GetMask("Ground");
        playerAudio = GetComponentInChildren<PlayerAudio>();
    }

    
    void Update()
    {

        moveInput = InputManager.GethorizontalInput();
        isInteract = InputManager.GetJInput();
        isJump = InputManager.GetJumpInput();
        Jump();
        Skill();

        if (isGround)
        {
            gameObject.GetComponent<BoxCollider2D>().sharedMaterial = playerMaterials2D[1];
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().sharedMaterial = playerMaterials2D[0];
        }
    }
    void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        animator.SetBool("isGround", isGround);
        MoveToFlip();
        Move();
        
    }
    void Skill()
    {
        if(skill_Timer > 0)
            skill_Timer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.X) && other == null  && skill_Timer <= 0 && !skill_RecordStart)
        {
            FindObjectOfType<SkillCountdown>().Target_player = gameObject;
            skill_RecordStart = true;
            //Now_Time = FindObjectOfType<Countdown>().Now_Time;
            Record_Position = transform.position;
            other = Instantiate(skill_perfab, transform.position, Quaternion.identity);
            other.name = "player_skill_perfab";
        }
        else if (Input.GetKeyDown(KeyCode.Z) && other != null && skill_Timer <=0 && skill_RecordStart)
        {
            FindObjectOfType<SkillCountdown>().countdown.color = new Color(FindObjectOfType<SkillCountdown>().countdown.color.r,
                                                                           FindObjectOfType<SkillCountdown>().countdown.color.g,
                                                                           FindObjectOfType<SkillCountdown>().countdown.color.b, 0);
            FindObjectOfType<SkillCountdown>().countdown.fontSize = 100;
            FindObjectOfType<SkillCountdown>().Now_Time = FindObjectOfType<Countdown>().Now_Time;
            FindObjectOfType<SkillCountdown>().gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -95, 0);
            skill_Timer = 0.2f;
            skill_RecordStart = false;
            StartCoroutine(other.GetComponent<PlayerSkill>().Delete());
            return;
        }
        else if (Input.GetKeyDown(KeyCode.X) && other != null && skill_Timer <=0 && skill_RecordStart)
        {
            FindObjectOfType<SkillCountdown>().Target_player = FindObjectOfType<PlayerSkill>().gameObject; 
            FindObjectOfType<SkillCountdown>().countdown.text = FindObjectOfType<Countdown>().countdown.text;
            FindObjectOfType<SkillCountdown>().Now_Time = FindObjectOfType<Countdown>().Now_Time;
            skill_Timer = 0.2f;
            skill_RecordStart = false;
            skill_Start = true;
            transform.position = Record_Position;
        }


    }

    void Jump() 
    {
        if (isJump && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            playerAudio.Play(PlayerAudio.AudioType.Jump,true);
            animator.SetBool("Jump",true);
           
        }
        else
        {
            animator.SetBool("Jump", false);
           
        }
    }
    public void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
            if (Mathf.Abs(moveInput) > 0f)
                animator.SetFloat("Movement", 1);
            else
                animator.SetFloat("Movement", 0);
            playerAudio.Play(PlayerAudio.AudioType.Walk, true);
        }
        else
        {
            rb.velocity = Vector2.zero;
            animator.SetFloat("Movement", 0);
        }
    }

    void MoveToFlip()
    {
        if (canMove)
        {
            if ((!faceRight && moveInput > 0f) || (faceRight && moveInput < 0f))
            {
                faceRight = !faceRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }
        }
    }
}
