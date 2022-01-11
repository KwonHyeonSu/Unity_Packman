using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by 현수 - Jake 로직
//플레이어와의 위치 차이가 반경 8 이상일경우 Chase, 아닐경우 Scatter
public class Jake : Enemy
{
    public override void Init()
    {
        beginPos = new Vector2Int(5, 8);
        speed = 0.3f;
        
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
            scatterRoutine.Add(T.PojangArr[2, 10]);
            scatterRoutine.Add(T.PojangArr[19, 11]);
            scatterRoutine.Add(T.PojangArr[19, 2]);
            scatterRoutine.Add(T.PojangArr[13, 2]);
        }

        base.ScatterLogic();
    }

    //frightened 상태일 때는, Scatter상태로 움직인다. - 22.01.10
    public override void FrightenedLogic()
    {
        base.FrightenedLogic();
        ScatterLogic();
    }

    public override void Move(Node targetNode)
    {
        if(!isMove)
        {
            PathFinding(T.CurrentMap[currentPos.x, currentPos.y], targetNode);
            if(FinalNodeList.Count>0)
                SetDirection();
            StartCoroutine(MoveTo());
        }
    }

    protected override void OnDrawGizmos()
    {
        if(FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
            Debug.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y), Color.green);
    }
}
