using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    public Slider loadingBar;
    
    private void Start()
    {
        loadingBar.value = 0;
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        yield return null;

        AsyncOperation asyncScene = SceneManager.LoadSceneAsync("Game1");
        asyncScene.allowSceneActivation = false;
        float timeC = 0;

        while(!asyncScene.isDone)
        {
            yield return null;
            timeC += Time.deltaTime;
            if(asyncScene.progress >= 0.9f)
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, 1, timeC);
                if(loadingBar.value >= 1.0f)
                {
                    asyncScene.allowSceneActivation = true;
                }
            }
            else
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value, asyncScene.progress, timeC);
                if(loadingBar.value >= asyncScene.progress)
                {
                    timeC = 0f;
                }
            }
        }
    }
}
