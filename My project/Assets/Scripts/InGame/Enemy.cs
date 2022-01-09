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
    private StateMachine stateMachine;
    
    //각 상태에 따른 행동 state를 보관
    private Dictionary<EnemyState, IState> dicState = new Dictionary<EnemyState, IState>();

    public override void Start()
    {
        base.Start();
        
        dicState.Add(EnemyState.Wait, wait);
        dicState.Add(EnemyState.Run, run);
        dicState.Add(EnemyState.Die, die);
        dicState.Add(EnemyState.Fear_first, fear_first);
        dicState.Add(EnemyState.Fear_last, fear_last);

        stateMachine = new StateMachine(wait); //처음엔 대기상태
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

    public override void Init()
    {
        base.Init();
        
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

    
}
