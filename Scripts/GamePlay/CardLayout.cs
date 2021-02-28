using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Card))]
public class CardLayout : MonoBehaviour
{
    public Text CardID;
    public Text CardName;
    public Image CardProfile;
    public Text Rank;
    public Text Power;
    public Text Strength;
    public Text Intelligent;
    public GameObject Hide;

    private Card card;

    private void Awake()
    {
        card = GetComponent<Card>();
        card.OnCardChange += AssignInfo;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AssignInfo(int _ID,string _Name,Sprite _profile,int _Rank,int _Power,int _Aglity,int _Intelligent)
    {
        CardID.text = _ID.ToString();
        CardName.text = _Name;
        Rank.text = _Rank.ToString();
        Power.text = _Power.ToString();
        Strength.text = _Aglity.ToString();
        Intelligent.text = _Intelligent.ToString();

        if (_profile != null)
        {
            CardProfile.sprite = _profile;
        }

    }
    public void AssignInfo(CardInfo _cardInfo)
    {
        CardID.text = _cardInfo.ID.ToString();
        CardName.text = _cardInfo.CardName;
        Rank.text = _cardInfo.Aspects[AspectType.RANK].Value.ToString();
        Power.text = _cardInfo.Aspects[AspectType.POWER].Value.ToString();
        Strength.text = _cardInfo.Aspects[AspectType.AGLITY].Value.ToString();
        Intelligent.text = _cardInfo.Aspects[AspectType.INTELLIGENT].Value.ToString();

        if (_cardInfo.Profile != null)
        {
            CardProfile.sprite = _cardInfo.Profile;
        }
       // Debug.Log("CardLayout Updated");
    }
    public void HideCard(bool _value)
    {
        Hide.SetActive(_value);
    }
}
