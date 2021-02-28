using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Transform LocalPlayerPosition;
    public Transform OtherPlayerPostion;
    public OtherPlayerSpot[] OtherPlayerSpot;

    public Transform LocalHandCardsParent;
    public Transform LocalSelectedCardParent;
    public Transform AspectSelectionParent;
    public Transform AspectIndocatorParent;

    public Transform PlayerConsoleParent;

    public GameObject EndOfRoundButtonParent;

    public GameObject PlayerConsoleEntry;
    [Header("Win/Loss Panel")]
    public GameObject WinPanel;
    public GameObject LossPanel;

    public Text CardCountText;

    public GameObject PausePanel;
    private bool isPausePanelOpen;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (MatchExecutor.Instance == null)
                {
                    Debug.Log("GameManager MatchExecutor Instance");
                    PhotonNetwork.Instantiate("MatchexecutorPrefeb", Vector3.zero, Quaternion.identity);
                }
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            
            PhotonNetwork.Instantiate("Player", LocalPlayerPosition.position, Quaternion.identity);
            Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is Instanitates");
        }

    }

    public void FillOtherPlayerSpot(Transform _otherPlayer)
    {
        bool isDone = false;
        foreach (var spot in OtherPlayerSpot)
        {
            isDone = spot.FillSpot(_otherPlayer);
            if (isDone)
            {
                break;
            }
        }
        if (!isDone)
        {
            Debug.LogError("Other Player Spot havn't Filled");

        }
    }

    public void AddPlayerConsoleEntry(string _Entry)
    {
        GameObject entry = Instantiate(PlayerConsoleEntry, PlayerConsoleParent);
        entry.transform.localScale = Vector3.one;
        entry.GetComponentInChildren<Text>().text = _Entry;
        Debug.Log(_Entry);
    }
    public void UpdateCardCount(int _count)
    {
        CardCountText.text = _count.ToString();
    }

    public void LeaveMatch()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MenuScreen");
    }
    public void PausePanelButton()
    {
        isPausePanelOpen = !isPausePanelOpen;

        PausePanel.SetActive(isPausePanelOpen);

    }

}
