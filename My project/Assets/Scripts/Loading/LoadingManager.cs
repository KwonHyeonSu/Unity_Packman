using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


//22.01.12 - 현수
//싱글톤을 추가해서 어느 씬이던지 GamaManager를 통해 로딩씬을 불러올 수 있도록 한다.
public class LoadingManager : MonoBehaviour
{

    #region 싱글톤
    public static LoadingManager instance_loading = null;

    void Awake()
    {
        if(null == instance_loading)
        {
            instance_loading = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    public static LoadingManager Instance
    {
        get
        {
            if(null == instance_loading)
            {
                return null;
            }
            return instance_loading;
        }
    }

    #endregion
    
    public Slider loadingBar;
    public string sceneName = "";
    public bool SceneChangedFlag = false;

    public void LateUpdate()
    {
        if(SceneChangedFlag == false && SceneManager.GetActiveScene().name == "Loading")
        {
            SceneChangedFlag = true;

            if(loadingBar == null)
            {
                loadingBar = GameObject.Find("LoadingBar").GetComponent<Slider>();
            }

            if(!loadingBar)
            {
                Debug.LogError("로딩바를 찾지 모함!");
                return;
            }

            loadingBar.value = 0;

            //22.01.12
            if(sceneName != "")
                StartCoroutine(LoadAsyncScene(sceneName));
        }

        if(SceneManager.GetActiveScene().name != "Loading") SceneChangedFlag = false;
    }
    
    public void LoadingStart(string _sceneName)
    {
        sceneName = _sceneName;
        SceneManager.LoadScene("Loading");
        
        
    }

    IEnumerator LoadAsyncScene(string SceneName)
    {
        yield return null;


        //2초간 대기
        yield return new WaitForSecondsRealtime(1.0f);

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync(SceneName);
        asyncScene.allowSceneActivation = false;
        float timeC = 0;

        while(!asyncScene.isDone)
        {
            yield return null;
            timeC += Time.deltaTime;
            if(asyncScene.progress >= 0.9f)
            {
                //로딩이 다 되었을 경우
                loadingBar.value = Mathf.Lerp(loadingBar.value, 1, timeC);
                if(loadingBar.value >= 1.0f)
                {
                    asyncScene.allowSceneActivation = true;
                }
            }
            else
            {   
                //로딩 중...
                loadingBar.value = Mathf.Lerp(loadingBar.value, asyncScene.progress, timeC);
                if(loadingBar.value >= asyncScene.progress)
                {
                    timeC = 0f;
                }
            }
        }
    }
}
