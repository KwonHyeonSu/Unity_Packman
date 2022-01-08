using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//by 현수 - Map정보가 담긴 Txt파일을 불러와 배열에 저장.

public class Node
{
    public bool wall;
    public int x;
    public int y;
    public Node Parent;

    //by 현수 - Node 생성자
    public Node(bool _wall, int _x, int _y)
    {
        wall = _wall;
        x = _x;
        y = _y;
    }

    public int g, h;
    public int f{get{return g+h;}}
}

public class MapManager : MonoBehaviour
{
    //Node [,] PojangArr = new Node[41, 32];

    void Start()
    {
        //text 맵파일 불러오고, Node 2차원 배열 초기화
        GetMap("Pojang");
        Debug.Log("맵 불러오기 완료");
    }

    //name에 해당하는 맵을 불러온 후, Node화 하여 초기화하는 메서드
    public void GetMap(string name)
    {
        string path = "Assets/TextMap/";
        string value = ""; //불러온 txt파일 내의 정보
        if(name == "Pojang")
        {
            path += name + ".txt";
            value = ReadTxt(path);
            
            //문자열 자르기
            string[] text = value.Split(' ');
            for(int i=0;i<text.Length/3;i++)
            {
                bool isWall = false;
                int x = int.Parse(text[i*3]);
                int y = int.Parse(text[i*3+1]);

                if(text[i*3+2] == "1") isWall = true;
                else isWall = false;


                Node tmpNode = new Node(isWall, x, y);

                T.PojangArr[x,y] = tmpNode;
            }
        }

    }

    public string ReadTxt(string filePath)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        string value = "";

        if(fileInfo.Exists)
        {
            StreamReader reader = new StreamReader(filePath);
            value = reader.ReadToEnd();
            reader.Close();
            return value;
        }

        else return "";
    }
}
