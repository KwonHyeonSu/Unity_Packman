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
        
        //상태
        IState wait = new StateWait();
        IState run = new StateRun();
        IState die = new StateDie();
        IState fear_first = new StateFear_First();
        IState fear_last = new StateFear_Last();

        dicState.Add(EnemyState.Wait, wait);
        dicState.Add(EnemyState.Run, run);
        dicState.Add(EnemyState.Die, die);
        dicState.Add(EnemyState.Fear_first, fear_first);
        dicState.Add(EnemyState.Fear_last, fear_last);

        stateMachine = new StateMachine(wait);

    }
}
