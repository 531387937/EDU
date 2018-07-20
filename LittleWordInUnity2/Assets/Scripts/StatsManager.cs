using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsManager : MonoBehaviour
{

    #region Variables

    public static StatsManager instance;

    //[HideInInspector]
    public int currentArea;
    //[HideInInspector]
    public int currentSlot;
    //[HideInInspector]
    public int currentInsert;
    //[HideInInspector]
    public int nextSlot;
    //[HideInInspector]
    public Card_Loop existingLoop;

    [SerializeField]
    private GameObject emptyCard;
    [SerializeField]
    private GameObject loopEndCard;
    [SerializeField]
    private GameObject ifExecuteField;
    [SerializeField]
    private GameObject[] slotOBJ;
    [SerializeField]
    private GameObject[] loopMid;

 

    [SerializeField]
    private Sprite loopStartSprite;
    [SerializeField]
    private Sprite ifStartSprite;

    public const int GAME_AREA = 0;
    public const int PICK_AREA = 1;
    public const int TIMELINE = 2;

    public const int INSERT_NA = 0;
    public const int INSERT_BEFORE = 1;
    public const int INSERT_AFTER = 2;

    public const int LOOP_CARD = 6;
    public const int LOOP_END = 7;
    public const int EMPTY = 10;//

    public const int IF_CARD = 8;
    public const int NOT_IF = 0;
    public const int SINGLE_IF = 1;
    //public const int IF_ELSE = 2;
    public const int IF_Flower = 3;
    public const int IF_Honey = 4;

    public static string SLOT_ID_PREFIX = "Slot_";

    public Transform card1;
    public Transform card2;
    public Transform card3;
    float yoffset = 0;
    float xoffset = 0;

    private GUIStyle textStyle = new GUIStyle();

    private void Start()
    {
        xoffset = card2.position.x - card1.position.x;
        yoffset = card1.position.y - card3.position.y;

       
    }



    public struct Node
    {
        public GameObject cardGameObject;
        public int cardType;

        public bool isLoopHead;
        public bool isLoop;
        public bool isLoopTail;
        public int loopLength;

        public bool isIfHead;
        public bool isExecute;
        public int ifType;

        public Transform linkHead;
        public Transform linkIfField;
        public int looptimes;
    };

    public List<Node> list = new List<Node>();
    private List<Node> tempList = new List<Node>();
    public int listLength;
    public int tempListLength;

    #endregion

    public void Paixu()
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            int line = (i) / 7;
            float myxoffset = xoffset * (i % 7);
            float myyoffset = yoffset * line;
            Vector3 newpos = new Vector3(card1.position.x + myxoffset, card1.position.y - myyoffset, card1.position.z);
            iTween.MoveTo(list[i].cardGameObject, newpos, 0.5f);
        }
    }

    public void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one StatsManager in scene");
        }
        else
        {
            instance = this;
        }

        currentArea = 0;
        currentSlot = 0;
        currentInsert = 0;
        textStyle.fontSize = 30;
        nextSlot = 1;
        existingLoop = null;

        listLength = 0;
    }

    #region NewCardLogic

    public void NewCardToSlot(int _cardType, int _slotInt, GameObject _cardGameObject)
    {
        if (_slotInt > listLength)
        {
            //New card to Slot out of queue
            Debug.Log("New card to Slot out of queue");

            NewCardAdd_Generate(_cardType, _cardGameObject);
        }
        else
        {
            //New card to Slot inside queue
            Debug.Log("New card to Slot inside queue" + list[_slotInt - 1].cardType);
            if (list[_slotInt - 1].cardType == EMPTY)//
            {
                Debug.Log("emtpycardslot");
                //Empty loop or if slot
                if (list[_slotInt - 1].isLoop)
                {
                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = false,
                        isLoop = true,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = list[_slotInt - 2].cardGameObject.transform,

                    };
                    _cardGameObject.transform.SetParent(newNode.linkHead);

                    Destroy(list[_slotInt - 1].cardGameObject);
                    list.Remove(list[_slotInt - 1]);
                    list.Insert(_slotInt - 1, newNode);

                    listLength = list.Count;
                    Paixu();
                }
                else if (list[_slotInt - 1].isExecute)
                {
                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = true,
                        ifType = NOT_IF,
                        linkHead = list[_slotInt - 2].cardGameObject.transform,
                    };
                    _cardGameObject.transform.SetParent(newNode.linkHead);
                    list[_slotInt - 2].linkIfField.transform.SetParent(_cardGameObject.transform);
                    list[_slotInt - 2].linkIfField.transform.localPosition = new Vector3(-12, 0, 0);
                    /*list[_slotInt - 2].linkIfField.transform.SetSiblingIndex(1);
                    _cardGameObject.transform.SetSiblingIndex(5);*/

                    Destroy(list[_slotInt - 1].cardGameObject);
                    list.RemoveAt(_slotInt - 1);
                    list.Insert(_slotInt - 1, newNode);


                    listLength = list.Count;
                    Paixu();
                }
            }
            else
            {
                //Taken slot
                Destroy(_cardGameObject);
            }
        }
    }




    public void NewCardToInsert(int _cardType, int _insertInt, GameObject _cardGameObject)
    {

        if (_insertInt == 0)
        {
            NewCardInsert_Generate(_cardType, _insertInt, _cardGameObject);
            return;
        }
        if (_insertInt >= listLength)
        {

            //Insert out of Queue
            NewCardAdd_Generate(_cardType, _cardGameObject);

        }
        else
        {
            //Insert inside Queue
            if (list[_insertInt - 1].isLoopHead)
            {
                //if (list[_insertInt].cardType == EMPTY)
                //{
                //    //Insert to empty loop slot
                //    Destroy(list[_insertInt].cardGameObject);
                //    list.Remove(list[_insertInt]);
                //    NewCardInsert_ToLoop_Generate(_cardType, _insertInt, _cardGameObject, list[_insertInt].linkHead);

                //}
                ////else
                //{
                //Insert to loop

                NewCardInsert_ToLoop_Generate(_cardType, _insertInt, _cardGameObject, list[_insertInt].linkHead);

                //}
            }
            else if (list[_insertInt - 1].isLoop)
            {
                //if (list[_insertInt - 1].cardType == EMPTY)
                //{
                //    //Insert to empty loop slot
                //    Destroy(list[_insertInt - 1].cardGameObject);
                //    list.Remove(list[_insertInt - 1]);
                //    NewCardInsert_ToLoop_Generate(_cardType, _insertInt - 1, _cardGameObject, list[_insertInt - 1].linkHead);

                //}
                ////else
                //{
                //Insert to loop

                NewCardInsert_ToLoop_Generate(_cardType, _insertInt, _cardGameObject, list[_insertInt - 1].linkHead);

                //}
            }
            else if (list[_insertInt - 1].isIfHead)
            {
                if (list[_insertInt].cardType == EMPTY)
                {
                    //Insert to empty if
                    
                    NewCardInsert_ToIf_Generate(_cardType, _insertInt, _cardGameObject, list[_insertInt].linkHead);

                }
                else
                {
                    //Insert to filled if
                    Destroy(_cardGameObject);
                }
            }
            /*else if (list[_insertInt - 1].isExecute)
            {
                if (list[_insertInt - 1].cardType == EMPTY)
                {
                    //Insert to empty if
                    Destroy(list[_insertInt - 1].cardGameObject);
                    list.Remove(list[_insertInt - 1]);
                    NewCardInsert_ToIf_Generate(_cardType, _insertInt, _cardGameObject, list[_insertInt - 1].linkHead);
                }
                else
                {
                    if (list[_insertInt].isLoop || list[_insertInt].isLoopTail)
                    {
                        //Insert behind a if that is inside a loop
                        NewCardInsert_ToLoop_Generate(_cardType, _insertInt, _cardGameObject, list[_insertInt].linkHead);
                    }
                    else
                    {
                        //Insert behind a if
                        NewCardInsert_Generate(_cardType, _insertInt, _cardGameObject);
                    }
                }
            }*/
            else
            {

                //Insert behind a normal card
                NewCardInsert_Generate(_cardType, _insertInt, _cardGameObject);
                
            }

        }
    }

    #endregion


    #region NewCardGenerates

    private void NewCardAdd_Generate(int _cardType, GameObject _cardGameObject)
    {
        switch (_cardType)
        {
            case LOOP_CARD:
                {
                    Image _image = _cardGameObject.GetComponent<Image>();

                   

                    GameObject loopMidObj = Instantiate(emptyCard);
                    GameObject loopEndObj = Instantiate(loopEndCard);
                    loopMidObj.transform.SetParent(_cardGameObject.transform);
                    loopEndObj.transform.SetParent(_cardGameObject.transform);
                    _image.color= new Color((float)0.3451,1,(float)0.8902,1);
                    _image.sprite = loopStartSprite;

                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = true,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 1,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = null,
                        linkIfField = null
                    };
                    list.Add(newNode);
                    Paixu();
                    //listLength = list.Count;
                    //int line = (listLength - 1) / 7;
                    //float myxoffset = xoffset * ((listLength - 1) % 7);
                    //float myyoffset = yoffset * line;
                    //Vector3 newpos = new Vector3(card1.position.x + myxoffset, card1.position.y - myyoffset, card1.position.z);
                    //iTween.MoveTo(list[listLength - 1].cardGameObject, newpos, 0.5f);

                    //Node newNode_2 = new Node
                    //{
                    //    cardGameObject = loopMidObj,
                    //    cardType = EMPTY,
                    //    isLoopHead = false,
                    //    isLoop = true,
                    //    isLoopTail = false,
                    //    loopLength = 0,
                    //    isIfHead = false,
                    //    isExecute = false,
                    //    ifType = NOT_IF,
                    //    linkHead = _cardGameObject.transform,
                    //    linkIfField = null
                    //};

                    //list.Add(newNode_2);

                    //listLength = list.Count;
                    //line = (listLength - 1) / 7;
                    //myxoffset = xoffset * ((listLength - 1) % 7);
                    //myyoffset = yoffset * line;
                    //newpos = new Vector3(card1.position.x + myxoffset, card1.position.y - myyoffset, card1.position.z);
                    //iTween.MoveTo(list[listLength - 1].cardGameObject, newpos, 0.5f);

                    Node newNode_3 = new Node
                    {
                        cardGameObject = loopEndObj,
                        cardType = LOOP_END,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = true,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = _cardGameObject.transform,
                        linkIfField = null
                    };
                    list.Add(newNode_3);
                    Paixu();
                    //listLength = list.Count;
                    //line = (listLength - 1) / 7;
                    //myxoffset = xoffset * ((listLength - 1) % 7);
                    //myyoffset = yoffset * line;
                    //newpos = new Vector3(card1.position.x + myxoffset, card1.position.y - myyoffset, card1.position.z);
                    //iTween.MoveTo(list[listLength - 1].cardGameObject, newpos, 0.5f);

                  
                }
                break;

            case IF_CARD:
                {
                    Image _image = _cardGameObject.GetComponent<Image>();

                    GameObject ifExecute = Instantiate(emptyCard);
                    GameObject ifField = Instantiate(ifExecuteField);
                    ifExecute.transform.SetParent(_cardGameObject.transform, false);
                    /*ifField.transform.SetParent(_cardGameObject.transform);
                    ifField.transform.localPosition = new Vector3(155, 0, 0);*/
                    _image.sprite = ifStartSprite;

                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = true,
                        isExecute = false,
                        ifType = SINGLE_IF,
                        linkHead = null,
                        linkIfField = ifField.transform
                    };
                    list.Add(newNode);



                    Node newNode_2 = new Node
                    {
                        cardGameObject = ifExecute,
                        cardType = EMPTY,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = true,
                        ifType = NOT_IF,
                        linkHead = _cardGameObject.transform,
                        linkIfField = null
                    };
                    list.Add(newNode_2);

                    Paixu();
                    //ifExecute.transform.SetParent(_cardGameObject.transform,false);
                    listLength = list.Count;
                    ifField.transform.SetParent(ifExecute.transform, true);
                    ifField.transform.localPosition = new Vector3(-12, 0, 0);
                }
                break;


            default:
                //Normal Card
                {
                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = null,
                        linkIfField = null
                    };
                    list.Add(newNode);
                    Paixu();
                    listLength = list.Count;
                    int line = (listLength - 1) / 7;
                    float myxoffset = xoffset * ((listLength - 1) % 7);
                    float myyoffset = yoffset * line;
                    Vector3 newpos = new Vector3(card1.position.x + myxoffset, card1.position.y - myyoffset, card1.position.z);
                    iTween.MoveTo(list[listLength - 1].cardGameObject, newpos, 0.5f);
                }
                break;
        }
    }

    private void NewCardInsert_ToLoop_Generate(int _cardType, int _insertInt, GameObject _cardGameObject, Transform _linkHead)
    {
        switch (_cardType)
        {
            case LOOP_CARD:
                {
                    Image _image = _cardGameObject.GetComponent<Image>();

                    GameObject loopMidObj = Instantiate(emptyCard);
                    GameObject loopEndObj = Instantiate(loopEndCard);
                    loopMidObj.transform.SetParent(_cardGameObject.transform);
                    loopEndObj.transform.SetParent(_cardGameObject.transform);
                    _image.color = new Color((float)0.3451, 1, (float)0.8902, 1);
                    _image.sprite = loopStartSprite;

                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = true,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 1,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = _linkHead,
                        linkIfField = null
                    };
                    list.Insert(_insertInt, newNode);
                    _cardGameObject.transform.SetParent(_linkHead);
                    listLength = list.Count;

                    //Node newNode_2 = new Node
                    //{
                    //    cardGameObject = loopMidObj,
                    //    cardType = EMPTY,
                    //    isLoopHead = false,
                    //    isLoop = true,
                    //    isLoopTail = false,
                    //    loopLength = 0,
                    //    isIfHead = false,
                    //    isExecute = false,
                    //    ifType = NOT_IF,
                    //    linkHead = _cardGameObject.transform,
                    //    linkIfField = null
                    //};
                    //list.Insert(_insertInt + 1, newNode_2);

                    Node newNode_3 = new Node
                    {
                        cardGameObject = loopEndObj,
                        cardType = LOOP_END,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = true,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = _cardGameObject.transform,
                        linkIfField = null
                    };
                    list.Insert(_insertInt + 1, newNode_3);

                    listLength = list.Count;
                    Paixu();
                }
                break;

            case IF_CARD:
                {
                    Image _image = _cardGameObject.GetComponent<Image>();

                    GameObject ifExecute = Instantiate(emptyCard);
                    GameObject ifField = Instantiate(ifExecuteField);
                    /*ifField.transform.SetParent(_cardGameObject.transform);
                    ifField.transform.localPosition = new Vector3(155, 0, 0);*/
                    _image.sprite = ifStartSprite;

                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = true,
                        isExecute = false,
                        ifType = SINGLE_IF,
                        linkHead = null,
                        linkIfField = ifField.transform
                    };
                    /// st.Add(newNode);
                    list.Insert(_insertInt, newNode);



                    Node newNode_2 = new Node
                    {
                        cardGameObject = ifExecute,
                        cardType = EMPTY,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = true,
                        ifType = NOT_IF,
                        linkHead = _cardGameObject.transform,
                        linkIfField = null
                    };
                    list.Insert(_insertInt + 1, newNode_2);

                    Paixu();
                    ifExecute.transform.SetParent(_cardGameObject.transform, false);
                    listLength = list.Count;
                    ifField.transform.SetParent(ifExecute.transform, true);
                    ifField.transform.localPosition = new Vector3(-12, 0, 0);
                }

                break;


            default:
                //Normal Card
                {
                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = false,
                        isLoop = true,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = _linkHead,
                        linkIfField = null
                    };

                    list.Insert(_insertInt, newNode);
                    _cardGameObject.transform.SetParent(_linkHead);

                    listLength = list.Count;
                    Paixu();


                }
                break;
        }
    }

    private void NewCardInsert_ToIf_Generate(int _cardType, int _insertInt, GameObject _cardGameObject, Transform _linkHead)
    {
        Debug.Log("insert into if");

        switch (_cardType)
        {
            case LOOP_CARD:
                {
                    Destroy(_cardGameObject);
                }
                break;

            case IF_CARD:
                {
                    Destroy(_cardGameObject);
                }
                break;

            default:
                //Normal Card
                {
                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = true,
                        ifType = NOT_IF,
                        linkHead = _linkHead,
                        linkIfField = null
                    };
                    list[_insertInt - 1].linkIfField.gameObject.transform.SetParent(_cardGameObject.transform);
                    list[_insertInt - 1].linkIfField.gameObject.transform.localPosition = new Vector3(-12, 0, 0);
                    Destroy(list[_insertInt].cardGameObject);
                    list.Remove(list[_insertInt]);
                    list.Insert(_insertInt, newNode);
                    _cardGameObject.transform.SetParent(_linkHead);
                    Paixu();
                    listLength = list.Count;
                }
                break;
        }
    }


    private void NewCardInsert_Generate(int _cardType, int _insertInt, GameObject _cardGameObject)
    {
        switch (_cardType)
        {
            case LOOP_CARD:
                {
                    Image _image = _cardGameObject.GetComponent<Image>();

                    GameObject loopMidObj = Instantiate(emptyCard);
                    GameObject loopEndObj = Instantiate(loopEndCard);
                    loopMidObj.transform.SetParent(_cardGameObject.transform);
                    loopEndObj.transform.SetParent(_cardGameObject.transform);
                    _image.color = new Color((float)0.3451, 1, (float)0.8902, 1);
                    _image.sprite = loopStartSprite;

                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = true,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 1,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = null,
                        linkIfField = null
                    };

                    list.Insert(_insertInt, newNode);

                    //Node newNode_2 = new Node
                    //{
                    //    cardGameObject = loopMidObj,
                    //    cardType = EMPTY,
                    //    isLoopHead = false,
                    //    isLoop = true,
                    //    isLoopTail = false,
                    //    loopLength = 0,
                    //    isIfHead = false,
                    //    isExecute = false,
                    //    ifType = NOT_IF,
                    //    linkHead = _cardGameObject.transform,
                    //    linkIfField = null
                    //};
                    //list.Insert(_insertInt + 1, newNode_2);

                    Node newNode_3 = new Node
                    {
                        cardGameObject = loopEndObj,
                        cardType = LOOP_END,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = true,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = _cardGameObject.transform,
                        linkIfField = null
                    };
                    list.Insert(_insertInt + 1, newNode_3);

                    listLength = list.Count;
                    Paixu();
                }
                break;

            case IF_CARD:
                {
                    Image _image = _cardGameObject.GetComponent<Image>();

                    GameObject ifExecute = Instantiate(emptyCard);
                    GameObject ifField = Instantiate(ifExecuteField);
                    /*ifField.transform.SetParent(_cardGameObject.transform);
                    ifField.transform.localPosition = new Vector3(155, 0, 0);*/
                    _image.sprite = ifStartSprite;

                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = true,
                        isExecute = false,
                        ifType = SINGLE_IF,
                        linkHead = null,
                        linkIfField = ifField.transform
                    };
                    /// st.Add(newNode);
                    list.Insert(_insertInt, newNode);



                    Node newNode_2 = new Node
                    {
                        cardGameObject = ifExecute,
                        cardType = EMPTY,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = true,
                        ifType = NOT_IF,
                        linkHead = _cardGameObject.transform,
                        linkIfField = null
                    };
                    list.Insert(_insertInt + 1, newNode_2);

                    Paixu();
                    ifExecute.transform.SetParent(_cardGameObject.transform, false);
                    listLength = list.Count;
                    ifField.transform.SetParent(ifExecute.transform, true);
                    ifField.transform.localPosition = new Vector3(-12, 0, 0);
                }

                break;


            default:
                //Normal Card
                {
                    Node newNode = new Node
                    {
                        cardGameObject = _cardGameObject,
                        cardType = _cardType,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = null,
                        linkIfField = null
                    };
                    listLength = list.Count;

                    list.Insert(_insertInt, newNode);

                    Paixu();
                }
                break;
        }
    }


    #endregion



    #region CardLogic

    public void CardToSlot(int _pickedUpFrom, int _slotInt)
    {
        if (_slotInt >= listLength)
        {
            //Card to out of Queue

            CardTo_MoveToTail_Generate(_pickedUpFrom);
        }
        else
        {
            //Card to inside Queue
            if (list[_slotInt].cardType == EMPTY)
            {
                //Card to empty Slot inside Queue

                if (list[_slotInt].isLoop)
                {
                    CardToSlot_InsideQueue_ToLoop_Generate(_pickedUpFrom, _slotInt);
                }
                else
                {
                    //Shouldn't happen...Execute should only be placed directly into if...

                    //Do nothing...
                }
            }
            else
            {
                //Card to taken Slot inside Queue

                //Do nothing... instead of removing....                
            }
        }
    }

    public void CardToInsert(int _pickedUpFrom, int _insertInt)
    {
        if (_insertInt == 0)
        {
            CardToInsert_BehindCard_Generate(_pickedUpFrom, _insertInt);
            return;
        }
        if (_insertInt >= listLength)
        {
            //Card insert out of Queue

            CardTo_MoveToTail_Generate(_pickedUpFrom);
        }
        else
        {
            ////Card insert inside Queue
            //if (list[_insertInt].cardType == EMPTY)
            //{
            //    //Card insert before a empty card

            //    if (list[_insertInt].isLoop)
            //    {
            //        //CardToInsert_BeforeEmpty_ToLoop_Generate(_pickedUpFrom, _insertInt);
            //    }
            //    else
            //    {
            //        //Shouldn't happen...Execute should only be placed directly into if...

            //        //Do nothing...
            //    }
            //}
            //else if (list[_insertInt - 1].cardType == EMPTY)
            //{
            //    //Card insert behind a empty card

            //    if (list[_insertInt - 1].isLoop)
            //    {
            //        //CardToInsert_BehindEmpty_ToLoop_Generate(_pickedUpFrom, _insertInt);
            //    }
            //    else
            //    {
            //        //Shouldn't happen...Execute should only be placed directly into if...

            //        //Do nothing...
            //    }
            //}
            //else
            {
                //Card insert behind a card

                if (list[_insertInt - 1].isLoopHead || list[_insertInt - 1].isLoop || list[_insertInt].isLoop || list[_insertInt].isLoopTail)
                {
                    //Card insert behind LoopHead or behind a card that is inside loop

                    CardToInsert_ToLoop_Generate(_pickedUpFrom, _insertInt);
                }
                else if (list[_insertInt - 1].isIfHead /*|| list[_insertInt - 1].isExecute*/)
                {
                    //Card insert behind ifHead or Execute

                    //Shouldn't happen...Execute should only be placed directly into if...
                    //Do nothing...
                }
                else
                {
                    //Card insert behind a normal card

                    CardToInsert_BehindCard_Generate(_pickedUpFrom, _insertInt);
                }
            }
        }
    }

    public void CardToOutOfBounds(int _pickedUpFrom)
    {
        switch (list[_pickedUpFrom].cardType)
        {
            case LOOP_CARD:
                {
                    int i = _pickedUpFrom;
                    while (list[i].cardType != LOOP_END)
                    {
                        i++;
                    }

                    for (int j = i; list[j].cardType != LOOP_CARD; j--)
                    {
                        Destroy(list[j].cardGameObject);
                        list.Remove(list[j]);
                        i = j;
                    }
                    if (list[i - 1].cardType == LOOP_CARD)
                    {
                        Destroy(list[i - 1].cardGameObject);
                        list.Remove(list[i - 1]);


                    }
                    Paixu();
                    listLength = list.Count;
                }
                break;

            case IF_CARD:
                {
                    Destroy(list[_pickedUpFrom].cardGameObject);
                    list.Remove(list[_pickedUpFrom]);
                    Destroy(list[_pickedUpFrom].cardGameObject);
                    list.Remove(list[_pickedUpFrom]);
                    Paixu();
                    listLength = list.Count;
                }
                break;

            default:
                {
                    if (list[_pickedUpFrom].isExecute == true)
                    {
                        GameObject ifExecute = Instantiate(emptyCard);
                        Node newNode_2 = new Node
                        {
                            cardGameObject = ifExecute,
                            cardType = EMPTY,
                            isLoopHead = false,
                            isLoop = false,
                            isLoopTail = false,
                            loopLength = 0,
                            isIfHead = false,
                            isExecute = true,
                            ifType = NOT_IF,
                            linkHead = list[_pickedUpFrom - 1].cardGameObject.transform,
                            linkIfField = null
                        };
                        ifExecute.transform.SetParent(list[_pickedUpFrom - 1].cardGameObject.transform);
                        list[_pickedUpFrom - 1].linkIfField.transform.SetParent(ifExecute.transform);
                        list[_pickedUpFrom - 1].linkIfField.transform.localPosition = new Vector3(-12, 0, 0);
                        Destroy(list[_pickedUpFrom].cardGameObject);
                        list.Remove(list[_pickedUpFrom]);
                        list.Insert(_pickedUpFrom, newNode_2);
                    }
                    else
                    {
                        Destroy(list[_pickedUpFrom].cardGameObject);
                        list.Remove(list[_pickedUpFrom]);
                    }
                    listLength = list.Count;
                    Paixu();
                }
                break;

        }
        Paixu();

    }

    #endregion



    #region CardGenerate

    private void CardToSlot_InsideQueue_ToLoop_Generate(int _pickedUpFrom, int _slotInt)
    {
        Debug.Log("CardToSlot_InsideQueue_ToLoop_Generate");
        switch (list[_pickedUpFrom].cardType)
        {
            case LOOP_CARD:
                {

                    int i = _pickedUpFrom;
                    if (i == 0)
                    {
                        tempList.Add(list[i]);
                        i++;
                    }
                    while (list[i - 1].cardType != LOOP_END)
                    {
                        tempList.Add(list[i++]);
                    }

                    list.Remove(list[_slotInt]);

                    for (i = tempList.Count - 1; i >= 0; i--)
                    {
                        list.Insert(_slotInt, tempList[i]);
                    }

                    tempList.Clear();
                    Paixu();
                    listLength = list.Count;
                }
                break;

            case IF_CARD:
                {
                    int i = _pickedUpFrom;
                    while ((list[i].isIfHead) || (list[i].isExecute))
                    {
                        tempList.Add(list[i++]);
                    }

                    list.Remove(list[_slotInt]);

                    for (i = 1; i <= tempList.Count; i++)
                    {
                        list.Insert(_slotInt, tempList[i]);
                    }
                    Paixu();
                    tempList.Clear();
                    listLength = list.Count;
                }
                break;

            default:
                {
                    Node newNode = new Node
                    {
                        cardGameObject = list[_pickedUpFrom].cardGameObject,
                        cardType = list[_pickedUpFrom].cardType,
                        isLoopHead = false,
                        isLoop = true,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = null,
                        linkIfField = null
                    };

                    list.Remove(list[_pickedUpFrom]);
                    list.Insert(_slotInt, newNode);
                    Paixu();
                    listLength = list.Count;
                }
                break;
        }
    }

    private void CardTo_MoveToTail_Generate(int _pickedUpFrom)
    {
        switch (list[_pickedUpFrom].cardType)
        {
            case LOOP_CARD:
                {
                    int i = _pickedUpFrom;
                    if (i == 0)
                    {
                        tempList.Add(list[i]);
                        i++;
                    }
                    while (list[i - 1].cardType != LOOP_END)
                    {
                        tempList.Add(list[i++]);
                    }

                    for (int j = 0; j < tempList.Count; j++)
                    {
                        list.Remove(tempList[j]);
                        list.Add(tempList[j]);
                    }
                    Paixu();
                    tempList.Clear();
                    listLength = list.Count;

                }
                break;

            case IF_CARD:
                {
                    list.Add(list[_pickedUpFrom]);
                    list.Remove(list[_pickedUpFrom]);
                    list.Add(list[_pickedUpFrom]);
                    list.Remove(list[_pickedUpFrom]);

                    Paixu();
                    //tempList.Clear();
                    listLength = list.Count;
                }
                break;

            default:
                {
                    Node newNode = new Node
                    {
                        cardGameObject = list[_pickedUpFrom].cardGameObject,
                        cardType = list[_pickedUpFrom].cardType,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = null,
                        linkIfField = null
                    };
                    list.Remove(list[_pickedUpFrom]);
                    Paixu();
                    list.Add(newNode);
                    listLength = list.Count;
                    int line = (listLength - 1) / 7;
                    float myxoffset = xoffset * ((listLength - 1) % 7);
                    float myyoffset = yoffset * line;
                    Vector3 newpos = new Vector3(card1.position.x + myxoffset, card1.position.y - myyoffset, card1.position.z);
                    iTween.MoveTo(list[listLength - 1].cardGameObject, newpos, 0.5f);
                }
                break;
        }
    }

    //private void CardToInsert_BeforeEmpty_ToLoop_Generate(int _pickedUpFrom, int _insertInt)
    //{
    //    //Destroy(list[_insertInt].cardGameObject);
    //    //list.Remove(list[_insertInt]);
    //    //Paixu();
    //    switch (list[_pickedUpFrom].cardType)
    //    {
    //        case LOOP_CARD:
    //            {

    //                int i = _pickedUpFrom;

    //                while (list[i].cardType != LOOP_END)
    //                {
    //                    tempList.Add(list[i++]);
    //                }
    //                if(list[i].cardType==LOOP_END)
    //                {
    //                    tempList.Add(list[i]);
    //                }
    //                for (i = tempList.Count-1; i >=0 ; i--)
    //                {                     
    //                    list.Insert(_insertInt,tempList[i]);

    //                }                  
    //                tempList.Clear();
    //                listLength = list.Count;
    //                Paixu();

    //            }
    //            break;

    //        case IF_CARD:
    //            {

    //            }
    //            break;

    //        default:
    //            {
    //                Node newNode = new Node
    //                {
    //                    cardGameObject = list[_pickedUpFrom].cardGameObject,
    //                    cardType = list[_pickedUpFrom].cardType,
    //                    isLoopHead = false,
    //                    isLoop = false,
    //                    isLoopTail = false,
    //                    loopLength = 0,
    //                    isIfHead = false,
    //                    isExecute = false,
    //                    ifType = NOT_IF,
    //                    linkHead = null,
    //                    linkIfField = null
    //                };

    //                list.Remove(list[_pickedUpFrom]);

    //                list.Insert(_insertInt, newNode);

    //                listLength = list.Count;
    //                Paixu();
    //            }
    //            break;

    //    }

    //}

    //private void CardToInsert_BehindEmpty_ToLoop_Generate(int _pickedUpFrom, int _insertInt)
    //{
    //    switch (list[_pickedUpFrom].cardType)
    //    {
    //        case LOOP_CARD:
    //            {
    //                int i = _pickedUpFrom;

    //                while (list[i].cardType != LOOP_END)
    //                {
    //                    tempList.Add(list[i++]);
    //                }
    //                if (list[i].cardType == LOOP_END)
    //                {
    //                    tempList.Add(list[i]);
    //                }
    //                for (i = tempList.Count-1; i>=0; i--)
    //                {
    //                    list.Insert(_insertInt, tempList[i]);

    //                }

    //                tempList.Clear();
    //                listLength = list.Count;
    //                Paixu();
    //            }
    //            break;

    //        case IF_CARD:
    //            {

    //            }
    //            break;

    //        default:
    //            {
    //                Node newNode = new Node
    //                {
    //                    cardGameObject = list[_pickedUpFrom].cardGameObject,
    //                    cardType = list[_pickedUpFrom].cardType,
    //                    isLoopHead = false,
    //                    isLoop = false,
    //                    isLoopTail = false,
    //                    loopLength = 0,
    //                    isIfHead = false,
    //                    isExecute = false,
    //                    ifType = NOT_IF,
    //                    linkHead = null,
    //                    linkIfField = null
    //                };

    //                list.Remove(list[_pickedUpFrom]);

    //                list.Insert(_insertInt, newNode);
    //                Paixu();            
    //            }
    //            break;

    //    }
    //} 

    private void CardToInsert_ToLoop_Generate(int _pickedUpFrom, int _insertInt)
    {
        switch (list[_pickedUpFrom].cardType)
        {
            case LOOP_CARD:
                {
                    if (list[_insertInt - 1].isLoop || list[_insertInt].isLoop || list[_insertInt - 1].isLoopHead || list[_insertInt].isLoopTail)
                    {
                        tempList.Clear();
                        Paixu();
                    }
                    else
                    {
                        int i = _pickedUpFrom;


                        while (list[i].cardType != LOOP_END)
                        {
                            tempList.Add(list[i++]);
                        }
                        if (list[i].cardType == LOOP_END)
                        {
                            tempList.Add(list[i]);
                        }
                        for (i = tempList.Count - 1; i >= 0; i--)
                        {
                            list.Insert(_insertInt, tempList[i]);

                        }
                    }

                }
                break;

            case IF_CARD:
                {
                    int i = _pickedUpFrom;

                    tempList.Add(list[i++]);
                    tempList.Add(list[i++]);
                    for (int a = 0; a < listLength; a++)
                    {
                        Debug.Log(list[a].cardType);
                    }
                    list.Remove(list[_pickedUpFrom]);
                    list.Remove(list[_pickedUpFrom]);
                    for (int a = 0; a < listLength - 2; a++)
                    {
                        Debug.Log(list[a].cardType);
                    }
                    i = i - 2;
                    if (_insertInt > _pickedUpFrom)
                    {
                        list.Insert(_insertInt - 2, tempList[0]);
                        list.Insert(_insertInt - 1, tempList[1]);
                    }
                    else
                    {
                        list.Insert(_insertInt, tempList[0]);
                        list.Insert(_insertInt + 1, tempList[1]);
                    }
                    Paixu();
                    tempList.Clear();
                }
                break;

            default:
                {

                    Node newNode = new Node
                    {
                        cardGameObject = list[_pickedUpFrom].cardGameObject,
                        cardType = list[_pickedUpFrom].cardType,
                        isLoopHead = false,
                        isLoop = true,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = list[_insertInt - 1].cardGameObject.transform,
                        linkIfField = null
                    };

                    list.Remove(list[_pickedUpFrom]);
                    newNode.cardGameObject.transform.SetParent(newNode.linkHead);
                    list.Insert(_insertInt, newNode);
                    Paixu();
                    //listLength = list.Count;
                    //int line = (listLength - 1) / 7;
                    //float myyoffset = line * yoffset;
                    //float myxoffset = xoffset * ((_insertInt) % 7);
                    //Vector3 newpos = new Vector3(card1.position.x + myxoffset, card1.position.y - myyoffset, card1.position.z);
                    //iTween.MoveTo(newNode.cardGameObject, newpos, 0.5f);
                    //Paixu();
                }
                break;

        }
    }

    private void CardToInsert_BehindCard_Generate(int _pickedUpFrom, int _insertInt)
    {
        switch (list[_pickedUpFrom].cardType)
        {
            case LOOP_CARD:
                {

                    int i = _pickedUpFrom;

                    while (list[i].cardType != LOOP_END)
                    {
                        tempList.Add(list[i++]);
                    }
                    if (list[i].cardType == LOOP_END)
                    {
                        tempList.Add(list[i++]);
                    }
                    for (int j = tempList.Count - 1; j >= 0; j--)
                    {

                        list.Remove(tempList[j]);
                        list.Insert(_insertInt, tempList[j]);

                    }
                    Paixu();
                    tempList.Clear();
                    listLength = list.Count;

                }
                break;

            case IF_CARD:
                {
                    int i = _pickedUpFrom;

                    tempList.Add(list[i++]);
                    tempList.Add(list[i++]);
                    for (int a = 0; a < listLength; a++)
                    {
                        Debug.Log(list[a].cardType);
                    }
                    list.Remove(list[_pickedUpFrom]);
                    list.Remove(list[_pickedUpFrom]);
                    for (int a = 0; a < listLength - 2; a++)
                    {
                        Debug.Log(list[a].cardType);
                    }
                    i = i - 2;
                    if (_insertInt > _pickedUpFrom)
                    {
                        list.Insert(_insertInt - 2, tempList[0]);
                        list.Insert(_insertInt - 1, tempList[1]);
                    }
                    else
                    {
                        list.Insert(_insertInt, tempList[0]);
                        list.Insert(_insertInt + 1, tempList[1]);
                    }
                    Paixu();
                    tempList.Clear();
                }
                break;

            default:
                {
                    Node newNode = new Node
                    {
                        cardGameObject = list[_pickedUpFrom].cardGameObject,
                        cardType = list[_pickedUpFrom].cardType,
                        isLoopHead = false,
                        isLoop = false,
                        isLoopTail = false,
                        loopLength = 0,
                        isIfHead = false,
                        isExecute = false,
                        ifType = NOT_IF,
                        linkHead = null,
                        linkIfField = null
                    };

                    list.Remove(list[_pickedUpFrom]);

                    list.Insert(_insertInt, newNode);
                    Paixu();
                }
                break;
        }
    }

    #endregion



    public void IfCardTypeChangeToFlower(int _slotInt)
    {
        Debug.Log(_slotInt);
        Node tempnode = list[_slotInt];
        tempnode.ifType = IF_Flower;
        list[_slotInt] = tempnode;
    }

    public void IfCardTypeChangeToHoney(int _slotInt)
    {
        Debug.Log(_slotInt);
        Node tempnode = list[_slotInt];
        tempnode.ifType = IF_Honey;
        list[_slotInt] = tempnode;
    }

    ////Visualizing List
    //private void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 800));
    //    GUILayout.BeginVertical();

    //    for (int i = 0; i <= listLength; i++)
    //    {
    //        GUILayout.Label("Node" + i + "-------" + list[i].cardType, textStyle);
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();

    //    //GUILayout.BeginArea(new Rect(800, 200, 400, 800));
    //    //GUILayout.BeginVertical();

    //    //foreach (string _slotID in slots_isLoop.Keys)
    //    //{
    //    //    GUILayout.Label(_slotID + "-------" + slots_isLoop[_slotID], textStyle);
    //    //}

    //    //GUILayout.EndVertical();
    //    //GUILayout.EndArea();
    //}

}
