using UnityEngine;

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
    public static Node [,] PojangArr = new Node[41, 32];
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