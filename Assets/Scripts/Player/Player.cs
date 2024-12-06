using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator animator;
    //��ŵ�Ԥ����
    public GameObject skill_perfab;

    public PlayerAudio playerAudio;

    //��ȡ��������
    public float moveInput;
    //�Ƿ�j������
    public bool isInteract;
    //�Ƿ���Ծ��
    public bool isJump;
    //�Ƿ�ʼʹ�ü���
    public bool skill_Start;
    //����¼���Ƿ�ʼ
    public bool skill_RecordStart;
    //������ȴʱ��
    public float skill_Timer;
    //�Ƿ����ɵĴ�ʱ��Ԥ����
    public bool skill_hasPerfab;
    //��¼���λ�����ڻط�
    public Vector3 Record_Position;

    //�Ƿ�����ƶ�
    public float speed = 2f;
    public bool canMove = true;
    public bool faceRight = true;
    public bool isGround = false;
    //�������λ
    public Transform groundCheck;
    //�Ƿ��ڵ���
    public LayerMask whatIsGround;
    //������뾶
    public float groundCheckRadius = 0.2f;
    //��Ծ����
    public float jumpForce = 5f;
    public Object other;
    //public float Now_Time;


   
    //����Ħ���������л�
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
        //�������
        moveInput = InputManager.GethorizontalInput();
        isInteract = InputManager.GetJInput();
        isJump = InputManager.GetJumpInput();

        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        Jump();

        Skill();

        //�Ƿ��ڵ�������Ħ���������л�
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
        // ��鼼�ܼ�ʱ���Ƿ����0������ǣ���ÿ֡���ټ�ʱ����ֵ�����ڼ�����ȴ
        if (skill_Timer > 0)
            skill_Timer -= Time.deltaTime;

        // ������X��������other����Ϊnull����ʾ��ǰû�м���ʵ�����������ܼ�ʱ��С�ڵ���0���Ҽ���¼��δ��ʼ
        if (Input.GetKeyDown(KeyCode.X) && other == null && skill_Timer <= 0 && !skill_RecordStart)
        {
            // �ҵ�������SkillCountdown��������ã���������Ŀ��Ϊ��ǰ��ҵ���Ϸ����
            FindObjectOfType<SkillCountdown>().Target_player = gameObject;
            // ���ü���¼�ƿ�ʼ�ı�־Ϊtrue
            skill_RecordStart = true;
            // ��¼��ǰ��ҵ�λ�ã��������ڼ��ܻط�
            Record_Position = transform.position;
            // ʵ��������Ԥ��������ҵ�ǰλ�ã���������Ч��
            other = Instantiate(skill_perfab, transform.position, Quaternion.identity);
            // ���´����ļ���ʵ������
            other.name = "player_skill_perfab";
        }
        //// ������Z��������other������Ϊnull����ʾ��ǰ�м���ʵ�����������ܼ�ʱ��С�ڵ���0���Ҽ���¼���Ѿ���ʼ
        //else if (Input.GetKeyDown(KeyCode.Z) && other != null && skill_Timer <= 0 && skill_RecordStart)
        //{
        //    // �����ܵ���ʱ�ı�����ɫ��alphaֵ����Ϊ0��ʹ��͸�������ɼ���
        //    FindObjectOfType<SkillCountdown>().countdown.color = new Color(
        //        FindObjectOfType<SkillCountdown>().countdown.color.r,
        //        FindObjectOfType<SkillCountdown>().countdown.color.g,
        //        FindObjectOfType<SkillCountdown>().countdown.color.b, 0);
        //    // ���ü��ܵ���ʱ�ı��������С
        //    FindObjectOfType<SkillCountdown>().countdown.fontSize = 100;
        //    // �ӳ������ҵ�Countdown��������ã�����SkillCountdown��Now_Time��������Ϊ��Now_Time���Ե�ֵ
        //    FindObjectOfType<SkillCountdown>().Now_Time = FindObjectOfType<Countdown>().Now_Time;
        //    // ���ü��ܵ���ʱ�ı���λ��
        //    FindObjectOfType<SkillCountdown>().gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -95, 0);
        //    // ���ü��ܼ�ʱ����ֵΪ0.2��
        //    skill_Timer = 0.2f;
        //    // ���ü���¼�ƿ�ʼ�ı�־Ϊfalse����ʾ����¼�ƽ���
        //    skill_RecordStart = false;
        //    // ����Э�̣�����PlayerSkill�����Delete������ɾ������ʵ��
        //    StartCoroutine(other.GetComponent<PlayerSkill>().Delete());
        //    // �˳�����������ִ������Ĵ���
        //    return;
        //}
        // ���ٴΰ���X��������other������Ϊnull����ʾ��ǰ�м���ʵ�����������ܼ�ʱ��С�ڵ���0���Ҽ���¼���Ѿ���ʼ
        else if (Input.GetKeyDown(KeyCode.X) && other != null && skill_Timer <= 0 && skill_RecordStart)
        {
            // �ҵ�������SkillCountdown��������ã���������Ŀ��ΪPlayerSkill������ڵ���Ϸ����
            FindObjectOfType<SkillCountdown>().Target_player = FindObjectOfType<PlayerSkill>().gameObject;
            // ���ü��ܵ���ʱ�ı�������
            FindObjectOfType<SkillCountdown>().countdown.text = FindObjectOfType<Countdown>().countdown.text;
            // �ӳ������ҵ�Countdown��������ã�����SkillCountdown��Now_Time��������Ϊ��Now_Time���Ե�ֵ
            FindObjectOfType<SkillCountdown>().Now_Time = FindObjectOfType<Countdown>().Now_Time;
            // ���ü��ܼ�ʱ����ֵΪ0.2��
            skill_Timer = 0.2f;
            // ���ü���¼�ƿ�ʼ�ı�־Ϊfalse����ʾ����¼�ƽ���
            skill_RecordStart = false;
            // ���ü��������ı�־Ϊtrue���������ڴ�������Ч��
            skill_Start = true;
            // ����ҵ�λ������Ϊ��¼��λ�ã����ڼ��ܻط�
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
       
        //����ڵ����ϣ�������Ծ������Ҫ������Ⲣ���ǵ����Ծ�������ڵ����ϲ�����Ծ�����뿪�������Զ��ж϶�������״̬
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

            //ֱ�����������ж�
            animator.SetFloat("Movement", Mathf.Abs(rb.velocity.x));

            playerAudio.Play(PlayerAudio.AudioType.Walk, true);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            
        }

        //������������ٶ�
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
