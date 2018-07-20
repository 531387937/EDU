using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour {

    public AudioSource WalkA;
    public AudioSource JumpA;
    public AudioSource HomeA;
    public AudioSource CollectA;
    public AudioSource FlowerA;
    public AudioSource CaseA;

    public float moveSpeed = 1f;
    public float turnSpeed = 1f;
    public float offSet = 0.2f;
    static public int step;
    private bool clicked;
    public bool clear;
    private bool moved;
    private bool turned;
    private bool isMoving = false;
    private bool Fetched = false;

    private float turnedAngle = 0f;
    private bool recording = false;
    static public int actStep = -1;
   
    private bool queueFinished;

    private RaycastHit2D currentGrid;
    private RaycastHit2D nextGrid;
    [SerializeField]
    private Transform startGrid;
    [SerializeField]
    private Transform clearScreen;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    public Animator animC;
    [SerializeField]
    public Animator animP;
    [SerializeField]
    private GameObject Tar;

    private const int ignoreRayCast = 2;
    private const int accepetRayCast = 0;
    private int empty = 88;
    private int loopstart = 6;
    private int loopend = 7;
    bool beginloop = false;
    int loopstep = 0;
    int m = 0;
    public int[] times ;
    public int numloop;
    //private int error = 99;

    private bool bugged;
    private GridType nextGrid_GridType;
    private GridType currentGrid_GridType;

    private int Collect_num = 0;
    [SerializeField]
    int ToCollect;
    [SerializeField]
    GameObject Grid_ImageObj;

    //Could do an Error throw, break execution & highlight error

    static public float dice;
    static public int count;

    //static public RaycastHit2D uncertain;
    static public GameObject uncertain;
    static public GameObject parent;
    static public GameObject pro;
    static public GameObject col;

    private void Start()
    {
     
        transform.position = startGrid.position;
        transform.rotation = startGrid.rotation;

        Tar.transform.position = startGrid.position;
        Tar.transform.rotation = startGrid.rotation;

        bugged = false;
        queueFinished = false;
        recording = false;
        actStep = -1;
        loopstep = 0;
        m =0;
        beginloop = false;
        clear = false;
        step = 0;
        clicked = false;

        count = 0;

          numloop = 0;
        times = new int[10];
    }

    public void Act()
    {
        recording = true;
        bugged = false;
        //actStep = 0;

        if (queueFinished)
        {
            queueFinished = false;
        }
    }

    public void Return()
    {
        if(!recording)
        {
            transform.position = startGrid.position;
            transform.rotation = startGrid.rotation;
            Tar.transform.position = startGrid.position;
            Tar.transform.rotation = startGrid.rotation;
        }        
    }

    public void Clear()
    {
        if(!recording)
        {
            for (int i = StatsManager.instance.list.Count - 1; i >= 0; i--)
            {
                Destroy(StatsManager.instance.list[i].cardGameObject);
                StatsManager.instance.list.RemoveAt(i);
            }
        }
        StatsManager.instance.list.Clear();
        StatsManager.instance.Awake();
        Start();
       
    }

    public void Action()
    {
        
        if (!bugged)
        {
            if (actStep >= 21)
            {
                recording = false;
                return;
            }
            if ( (!isMoving) && (!queueFinished)&&(!beginloop) )
            {
                actStep++;
                Debug.Log("actstep" + actStep);
                if (actStep >= StatsManager.instance.list.Count)
                {
                    recording = false;
                    return;
                }
                var Dir = StatsManager.instance.list[actStep].cardType;

                if (actStep >= StatsManager.instance.list.Count + 1)
                {
                    Debug.Log("queuefinished");
                    queueFinished = true;
                    return;
                }



                if (Dir == loopstart)
                {
                    beginloop = true;
                    loopstep = actStep ;
                    return;
                }

                switch (Dir)
                {
                    case 0:
                        MoveForward();
                        break;
                    case 1:
                        TurnLeft();
                        break;
                    case 2:
                        TurnRight();
                        break;
                    case 3:
                        Jump();
                        break;
                    case 4:
                        Push();
                        break;
                    case 8:
                        If_Card();
                        break;
                    case 11:
                        Fetch();
                        break;
                    case 12:
                        Brew();
                        break;
                    case 13:
                        Collect();
                        break;
                    default:
                        break;
                }
                step++;
            }
            if ((!isMoving) && (!queueFinished) &&beginloop)
            {
                loopstep++;
                var LDir= StatsManager.instance.list[loopstep].cardType;
                if (LDir == empty)
                {
                    Debug.Log("list is empty");
                    queueFinished = true;
                    return;
                }
                if(LDir==loopend&&m!=times[numloop])
                {
                    loopstep = actStep;
                    m++;
                }
                switch (LDir)
                {
                    case 0:
                        MoveForward();
                        break;
                    case 1:
                        TurnLeft();
                        break;
                    case 2:
                        TurnRight();
                        break;
                    case 3:
                        Jump();
                        break;
                    case 4:
                        Push();
                        break;
                    case 8:
                        If_Card();
                        loopstep++;
                        step++;
                        break;
                    case 11:
                        Fetch();
                        
                        break;
                    case 12:
                        Brew();
                       
                        break;
                    case 13:
                        Collect();
                        break;
                    default:
                        break;
                }
                if (LDir == loopend&&m == times[numloop])
                {
                    beginloop = false;
                    actStep = loopstep;
                    m = 0;
                    numloop++;
                }
                Debug.Log(actStep);
                Debug.Log(loopstep);
                step++;
            }
        }
        else
        {
            Debug.Log("Bug detected.");
            recording = false;
        }
            
    }


   
    public void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, Tar.transform.position, Time.deltaTime);

        if (queueFinished)
        {
            UpdateStats();

            if (currentGrid_GridType.type == 4)
            {
                clearScreen.gameObject.SetActive(true);
            }

           

            queueFinished = false;
        }

        if (recording)
        {
            Action();
        }
        
    }

    public void MoveForward()
    {
        UpdateStats();
        Debug.Log("current : " + currentGrid.transform.name + "   next : " + nextGrid.transform.name);

        

        if (currentGrid.transform.name != nextGrid.transform.name)
        {
            if( (nextGrid_GridType.type == 0) 
                || (nextGrid_GridType.type == 4) 
                || (nextGrid_GridType.type == currentGrid_GridType.type) 
                || (nextGrid_GridType.type == 5)
                || (nextGrid_GridType.type == 8) 
                || (nextGrid_GridType.type == 9) 
                || (nextGrid_GridType.type == 10) 
                || (nextGrid_GridType.type == 11))
            {
                bugged = false;
            }
            else
            {
                bugged = true;
            }
        }
        else
        {
            bugged = true;
        }

        if (!bugged)
        {
            Debug.Log("Different grids");
            moved = false;

            var target = nextGrid.transform.position;

            anim.SetBool("IsMoving", true);
            StartCoroutine(Move(target));
            Debug.Log("target" + target);
            
         }

        if (nextGrid_GridType.type == 4)
        {
            Debug.Log("Clear!");
            clear = true;
            if (!HomeA.isPlaying)
            {
                StartCoroutine(Wait());

            }
        }

        if (nextGrid_GridType.type == 11)
        {
            Debug.Log("Uncertain Grid!");
            uncertain = nextGrid.transform.gameObject;
            parent = nextGrid.transform.parent.gameObject;
            col = parent.transform.GetChild(0).gameObject;
            pro = parent.transform.GetChild(1).gameObject;
            //获得0-1之间的随机数
            dice = Random.Range(0.0f, 1.0f);
            if (count >= 6)
            {
                dice = 0;
            }
            else if (count == 2)
            {
                dice = 1;
            }
            //如果小于0.5是花，大于是酿蜜机
            if (dice <= 0.5)
            {
                //Uncertain.grid1.GetComponent<GridType>().Grid_Type = GridType.Type.Flower;
                //Uncertain.grid1.GetComponent<GridType>().type = 9;
                nextGrid_GridType.type = 9;
                col.SetActive(true);
                animC = col.GetComponent<Animator>();
                uncertain.SetActive(false);
                count++;
                Debug.Log("Flower");
            }
            else
            {
                //Uncertain.grid1.GetComponent<GridType>().Grid_Type = GridType.Type.Case;
                //Uncertain.grid1.GetComponent<GridType>().type = 10;
                nextGrid_GridType.type = 10;
                pro.SetActive(true);
                animP = pro.GetComponent<Animator>();
                uncertain.SetActive(false);
                count += 3;
                Debug.Log("Case");
            }
        }
    }
    IEnumerator IsJump()
    {

        //Debug.Log("Before Waiting 1 seconds");
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("After Waiting 1 Seconds");
        anim.SetBool("IsJumping", false);
    }

    /*IEnumerator IsCollect()
    {

        //Debug.Log("Before Waiting 1 seconds");
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("After Waiting 1 Seconds");
        anim.SetBool("IsCollect", false);
    }*/

    IEnumerator Wait()
    {

        //Debug.Log("Before Waiting 1 seconds");
        yield return new WaitForSeconds(1f);
        //Debug.Log("After Waiting 1 Seconds");
        HomeA.Play();
    }

    IEnumerator Move(Vector2 Target)
    {
        while (!moved)
        {
            isMoving = true;

            if (!WalkA.isPlaying)
            {
                WalkA.Play();
            }
            //var dis = Vector2.Distance(transform.position, Target);
            //Debug.Log("dis" + dis);
            Debug.Log("Target" + Target);

            //if (dis < offSet)
            //{
            //    transform.position = Target;
            //    moved = true;             
            //}
            //transform.Translate(new Vector2(0f, -1f) * moveSpeed * Time.deltaTime,Space.Self);
            Tar.transform.position = Target;
            yield return new WaitForSeconds(0.7f);
            moved = true;
            yield return 0;
        }
       
        StopCoroutine(Move(Target));
        isMoving = false;
        anim.SetBool("IsMoving", false);
        yield return 0;
    }


    public void TurnLeft()
    {
        turnedAngle = 0f;
        turned = false;
        StartCoroutine("Turn",transform.forward);
    }

    public void TurnRight()
    {
        turnedAngle = 0f;
        turned = false;
        StartCoroutine("Turn",-transform.forward);
    }

    IEnumerator Turn(Vector3 TargetAxis)
    {
        while (!turned)
        {
            isMoving = true;
            Tar.transform.Rotate(TargetAxis);
            if (turnedAngle < 90f)
            {
                transform.Rotate(TargetAxis, turnSpeed, Space.Self);
                turnedAngle += turnSpeed;
            }
            else
            {
                turned = true;
            }

            yield return 0;
        }

        StopCoroutine("Turn");
        isMoving = false;
        yield return 0;
    }

    IEnumerator IsCollect()
    {

        //Debug.Log("Before Waiting 1 seconds");
        yield return new WaitForSeconds(2f);
        //Debug.Log("After Waiting 1 Seconds");
        anim.SetBool("Collect", false);
        isMoving = false;
    }



    public void Jump()
    {
        UpdateStats();

        if ((currentGrid_GridType.type == 0 || currentGrid_GridType.type == 8) && (nextGrid_GridType.type == 1))
        {
            anim.SetBool("IsJumping", true);
            if (!JumpA.isPlaying)
            {
                JumpA.Play();
            }
            moved = false;
            StartCoroutine(IsJump());
            var target = nextGrid.transform.position;

            StartCoroutine(Move(target));
            Debug.Log("target" + target);
        }
        if ((currentGrid_GridType.type == 1) && (nextGrid_GridType.type ==6))
        {
            anim.SetBool("IsJumping", true);
            if (!JumpA.isPlaying)
            {
                JumpA.Play();
            }
            moved = false;
            StartCoroutine(IsJump());
            var target = nextGrid.transform.position;

            StartCoroutine(Move(target));
            Debug.Log("target" + target);
        }
        if ((currentGrid_GridType.type == 6) && (nextGrid_GridType.type == 7))
        {
            anim.SetBool("IsJumping", true);
            if (!JumpA.isPlaying)
            {
                JumpA.Play();
            }
            moved = false;
            StartCoroutine(IsJump());
            var target = nextGrid.transform.position;

            StartCoroutine(Move(target));
            Debug.Log("target" + target);
        }
        else
        {
            return;
        }
        
    }

    public void Push()
    {
        UpdateStats();
        anim.SetTrigger("Push");
        if (currentGrid_GridType.type == 5)
        {
            clear = true;
            if (!HomeA.isPlaying)
            {
                StartCoroutine(Wait());

            }
        }
    }

    public void Collect()
    {
        UpdateStats();
        if(currentGrid_GridType.type == 8)
        {
            //anim.SetBool("Collect", true);
            //StartCoroutine(IsCollect());
            //Grid_ImageObj.SendMessage("Collect", currentGrid_GridType.num);
            isMoving = true;
            if (!CollectA.isPlaying)
            {
                StartCoroutine(Wait());
            }
            anim.SetBool("Collect", true);
            StartCoroutine(IsCollect());
            Grid_ImageObj.SendMessage("Collect", currentGrid_GridType.num);
            Collect_num++;
        }
        if(Collect_num==ToCollect)
        {
            clear = true;
        }
    }

    public void If_Card()
    {
        UpdateStats();
        Debug.Log("if" + StatsManager.instance.list[actStep].ifType + " " + currentGrid_GridType.type);
        if (!beginloop)
        {
            actStep++;
            int LDir = StatsManager.instance.list[actStep].cardType;
            if (currentGrid_GridType.type == 9 && StatsManager.instance.list[actStep - 1].ifType == 3)
            {
                switch (LDir)
                {
                    case 0:
                        MoveForward();
                        break;
                    case 1:
                        TurnLeft();
                        break;
                    case 2:
                        TurnRight();
                        break;
                    case 3:
                        Jump();
                        break;
                    case 4:
                        Push();
                        break;
                    case 11:
                        Fetch();
                        break;
                    case 12:
                        Brew();
                        break;
                    case 13:
                        Collect();
                        break;
                    default:
                        break;
                }
            }
            if (currentGrid_GridType.type == 10 && StatsManager.instance.list[actStep - 1].ifType == 4)
            {
                switch (LDir)
                {
                    case 0:
                        MoveForward();
                        break;
                    case 1:
                        TurnLeft();
                        break;
                    case 2:
                        TurnRight();
                        break;
                    case 3:
                        Jump();
                        break;
                    case 4:
                        Push();
                        break;
                    case 11:
                        Fetch();
                        break;
                    case 12:
                        Brew();
                        break;
                    case 13:
                        Collect();
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            loopstep++;
            int LDir = StatsManager.instance.list[loopstep].cardType;
            if (currentGrid_GridType.type == 9 && StatsManager.instance.list[loopstep - 1].ifType == 3)
            {
                switch (LDir)
                {
                    case 0:
                        MoveForward();
                        break;
                    case 1:
                        TurnLeft();
                        break;
                    case 2:
                        TurnRight();
                        break;
                    case 3:
                        Jump();
                        break;
                    case 4:
                        Push();
                        break;
                    case 11:
                        Fetch();
                        break;
                    case 12:
                        Brew();
                        break;
                    case 13:
                        Collect();
                        break;
                    default:
                        break;
                }
            }
            if (currentGrid_GridType.type == 10 && StatsManager.instance.list[loopstep - 1].ifType == 4)
            {
                switch (LDir)
                {
                    case 0:
                        MoveForward();
                        break;
                    case 1:
                        TurnLeft();
                        break;
                    case 2:
                        TurnRight();
                        break;
                    case 3:
                        Jump();
                        break;
                    case 4:
                        Push();
                        break;
                    case 11:
                        Fetch();
                        break;
                    case 12:
                        Brew();
                        break;
                    case 13:
                        Collect();
                        break;
                    default:
                        break;
                }
            }
        }
    }


    public void Fetch()
    {
        Debug.Log("F");
        UpdateStats();
        if (currentGrid_GridType.type == 9)
        {

            animC.SetBool("IsCollected", true);
            Fetched = true;
            //StartCoroutine(IsCollect());
            if (!FlowerA.isPlaying)
            {
                FlowerA.Play();
            }
        }
        if (SceneManager.GetActiveScene().name == "Stage5-1" || SceneManager.GetActiveScene().name == "Stage5-3")
        {
            clear = true;
        }
    }

    public void Brew()
    {
        UpdateStats();
        if (currentGrid_GridType.type == 10)
        {
            if (SceneManager.GetActiveScene().name != "Stage5-3" && Fetched == false)
                return;
            animP.SetBool("IsProduced", true);
            //StartCoroutine(IsCollect());
            if (!CaseA.isPlaying)
            {
                CaseA.Play();
            }
            clear = true;
        }
    }


    private void UpdateStats()
    {
        RaycastHit2D _hit;
        _hit = Physics2D.Raycast(transform.position, -transform.up);

        if(_hit.collider != null)
        {
            currentGrid = _hit;
            currentGrid.transform.gameObject.layer = ignoreRayCast;

            _hit = Physics2D.Raycast(transform.position, -transform.up);

            if (_hit.collider != null)
            {
                nextGrid = _hit;
            }
            else
            {
                nextGrid = currentGrid;
            }

            currentGrid.transform.gameObject.layer = accepetRayCast;

            nextGrid_GridType = nextGrid.transform.gameObject.GetComponent<GridType>();
            currentGrid_GridType = currentGrid.transform.gameObject.GetComponent<GridType>();
        }
    }

}
