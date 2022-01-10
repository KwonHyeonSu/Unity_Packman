using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour
{

    //움직임 관련
    [SerializeField]
    protected Direction directionQueue = Direction.Down;
    [SerializeField]
    protected Direction direction = Direction.Down;
    protected Vector2Int beginPos;

    [SerializeField]
    protected Vector2Int currentPos;

    [SerializeField]
    protected Vector2Int endPos;
    [SerializeField]
    protected bool isMove = false;
    protected float speed;

    //상태 관련
    public IState wait = new StateWait();
    public IState run = new StateRun();
    public IState die = new StateDie();
    protected IState fear_first = new StateFear_First();
    protected IState fear_last = new StateFear_Last();

    //애니메이션
    [HideInInspector]
    public Animator animator;

    public virtual void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //하위 클래스에서 꼭 작성하기. Player.cs의 Init()함수를 참고
    public virtual void Init()
    {
        currentPos = beginPos;
        //애니메이션
        animator = this.GetComponent<Animator>();
        animator.Play("Down");

        transform.position = new Vector2(beginPos.x, beginPos.y);
    }

}

//상태에 따른 행동을 내리는 클래스
public class StateMachine
{
    public IState CurrentState {get; private set;}

    //기본상태 생성시 수행되는 생성자
    public StateMachine(IState defaultState)
    {
        CurrentState = defaultState;
    }

    public void SetState(IState state)
    {
        //같은 상태일 경우 리턴
        if(CurrentState == state) return;

        //상태가 바뀌기 전, 이전 상태의 Exit 호출
        CurrentState.OperateExit();

        //상태 교체
        CurrentState = state;

        //새로운 상태의 Enter 수행
        CurrentState.OperateEnter();
    }

    public void DoOperateUpdate()
    {
        CurrentState.OperateUpdate();
    }
}

//상태들의 인터페이스
public interface IState
{
    void OperateEnter();
    void OperateUpdate();
    void OperateExit();
}



//게임 시작 전, 기다리는 상태
public class StateWait : IState
{
    public void OperateEnter()
    {

    }
    public void OperateUpdate()
    {

    }
    public void OperateExit()
    {

    }
}

//움직이는 상태
public class StateRun : IState
{
    public void OperateEnter()
    {

    }
    public void OperateUpdate()
    {
        
    }
    public void OperateExit()
    {

    }
}

public class StateDie : IState
{
    public void OperateEnter()
    {

    }
    public void OperateUpdate()
    {

    }
    public void OperateExit()
    {

    }
}

public class StateFear_First : IState
{
    public void OperateEnter()
    {

    }
    public void OperateUpdate()
    {

    }
    public void OperateExit()
    {

    }
}

public class StateFear_Last : IState
{
    public void OperateEnter()
    {

    }
    public void OperateUpdate()
    {

    }
    public void OperateExit()
    {

    }
}