using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

public class Pojang_Maker : MonoBehaviour
{
    
    [Multiline(20)]
    [Tooltip("맵에서 벽으로 인식했으면 하는 부분을 \",\"로 구분하여 넣어준다. ")]
    public string forException = "";

    [Multiline(20)]
    [Tooltip("맵에서 길로 인식했으면 하는 부분을 \",\"로 구분하여 넣어준다. ")]
    public string forException_2 = "";
    string value = "";
    public GameObject White;
    public GameObject Black;

    void Start()
    {
        StartCoroutine(Mapping("Pojang", 41, 32));
    }

    //by 현수 - 맵 전체의 가로세로 길이를 파라미터로 넣어준다.
    List<Tuple<int, int>> ExceptionPosList = new List<Tuple<int, int>>(); //wall이 되었으면 하는 리스트
    List<Tuple<int, int>> ExceptionPosList_2 = new List<Tuple<int, int>>(); //wall이 되지 않았으면 하는 리스트
    IEnumerator Mapping(string name, int x, int y)
    {
        Debug.Log("매핑 시작");
        
        //Exception 인덱싱
        ExceptionPosList.Clear();
        string[] indexing = forException.Split('\n');
        foreach(var a in indexing)
        {
            string [] s = a.Split(',');
            Tuple<int, int> tmp = new Tuple<int, int>(int.Parse(s[0]), int.Parse(s[1]));
            if(!ExceptionPosList.Contains(tmp))
            {
                ExceptionPosList.Add(new Tuple<int, int>(int.Parse(s[0]), int.Parse(s[1])));
            }
        
        }

        //Exception_2 인덱싱
        ExceptionPosList_2.Clear();
        string[] indexing_2 = forException_2.Split('\n');
        foreach(var a in indexing_2)
        {
            string [] s_2 = a.Split(',');
            Tuple<int, int> tmp = new Tuple<int, int>(int.Parse(s_2[0]), int.Parse(s_2[1]));
            if(!ExceptionPosList_2.Contains(tmp))
            {
                ExceptionPosList_2.Add(new Tuple<int, int>(int.Parse(s_2[0]), int.Parse(s_2[1])));
            }
        
        }


        GameObject MappingObject = new GameObject(); //mapping visualize 오브젝트들의 부모 오브젝트
        bool flag = false;
        bool flag_2 = false;

        for(int i=0;i<x;i++)
        {
            for(int j=0;j<y;j++){

                flag = false;
                flag_2 = false;

                //예외적인 부분이 있는지 확인
                if(ExceptionPosList.Contains(new Tuple<int, int>(i,j)))
                {
                    flag = true;
                }

                if(ExceptionPosList_2.Contains(new Tuple<int, int>(i,j)))
                {
                    flag_2 = true;
                }

                bool cols = Physics2D.OverlapCircle(new Vector3(i, j, 0), 0.5f);

                if((cols || flag) && !flag_2)
                {
                    //Debug.LogWarning(i + ", " + j + "에 벽 발견");
                    MakeObject(i, j, 1, MappingObject);
                    value += i + " " + j + " " + 1 + " ";
                }
                else if(!cols || flag_2){
                    //Debug.Log(i + ", " + j + "에 아무것도 없음");
                    MakeObject(i, j, 0, MappingObject);
                    value += i + " " + j + " " + 0 + " ";
                }
                //yield return new WaitForSecondsRealtime(0.02f);
                
            }
        }
        
        CreateTextFile(name, value);
        yield return null;
    }

    //by 현수 - MappingObject의 자식으로 생성한다. - 22.01.09
    void MakeObject(int i, int j, int v, GameObject MappingObject)
    {
        GameObject Obj = null;
        if(v == 1){
            Obj = Instantiate(Black, new Vector3(i,j,0), Quaternion.identity);
        }
        else if(v == 0){
            Obj = Instantiate(White, new Vector3(i,j,0), Quaternion.identity);
            
        }
        string s = i + ", " + j + "\t->" + v;
        Obj.name = s;

        //부모 오브젝트로 수정
        Obj.transform.parent = MappingObject.transform;
    }

    void CreateTextFile(string name, string value)
    {
        string path = "Assets/TextMap/";
        path += name + ".txt";

        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

        writer.WriteLine(value);
        writer.Close();

        Debug.Log(name + ".txt 가 만들어졌습니다.");
    }

}
