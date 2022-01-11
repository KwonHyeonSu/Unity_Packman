using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by 현수 - Jake 로직
//플레이어와의 위치 차이가 반경 shyDistance 이상일경우 Chase, 아닐경우 Scatter
public class Jake : Enemy
{
    private int shyDistance = 14;
    public override void Init()
    {
        beginPos = new Vector2Int(5, 8);
        initSpeed = 0.3f;
        
        base.Init();
    }

    //RunLogic() - 22.01.11
    public override void RunLogic()
    {
        //플레이어와의 거리 계산
        Vector2Int temp = game_Pojang.player.currentPos - currentPos;
        float dist = temp.x * temp.x + temp.y * temp.y;

        //거리가 가까울 경우, scatter.
        if(dist < shyDistance * shyDistance)
        {
            ScatterLogic();
        }
        //거리가 멀 경우, 따라감 like rosa
        else{
            Move(T.CurrentMap[game_Pojang.player.currentPos.x, game_Pojang.player.currentPos.y]); //플레이어 위치로 이동'

        }

        
    }

    #region <Scatter> ScatterLogic() - scatter모드일때 어디를 돌 것인지 지정 - 22.01.10
    public override void ScatterLogic()
    {
        if(scatterRoutine.Count == 0)
        {
            //scatter모드일때 어디를 찍을것인지
            scatterRoutine.Add(T.PojangArr[2, 10]);
            scatterRoutine.Add(T.PojangArr[19, 11]);
            scatterRoutine.Add(T.PojangArr[19, 2]);
            scatterRoutine.Add(T.PojangArr[13, 2]);
        }

        base.ScatterLogic();
    }
    #endregion
    
    #region <Frightened> frightened 상태일 때는, Scatter상태로 움직인다. - 22.01.10
    public override void FrightenedLogic()
    {
        base.FrightenedLogic();
        ScatterLogic();
    }
    #endregion
    
    public override void Move(Node targetNode)
    {
        if(!isMove)
        {
            base.Move(targetNode);
        }
    }

    //먹혔을 때, 하위 객체 (rosa, raymond, ...)에서 어디 방향으로 이동할 지 정하기.
    public override void EatenMove()
    {
        base.EatenMove();
        Move(T.CurrentMap[27, 13]);
    }

    protected override void OnDrawGizmos()
    {
        if(FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
            Debug.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y), Color.green);
    }
}
