using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Charactor
{
    //상태 정리
    private enum EnemyState
    {
        Wait,
        Run,
        Die,
        Fear_first,
        Fear_last
    }

    //상태기계 클래스객체
    public StateMachine stateMachine;
    
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
        dicState.Add(EnemyState.Die, die);
        dicState.Add(EnemyState.Fear_first, fear_first);
        dicState.Add(EnemyState.Fear_last, fear_last);

        stateMachine = new StateMachine(wait); //처음엔 대기상태

        base.Init();
    }

    public virtual void Update()
    {
        if(stateMachine.CurrentState == run)
            Move();
    }

    public void Move()
    {
        if(!isMove)
            StartCoroutine(MoveTo());
    }


    public void SetStateThis(IState state)
    {
        stateMachine.SetState(state);
    }

    // 한 칸당 움직임과 애니메이션 방향도 정한다.
    // direction 대로 움직인다.
    //벽을 감지할 필요가 없다.
    IEnumerator MoveTo()
    {
        Vector2Int startPos = currentPos;

        endPos = startPos + T.Direction2D(direction);

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

    #region A*
    //A* 알고리즘을 통한 PathFinding() - 22.01.10
    public List<Node> FinalNodeList;
    public Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;
    public void PathFinding(Node startNode, Node targetNode)
    {
        StartNode = startNode;
        TargetNode = targetNode;

        OpenList = new List<Node>(){StartNode};
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

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

                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                for(int i=0;i<FinalNodeList.Count;i++)
                {
                    Debug.Log(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                }
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
        if(checkX >= 0 && checkY >= 0 && checkX < T.CurrentMap.GetLength(1) && checkY < T.CurrentMap.GetLength(1)
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


    #endregion
    
}
