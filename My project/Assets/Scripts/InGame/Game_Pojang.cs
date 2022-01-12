using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Pojang : MonoBehaviour
{

    //게임 내 오브젝트들 컨트롤
    public Player player;

    [SerializeField]
    public List<Enemy> enemies = new List<Enemy>();

    public GameObject Panel_Pause;

    //점수 및 스테이지 22.01.09
    public Text txt_score;
    public Text txt_stage;

    public List<GameObject> Obj_life = new List<GameObject>();

    //소리 - 22.01.12
    public SoundManager soundManager;


    //현재 맵에서 몬스터만 지나갈 수 있는 길  리스트 - 22.01.11
    public List<Node> only_Monster = new List<Node>();

    void Start()
    {
        //게임 시작 전, 변수 할당
        Ready();
    }

    
    void Update()
    {
        //아무 키나 누르면 게임 시작
        Playing();
        
    }
    
    //Ready() - 게임 시작 전, 변수 할당 22.01.09
    void Ready()
    {
        //-----게임 오브젝트 / 변수 초기화 - 22.01.10-----
        Panel_Pause.SetActive(false);

        T.score = 100;
        //T.stage = "포장마차"; - MapManager클래스에서 할당하도록.
        T.life = 2;


        //플레이어 할당
        if(null == player)
        {
            player = GameObject.Find("_Player_").GetComponent<Player>();
        }
        
        //enemies에 4명의 경찰 할당

        try{ enemies.Add(GameObject.Find("_Rosa_").GetComponent<Enemy>()); }
        catch{ Debug.LogWarning("_Rosa_ 없음"); }

        try{ enemies.Add(GameObject.Find("_Amy_").GetComponent<Enemy>()); }
        catch{ Debug.LogWarning("_Emy_ 없음");}

        try{ enemies.Add(GameObject.Find("_Jake_").GetComponent<Enemy>()); }
        catch{ Debug.LogWarning("_Jake_ 없음");}

        try{ enemies.Add(GameObject.Find("_Raymond_").GetComponent<Enemy>()); }
        catch{ Debug.LogWarning("_Raymond_ 없음"); }        

        //Enemy의 game_pojang변수 할당 
        foreach(var e in enemies)
        {
            e.game_Pojang = this.GetComponent<Game_Pojang>();
        }

        //소리 할당 - 22.01.12
        if(null == soundManager)
        {
            soundManager = GameObject.Find("[SoundManager]").GetComponent<SoundManager>();
            soundManager.PlayAudio("beginning");
        }


        //몬스터만 지나다닐 수 있는 Exception 길 설정- 22.01.11
        Only_Monster_Setting();


        //모든 변수 할당을 완료하고, 게임 시작 준비
        ResetGame();
    }

    void ResetGame()
    {
        //모든 캐릭터 wait 상태
        SetAllCharactorState("Wait");

        //모든 플레이어 제자리
        player.Reset();
        foreach(var e in enemies) e.Reset();

        //GameState -> Ready
        T.currentGameState = GameState.Ready;

        Debug.Log("게임 대기중... 아무 키나 입력하세요");
    }


    //Playing() - 게임 시작 후 처리
    void Playing()
    {
        if(T.currentGameState == GameState.Ready)
        {
            if(Input.anyKeyDown) {
                //beginning 사운드 멈추기
                soundManager.audioSource.Stop();

                T.currentGameState = GameState.Playing;
                SetAllCharactorState("Run");
                //SetAllCharactorState("Scatter");
                //SetAllCharactorState("Frightened");
                //SetAllCharactorState("Eaten");

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
        txt_stage.text = "STAGE : " + T.stage.ToString();
    }

    #if UNITY_EDITOR //pc 디버깅 모드에만 키 받을 수 있게
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
    public void SetAllCharactorState(string stateName)
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


            //scatter는 Enemies에게만 적용
            case "Scatter": //22.01.10 in cafe
                //Scatter 상태
                foreach(var e in enemies) e.SetStateThis(e.scatter);
                break;


            //Frightened는 Enemies에게만 적용 - 22.01.10
            case "Frightened":
                foreach(var e in enemies)
                {
                    //달리는 상태일 때나 Frightened 상태일 때만 Frightend 상태 적용 - 22.01.12
                    if(e.stateMachine.CurrentState == e.run || e.stateMachine.CurrentState == e.frightend)
                    {
                        e.SetStateThis(e.frightend);
                    }
                }
                break;

            //Frightened는 Enemies에게만 적용 - 22.01.10
            case "Eaten":
                foreach(var e in enemies) e.SetStateThis(e.eaten);
                break;

        }
        
    }

    //몬스터만 지나다닐 수 있는 길 리스트 설정 - 22.01.11
    
    private void Only_Monster_Setting()
    {
        
        if(only_Monster.Count > 0) only_Monster.Clear();

        //포장마차 맵에서의 예외부분
        if(T.stage == "포장마차")
        {
            only_Monster.Add(T.CurrentMap[25, 13]);
            only_Monster.Add(T.CurrentMap[26, 13]);
            only_Monster.Add(T.CurrentMap[27, 13]);
            only_Monster.Add(T.CurrentMap[28, 13]);

            only_Monster.Add(T.CurrentMap[26, 14]);
            only_Monster.Add(T.CurrentMap[27, 14]);

            only_Monster.Add(T.CurrentMap[26, 15]);
            only_Monster.Add(T.CurrentMap[27, 15]);

            Debug.Log("포장마차 예외 벽 설정 완료 (" + only_Monster.Count + "개의 노드)");
        }
    }

    #region <게임 상태 변화 [점수, 목숨]>
    
    //<플레이어 사망> - 22.01.11 (수정)
    public void LifeDiscount()
    {
        soundManager.PlayAudio("death"); //죽었을 때 소리 추가 - 22.01.12

        if(T.life > 0)
        {
            Obj_life[T.life-1].SetActive(false);
            T.life--;
            ResetGame();
        }

        //게임오버!
        else if(T.life == 0)
        {
            GameOver();
            
        }

    }

    //겁에 질린 적을 먹었을 때
    public void Score_Up()
    {
        T.score += 100;
    }

    #endregion
    
    //by 현수 - GameOver() 채우기 - 22.01.10
    void GameOver()
    {
        T.currentGameState = GameState.End;
        GameManager.Instance.GoToScene("Main");
        
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
