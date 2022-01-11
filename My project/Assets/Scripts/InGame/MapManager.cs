using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
    //Node [,] PojangArr = new Node[41, 32]; -> Define으로 이동 22.01.07

    public GameObject cookiePref;
    public GameObject powerCookiePref;

    void OnEnable()
    {
        //Pojang - text 맵파일 불러오고, Node 2차원 배열 초기화 22-01-10
        T.CurrentMap = T.PojangArr;
        
        GetMap("Pojang");
        Debug.Log("맵 불러오기 완료");

        //Pojang맵에 쿠키 배치 22.01.08
        //원하지 않는 곳에 생성하지 않도록 수정 - 22.01.11
        Cookies(T.PojangArr);
        Debug.Log("쿠키 배치 완료");
    }


    #region 모든 길에 쿠키 배치 22.01.08
    List<Tuple<int, int>> ExceptionPosList = new List<Tuple<int, int>>(); //wall이 되었으면 하는 리스트

    public void Cookies(Node[,] map, int powerCookie = 5)
    {
        List<Node> mapNode = new List<Node>();
        GameObject parentObj = new GameObject();

        //Exception 인덱싱
        if(T.stage == "포장마차")
        {
        
            ExceptionPosList.Clear();
            string[] indexing = T.pojang_exception_wall.Split('\n');
            foreach(var a in indexing)
            {
                string [] s = a.Split(',');
                Tuple<int, int> tmp = new Tuple<int, int>(int.Parse(s[0]), int.Parse(s[1]));
                if(!ExceptionPosList.Contains(tmp))
                {
                    ExceptionPosList.Add(new Tuple<int, int>(int.Parse(s[0]), int.Parse(s[1])));
                }
            
            }

        }


        parentObj.name = "Cookies";

        for(int i=0;i<map.GetLength(0);i++)
        {
            for(int j=0;j<map.GetLength(1);j++)
            {
                if(!map[i,j].wall && !ExceptionPosList.Contains(new Tuple<int, int>(i,j)))
                {
                    mapNode.Add(map[i,j]);
                }
            }
        }

        //by 현수 - 쿠키 및 파워쿠키 배치 - 22.01.10
        List<int> arr = FindUnDuplicated(mapNode.Count, 5);
        for(int i=0;i<mapNode.Count;i++)
        {
            //파워쿠키 생성
            if(arr.Contains(i)){
                GameObject tmp = Instantiate(powerCookiePref, new Vector3(mapNode[i].x, mapNode[i].y, 5), Quaternion.identity);
                tmp.name = "Power Cookies(" + mapNode[i].x + ", " + mapNode[i].y + ")";
                tmp.gameObject.transform.parent = parentObj.transform;
            }


            //일반쿠키 생성
            else{
                GameObject tmp = Instantiate(cookiePref, new Vector3(mapNode[i].x, mapNode[i].y, 5), Quaternion.identity);
                tmp.name = "Cookies(" + mapNode[i].x + ", " + mapNode[i].y + ")";
                tmp.gameObject.transform.parent = parentObj.transform;
            }
        }
    }

    //count개수에서 num만큼 중복없이 뽑는다. (파워쿠키 생성용) - 22.01.10
    List<int> FindUnDuplicated(int count, int num)
    {
        List<int> re = new List<int>();

        while(re.Count != num)
        {
            int curNum = UnityEngine.Random.Range(0, count);
            if(re.Contains(curNum)) continue;
            else re.Add(curNum);
        }
        return re;
    }

    #endregion
    
    
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

            T.stage = "포장마차";
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

