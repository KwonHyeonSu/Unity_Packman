using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Charactor
{
    public Game_Pojang game_Pojang;

    //상태 정리
    private enum EnemyState
    {
        Wait, // 게임 시작 전
        Run, // 게임 중
        Scatter, // 게임 중
        Frightened, // 술맨이 아이템을 먹었을 때
        Eaten // Fear_first 상태에서 팩맨에게 먹혔을 때
    }

    //각 상태에 따른 행동 state를 보관
    private Dictionary<EnemyState, IState> dicState = new Dictionary<EnemyState, IState>();

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
            RunLogic();
        }

        //scatter 상태
        else if(stateMachine.CurrentState == scatter)
        {
            ScatterLogic();
        }

        //frightened 상태
        else if(stateMachine.CurrentState == frightend)
        {
            FrightenedLogic();
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
    
    #region <FRIGHTENED>
    //frightened 상태일 때는, Scatter상태로 움직인다.
    public virtual void FrightenedLogic()
    {
        animator.Play("Frightened_1");
    }
    #endregion
    public virtual void Move(Node targetNode)
    {

        PathFinding(T.CurrentMap[currentPos.x, currentPos.y], targetNode);
        if(FinalNodeList.Count>0)
            SetDirection();
        StartCoroutine(MoveTo());

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
        //범위 / 벽 체크
        if(checkX >= 0 && checkY >= 0 && checkX < T.CurrentMap.GetLength(0) && checkY < T.CurrentMap.GetLength(1)
        && !T.CurrentMap[checkX, checkY].wall && !ClosedList.Contains(T.CurrentMap[checkX, checkY]))
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

    //경로 확인용 - 22.01.10
    protected virtual void OnDrawGizmos()
    {
        
    }


    #endregion
    
}
