using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static bool isLogined = false;
    #region PublicFields

    [Header("All the Panels")]
    //public GameObject MenuPanel;
    public GameObject Login_Panel;
    public GameObject GameOptions_Panel;
    public GameObject CreateRoom_Panel;
    public GameObject InsideRoom_Panel;
    public GameObject RoomList_Panel;

    [Header("Login UI Panel")]
    public TMP_InputField PlayerNameIF;
    [Header("GameOption UI Panel")]
    public TMP_InputField NickNameIF;
    [Header("Create Room Panel")]
    public TMP_InputField RoomNameIF;
    [Header("Show Room List Panel")]
    public GameObject RoomPanelPrefab;
    public Transform RoomPanelParent;
    [Header("Inside Room Panel")]
    public GameObject PlayerPanelPrefab;
    public Transform PlayerPanelParent;
    public GameObject startGameButton;
    public TMP_Text roomInfoText;


    private Dictionary<string, RoomInfo> m_RoomList;
    private Dictionary<string, GameObject> m_RoomPanelGOs;
    private Dictionary<int, GameObject> m_PlayerPanelGOs;

    #endregion
    
    #region UnityMethods
    // Start is called before the first frame update
    void Start()
    {
        if (!isLogined)
        {
            ActivatePanel(Login_Panel.name);
        }
        else
        {
            ActivatePanel(GameOptions_Panel.name);
        }
        m_RoomList = new Dictionary<string, RoomInfo>();
        m_RoomPanelGOs = new Dictionary<string, GameObject>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion


    #region PUNCallBacks

    public override void OnConnected()
    {
        Debug.Log("Connect to Internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " IsConnected To Photon");
        ActivatePanel(GameOptions_Panel.name);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name + " is Created");
    }
    public override void OnJoinedRoom()
    {
        if (m_PlayerPanelGOs == null)
        {
            m_PlayerPanelGOs = new Dictionary<int, GameObject>();
        }
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is Joined the" + PhotonNetwork.CurrentRoom.Name + " room");
        ActivatePanel(InsideRoom_Panel.name);
        roomInfoText.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name + " \n " + "Players Count/MaxCount : "
            + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject Player = Instantiate(PlayerPanelPrefab, PlayerPanelParent);
            Player.transform.localScale = Vector3.one;

            Player.transform.Find("PlayerNameText").GetComponent<TMP_Text>().text = player.NickName;
            if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                Player.transform.Find("PlayerIndicator").gameObject.SetActive(true);
            }
            else
            {
                Player.transform.Find("PlayerIndicator").gameObject.SetActive(false);

            }
            m_PlayerPanelGOs.Add(player.ActorNumber, Player);
        }
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
        else
        {
            startGameButton.SetActive(false);
        }

    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListview();
       
        Debug.Log(roomList.Count);
        
        foreach (RoomInfo room in roomList)
        {
            Debug.Log(room.Name);
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (m_RoomList.ContainsKey(room.Name))
                {
                    m_RoomList.Remove(room.Name);
                }
            }
            else
            {
                if (m_RoomList.ContainsKey(room.Name))
                {
                    m_RoomList[room.Name] = room;
                }
                else
                {
                    m_RoomList.Add(room.Name, room);
                    Debug.Log("added");
                }
            }
        }

        foreach (RoomInfo room in m_RoomList.Values)
        {
            Debug.Log("created");
            GameObject RoomEntry = Instantiate(RoomPanelPrefab, RoomPanelParent);

            RoomEntry.transform.localScale = Vector3.one;

            RoomEntry.transform.Find("RoomName").GetComponent<TMP_Text>().text = room.Name;
            RoomEntry.transform.Find("RoomMaxPlayer").GetComponent<TMP_Text>().text = room.PlayerCount+"/"+room.MaxPlayers;
            RoomEntry.transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(()=>OnJoinRoomButtonClicked(room.Name));


            m_RoomPanelGOs.Add(room.Name, RoomEntry);


        }

    }

    public override void OnLeftLobby()
    {
        ClearRoomListview();
        m_RoomList.Clear();
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        roomInfoText.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name + " /n " + "Players Count/MaxCount : " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        GameObject Player = Instantiate(PlayerPanelPrefab, PlayerPanelParent);
        Player.transform.localScale = Vector3.one;

        Player.transform.Find("PlayerNameText").GetComponent<TMP_Text>().text = player.NickName;
        if (player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            Player.transform.Find("PlayerIndicator").gameObject.SetActive(true);
        }
        else
        {
            Player.transform.Find("PlayerIndicator").gameObject.SetActive(false);

        }
        m_PlayerPanelGOs.Add(player.ActorNumber, Player);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        roomInfoText.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name + " /n " + "Players Count/MaxCount : " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        Destroy(m_PlayerPanelGOs[otherPlayer.ActorNumber].gameObject);
        m_PlayerPanelGOs.Remove(otherPlayer.ActorNumber);
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
    }

    public override void OnLeftRoom()
    {
        ActivatePanel(GameOptions_Panel.name);
        foreach (GameObject gameObject in m_PlayerPanelGOs.Values)
        {
            Destroy(gameObject);
        }
        m_PlayerPanelGOs.Clear();
        m_PlayerPanelGOs = null;
    }

    #endregion

    #region PublicMethod

    public void OnStartButtonClicked()
    {
        ActivatePanel(Login_Panel.name);
    }

    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void OnLoginButtonClicked()
    {
        string PlayerName = PlayerNameIF.text;
        if (!string.IsNullOrEmpty(PlayerName))
        {
            PhotonNetwork.LocalPlayer.NickName = PlayerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PlayerName = "NoName" + Random.Range(100, 10000);
            PhotonNetwork.LocalPlayer.NickName = PlayerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        isLogined = true;
    }
    public void OnNickNameChange()
    {
        string Nickname = NickNameIF.text;
        if (!string.IsNullOrEmpty(Nickname))
        {
            PhotonNetwork.LocalPlayer.NickName = Nickname;
        }
    }
    public void OnCreateRoomButtonInGameOption()
    {
        ActivatePanel(CreateRoom_Panel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        string RoomName = RoomNameIF.text;
        if (string.IsNullOrEmpty(RoomName))
        {
            RoomName = "Room " + Random.Range(10, 99999);
        }
        RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 4,BroadcastPropsChangeToAll=true };

        PhotonNetwork.CreateRoom(RoomName, roomOptions);
    }

    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptions_Panel.name);
    }

    public void OnShowRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        ActivatePanel(RoomList_Panel.name);
    }

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        ActivatePanel(GameOptions_Panel.name);
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnStartGameButtonClicked()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("GameScene");
        }
    }

    #endregion

    #region PrivateMethods

    private void ActivatePanel(string _PanelToActivate)
    {
        //MenuPanel.SetActive(_PanelToActivate.Equals(MenuPanel.name));
        Login_Panel.SetActive(_PanelToActivate.Equals(Login_Panel.name));
        GameOptions_Panel.SetActive(_PanelToActivate.Equals(GameOptions_Panel.name));
        CreateRoom_Panel.SetActive(_PanelToActivate.Equals(CreateRoom_Panel.name));
        InsideRoom_Panel.SetActive(_PanelToActivate.Equals(InsideRoom_Panel.name));
        RoomList_Panel.SetActive(_PanelToActivate.Equals(RoomList_Panel.name));

    }

    private void OnJoinRoomButtonClicked(string _roomName)
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(_roomName);
    }

    private void ClearRoomListview()
    {
        foreach (GameObject gameObject in m_RoomPanelGOs.Values)
        {
            Destroy(gameObject);
        }
        m_RoomPanelGOs.Clear();
    }

    #endregion

}
