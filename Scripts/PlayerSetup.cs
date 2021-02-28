using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSetup : MonoBehaviourPun
{
    
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            transform.position = GameManager.Instance.LocalPlayerPosition.position;
        }
        else
        {
            //transform.position = GameManager.Instance.OtherPlayerPostion.position;
            GameManager.Instance.FillOtherPlayerSpot(transform);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
