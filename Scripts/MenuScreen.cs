using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreen : MonoBehaviour
{
    public GameObject EnterNamePanel;
    public InputField enterNameIF;

    #region unity Function
    // Start is called before the first frame update
    void Start()
    {
        //if (!Directory.Exists(SavaManager.SavaPath))
        //{
        //    //EnterNamePanel.SetActive(true);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion


    #region OnclickFunction

    public void OnStartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void OnEnterNameOKButton()
    {
        if (!string.IsNullOrEmpty(enterNameIF.text))
        {
            EnterNamePanel.SetActive(false);
        }
    }

    #endregion
}
