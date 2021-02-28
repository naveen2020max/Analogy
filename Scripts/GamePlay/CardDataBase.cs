using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataBase : MonoBehaviour
{

    public static CardDataBase Instance;
    public Sprite[] CardProfile;

    //float[4] { Rank,Power=100,Agililty=100,Intelligent=100}
    public readonly CardInfo NullCard = new CardInfo(0, null, "Null", new int[4] { 100, 0, 0, 0 });

    public readonly CardInfo Ben = new CardInfo(1, null, "Ben", new int[4] { 1, 17, 24, 44 });
    public readonly CardInfo Azmuth = new CardInfo(2, null, "Azmutt", new int[4] { 2, 6, 11, 99 });
    public readonly CardInfo Gwen = new CardInfo(3, null, "Gwen", new int[4] { 3, 34, 28, 46 });
    public readonly CardInfo Kevin = new CardInfo(4, null, "Kevin", new int[4] { 4, 43,25, 31 });
    public readonly CardInfo GrandpaMax = new CardInfo(5, null, "Grandpa Max", new int[4] { 5, 22, 13, 53 });

    public readonly CardInfo Vilgax = new CardInfo(6, null, "Vilgax", new int[4] { 6, 79, 44, 51 });
    public readonly CardInfo Heatblast = new CardInfo(7, null, "Heatblast", new int[4] { 7, 43, 58, 35 });
    public readonly CardInfo Diamondhead = new CardInfo(8, null, "Diamondhead", new int[4] { 8, 71, 26, 44 });
    public readonly CardInfo WayBig = new CardInfo(9, null, "WayBig", new int[4] { 9, 99, 23, 26 });
    public readonly CardInfo Ditto = new CardInfo(10, null, "Ditto", new int[4] { 10, 20, 33, 64 });

    public readonly CardInfo Tetrax = new CardInfo(11, null, "Tetrax", new int[4] { 11, 66, 21, 47 });
    public readonly CardInfo Cannonbolt = new CardInfo(12, null, "Cannonbolt", new int[4] { 12, 57, 69, 19 });
    public readonly CardInfo Charmcaster = new CardInfo(13, null, "Charmcaster", new int[4] { 13, 32, 22, 41 });
    public readonly CardInfo DrAnimo = new CardInfo(14, null, "Dr.Animo", new int[4] { 14, 9, 18, 73 });
    public readonly CardInfo Upgrade = new CardInfo(15, null, "Upgrade", new int[4] { 15, 22, 43, 53 });

    public readonly CardInfo Xlr8 = new CardInfo(16, null, "Xlr8", new int[4] { 16, 32, 95, 32 });
    public readonly CardInfo Ghostfreak = new CardInfo(17, null, "Ghostfreak", new int[4] { 17, 66, 26, 34 });
    public readonly CardInfo GreyMatter = new CardInfo(18, null, "GreyMatter", new int[4] { 18, 7, 12, 85 });
    public readonly CardInfo Upchuck = new CardInfo(19, null, "Upchuck", new int[4] { 19, 72, 19, 24 });
    public readonly CardInfo Hex = new CardInfo(20, null, "Hex", new int[4] { 20, 45, 25, 25 });

    public readonly CardInfo Sixsix = new CardInfo(21, null, "Sixsix", new int[4] { 21, 44, 51, 39 });
    public readonly CardInfo Wildmutt = new CardInfo(22, null, "Wildmutt", new int[4] { 22, 40, 60, 5 });
    public readonly CardInfo FourArms = new CardInfo(23, null, "FourArms", new int[4] { 23, 70, 42, 20 });
    public readonly CardInfo Stinkfly = new CardInfo(24, null, "Stinkfly", new int[4] { 24, 20, 50, 15 });
    public readonly CardInfo Ripjaws = new CardInfo(25, null, "Ripjaws", new int[4] { 25, 45, 30, 20 });

    public readonly CardInfo Buzzshock = new CardInfo(26, null, "Buzzshock", new int[4] { 26, 20, 69, 15 });
    public readonly CardInfo Xylene = new CardInfo(27, null, "Xylene", new int[4] { 27, 59, 66, 61 });
    public readonly CardInfo Ultimos = new CardInfo(28, null, "Ultimos", new int[4] { 28, 0, 0, 7 });
    public readonly CardInfo Tini = new CardInfo(29, null, "Tini", new int[4] { 29, 0, 6, 9 });
    public readonly CardInfo Synaptak = new CardInfo(30, null, "Synaptak", new int[4] { 30, 3, 0, 0 });

    public readonly CardInfo BenWolf = new CardInfo(31, null, "BenWolf", new int[4] { 31, 43, 38, 21 });
    public readonly CardInfo Eyeguy = new CardInfo(32, null, "Eyeguy", new int[4] { 32, 67, 23, 29 });
    public readonly CardInfo Wildvine = new CardInfo(33, null, "Wildvine", new int[4] { 33, 48, 48, 25 });
    public readonly CardInfo Driscoll = new CardInfo(34, null, "Driscoll", new int[4] { 34, 57, 23, 26 });
    public readonly CardInfo Gluto = new CardInfo(35, null, "Gluto", new int[4] { 35, 0, 2, 40 });

    public readonly CardInfo Mummy = new CardInfo(36, null, "Mummy", new int[4] { 36, 41, 30, 29 });
    public readonly CardInfo Frankenstrike = new CardInfo(37, null, "Frankenstrike", new int[4] { 37, 36, 25, 15 });
    public readonly CardInfo Spitter = new CardInfo(38, null, "Spitter", new int[4] { 38, 0, 0, 0 });







    public Dictionary<int, CardInfo> AllCards;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

       

    }

    // Start is called before the first frame update
    void Start()
    {
        
        AllCards = new Dictionary<int, CardInfo>
        {
            {NullCard.ID,NullCard },
            { Ben.ID, Ben },
            { Gwen.ID, Gwen },
            { Kevin.ID, Kevin },
            { GrandpaMax.ID, GrandpaMax },

            { Vilgax.ID, Vilgax },
            { WayBig.ID, WayBig },
            { Upchuck.ID, Upchuck },
            { FourArms.ID, FourArms },
            { Tetrax.ID, Tetrax },

            { Gluto.ID, Gluto },
            { Azmuth.ID, Azmuth },
            { Xylene.ID, Xylene},
            { Ultimos.ID, Ultimos },
            { Tini.ID, Tini },

            { Heatblast.ID, Heatblast },
            { Wildmutt.ID, Wildmutt},
            { GreyMatter.ID, GreyMatter},
            { Xlr8.ID, Xlr8},
            { Ripjaws.ID, Ripjaws},

            { Wildvine.ID, Wildvine},
            { BenWolf.ID, BenWolf},
            { Upgrade.ID, Upgrade},
            { Ghostfreak.ID, Ghostfreak},
            { Mummy.ID, Mummy},

            { Ditto.ID, Ditto},
            { Buzzshock.ID, Buzzshock},
            { Stinkfly.ID, Stinkfly},
            { Eyeguy.ID, Eyeguy},
            { Spitter.ID, Spitter},

            { Frankenstrike.ID, Frankenstrike},
            { Diamondhead.ID, Diamondhead},
            { Cannonbolt.ID, Cannonbolt},
            { Driscoll.ID, Driscoll},
            { DrAnimo.ID, DrAnimo},

            { Sixsix.ID, Sixsix},
            { Hex.ID, Hex},
            { Charmcaster.ID, Charmcaster},
            { Synaptak.ID, Synaptak },



        };

        for (int i = 1; i < AllCards.Count; i++)
        {
            AllCards[i].Profile = CardProfile[i-1];
        }

        //Debug.Log(AllCards.Values.Count);
    }

    

    #region Static Methods

    public  CardInfo GetCardInfo(int _ID)
    {
        return AllCards[_ID];
    }


    #endregion

}
