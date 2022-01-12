using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by 현수 - 22.01.12 음향 컨트롤 스크립트
public class SoundManager : MonoBehaviour
{
    #region 싱글톤
    public static SoundManager instance_sound = null;

    void Awake()
    {
        if(null == instance_sound)
        {
            instance_sound = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    public static SoundManager Instance
    {
        get
        {
            if(null == instance_sound)
            {
                return null;
            }
            return instance_sound;
        }
    }

    #endregion

    public AudioSource audioSource;

    public AudioClip beginning; //인트로 소리
    public AudioClip eating; //먹을 때 내는 소리
    public AudioClip death; //죽을 때
    public AudioClip eating_powerCookie; //파워 쿠키 먹을 때
    public AudioClip eating_ghost; //frightened 경찰 먹을 때

    private void Start()
    {
        if(null == audioSource) audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 2.0f; //2배 속도
    }

    public void PlayAudio(string audioName)
    {
        switch(audioName)
        {
            case "beginning":
                audioSource.PlayOneShot(beginning);
                break;

            case "eating": //사운드 겹침 방지
                if(T.currentGameState == GameState.Playing) //게임 시작 후 소리내기
                {
                    if(!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(eating);
                    }
                }
                break;

            case "death":
                audioSource.PlayOneShot(death);
                break;

            case "eating_powerCookie":
                audioSource.PlayOneShot(eating_powerCookie);
                break;

            case "eating_ghost":
                audioSource.PlayOneShot(eating_ghost);
                break;
        }
    }

}
