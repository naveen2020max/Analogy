using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AspectType { RANK = 0, POWER = 1, AGLITY = 2, INTELLIGENT = 3,NONE = -1 }

[Serializable]
public class CardInfo
{

    public string CardName;
    public Sprite Profile;
    public int ID;
    public Dictionary<AspectType, Aspect> Aspects;

    public CardInfo(int iD, Sprite profile, string cardName,int[] _aspectValues )
    {
        CardName = cardName;
        Profile = profile;
        ID = iD;

        

        Aspects = new Dictionary<AspectType, Aspect>();
        for (int i = 0; i < 4; i++)
        {
            Aspects.Add((AspectType)i, new Aspect(i,_aspectValues[i]));
        }

    }

    public int GetID()
    {
        return ID;
    }

    
    
}


