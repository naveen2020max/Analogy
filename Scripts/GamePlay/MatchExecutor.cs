using System;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.IO;
using ExitGames.Client.Photon;
using System.Collections;

public enum MatchState { None,AspectSelection,CardSelection,CardComparing,Scoring,EndOfRound}


public class MatchExecutor : MonoBehaviourPun
{
    //Singleton
    public static MatchExecutor Instance;

    //States
    public MatchState CurrentState;
    public AspectType CurrentRoundAspect;

    //Player Handling
    public Player CurrentStartPlayer;
    public Dictionary<int, PlayerData> PlayerList;
    public List<int> PlayerIdList;
    public PlayerData MatchWinner;

    //events
    public event Action E_OnMatchStateChange;
    public event Action E_OnAspectSelectionState;
    public event Action E_OnCardSelectionState;
    public event Action E_OnCardComparingState;
    public event Action E_OnEndOfRoundState;
    public event Action E_OnMatchOver;

    //Round State
    public int RoundCount = 0;


    //private fields
    private MatchState m_PreviousState=MatchState.None;
    private Text AspectTypeIndicator;
    private Text MatchStateIndicator;
    private Text ConsoleText;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("MatchExecutor Instantiated");

            //Debug.Log("Instance");
        }
        else
        {
            Debug.LogError("two Instances");
        }
    }

    private void Awake()
    {
        PhotonPeer.RegisterType(typeof(PlayerData), (byte)'D', SerializePlayerData, DeserializePlayerData);
        
        PlayerList = new Dictionary<int, PlayerData>();
        PlayerIdList = new List<int>();
        CurrentStartPlayer = PhotonNetwork.PlayerList[0];
        
        CurrentState = MatchState.AspectSelection;
        E_OnMatchStateChange += OnMatchStateChange;
    }

    // Start is called before the first frame update
    void Start()
    {
        AspectTypeIndicator = GameManager.Instance.AspectIndocatorParent.Find("AspectIndicatorText").GetComponent<Text>();
        MatchStateIndicator = GameManager.Instance.AspectIndocatorParent.Find("MatchStateIndicatorText").GetComponent<Text>();
        ConsoleText = GameManager.Instance.AspectIndocatorParent.Find("Console").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_PreviousState != CurrentState)
        {
            //Debug.Log("Calling CallBacks");
            m_PreviousState = CurrentState;
            E_OnMatchStateChange?.Invoke();
            MatchStateIndicator.text = CurrentState.ToString();
        }
    }

    #region private methods
    private void StateChecker()
    {
        //Debug.Log("MatchExecutor");
        switch (CurrentState)
        {
            case MatchState.None:
                break;
            case MatchState.AspectSelection:

                E_OnAspectSelectionState?.Invoke();
                break;
            case MatchState.CardSelection:
                E_OnCardSelectionState?.Invoke();
                break;
            case MatchState.CardComparing:
                E_OnCardComparingState?.Invoke();
                break;
            case MatchState.EndOfRound:
                E_OnEndOfRoundState?.Invoke();
                break;
            default:
                break;
        }
    }

    private void UpdateAspectIndicator(string _aspecttype)
    {
        GameManager.Instance.AspectIndocatorParent.gameObject.SetActive(true);
        GameManager.Instance.AspectIndocatorParent.Find("AspectIndicatorText").GetComponent<Text>().text = _aspecttype;
    }

    IEnumerator StateDelay(MatchState _state)
    {
        yield return new WaitForSeconds(0.5f);
        photonView.RPC("RPC_ChangeMatchState", RpcTarget.AllBuffered, _state);

    }

    #endregion

    #region Custom CallBacks

    public void OnMatchStateChange()
    {
        //Debug.Log("State Changed");
        StateChecker();
    }

    #endregion

    #region public Methods

    public void CheckPlayersSelectedCard()
    {
        bool AllSelected = false;

        foreach (PlayerData data in PlayerList.Values)
        {
            AllSelected = data.m_IsCardSelected;
            //Debug.Log(data.m_Id + " " + data.m_IsCardSelected);
            if (AllSelected == false)
            {
                break;
            }
        }

        if (AllSelected == true)
        {
            ChangeMatchState(MatchState.CardComparing);
            WriteGameConsole("All of them Selected");
        }

    }

    public PlayerData CompareCards(out int[] _WinPack)
    {

        PlayerData Winner = new PlayerData(0);
        PlayerData P1 = PlayerList[PlayerIdList[0]];
        PlayerData P2 = PlayerList[ PlayerIdList[1]];

        PlayerData[] players = new PlayerData[PlayerList.Count];
        _WinPack = new int[PlayerList.Count];

        for (int i = 0; i < PlayerList.Count; i++)
        {
            players[i] = PlayerList[PlayerIdList[i]];
            _WinPack[i] = players[i].m_SelectedCardId;
        }
        int highestvalue = (CurrentRoundAspect == AspectType.RANK) ? 100 : -1;

        for (int i = 0; i < players.Length; i++)
        {
            if(GetAspectValue(players[i]) == highestvalue)
            {
                return new PlayerData(0);
            }

            if(CurrentRoundAspect == AspectType.RANK)
            {
                if(GetAspectValue(players[i]) < highestvalue)
                {
                    highestvalue = GetAspectValue(players[i]);
                }
            }
            else
            {
                if (GetAspectValue(players[i]) > highestvalue)
                {
                    highestvalue = GetAspectValue(players[i]);
                }
            }
        }
        foreach (PlayerData player in players)
        {
            if(GetAspectValue(player) == highestvalue)
            {
                Winner = player;
            }
        }

        //if (GetAspectValue(P1) == GetAspectValue(P2))
        //{
        //    return new PlayerData(0);
        //}

        //if (CurrentRoundAspect == AspectType.RANK)
        //{
        //    if(GetAspectValue(P1)< GetAspectValue(P2))
        //    {
        //        Winner = P1;
        //    }
        //    else
        //    {
        //        Winner = P2;
        //    }
        //}
        //else
        //{
        //    if (GetAspectValue(P1) > GetAspectValue(P2))
        //    {
        //        Winner = P1;
        //    }
        //    else
        //    {
        //        Winner = P2;
        //    }
        //}
        //WriteGameConsole(Winner.m_Id + " is the Winner");
        return Winner;
    }
    #region backup
    //public PlayerData CompareCards(out int[] _WinPack)
    //{
    //    PlayerData Winner;
    //    PlayerData P1 = PlayerList[PlayerIdList[0]];
    //    PlayerData P2 = PlayerList[PlayerIdList[1]];

    //    _WinPack = new int[2] { P1.m_SelectedCardId, P2.m_SelectedCardId };

    //    if (GetAspectValue(P1) == GetAspectValue(P2))
    //    {
    //        return new PlayerData(0);
    //    }

    //    if (CurrentRoundAspect == AspectType.RANK)
    //    {
    //        if (GetAspectValue(P1) < GetAspectValue(P2))
    //        {
    //            Winner = P1;
    //        }
    //        else
    //        {
    //            Winner = P2;
    //        }
    //    }
    //    else
    //    {
    //        if (GetAspectValue(P1) > GetAspectValue(P2))
    //        {
    //            Winner = P1;
    //        }
    //        else
    //        {
    //            Winner = P2;
    //        }
    //    }
    //    //WriteGameConsole(Winner.m_Id + " is the Winner");
    //    return Winner;
    //}
    #endregion
    private int GetAspectValue(PlayerData _data)
    {
        return CardDataBase.Instance.GetCardInfo(_data.m_SelectedCardId).Aspects[CurrentRoundAspect].Value;
    }

    public void UpdateMatchStateIndicatorText()
    {

    }
    public void EndOfRoundButton()
    {
        ChangeMatchState(MatchState.AspectSelection);
    }

    #endregion

    #region public RPCCalls

    public void ChangeAspect(AspectType _aspectType)
    {
        //CurrentRoundAspect = _aspectType;
        photonView.RPC("RPC_ChangeAspect", RpcTarget.All, _aspectType);

    }

    public void ChangeMatchState(MatchState _state)
    {
        StartCoroutine(StateDelay(_state));
    }

    public void AddPlayerList(PlayerData _playerData)
    {
        photonView.RPC("RPC_AddPlayerList", RpcTarget.All, _playerData);
    }

    public void RemovePlayer(PlayerData _playerData)
    {
        photonView.RPC("RPC_RemovePlayer", RpcTarget.All, _playerData);
    }

    public void UpdatePlayerdata(PlayerData _data)
    {
        photonView.RPC("RPC_UpdatePlayerdata", RpcTarget.All, _data);
    }

    public void UpdateStartPlayer(Player _player)
    {
        photonView.RPC("RPC_UpdateStartPlayer", RpcTarget.All, _player);

    }
    public void WriteGameConsole(string _Content)
    {
        photonView.RPC("RPC_WriteGameConsole", RpcTarget.All,_Content);
    }

    public void UpdateRoundCount(int _count)
    {
        photonView.RPC("RPC_UpdateRoundCount", RpcTarget.All,_count);

    }

    public void MatchOver(int _Playerid,bool IsWinner)
    {
        photonView.RPC("RPC_MatchOver", RpcTarget.All, _Playerid,IsWinner);

    }

    #endregion

    #region PunRPCs

    [PunRPC]
    public void RPC_ChangeAspect(AspectType _aspecttype)
    {
        CurrentRoundAspect = _aspecttype;

        if (_aspecttype!=AspectType.NONE)
        {
            CurrentState = MatchState.CardSelection; 
        }
        UpdateAspectIndicator(_aspecttype.ToString());
    }

    [PunRPC]
    public void RPC_ChangeMatchState(MatchState _state)
    {
        CurrentState = _state;
       // Debug.Log("matchExecutor " + _state.ToString());
    }

    [PunRPC]
    public void RPC_AddPlayerList(PlayerData _playerData)
    {
        if (!PlayerList.ContainsKey(_playerData.m_Id))
        {
            PlayerList.Add(_playerData.m_Id, _playerData);
            PlayerIdList.Add(_playerData.m_Id);

        }
    }

    [PunRPC]
    public void RPC_RemovePlayer(PlayerData _playerData)
    {
        if (PlayerList.ContainsKey(_playerData.m_Id))
        {
            PlayerList.Remove(_playerData.m_Id);
            PlayerIdList.Remove(_playerData.m_Id);
        }
    }

    [PunRPC]
    public void RPC_UpdatePlayerdata(PlayerData _data)
    {
        if (PlayerList.ContainsKey(_data.m_Id))
        {
            PlayerList[_data.m_Id] = _data;
        }
    }

    [PunRPC]
    public void RPC_UpdateStartPlayer(Player _player)
    {
        CurrentStartPlayer = _player;
        Debug.Log(_player.NickName + " is next StartPlayer");
    }

    [PunRPC]
    public void RPC_WriteGameConsole(string _Content)
    {
        ConsoleText.text = _Content;
        GameManager.Instance.AddPlayerConsoleEntry(_Content);

        //Debug.Log(_Content);
    }

    [PunRPC]
    public void RPC_UpdateRoundCount(int _count)
    {
        RoundCount = _count;
        ConsoleText.text = "Round "+RoundCount;
    }

    [PunRPC]
    public void RPC_MatchOver(int _playerId,bool isWiner)
    {
        foreach (PlayerData player in PlayerList.Values)
        {
            if (player.m_Id != _playerId && isWiner==false)
            {
                MatchWinner = player;
                if (PhotonNetwork.IsMasterClient)
                {
                    E_OnMatchOver?.Invoke();
                }
            }else if (player.m_Id == _playerId && isWiner == true)
            {
                MatchWinner = player;
                if (PhotonNetwork.IsMasterClient)
                {
                    E_OnMatchOver?.Invoke();
                }
            }
        }
    }

    #endregion

    #region Custom De/serializer Method

    private static byte[] SerializePlayerData(object customobject)
    {
        PlayerData data = (PlayerData)customobject;
        MemoryStream ms = new MemoryStream(2 * 4 + 1);

        ms.Write(BitConverter.GetBytes(data.m_Id), 0, 4);
        ms.Write(BitConverter.GetBytes(data.m_SelectedCardId), 0, 4);
        ms.Write(BitConverter.GetBytes(data.m_IsCardSelected), 0, 1);
        return ms.ToArray();
    }

    private static object DeserializePlayerData(byte[] bytes)
    {
        PlayerData data = new PlayerData();
        data.m_Id = BitConverter.ToInt32(bytes, 0);
        data.m_SelectedCardId = BitConverter.ToInt32(bytes, 4);
        data.m_IsCardSelected = BitConverter.ToBoolean(bytes, 8);
        return data;
    }

    #endregion
}
