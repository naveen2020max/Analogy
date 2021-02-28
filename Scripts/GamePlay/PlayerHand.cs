using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHand : MonoBehaviourPunCallbacks,IMatchStateCallBacks
{

    public List<CardInfo> Desk;
    public Card[] HandCards;
    public Card SelectedCard;
    public PlayerData playerData;

    private List<Card> EmptyCards;
    private Transform AspectSelectionPanel;
    private UnityEngine.UI.Button EndOfRoundButton;
    private bool m_startCalled = false;
    

    
    // Start is called before the first frame update
    void StartFun()
    {
        SelectedCard.gameObject.SetActive(false);
        

        if (photonView.IsMine)
        {
            //if (MatchExecutor.Instance == null)
            //{
            //    Debug.Log(photonView.ViewID + " MatchExecutor Instance");
            //    PhotonNetwork.Instantiate("MatchexecutorPrefeb", Vector3.zero, Quaternion.identity);

            //}
            playerData = new PlayerData(photonView.ViewID);
            Desk = new List<CardInfo>();
            EmptyCards = new List<Card>();
            EndOfRoundButton = GameManager.Instance.EndOfRoundButtonParent.GetComponentInChildren<UnityEngine.UI.Button>();
            EndOfRoundButton.gameObject.SetActive(false);
            // HandCards = new Card[4];
            List<CardInfo> temp = new List<CardInfo>();
            foreach (CardInfo card in CardDataBase.Instance.AllCards.Values)
            {
                if (card.ID!=0)
                {
                    temp.Add(card);
                    //Desk.Add(card); 
                }
            }
            while (temp.Count>0)
            {
                int rand = Random.Range(0, temp.Count);
                Desk.Add(temp[rand]);
                temp.RemoveAt(rand);
            }


            GetComponent<MeshRenderer>().material.color = Color.blue;

            AssignHandCards();

            AspectSelectionPanel = GameManager.Instance.AspectSelectionParent;

            MatchExecutor.Instance.AddPlayerList(playerData);
            EndOfRoundButton.onClick.AddListener(MatchExecutor.Instance.EndOfRoundButton);

            //Debug.Log("Calling Instance");


            //Debug.Log("PLayerHand");
            MatchExecutor.Instance.E_OnMatchOver += OnMatchOver;

        }
        MatchExecutor.Instance.E_OnAspectSelectionState += OnAspectSelectionState;
        MatchExecutor.Instance.E_OnCardSelectionState += OnCardSelectionState;
        MatchExecutor.Instance.E_OnCardComparingState += OnCardComparingState;
        MatchExecutor.Instance.E_OnEndOfRoundState += OnEndOfRoundState;


        //SelectedCard = GameManager.Instance.LocalSelectedCardParent.GetComponentInChildren<Card>();

    }


    // Update is called once per frame
    void Update()
    {
        if (!m_startCalled)
        {
            if (MatchExecutor.Instance!=null)
            {
                StartFun();
                m_startCalled = true; 
            }
        }
    }

    private void AssignHandCards()
    {
        HandCards=GameManager.Instance.LocalHandCardsParent.GetComponentsInChildren<Card>();

        //Debug.Log("HandCard " + HandCards.Length);


        for (int i = 0; i < HandCards.Length; i++)
        {
            HandCards[i].ChangeCard(Desk[i].ID);
            Desk.RemoveAt(i);
            HandCards[i].OnCardSelected += CardSelected;
        }
        GameManager.Instance.UpdateCardCount(Desk.Count);
    }
    private void CardSelected(Card _card)
    {
        if (playerData.m_IsCardSelected==false)
        {
            for (int i = 0; i < HandCards.Length; i++)
            {
                if (HandCards[i].CardNumber == _card.CardNumber)
                {
                    playerData.m_IsCardSelected = true;
                    playerData.m_SelectedCardId = _card.cardInfo.ID;
                    MatchExecutor.Instance.UpdatePlayerdata(playerData);
                    photonView.RPC("RPC_ChangeSelectedCard", RpcTarget.All, _card.cardInfo.ID);
                    //SelectedCard.ChangeCard(_card.cardInfo.ID);
                    if (Desk.Count>0)
                    {
                        HandCards[i].ChangeCard(Desk[0].ID);
                        Desk.RemoveAt(0); 
                    }else if (Desk.Count == 0)
                    {
                        HandCards[i].gameObject.SetActive(false);
                        EmptyCards.Add(HandCards[i]);
                    }

                    GameManager.Instance.UpdateCardCount(Desk.Count);

                    MatchExecutor.Instance.CheckPlayersSelectedCard();
                    //Debug.Log("Card DisCarded");
                    return;
                }
            } 
        }
    }

    public void OnAspectSelectionState()
    {
        SelectedCard.gameObject.SetActive(false);
        if (photonView.IsMine)
        {
            Debug.Log("AspectSelectionState");
            if (MatchExecutor.Instance.CurrentStartPlayer.IsLocal)
            {
                MatchExecutor.Instance.UpdateRoundCount(MatchExecutor.Instance.RoundCount+1);
                AspectSelectionPanel.gameObject.SetActive(true);
                MatchExecutor.Instance.ChangeAspect(AspectType.NONE);
            }
            else
            {
                AspectSelectionPanel.gameObject.SetActive(false);
                EndOfRoundButton.gameObject.SetActive(false);

            }

        }
       
    }

    public void OnCardSelectionState()
    {
        if (photonView.IsMine)
        {
            Debug.Log("Card selection state");
        }
        if (MatchExecutor.Instance.CurrentStartPlayer.IsLocal&&photonView.IsMine)
        {
            AspectSelectionPanel.gameObject.SetActive(false);
        }
    }

    public void OnCardComparingState()
    {

        if (photonView.IsMine)
        {
            Debug.Log("Card comparing state");

            int[] WinPack;
            PlayerData Winner = MatchExecutor.Instance.CompareCards(out WinPack);
            //MatchExecutor.Instance.WriteGameConsole(Winner.m_Id + " is winner");

            if(Winner.m_Id == 0)
            {
                Desk.Add(SelectedCard.cardInfo);
                GameManager.Instance.AddPlayerConsoleEntry("Drew");
                EndOfRoundButton.gameObject.SetActive(true);
                MatchExecutor.Instance.ChangeMatchState(MatchState.EndOfRound);

            }
            else if (photonView.ViewID != Winner.m_Id)
            {
                GameManager.Instance.AddPlayerConsoleEntry("You Lose");
                EndOfRoundButton.gameObject.SetActive(true);
                Debug.Log("EndOfRound Button is Popped");

            }else if (photonView.ViewID == Winner.m_Id)
            {
                foreach (int item in WinPack)
                {
                    Desk.Add(CardDataBase.Instance.GetCardInfo(item));
                    Debug.Log(item + "  Looted");
                }
                MatchExecutor.Instance.UpdateStartPlayer(photonView.Owner);
                MatchExecutor.Instance.WriteGameConsole(PhotonNetwork.LocalPlayer.NickName+" is winner");
                MatchExecutor.Instance.ChangeMatchState(MatchState.EndOfRound);
                GameManager.Instance.AddPlayerConsoleEntry("You Won!!");
                

            }
            GameManager.Instance.UpdateCardCount(Desk.Count);

        }
        else
        {
            SelectedCard.gameObject.GetComponent<CardLayout>().HideCard(false);
            Debug.Log("UnHided");
        }
    }

    public void OnEndOfRoundState()
    {
        if (photonView.IsMine)
        {
            Debug.Log("End Of Round");

            playerData.m_IsCardSelected = false;
            playerData.m_SelectedCardId = 0;
            MatchExecutor.Instance.UpdatePlayerdata(playerData);

            if (EmptyCards.Count > 0&&Desk.Count>0)
            {
                for (; EmptyCards.Count!=0;)
                {
                    if (Desk.Count == 0)
                        break;
                    EmptyCards[0].ChangeCard(Desk[0].ID);
                    Desk.RemoveAt(0);
                    EmptyCards.RemoveAt(0);
                }
            }
            GameManager.Instance.UpdateCardCount(Desk.Count);

            if (EmptyCards.Count >= 4)
            {
                //MatchExecutor.Instance.MatchOver(photonView.ViewID,false);
                MatchExecutor.Instance.RemovePlayer(playerData);
            }
        }
        
    }

    private void OnMatchOver()
    {
        if (photonView.IsMine)
        {
            if (MatchExecutor.Instance.MatchWinner.m_Id == photonView.ViewID)
            {
                MatchWon();
            }
            else
            {
                MatchLost();
            }
        }
    }

    private void MatchWon()
    {
        if (photonView.IsMine)
        {
            GameManager.Instance.WinPanel.SetActive(true);
        }
    }

    private void MatchLost()
    {
        if (photonView.IsMine)
        {
            GameManager.Instance.LossPanel.SetActive(true);

        }
    }


    #region PunRPCs

    [PunRPC]
    public void RPC_ChangeSelectedCard(int _CardID)
    {
        SelectedCard.ChangeCard(_CardID);
        SelectedCard.gameObject.SetActive(true);
        if (photonView.IsMine)
        {
            SelectedCard.GetComponent<CardLayout>().HideCard(false);
        }
        else
        {
            SelectedCard.GetComponent<CardLayout>().HideCard(true);

        }
    }

    #endregion

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (MatchExecutor.Instance.PlayerIdList.Count <= 1)
        {
            MatchExecutor.Instance.MatchOver(photonView.ViewID, true); 
        }
    }
    public override void OnLeftRoom()
    {
        MatchExecutor.Instance.RemovePlayer(playerData);
    }
}
