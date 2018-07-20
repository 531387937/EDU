using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAwake : MonoBehaviour
{
    [SerializeField]
    private GameObject CardToDuplicate;
    [SerializeField]
    private RectTransform ParentToSet;
    public int onAwake;
    private CanvasGroup canvasGroup;
    private bool isDuplicate;

    private CardType cardType_com;
    private int cardType;

    private int error = 99;
    private int empty = 88;
    public GameObject nextCard;
    public int pickedUpFrom;
    private void Awake()
    {
        isDuplicate = false;
        canvasGroup = GetComponent<CanvasGroup>();

        pickedUpFrom = 0;
    }
    // Use this for initialization
    void Start()
    {
        cardType_com = GetComponent<CardType>();
        cardType = cardType_com.type;
        print(cardType);
        for (int i = 0; i < StatsManager.instance.list.Count; i++)
        {
            if (StatsManager.instance.list[i].cardGameObject == CardToDuplicate)
                StatsManager.instance.currentSlot = i + 1;
        }

        pickedUpFrom = StatsManager.instance.currentSlot - 1; ;

        int currentSlot = onAwake;


        int currentInsert = 0;

        StatsManager.instance.listLength = StatsManager.instance.list.Count;
        if (!isDuplicate)
        {
            //Card from inventory
            if (currentSlot != 0 && (currentInsert == 0))
            {
                //New card to Slot
                Debug.Log("New card to Slot");
                StatsManager.instance.NewCardToSlot(cardType, currentSlot, gameObject);
                isDuplicate = true;
                canvasGroup.blocksRaycasts = true;
            }
            else if (currentSlot == 0 && currentInsert != 0)
            {
                //New card to Insert
                Debug.Log("New card to Insert");

                StatsManager.instance.NewCardToInsert(cardType, currentInsert - 1, gameObject);//////////////////////////////
                isDuplicate = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                //Out of Bounds
                Debug.Log("New card placed out of bounds");


                Destroy(gameObject);

            }

        }
        else
        {
            //Card from timeline
            if (currentSlot != 0 && currentInsert == 0)
            {
                //Card to Slot
                Debug.Log("Card to Slot");

                StatsManager.instance.CardToSlot(pickedUpFrom, currentSlot);
                canvasGroup.blocksRaycasts = true;
            }
            else if (currentSlot == 0 && currentInsert != 0)
            {
                //Card to Insert
                Debug.Log("Card to Insert");

                StatsManager.instance.CardToInsert(pickedUpFrom, currentInsert - 1);
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                //Out of Bounds
                Debug.Log("Card placed out of bounds");

                StatsManager.instance.CardToOutOfBounds(pickedUpFrom);
            }
        }

        if (nextCard != null)
        {
            nextCard.SetActive(true);
        }
    }
    public void MouseDown()
    {
        //isDuplicate = false ;
        if (!isDuplicate)
        {
            GameObject tempObj;
            tempObj = Instantiate(CardToDuplicate, gameObject.transform.position, gameObject.transform.rotation);
            tempObj.transform.SetParent(ParentToSet, true);
            tempObj.transform.name = CardToDuplicate.transform.name;

        }
        for (int i = 0; i < StatsManager.instance.list.Count; i++)
        {
            if (StatsManager.instance.list[i].cardGameObject == CardToDuplicate)
                StatsManager.instance.currentSlot = i + 1;
        }

        pickedUpFrom = StatsManager.instance.currentSlot - 1;

    }

    public void MouseDrag()
    {

        gameObject.transform.position = Input.mousePosition;

        canvasGroup.blocksRaycasts = false;

    }

    public void MouseDrop()
    {

        int currentSlot = StatsManager.instance.currentSlot;


        int currentInsert = StatsManager.instance.currentInsert;
        
        StatsManager.instance.listLength = StatsManager.instance.list.Count;
        if (!isDuplicate)
        {
            //Card from inventory
            if (currentSlot != 0 && (currentInsert == 0))
            {
                //New card to Slot
                Debug.Log("New card to Slot");
                StatsManager.instance.NewCardToSlot(cardType, currentSlot, gameObject);
                isDuplicate = true;
                canvasGroup.blocksRaycasts = true;
            }
            else if (currentSlot == 0 && currentInsert != 0)
            {
                //New card to Insert
                Debug.Log("New card to Insert");

                StatsManager.instance.NewCardToInsert(cardType, currentInsert - 1, gameObject);//////////////////////////////
                isDuplicate = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                //Out of Bounds
                Debug.Log("New card placed out of bounds");


                Destroy(gameObject);

            }

        }
        else
        {
            //Card from timeline
            if (currentSlot != 0 && currentInsert == 0)
            {
                //Card to Slot
                Debug.Log("Card to Slot");

                StatsManager.instance.CardToSlot(pickedUpFrom, currentSlot);
                canvasGroup.blocksRaycasts = true;
            }
            else if (currentSlot == 0 && currentInsert != 0)
            {
                //Card to Insert
                Debug.Log("Card to Insert");

                StatsManager.instance.CardToInsert(pickedUpFrom, currentInsert - 1);
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                //Out of Bounds
                Debug.Log("Card placed out of bounds");

                StatsManager.instance.CardToOutOfBounds(pickedUpFrom);
            }
        }

    }
}