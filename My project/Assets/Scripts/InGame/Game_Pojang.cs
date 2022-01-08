using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Pojang : MonoBehaviour
{
    public Player player;

    #if UNITY_EDITOR
    void Start()
    {
        Debug.Log("In Unity Editor!");
    }
    void Update()
    {
        KeyboardInput();
    }

    void KeyboardInput()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow)) Up();
        else if(Input.GetKeyDown(KeyCode.DownArrow)) Down();
        else if(Input.GetKeyDown(KeyCode.LeftArrow)) Left();
        else if(Input.GetKeyDown(KeyCode.RightArrow)) Right();
    }
    #endif

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
