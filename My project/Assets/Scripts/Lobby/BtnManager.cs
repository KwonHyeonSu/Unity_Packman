using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{
    private LobbyManager LM;

    void Start()
    {
        LM = this.transform.GetComponent<LobbyManager>();
    }


    //아래는 버튼 메서드
    public void Btn_Info()
    {
        Debug.Log("Info Clicked");
        LM.SendMessage("SetChange", "Info");
    }

    public void Btn_Ranking()
    {
        Debug.Log("Ranking Clicked");
    }   

    public void Btn_Play()
    {
        SceneManager.LoadScene("Loading");

    }

    public void Btn_BackToMain()
    {
        Debug.Log("Back_To_Main");
        SendMessage("SetChange", "Main");
    }
}
