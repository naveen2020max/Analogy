using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    public CardInfo cardInfo { get; private set; }
    public int CardNumber;

    public event Action<CardInfo> OnCardChange;
    public event Action<Card> OnCardSelected;

    public float offsetY=30f;

    private Vector3 StartPosition;
    private RectTransform rect;

    public Card()
    {
        
    }

    public Card(int cardID)
    {
        this.cardInfo = CardDataBase.Instance.GetCardInfo(cardID);
    }

    public CardInfo ChangeCard(int cardID)
    {
        gameObject.SetActive(true);
        cardInfo = CardDataBase.Instance.GetCardInfo(cardID);
        OnCardChange?.Invoke(cardInfo);
        //Debug.Log("card Changed");
        return cardInfo;
    }

    public void Activate(bool _value)
    {
        gameObject.SetActive(_value);

    }

    #region UnityMethods

    

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        StartPosition = rect.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MatchExecutor.Instance!=null)
        {
            if (MatchExecutor.Instance.CurrentState == MatchState.CardSelection)
            {
                rect.position = StartPosition + Vector3.up * offsetY;
            } 
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (MatchExecutor.Instance!=null)
        {
            if (MatchExecutor.Instance.CurrentState == MatchState.CardSelection)
            {
                rect.position = StartPosition;
            } 
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (MatchExecutor.Instance!=null)
        {
            if (MatchExecutor.Instance.CurrentState == MatchState.CardSelection)
            {
                OnCardSelected?.Invoke(this);

            } 
        }
    }
    #endregion
}
