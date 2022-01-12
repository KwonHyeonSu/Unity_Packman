using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Charactor
{

    public bool Invincible;

    //상태 정리
    private enum PlayerState
    {
        Wait,
        Run,
        Die,
    }

    
    //각 상태에 따른 행동 state를 보관
    private Dictionary<PlayerState, IState> dicState = new Dictionary<PlayerState, IState>();
    public Game_Pojang game_Pojang;

    public override void Start()
    {
        base.Start();

        dicState.Add(PlayerState.Wait, wait);
        dicState.Add(PlayerState.Run, run);

        stateMachine = new StateMachine(wait);
        if(null == game_Pojang)
        {
            game_Pojang = GameObject.Find("[PojangManager]").GetComponent<Game_Pojang>();
        }
    }

    public override void Init()
    {
        beginPos = new Vector2Int(20, 8);
        currentPos = beginPos;
        initSpeed = 0.2f;

        base.Init();
    }

    void Update()
    {   
        //wait이 아닐 때
        if(stateMachine.CurrentState != wait)
            Move();
    }

    // isMove == false일 때 MoveTo()를 수행한다.
    public void Move()
    {
        if(!isMove)
        {
            moveCoroutine = StartCoroutine(MoveTo());
        }
    }

    // 한 칸당 움직임과 애니메이션 방향도 정한다.
    // 움직이려는 칸이 벽인경우, 제자리 걸음을 한다.
    // direction 대로 움직인다.
    IEnumerator MoveTo()
    {
        Vector2Int startPos = currentPos;
        var checkPos = startPos + T.Direction2D(directionQueue);

        //가려는 방향이 뚫려있다면, 진행방향을 바꾸도록 한다.
        if(T.PojangArr[checkPos.x, checkPos.y].wall == false)
        {
            direction = directionQueue;
        }

        endPos = startPos + T.Direction2D(direction);
        if(T.PojangArr[endPos.x, endPos.y].wall == false)
        {
            animator.Play(T.Dir2Str(direction)); //애니메이션 전환

            float percent = 0;
            
            isMove = true;

            while(percent < speed)
            {
                percent += Time.deltaTime;
                transform.position = Vector2.Lerp(startPos, endPos, percent/speed);
                yield return null;
            }

            //현재 위치 갱신
            currentPos = endPos;
            isMove = false;
        }
        
    }

    public void ChangeDir(Direction dir)
    {
        directionQueue = dir;
    }


    //충돌 메서드
    private void OnTriggerEnter2D(Collider2D other) {
        //쿠키 먹었을 때
        if(other.gameObject.tag == "cookie")
        {
            other.gameObject.SetActive(false);
            T.score += 5;
            game_Pojang.soundManager.PlayAudio("eating");
        }

        //파워 쿠키 먹었을 때
        else if(other.gameObject.tag == "powercookie")
        {
            other.gameObject.SetActive(false);
            game_Pojang.SetAllCharactorState("Frightened");
            game_Pojang.soundManager.PlayAudio("eating_powerCookie");

        }

        //경찰랑 부딪혔을 때 - 22.01.11
        else if(other.gameObject.tag == "enemy")
        {
            var e = other.gameObject.GetComponent<Enemy>();


            //일반 상태(달리기)일 경우 
            if(e.stateMachine.CurrentState == e.run)
            {
                if(!Invincible)
                    game_Pojang.LifeDiscount(); //목숨 까기
            }

            //플레이어가 먹을 수 있을 경우 - enemy가 Frightened상태일 경우
            if(e.stateMachine.CurrentState == e.frightend)
            {
                e.SetStateThis(e.eaten); //적 상태 변화
                game_Pojang.Score_Up(); //점수 올리기
                game_Pojang.soundManager.PlayAudio("eating_ghost");
            }
        }

        
    }


    //상태 적용 메서드 - 22.01.09
    public void SetStateThis(IState state)
    {
        stateMachine.SetState(state);
    }
}

