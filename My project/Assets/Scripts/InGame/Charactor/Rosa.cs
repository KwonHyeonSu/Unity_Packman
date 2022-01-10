using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rosa : Enemy
{
    public override void Init()
    {
        beginPos = new Vector2Int(22, 16);
        speed = 0.3f;
        

        base.Init();
    }


    bool flag = false;
    public override void Update()
    {
        if(stateMachine.CurrentState == run)
        {
            //RosaAlgorithm(); //Player의 위치에 따라 A*알고리즘 수행
            if(!flag){
                flag = true;
                PathFinding(T.CurrentMap[currentPos.x, currentPos.y], T.CurrentMap[20,8]);
            }

            Move(); //direction방향으로 간다.
        }
    }

}
