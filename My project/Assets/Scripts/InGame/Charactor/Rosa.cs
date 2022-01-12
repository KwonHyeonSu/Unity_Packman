using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//rosa는 단순히 플레이어를 따라간다.
public class Rosa : Enemy
{
    public override void Init()
    {
        beginPos = new Vector2Int(25, 13);
        initSpeed = 0.3f;
        
        base.Init();
    }

    public override void RunLogic()
    {
        Move(T.CurrentMap[game_Pojang.player.currentPos.x, game_Pojang.player.currentPos.y]); //플레이어 위치로 이동'
    }

    //ScatterLogic() - scatter모드일때 어디를 돌 것인지 지정 - 22.01.10
    public override void ScatterLogic()
    {
        if(scatterRoutine.Count == 0)
        {
            //scatter모드일때 어디를 찍을것인지
            scatterRoutine.Add(T.PojangArr[2, 29]);
            scatterRoutine.Add(T.PojangArr[16, 29]);
            scatterRoutine.Add(T.PojangArr[16, 17]);
            scatterRoutine.Add(T.PojangArr[2, 17]);
        }

        base.ScatterLogic();
    }
    
    
    //frightened 상태일 때는, Scatter상태로 움직인다.
    public override void FrightenedLogic()
    {
        base.FrightenedLogic();
        ScatterLogic();
    }

    //먹혔을 때, 하위 객체 (rosa, raymond, ...)에서 어디 방향으로 이동할 지 정하기.
    public override void EatenMove()
    {
        base.EatenMove();
        Move(T.CurrentMap[25, 13]);
    }

    //targetNode 좌표로 a*알고리즘을 통해 이동
    public override void Move(Node targetNode)
    {
        if(!isMove)
        {
            base.Move(targetNode);
        }
    }

    protected override void OnDrawGizmos()
    {
        if(FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
            Debug.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y), Color.red);
    }
}

