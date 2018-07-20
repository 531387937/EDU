using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Loop : MonoBehaviour
{

    [SerializeField]
    private GameObject CardToDuplicate;
    [SerializeField]
    private RectTransform ParentToSet;

    [SerializeField]
    private GameObject Loop;
    [SerializeField]
    private GameObject LoopEnd;

    [SerializeField]
    private GameObject Forward;
    [SerializeField]
    private GameObject Left;
    [SerializeField]
    private GameObject Right;
    [SerializeField]
    private GameObject Jump;
    [SerializeField]
    private GameObject Push;

    private CanvasGroup canvasGroup;
    private bool isDuplicate;
    
    private CardType cardType_com;
    private int cardType;
    private Image cardImage;
    public int loopLength;

    private int error = 99;
    private int empty = 88;
    public int pickedUpFrom;

    private void Start()
    {
        isDuplicate = false;
        canvasGroup = GetComponent<CanvasGroup>();
        cardType_com = GetComponent<CardType>();
        cardType = cardType_com.type;
        cardImage = GetComponent<Image>();
        pickedUpFrom = 0;
    }

    public void MouseDown()
    {
        if (!isDuplicate)
        {
            GameObject tempObj;
            tempObj = Instantiate(CardToDuplicate, gameObject.transform.position, gameObject.transform.rotation);
            tempObj.transform.SetParent(ParentToSet, true);
            tempObj.transform.name = CardToDuplicate.transform.name;
        }

        pickedUpFrom = StatsManager.instance.currentSlot;
    }

    public void MouseDrag()
    {
        // N/A clear when Dragged
        gameObject.transform.position = Input.mousePosition;

        canvasGroup.blocksRaycasts = false;

    }

    public void MouseDrop()
    {
        int currentSlot = StatsManager.instance.currentSlot;
        int currentInsert = StatsManager.instance.currentInsert;


        #region Logic

        //if (wasPlaced)
        //{
        //    //Card form the Queue

        //    string[] pieces = placedSlot.Split('_');
        //    int placedSlot_int = int.Parse(pieces[1]);

        //    if (currentSlot == null)
        //    {
        //        if (currentInsert == null)
        //        {
        //            //Placed neither inside a slot or a insert area
        //            Debug.Log("Placed neither inside a slot or a insert area");

        //            LoopPlacedOutOfBounds(placedSlot_int);                    

        //        }
        //        else
        //        {
        //            int insert_int = int.Parse(currentInsert);

        //            if (insert_int >= placedSlot_int && insert_int <= (placedSlot_int + loopLength + 2))
        //            {
        //                //Inserted at its previous position
        //                Debug.Log("Inserted at its previous position");

        //                // [ Cards inside the Loop need to be Considered... ]
        //                // [ N/A clear on Drag ]
        //                LoopInsertAtPreviousPosition();
                        
        //            }
        //            else if (insert_int < placedSlot_int)
        //            {
        //                //Inserted before previous position
        //                Debug.Log("Inserted before previous position");

        //                LoopInsertBeforePreviousPosition(placedSlot_int, insert_int);

        //            }
        //            else
        //            {
        //                if (insert_int > (placedSlot_int + loopLength + 2) && insert_int < StatsManager.instance.nextSlot)
        //                {
        //                    //Inserted after previous position but still inside Queue
        //                    Debug.Log("Inserted after previous position but still inside Queue");

        //                    LoopInsertAfterPreviousPositionStillInsideQueue(placedSlot_int, insert_int);
                            
        //                }
        //                else
        //                {
        //                    //Inserted after previous position and out of Queue
        //                    Debug.Log("Inserted after previous position and out of Queue");

        //                    LoopInsertAfterPreviousPositionOutOfQueue(placedSlot_int);
                            
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        string[] pieces2 = currentSlot.Split('_');
        //        int currentSlot_int = int.Parse(pieces2[1]);

        //        if (currentSlot == placedSlot)
        //        {
        //            //Placed at its previous position
        //            Debug.Log("Placed at its previous position");

        //            LoopPlacedAtPreviousPosition();

        //        }
        //        else
        //        {
        //            if (currentSlot_int >= StatsManager.instance.nextSlot)
        //            {
        //                //Placed at an empty slot out of the Queue
        //                Debug.Log("Placed at an empty slot out of the Queue");

        //                LoopPlacedAtEmptySlotOutOfQueue(placedSlot_int);

        //            }
        //            else
        //            {
        //                //Placed at a slot that is already taken inside the Queue
        //                Debug.Log("Placed at a slot that is already taken inside the Queue");

        //                LoopPlacedAtTakenSlotInsideQueue(placedSlot_int);

        //            }
        //        }
        //    }


        //}
        //else
        //{
        //    //Card from the inventory

        //    if (currentSlot != null)
        //    {
        //        //New card placed to a slot
        //        Debug.Log("New Loop card placed to a slot");

        //        NewLoopToSlot(currentSlot);
        //    }
        //    else if (currentInsert != null)
        //    {
        //        int insert_int = int.Parse(currentInsert);

        //        if (insert_int > StatsManager.instance.nextSlot)
        //        {
        //            //Inserted out of the Queue
        //            Debug.Log("New Loop Card Inserted out of the Queue");

        //            NewLoopInsertOutOfQueue();
        //        }
        //        else
        //        {
        //            //Inserted inside the Queue
        //            Debug.Log("New Loop Card Inserted inside the Queue");

        //            NewLoopInsertInsideQueue(insert_int);
        //        }
        //    }
        //    else
        //    {
        //        Destroy(gameObject);
        //    }
        //}

        #endregion


        //canvasGroup.blocksRaycasts = true;
        //isDuplicate = true;
    }


}