using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    public static GameManager instance = null;

    void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    public static GameManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    #endregion
 
    #region <로딩화면 전환 후 원하는 씬 이동> 22.01.12

    public LoadingManager loadingManager = null;
    public void GoToScene(string SceneName)
    {
        loadingManager.LoadingStart(SceneName);
    }

    #endregion

}