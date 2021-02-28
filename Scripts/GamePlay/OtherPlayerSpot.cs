using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerSpot : MonoBehaviour
{
    public bool isFilled = false;
   

    public bool FillSpot(Transform _player)
    {
        if(isFilled == false)
        {
            isFilled = true;
            _player.position = transform.position;
            return true;
        }
        return false;
    }
}
