using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by 현수 - Amy는 술맨의 길을 따라간다. - 22.01.10
public class Amy : Enemy
{
    private List<Node> Routine = new List<Node>();
    public override void Init()
    {
        beginPos = new Vector2Int(31, 16);
        speed = 0.3f;
        
        base.Init();

    }

    // By 현수 - Amy Chase 로직
    // 플레이어의 좌표를 받아와서 Routine에 저장.
    // 지나간 길을 따라간다.
    public override void RunLogic()
    {
        if(!isMove)
        {
            //10까지만 받아오는 것으로 한다.
            if(Routine.Count < 10){
                Routine.Add(T.CurrentMap[game_Pojang.player.currentPos.x, game_Pojang.player.currentPos.y]);
            }

            //목표에 도착하면 Routine[0] 지우고 다음 routine으로 감
            if(currentPos.x == Routine[0].x && currentPos.y == Routine[0].y){
                Routine.RemoveAt(0);
            }

            Move(Routine[0]); //플레이어 위치로 이동'

        }
    }

    //ScatterLogic() - scatter모드일때 어디를 돌 것인지 지정 - 22.01.10
    public override void ScatterLogic()
    {
        if(scatterRoutine.Count == 0)
        {
            //scatter모드일때 어디를 찍을것인지
            scatterRoutine.Add(T.PojangArr[33, 29]);
            scatterRoutine.Add(T.PojangArr[38, 29]);
            scatterRoutine.Add(T.PojangArr[38, 21]);
            scatterRoutine.Add(T.PojangArr[33, 21]);
        }

        base.ScatterLogic();
    }

    //frightened 상태일 때는, Scatter상태로 움직인다. - 22.01.10
    public override void FrightenedLogic()
    {
        base.FrightenedLogic();
        ScatterLogic();
    }

    //targetNode 좌표로 a*알고리즘을 통해 이동
    public override void Move(Node targetNode)
    {
        
        PathFinding(T.CurrentMap[currentPos.x, currentPos.y], targetNode);
        if(FinalNodeList.Count>0)
            SetDirection();
        StartCoroutine(MoveTo());
        
    }

    //경로 확인용 - 22.01.10
    protected override void OnDrawGizmos()
    {
        if(FinalNodeList.Count != 0) for (int i = 0; i < FinalNodeList.Count - 1; i++)
            Debug.DrawLine(new Vector2(FinalNodeList[i].x, FinalNodeList[i].y), new Vector2(FinalNodeList[i + 1].x, FinalNodeList[i + 1].y), Color.blue);
    }
}
