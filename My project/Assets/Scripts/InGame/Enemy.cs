using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Charactor
{
    public Game_Pojang game_Pojang;

    //상태 정리
    protected enum EnemyState
    {
        Wait, // 게임 시작 전
        Run, // 게임 중
        Scatter, // 게임 중
        Frightened, // 술맨이 아이템을 먹었을 때
        Eaten // Fear_first 상태에서 팩맨에게 먹혔을 때
    }

    //각 상태에 따른 행동 state를 보관
    private Dictionary<EnemyState, IState> dicState = new Dictionary<EnemyState, IState>();

    private Coroutine coroutine; //22.01.11 - 코루틴 제어용 (Frightened)

    public override void Start()
    {
        base.Start();
        
        Init();
    }

    public override void Init()
    {
        dicState.Clear();
        dicState.Add(EnemyState.Wait, wait);
        dicState.Add(EnemyState.Run, run);
        dicState.Add(EnemyState.Scatter, scatter);
        dicState.Add(EnemyState.Frightened, frightend);
        dicState.Add(EnemyState.Eaten, eaten);

        stateMachine = new StateMachine(dicState[EnemyState.Wait]); //처음엔 대기상태
        if(null == game_Pojang) game_Pojang = GameObject.Find("[PojangManager]").GetComponent<Game_Pojang>();

        base.Init();
    }

    public virtual void Update()
    {
        //달리는 상태
        if(stateMachine.CurrentState == run)
        {
            SetSpeed();
            RunLogic();
        }

        //scatter 상태
        else if(stateMachine.CurrentState == scatter)
        {
            SetSpeed();
            ScatterLogic();
        }

        //frightened 상태
        else if(stateMachine.CurrentState == frightend)
        {
            SetSpeed();
            FrightenedLogic();
        }

        //eaten 상태
        else if(stateMachine.CurrentState == eaten)
        {
            SetSpeed(0.08f);
            EatenLogic();
        }
    }

    #region <RUN>
    // by 현수 - 경찰별로 재정의하기 - 22.01.10
    public virtual void RunLogic()
    {

    }
    #endregion


    #region <SCATTER>
    // by 현수 - Scatter() 경찰별로 재정의하기 - 22.01.10
    protected List<Node> scatterRoutine = new List<Node>(); //움직이는 노드가 경찰별로 다르다. 각 꼭짓점 근처 4개의 노드 지정.
    int n=0;
    public virtual void ScatterLogic()
    {
        if(scatterRoutine != null && scatterRoutine.Count == 4 && !isMove)
        {
            if(currentPos == new Vector2Int(scatterRoutine[n].x, scatterRoutine[n].y)) n++;

            if(n >= scatterRoutine.Count) n = 0;

            Move(scatterRoutine[n]);
        
        }

        
    }

    #endregion


    #region <FRIGHTENED> by 현수 22.01.11

    public virtual void FrightenedLogic()
    {
        if(null == coroutine)
        {
            //애니메이션 변경 코드
            coroutine = StartCoroutine(FrightenedAnimRoutine(4.0f, 3.0f));
        }
    }
    float _fright_1_time = 0.0f; //fright_1_time - 피버타임_1 시간
    float _fright_2_time = 0.0f; //fright_2_time - 피버타임_2 시간

    //frightened 상태일 때는, Scatter상태로 움직인다.
    //일정 시간이 지나면 원래대로 돌아오도록 - 22.01.11
    
    IEnumerator FrightenedAnimRoutine(float fright_1_time, float fright_2_time)
    {
        _fright_1_time = 0.0f;
        _fright_2_time = 0.0f;

        animator.Play("Frightened_1");
        //fright_1
        while(_fright_1_time < fright_1_time)
        {
            _fright_1_time += Time.deltaTime;
            yield return null;
        }

        //fright_2
        animator.Play("Frightened_2");
        while(_fright_2_time < fright_2_time)
        {
            _fright_2_time += Time.deltaTime;
            yield return null;
        }

        //시간이 끝나면 run 상태로
        SetStateThis(run);
        coroutine = null;
        yield return null;
    }

    
    #endregion


    #region <EATEN> -22.01.11 by 현수 [frightened모드에서 먹혔을 때]
    
    //eaten 스프라이트
    public virtual void EatenLogic()
    {
        EatenAnim();
        EatenMove();
    }


    //먹혔을 때, 하위 객체 (rosa, raymond, ...)에서 어디 방향으로 이동할 지 정하기.
    public virtual void EatenMove()
    {

        //먹히고 난 후, 목표위치에 도달하면 Run상태로 바뀌기
        if(T.stage == "포장마차") //(26, 15), (27,15) -> 입구 좌표
        {
            if(T.CurrentMap[currentPos.x, currentPos.y] == T.CurrentMap[26, 15]
            || T.CurrentMap[currentPos.x, currentPos.y] == T.CurrentMap[27, 15])
            {
                stateMachine.SetState(run);
                animator.enabled = true;
            }
        }
    }
    
    
    private void EatenAnim()
    {
        switch(direction)
        {
            case Direction.Up:
                sprite = eaten_Sprites[0] as Sprite;
                break;
            
            case Direction.Down:
                sprite = eaten_Sprites[1] as Sprite;
                break;
            
            case Direction.Right:
                sprite = eaten_Sprites[2] as Sprite;
                break;
            
            case Direction.Left:
                sprite = eaten_Sprites[3] as Sprite;
                break;
        }

        //애니메이터를 끄고 스프라이트로.
        animator.enabled = false;

        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    #endregion

    public virtual void Move(Node targetNode)
    {

        PathFinding(T.CurrentMap[currentPos.x, currentPos.y], targetNode);
        if(FinalNodeList.Count>0)
            SetDirection();
        moveCoroutine = StartCoroutine(MoveTo());

    }

    //FinalNodeList에 맞춰 방향 설정
    public void SetDirection()
    {
        //Debug.Log("finalNode[0] : " + FinalNodeList[0].x + ", " + FinalNodeList[0].y + "\tcount : " + FinalNodeList.Count);
        Vector2Int toDir = new Vector2Int(FinalNodeList[0].x - currentPos.x, FinalNodeList[0].y - currentPos.y);
        if(toDir != Vector2Int.zero)
        {
            if(T.Direction2D(Direction.Up) == toDir) direction = Direction.Up;
            else if(T.Direction2D(Direction.Down) == toDir) direction = Direction.Down;
            else if(T.Direction2D(Direction.Left) == toDir) direction = Direction.Left;
            else if(T.Direction2D(Direction.Right) == toDir) direction = Direction.Right;
        }
    }

    public void SetStateThis(IState state)
    {
        stateMachine.SetState(state);
    }

    // 속도 초기화
    public void SetSpeed(float _speed = 0)
    {
        if(_speed == 0) speed = initSpeed;
        else speed = _speed;
    }

    // 한 칸당 움직임과 애니메이션 방향도 정한다.
    // direction 대로 움직인다.
    // enemy는 벽을 감지할 필요가 없다.
    protected IEnumerator MoveTo()
    {
        Vector2Int startPos = currentPos;

        endPos = startPos + T.Direction2D(direction);

        if(!T.CurrentMap[endPos.x, endPos.y].wall)
        {
            if(stateMachine.CurrentState != frightend) //frightened 상태일 때는 제외
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

    #region A*
    //A* 알고리즘을 통한 PathFinding() - 22.01.10
    protected List<Node> FinalNodeList = new List<Node>();
    public Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;
    public void PathFinding(Node startNode, Node targetNode)
    {
        StartNode = startNode;
        TargetNode = targetNode;

        OpenList = new List<Node>(){StartNode};
        ClosedList = new List<Node>();
        FinalNodeList.Clear();

        while(OpenList.Count > 0)
        {
            //OpenList중 가장 F가 작고, F가 같다면 H가 작은 걸 현재 노드로 하고 Open->Closed
            CurNode = OpenList[0];
            for(int i=1;i<OpenList.Count;i++)
                if(OpenList[i].f <= CurNode.f && OpenList[i].h < CurNode.h)
                    CurNode = OpenList[i];
            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            if(CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while(TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.Parent;
                }

                //FinalNodeList.Add(StartNode); //시작부분을 넣을 필요 없다. -- 22.01.10
                FinalNodeList.Reverse();

            }
            
            OpenListAdd(CurNode.x, CurNode.y+1);
            OpenListAdd(CurNode.x+1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y-1);
            OpenListAdd(CurNode.x-1, CurNode.y);
        }
    }

    void OpenListAdd(int checkX, int checkY)
    {
        //범위 체크
        if(checkX >= 0 && checkY >= 0 && checkX < T.CurrentMap.GetLength(0) && checkY < T.CurrentMap.GetLength(1))
        {
            //벽 체크
            if((!T.CurrentMap[checkX, checkY].wall /*|| game_Pojang.only_Monster.Contains(T.CurrentMap[checkX, checkY])*/
                && !ClosedList.Contains(T.CurrentMap[checkX, checkY])))
            {
                Node NeighborNode = T.CurrentMap[checkX, checkY];
                int MoveCost = CurNode.g + 1;

                //이동 비용이 이웃노드 g보다 작거나 열린리스트에 이웃노드가 없다면 g,h ,parent설정후 openlist에 추가
                if(MoveCost < NeighborNode.g || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.g = MoveCost;
                    NeighborNode.h = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y));
                    NeighborNode.Parent = CurNode;

                    OpenList.Add(NeighborNode);
                }
            }

        }

    }


    //경로 확인용 - 22.01.10
    protected virtual void OnDrawGizmos()
    {
        
    }


    #endregion
    
}
