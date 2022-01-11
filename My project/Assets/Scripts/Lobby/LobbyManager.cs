using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    //ui를 가지고 있는 모든 게임 오브젝트
    public GameObject [] arr;

    void Start()
    {
        SetChange("Main");
    }

    //by 현수 - 인자로 받은 오브젝트 외의 다른 씬 오브젝트를 모두 끈다.
    void SetChange(string s)
    {
        for(int i = 0; i<arr.Length; i++)
        {
            if(arr[i].name != s) arr[i].SetActive(false);
            else arr[i].SetActive(true);
        }
    }

}
