using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator;
    //存放的预制体
    public GameObject skill_perfab;

    public PlayerAudio playerAudio;

    //获取按键输入
    public float moveInput;
    //是否j键交互
    public bool isInteract;
    //是否跳跃，
    public bool isJump;
    //是否开始使用技能
    public bool skill_Start;
    //技能录制是否开始
    public bool skill_RecordStart;
    //技能冷却时间
    public float skill_Timer;
    //是否生成的次时空预制体
    public bool skill_hasPerfab;
    //记录玩家位置用于回放
    public Vector3 Record_Position;

    //是否可以移动
    public float speed = 2f;
    public bool canMove = true;
    public bool faceRight = true;
    public bool isGround = false;
    //地面检查点位
    public Transform groundCheck;
    //是否处于地面
    public LayerMask whatIsGround;
    //地面检查半径
    public float groundCheckRadius = 0.2f;
    //跳跃力度
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
        //检查输入
        moveInput = InputManager.GethorizontalInput();
        isInteract = InputManager.GetJInput();
        isJump = InputManager.GetJumpInput();

        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        Jump();

        Skill();

        //是否处于地面上且摩擦力材质切换
        if (isGround)
        {
            gameObject.GetComponent<Collider2D>().sharedMaterial = playerMaterials2D[1];
        }
        else
        {
            gameObject.GetComponent<Collider2D>().sharedMaterial = playerMaterials2D[0];
        }
    }
    void FixedUpdate()
    {
        

       

        MoveToFlip();

        Move();
        
    }
    void Skill()
    {
        // 检查技能计时器是否大于0，如果是，则每帧减少计时器的值，用于技能冷却
        if (skill_Timer > 0)
            skill_Timer -= Time.deltaTime;

        // 当按下X键，并且other变量为null（表示当前没有技能实例化），技能计时器小于等于0，且技能录制未开始
        if (Input.GetKeyDown(KeyCode.X) && other == null && skill_Timer <= 0 && !skill_RecordStart)
        {
            // 找到场景中SkillCountdown组件的引用，并设置其目标为当前玩家的游戏对象
            FindObjectOfType<SkillCountdown>().Target_player = gameObject;
            // 设置技能录制开始的标志为true
            skill_RecordStart = true;
            // 记录当前玩家的位置，可能用于技能回放
            Record_Position = transform.position;
            // 实例化技能预制体在玩家当前位置，创建技能效果
            other = Instantiate(skill_perfab, transform.position, Quaternion.identity);
            // 给新创建的技能实例命名
            other.name = "player_skill_perfab";
        }
        //// 当按下Z键，并且other变量不为null（表示当前有技能实例化），技能计时器小于等于0，且技能录制已经开始
        //else if (Input.GetKeyDown(KeyCode.Z) && other != null && skill_Timer <= 0 && skill_RecordStart)
        //{
        //    // 将技能倒计时文本的颜色的alpha值设置为0，使其透明（不可见）
        //    FindObjectOfType<SkillCountdown>().countdown.color = new Color(
        //        FindObjectOfType<SkillCountdown>().countdown.color.r,
        //        FindObjectOfType<SkillCountdown>().countdown.color.g,
        //        FindObjectOfType<SkillCountdown>().countdown.color.b, 0);
        //    // 设置技能倒计时文本的字体大小
        //    FindObjectOfType<SkillCountdown>().countdown.fontSize = 100;
        //    // 从场景中找到Countdown组件的引用，并将SkillCountdown的Now_Time属性设置为其Now_Time属性的值
        //    FindObjectOfType<SkillCountdown>().Now_Time = FindObjectOfType<Countdown>().Now_Time;
        //    // 设置技能倒计时文本的位置
        //    FindObjectOfType<SkillCountdown>().gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -95, 0);
        //    // 重置技能计时器的值为0.2秒
        //    skill_Timer = 0.2f;
        //    // 设置技能录制开始的标志为false，表示技能录制结束
        //    skill_RecordStart = false;
        //    // 启动协程，调用PlayerSkill组件的Delete方法来删除技能实例
        //    StartCoroutine(other.GetComponent<PlayerSkill>().Delete());
        //    // 退出方法，不再执行下面的代码
        //    return;
        //}
        // 当再次按下X键，并且other变量不为null（表示当前有技能实例化），技能计时器小于等于0，且技能录制已经开始
        else if (Input.GetKeyDown(KeyCode.X) && other != null && skill_Timer <= 0 && skill_RecordStart)
        {
            // 找到场景中SkillCountdown组件的引用，并设置其目标为PlayerSkill组件所在的游戏对象
            FindObjectOfType<SkillCountdown>().Target_player = FindObjectOfType<PlayerSkill>().gameObject;
            // 设置技能倒计时文本的内容
            FindObjectOfType<SkillCountdown>().countdown.text = FindObjectOfType<Countdown>().countdown.text;
            // 从场景中找到Countdown组件的引用，并将SkillCountdown的Now_Time属性设置为其Now_Time属性的值
            FindObjectOfType<SkillCountdown>().Now_Time = FindObjectOfType<Countdown>().Now_Time;
            // 重置技能计时器的值为0.2秒
            skill_Timer = 0.2f;
            // 设置技能录制开始的标志为false，表示技能录制结束
            skill_RecordStart = false;
            // 设置技能启动的标志为true，可能用于触发技能效果
            skill_Start = true;
            // 将玩家的位置重置为记录的位置，用于技能回放
            transform.position = Record_Position;
        }
    }

    void Jump() 
    {
        
        if (isJump && isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            playerAudio.Play(PlayerAudio.AudioType.Jump,true);

            
        }
       
        //如果在地面上，则不再跳跃，动画要持续检测并不是点击跳跃键后且在地面上才能跳跃，当离开地面后会自动判断动画机的状态
        if (isGround)
        {
            animator.SetBool("Jump", false);
        }
        else
        {
            animator.SetBool("Jump", true);
        }

        float verticalVelocity =rb.velocity.y;
        animator.SetFloat("JumpState", verticalVelocity);

    }
    public void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

            //直接设置正则判定
            animator.SetFloat("Movement", Mathf.Abs(rb.velocity.x));

            playerAudio.Play(PlayerAudio.AudioType.Walk, true);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            
        }

        //限制最大下落速度
        if (rb.velocity.y < 0f)
        {
            float maxFallSpeed = -11.5f;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, maxFallSpeed));
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
