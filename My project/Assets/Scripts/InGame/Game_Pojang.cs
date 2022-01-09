using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Pojang : MonoBehaviour
{
    public Player player;

    [SerializeField]
    private List<Enemy> enemies = new List<Enemy>();

    public GameObject Panel_Pause;

    //점수 및 스테이지 22.01.09
    public Text txt_score;
    public Text txt_stage;

    public List<GameObject> Obj_life = new List<GameObject>(); 

    void Start()
    {
        Panel_Pause.SetActive(false);

        //나중에 수정하기
        T.score = 100;
        T.stage = 2;
        T.life = 3;

        //게임 시작 전, 변수 할당
        Ready();
        
    }

    
    void Update()
    {
        //아무 키나 누르면 게임 시작
        Playing();
        
    }
    

    void Ready()
    {
        //플레이어 할당
        if(null == player)
        {
            player = GameObject.Find("_Player_").GetComponent<Player>();
        }
        
        //enemies에 4명의 경찰 할당

        try{
            enemies.Add(GameObject.Find("_Rosa_").GetComponent<Enemy>());
        }
        catch{
            Debug.LogWarning("_Rosa_ 없음");
        }

        try{
            enemies.Add(GameObject.Find("_Emy_").GetComponent<Enemy>());
        }
        catch{
            Debug.LogWarning("_Emy_ 없음");
        }

        try{
            enemies.Add(GameObject.Find("_Jake_").GetComponent<Enemy>());
        }
        catch{
            Debug.LogWarning("_Jake_ 없음");
        }

        try{
            enemies.Add(GameObject.Find("_Raymond_").GetComponent<Enemy>());
        }
        catch{
            Debug.LogWarning("_Raymond_ 없음");
        }


        //모든 캐릭터 wait 상태
        SetAllCharactorState("Wait");

        //GameState -> Ready
        T.currentGameState = GameState.Ready;
        Debug.Log("게임 대기중... 아무 키나 입력하세요");
    }

    void Playing()
    {
        if(T.currentGameState == GameState.Ready)
        {
            if(Input.anyKeyDown) {
                T.currentGameState = GameState.Playing;
                SetAllCharactorState("Run");

                Debug.Log("게임을 시작합니다!");
            }
        }

        else if(T.currentGameState == GameState.Playing)
        {
            TextUpdate();

            #if UNITY_EDITOR
            KeyboardInput();
            #endif
        }
    }

    //점수 텍스트 업데이트 - 22.01.10
    void TextUpdate()
    {
        txt_score.text = "SCORE : " + T.score;
        txt_stage.text = "STAGE : " + T.stage;
    }

    #if UNITY_EDITOR
    void KeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) Up();
        else if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) Down();
        else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) Left();
        else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) Right();
    }
    #endif


    //게임 내 모든 캐릭터들의 상태를 설정 - 22.01.09
    //파라미터 : "Wait" , "Run"
    void SetAllCharactorState(string stateName)
    {   
        
        switch(stateName)
        {
            case "Wait":
                //wait 상태
                player.SetStateThis(player.wait);
                foreach(var e in enemies) e.SetStateThis(e.wait);
                break;
            
            case "Run":
                //Run 상태
                player.SetStateThis(player.run);
                foreach(var e in enemies) e.SetStateThis(e.run);
                break;

        }
        
    }

    void LifeDiscount()
    {
        if(T.life > 0)
        {
            T.life--;
            Obj_life[Obj_life.Count-1].SetActive(false);
        }

        //게임오버!
        else if(T.life == 0)
        {
            GameOver();
        }
    }
    
    //by 현수 - GameOver() 채우기 - 22.01.10
    void GameOver()
    {

    }

    #region 버튼

    //-----일시정지 패널-----

    
    public void Btn_Pause()
    {
        Panel_Pause.SetActive(true);
        //일시 정지
        Time.timeScale = 0.0f;

        //정지 패널 보이기
        //ShowPausePanel();
    }

    
    public void Btn_Resume() //게임 재개 버튼
    {
        Time.timeScale = 1.0f;
        Panel_Pause.SetActive(false);
    }

    //-----조이스틱-----
    public void Up()
    {
        player.ChangeDir(Direction.Up);
    }

    public void Down()
    {
        player.ChangeDir(Direction.Down);
    }

    public void Left()
    {
        player.ChangeDir(Direction.Left);
    }

    public void Right()
    {
        player.ChangeDir(Direction.Right);
    }

    #endregion
}
