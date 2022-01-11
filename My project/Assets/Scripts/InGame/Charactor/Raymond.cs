using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// by 현수 - Raymond 술맨을 중심으로 로사와 대칭인 점으로 간다.- 22.01.10
public class Raymond : Enemy
{
    public override void Init()
    {
        beginPos = new Vector2Int(16, 14);
        initSpeed = 0.3f;
        
        base.Init();
    }

    // By 현수 - Raymone Chase 로직
    // 대칭점 좌표를 구해서(Vector2Int) 이동한다.
    public override void RunLogic()
    {
        if(!isMove)
        {
            Node targetNode = FindRayMondTarget();
            Move(targetNode); //플레이어 위치로 이동'
        }
    }

    //ScatterLogic() - scatter모드일때 어디를 돌 것인지 지정 - 22.01.10
    public override void ScatterLogic()
    {
        if(scatterRoutine.Count == 0)
        {
            //scatter모드일때 어디를 찍을것인지
            scatterRoutine.Add(T.PojangArr[30, 2]);
            scatterRoutine.Add(T.PojangArr[30, 10]);
            scatterRoutine.Add(T.PojangArr[38, 7]);
            scatterRoutine.Add(T.PojangArr[38, 4]);
        }

        base.ScatterLogic();
    }

    //frightened 상태일 때는, Scatter상태로 움직인다. - 22.01.10
    public override void FrightenedLogic()
    {
        base.FrightenedLogic();
        ScatterLogic();
    }

    //먹혔을 때, 하위 객체 (rosa, raymond, ...)에서 어디 방향으로 이동할 지 정하기.
    public override void EatenMove()
    {
        base.EatenMove();
        if(!isMove)
            base.Move(T.CurrentMap[28, 13]);
    }

    #region Raymond 타겟 찾는 함수들
    // 확인용
    public GameObject White;

    //근처 갈 수 있는 가장 가까운 노드를 찾기용 - 22.01.10
    public List<Node> nodeList = new List<Node>();
    private Node FindRayMondTarget()
    {
        Node reNode = null;

        // by 현수 - Rosa의 좌표 - player의 좌표 -> 대칭된 좌표 - 22.01.10
        int x = (game_Pojang.enemies[0].currentPos.x * 2) - game_Pojang.player.currentPos.x;
        int y = (game_Pojang.enemies[0].currentPos.y * 2) - game_Pojang.player.currentPos.y;

        // 범위 제한
        x = Mathf.Abs(x);
        y = Mathf.Abs(y);

        if(x >= T.PojangArr.GetLength(0)) x = T.PojangArr.GetLength(0)-1;
        if(y >= T.PojangArr.GetLength(1)) y = T.PojangArr.GetLength(1)-1;

        

        reNode = FindNearest(x, y);

        return reNode;
    }

    private Node FindNearest(int x, int y)
    {
        Node reNode = null;
        bool found= false;
        int findingRadius = 7; //findingRadius^2 만큼의 주변 노드를 검색
        nodeList.Clear();

        for(int i=-findingRadius;i<findingRadius;i++)
        {
            if(found) break;
            for(int j=-findingRadius;j<findingRadius;j++)
            {
                if(x+i < T.PojangArr.GetLength(0) && x + i > 0 
                && y+j < T.PojangArr.GetLength(1) && y + j > 0 
                && T.PojangArr[x+i, y+j].wall == false)
                {
                    reNode = T.PojangArr[x+i,y+j];
                    found = true;
                    break;
                }
            }
        }

        if(!found) Debug.Log("Not Found!! in Raymond!");

        /*
        //알고리즘 시각적으로 확인용
        var tmp = Instantiate(White, new Vector2(reNode.x, reNode.y), Quaternion.identity);
        Destroy(tmp,2.0f);
        */

        return reNode;
    }

    #endregion
    protected override void OnDrawGizmos()
    {
        if(FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
            Debug.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y), Color.cyan);
    }
}
