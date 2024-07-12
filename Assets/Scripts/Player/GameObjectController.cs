using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GameObjectController : MonoBehaviour
{



    public GameObject controller;

    //移动的木板
    public GameObject Floor;

    //移动的合体墙壁
    public GameObject CombineWall;

    public Transform FloorStartPoint;

    public Transform FloorEndPoint;

    public Transform WallStartPoint;

    public Transform WallEndPoint;

    public List<GameObject> interactGameObject;



    private bool isPressedState = false;

    private bool isColorState = false;

    private bool isMoveFloor = false;

   // private bool isRobing = false;

    private bool isMoveWall = false;

    public Dictionary<GameObject, bool> isRobingState = new Dictionary<GameObject, bool>();


    [SerializeField] Sprite[] sprites;



    void Start()
    {
        
        controller = GameObject.Find("BookShelf");


        Floor = controller.transform.Find("MoveFloor").gameObject;

        FloorStartPoint = controller.transform.Find("FloorStart").transform;
        FloorEndPoint = controller.transform.Find("FloorEnd").transform;
        WallStartPoint = controller.transform.Find("WallStart").transform;
        WallEndPoint = controller.transform.Find("WallEnd").transform;



        //FloorStartPoint.parent = null;
        //FloorEndPoint.parent = null;



        CombineWall = controller.transform.Find("MovingWall").gameObject;

       
        




        for (int i = 0; i < controller.transform.childCount; i++)
        {
            interactGameObject.Add(controller.transform.GetChild(i).gameObject);
           
        }

       //foreach进行初始设置
        foreach (GameObject obj in interactGameObject)
        {
            if (obj.CompareTag("InteractButton"))
            {
                if (obj.name == "InteractButton_Red")
                {
                    obj.transform.Find("OpenDoor").gameObject.SetActive(false);
                }

            }

            if (obj.CompareTag("RodItem"))
            {
                isRobingState.Add(obj, false);
                if (obj.name == "Rod_Red_Map2")
                {
                    obj.transform.GetChild(0).gameObject.SetActive(false);
                }

                if(obj.name == "Rod_Yellow_Map3")
                {
                    obj.transform.GetChild(0).gameObject.SetActive(false);
                }
            }


        }
    }

     void FixedUpdate()
    {

        
            if (Floor != null)
            {
                

                if (isMoveFloor)
                {
                    if(Floor.transform.position != FloorEndPoint.position)
                    Floor.transform.position = Vector2.MoveTowards(Floor.transform.position, FloorEndPoint.position, Time.deltaTime * 1.5f);
                }
                else
                {
                    if (Floor.transform.position != FloorStartPoint.position)
                    Floor.transform.position = Vector2.MoveTowards(Floor.transform.position, FloorStartPoint.position, Time.deltaTime * 1.5f);
                }
            }
        

        
            if (CombineWall != null)
            {
               
               if (isMoveWall)
               {

                if (CombineWall.transform.position != WallEndPoint.position)
                
                    CombineWall.transform.position = Vector2.MoveTowards(CombineWall.transform.position, WallEndPoint.position, Time.deltaTime * 3f);
               
                   
                 
                
                    
               }
               else
               {
                 
                  if(CombineWall.transform.position != WallStartPoint.position)
                  
                    CombineWall.transform.position = Vector2.MoveTowards(CombineWall.transform.position, WallStartPoint.position, Time.deltaTime * 3f);
                  
                    
                 
                   
               }
            }
        
    }

     void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckInteract(collision);

        if (collision.gameObject.tag == "RodItem")
        {
           // isRobing = !isRobing;
            //切换图片


            
            if (collision.name == "Rod_Red_Map2")
            {
                isRobingState[collision.gameObject] =!isRobingState[collision.gameObject];
                //开玻璃门让主角进入下一个房间
                collision.transform.GetChild(0).gameObject.SetActive( isRobingState[collision.gameObject]? true : false);
                collision.GetComponent<SpriteRenderer>().sprite = isRobingState[collision.gameObject] ? sprites[0] : sprites[1];
                collision.transform.GetChild(1).gameObject.GetComponent<Collider2D>().isTrigger = isRobingState[collision.gameObject] ? true : false;

            }

            //黄色拉杆解救动物
            if (collision.name == "Rod_Yellow_Map3")
            {
                isRobingState[collision.gameObject] = !isRobingState[collision.gameObject];
                //开玻璃门解救动物
                collision.transform.GetChild(0).gameObject.SetActive(isRobingState[collision.gameObject] ? true : false);
                collision.GetComponent<SpriteRenderer>().sprite = isRobingState[collision.gameObject] ? sprites[2] : sprites[3];
                collision.transform.GetChild(1).gameObject.GetComponent<Collider2D>().isTrigger = isRobingState[collision.gameObject] ? true : false;
            }

            if (collision.name == "Rod_Green_Map3")
            {
                isRobingState[collision.gameObject] = !isRobingState[collision.gameObject];
                //触发拉杆后板子消失，再次拉杆板子出现
                collision.transform.GetChild(0).gameObject.SetActive(isRobingState[collision.gameObject] ? false : true);
                collision.GetComponent<SpriteRenderer>().sprite = isRobingState[collision.gameObject] ? sprites[0] : sprites[1];
            }

            //白色拉杆移动墙壁
            if (collision.name == "Rod_While_Map3")
            {
                isRobingState[collision.gameObject] = !isRobingState[collision.gameObject];
                //使用墙壁移动
                isMoveWall = !isMoveWall;
                collision.GetComponent<SpriteRenderer>().sprite = isRobingState[collision.gameObject] ? sprites[4] : sprites[5];
            }


        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        CheckInteract(collision);

        
    }

 
    void CheckInteract(Collider2D collision)
    {
        
            if (collision.gameObject.tag=="InteractButton")
            {
                isPressedState = !isPressedState;

                collision.GetComponent<Animator>().SetBool("isPressed", isPressedState);

           
                    if (collision.name == "InteractButton_Red")
                    {
                       //开关门
                        collision.transform.Find("OpenDoor").gameObject.SetActive(isPressedState?true : false);
                        collision.transform.Find("Door").gameObject.SetActive(isPressedState? false : true);
                    }

                    if (collision.name == "InteractButton_Orange")
                    {
                        //使木板消失出现
                        collision.transform.Find("floor").gameObject.SetActive(false);
                    }

                    if(collision.name == "InteractButton_Pink")
                    {
                        
                        //木板移动
                       isMoveFloor = false;

                        
                    }

                    if(collision.name == "InteractButton_Purple")
                    {
                        //木板反向移动
                        isMoveFloor = true;
                        
                    }
                

                    
               
            
            }
            //透明化
        if (collision.gameObject.tag == "Transparency")
        {
            isColorState =!isColorState;

            collision.GetComponent<SpriteRenderer>().color=new Color(1,1,1,isColorState?0.65f:1);
        }
       

       
        
    }


    void CheckRod(Collider2D collision)
    {
       
    }
}
