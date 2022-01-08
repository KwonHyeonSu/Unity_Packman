using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Charactor
{

    //상태 정리
    private enum PlayerState
    {
        Wait,
        Run,
        Die,
    }

    //상태기계 클래스객체
    private StateMachine stateMachine;
    
    //각 상태에 따른 행동 state를 보관
    private Dictionary<PlayerState, IState> dicState = new Dictionary<PlayerState, IState>();
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        //초기 지정
        currentPos = new Vector2Int(21,8);
        this.transform.position = new Vector3(21,8,0);

        //상태
        IState wait = new StateWait();
        IState run = new StateRun();
        IState die = new StateDie();

        dicState.Add(PlayerState.Wait, wait);
        dicState.Add(PlayerState.Run, run);
        dicState.Add(PlayerState.Die, die);

        stateMachine = new StateMachine(wait);
    }

    public override void Init()
    {
        base.Init();
        beginPos = new Vector2Int(21, 8);
        currentPos = beginPos;
        speed = 0.3f;
    }

    void Update()
    {
        Move();
    }

    //움직이는 상태가 아닐 때 MoveTo()를 수행한다.
    
    public void Move()
    {
        if(!isMove)
        {
            StartCoroutine(MoveTo());
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


}

