
[System.Serializable]
public class PlayerData
{
    public int m_SelectedCardId;
    public int m_Id;
    public bool m_IsCardSelected;

    public PlayerData(int _ID, int _CardId=0,bool _value = false)
    {
        m_SelectedCardId = _CardId;
        m_IsCardSelected = _value;
        m_Id = _ID;
    }
    public PlayerData()
    {

    }

}
