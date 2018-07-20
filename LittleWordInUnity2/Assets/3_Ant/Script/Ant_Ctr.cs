using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant_Ctr : MonoBehaviour {
    public AudioSource WalkA;
    public AudioSource JumpA;
    public AudioSource HomeA;
    public bool clear = false;
    static public int step;
    public float moveSpeed = 1f;
    public float turnSpeed = 1f;
    public float offSet = 0.2f;
    private bool moved;
    private bool turned;
    private bool isMoving = false;
    private Animator anim;
    private float turnedAngle = 0f;
    private bool recording = false;
    private int actStep = -1;
    
    private bool queueFinished;

    private RaycastHit2D currentGrid;
    private RaycastHit2D nextGrid;
    [SerializeField]
    private Transform startGrid;
    [SerializeField]
    private Transform clearScreen;
    [SerializeField]
    private int look;
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
    //private int error = 99;

    private bool bugged;
    private GridType nextGrid_GridType;
    private GridType currentGrid_GridType;

    //Could do an Error throw, break execution & highlight error

    private void Start()
    {
        step = 0;
        anim = GetComponent<Animator>();
       
        transform.position = startGrid.position;
        transform.rotation = startGrid.rotation;

        transform.Rotate(transform.forward, look*90, Space.Self);
        Tar.transform.position = this.transform.position;
        Tar.transform.rotation = this.transform.rotation;
        bugged = false;
        queueFinished = false;
        recording = false;
        actStep = -1;
        loopstep = 0;
        m = 0;
        beginloop = false;
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
        //if (!recording)
        //{
        //    transform.position = startGrid.position;
        //    transform.rotation = startGrid.rotation;
        //}
    }

    public void Clear()
    {
        //if (!recording)
        //{
        //    for (int i = StatsManager.instance.list.Count - 1; i >= 0; i--)
        //    {
        //        Destroy(StatsManager.instance.list[i].cardGameObject);
        //        StatsManager.instance.list.RemoveAt(i);
        //    }
        //}
        //StatsManager.instance.list.Clear();
        //StatsManager.instance.Awake();
        //Start();
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void Action()
    {

        if (!bugged)
        {
            if (actStep >= 21)
            {
                recording = false;
            }
            if ((!isMoving) && (!queueFinished) && (!beginloop))
            {
                actStep++;
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
                    loopstep = actStep;
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
                    default:
                        break;
                }
                step++;
            }
            if ((!isMoving) && (!queueFinished) && beginloop)
            {
                loopstep++;
                var LDir = StatsManager.instance.list[loopstep].cardType;
                if (LDir == empty)
                {
                    Debug.Log("list is empty");
                    queueFinished = true;
                    return;
                }
                if (LDir == loopend && m != 5)
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
                    default:
                        break;
                }
                if (LDir == loopend && m == 5)
                {
                    beginloop = false;
                    actStep = loopstep;
                    m = 0;
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
            if ((nextGrid_GridType.type == 0) || (nextGrid_GridType.type == 4) || (nextGrid_GridType.type == currentGrid_GridType.type)||(nextGrid_GridType.type==5))
            {
                bugged = false;
            }
            else if(currentGrid_GridType.type==6)
            {
                bugged = false;
            }
            else
            {
                print("1");
                bugged = true;
            }
        }
        else
        {
            print("2");
            bugged = true;
        }

        if (!bugged)
        {
            Debug.Log("Different grids");
            moved = false;

            var target = nextGrid.transform.position;
            anim.SetBool("Walk", true);
            anim.SetBool("Jump", false);
            StartCoroutine(Move(target));
            Debug.Log("target" + target);
        }
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


            //if (dis < offSet)
            //{
            //    transform.position = Target;
            //    moved = true;
            //}
            //transform.Translate(new Vector2(0f, -1f) * moveSpeed * Time.deltaTime, Space.Self);

            //yield return 0;
            Tar.transform.position = Target;
            yield return new WaitForSeconds(1f);
            moved = true;
        }
        anim.SetBool("Walk", false);
        
        StopCoroutine(Move(Target));
        isMoving = false;
        yield return 0;
    }


    public void TurnLeft()
    {
        turnedAngle = 0f;
        turned = false;
        anim.SetBool("Jump", false);
        StartCoroutine("Turn", transform.forward);
    }

    public void TurnRight()
    {
        turnedAngle = 0f;
        turned = false;
        anim.SetBool("Jump", false);
        StartCoroutine("Turn", -transform.forward);
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


    public void Jump()
    {
        UpdateStats();

        if ((currentGrid_GridType.type == 0) && (nextGrid_GridType.type == 1))
        {
            if (!JumpA.isPlaying)
            {
                JumpA.Play();
            }
            moved = false;

            var target = nextGrid.transform.position;
            anim.SetBool("Jump", true);
            StartCoroutine(Move(target));
            Debug.Log("target" + target);
        }
        if ((currentGrid_GridType.type == 1) && (nextGrid_GridType.type == 6))
        {
            if (!JumpA.isPlaying)
            {
                JumpA.Play();
            }
            moved = false;
            
            var target = nextGrid.transform.position;

            StartCoroutine(Move(target));
            Debug.Log("target" + target);
        }
        else
        {
            bugged = false;
        }
    }

    public void Push()
    {
        UpdateStats();
        Debug.Log("Push");
        if(currentGrid_GridType.type==5)
        {
            anim.SetBool("Jump", false);
            anim.SetTrigger("Push");
            clear = true;
            if (!HomeA.isPlaying)
            {
                HomeA.Play();

            }
        }
    }
    IEnumerator Wait()
    {

        //Debug.Log("Before Waiting 1 seconds");
        yield return new WaitForSeconds(1f);
        //Debug.Log("After Waiting 1 Seconds");
        HomeA.Play();
    }
    private void UpdateStats()
    {
        RaycastHit2D _hit;
        _hit = Physics2D.Raycast(transform.position, -transform.up);

        if (_hit.collider != null)
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