using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Pojang : MonoBehaviour
{
    public Player player;


    /////조작 버튼 메서드
    public void Up()
    {
        player.ChangeDir(Direction.Up);
    }

    public void Down()
    {
        player.ChangeDir(Direction.Down);
    }

    public void Left()
    {
        player.ChangeDir(Direction.Left);
    }

    public void Right()
    {
        player.ChangeDir(Direction.Right);
    }
}
