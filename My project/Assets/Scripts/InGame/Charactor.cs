using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour
{

    //-----움직임 관련-----
    [SerializeField]
    protected Direction directionQueue = Direction.Down;
    [SerializeField]
    protected Direction direction = Direction.Down;
    protected Vector2Int beginPos;

    [SerializeField]
    public Vector2Int currentPos;

    [SerializeField]
    protected Vector2Int endPos;
    [SerializeField]
    protected bool isMove = false;
    protected float speed; //움직이는 속도
    protected float initSpeed; //원래 속도
    
    //움직임 코루틴
    protected Coroutine moveCoroutine = null;

    //-----상태 관련-----
    public IState wait = new StateWait();
    public IState run = new StateRun();
    public IState scatter = new StateScatter();
    public IState eaten = new StateEaten();
    public IState frightend = new StateFrightend();

    //-----애니메이션-----
    [HideInInspector]
    public Animator animator;
    public Sprite sprite; // 22.01.11
    protected Sprite[] eaten_Sprites = null; //eaten sprites

    //상태기계 클래스객체
    public StateMachine stateMachine;
    public string Display_Currentstate = "";

    public virtual void Start()
    {
        Init();
    }
/*
    // Update is called once per frame
    void Update()
    {
        stateMachine.DoOperateUpdate();
    }
*/

    //하위 클래스에서 꼭 작성하기. Player.cs의 Init()함수를 참고
    public virtual void Init()
    {

        //애니메이션
        animator = this.GetComponent<Animator>();
        

        //스프라이트 - 22.01.11
        sprite = GetComponent<SpriteRenderer>().sprite;
        
        //eatenSprite 배열 할당 - 22.01.11

        eaten_Sprites = Resources.LoadAll<Sprite>("eaten");

        Reset();
    }

    //모든 변수 할당 완료 후 reset - 22.01.11
    public void Reset()
    {
        //움직임 코루틴 정지
        if(null != moveCoroutine)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
            isMove = false;
        }
        
        animator.enabled = true;
        animator.Play("Down");

        //위치 지정
        currentPos = beginPos;
        endPos = currentPos;
        speed = initSpeed;
        transform.position = new Vector2(beginPos.x, beginPos.y);
    }

    

}

#region StateMachine

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
public class StateRun : MonoBehaviour, IState 
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

public class StateScatter : IState
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

public class StateFrightend : IState
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

public class StateEaten : IState
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

#endregion