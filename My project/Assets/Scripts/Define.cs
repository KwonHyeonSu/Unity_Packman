using UnityEngine;

//게임 상태
public enum GameState
{
    Pause = 0,
    Ready = 1,
    Playing = 2,
    End = 3

}

//방향 정의
public enum Direction
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}

//3차원 움직임 구현을 위한 방향 벡터화 및 2차원 노드 맵 접근성을 위하여
public static class T
{
    //맵
    public static Node [,] CurrentMap; //22.01.10 
    public static Node [,] PojangArr = new Node[41, 32];

    //플레이어 및 쿠키 생성 시 벽으로 인식하게 만드는 string[좌표] - 22.01.11
    public static string pojang_exception_wall = "28,13\n27,13\n26,13\n25,13\n26,14\n27,14\n26,15\n27,15\n7, 26\n8, 26\n9, 26\n10, 26\n11, 26"; 


    //점수 및 스테이지, 목숨
    public static int score;
    public static string stage;
    public static int life;

    //게임 상태
    public static GameState currentGameState = GameState.Pause;

    public static Vector2Int Direction2D(Direction dir)
    {
        Vector2Int re = Vector2Int.zero;
        if(dir == Direction.Up)
        {
            re = new Vector2Int(0, 1);
        }
        else if(dir == Direction.Down)
        {
            re = new Vector2Int(0, -1);
        }
        else if(dir == Direction.Left)
        {
            re = new Vector2Int(-1, 0);
        }
        else if(dir == Direction.Right)
        {
            re = new Vector2Int(1, 0);
        }

        return re;
    }
    
    public static string Dir2Str(Direction dir)
    {
        string re = "";
        if(dir == Direction.Up)
        {
            re = "Up";
        }
        else if(dir == Direction.Down)
        {
            re = "Down";
        }
        else if(dir == Direction.Left)
        {
            re = "Left";
        }
        else if(dir == Direction.Right)
        {
            re = "Right";
        }

        return re;
    }
}
