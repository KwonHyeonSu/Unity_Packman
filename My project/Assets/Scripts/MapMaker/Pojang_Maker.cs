using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Pojang_Maker : MonoBehaviour
{
    
    string value = "";
    public GameObject White;
    public GameObject Black;

    void Start()
    {
        StartCoroutine(Mapping("Pojang", 41, 32));
        
    }

    //by 현수 - 매핑 과정 가로세로 길이를 알아야함
    IEnumerator Mapping(string name, int x, int y)
    {
        Debug.Log("매핑 시작");
        for(int i=0;i<x;i++)
        {
            for(int j=0;j<y;j++){
                bool cols = Physics2D.OverlapCircle(new Vector3(i, j, 0), 0.5f);
                if(cols)
                {
                    Debug.LogWarning(i + ", " + j + "에 벽 발견");
                    MakeObject(i, j, 1);
                    value += i + " " + j + " " + 1 + " ";
                }
                else{
                    Debug.Log(i + ", " + j + "에 아무것도 없음");
                    MakeObject(i, j, 0);
                    value += i + " " + j + " " + 0 + " ";
                }
            }
        }
        
        CreateTextFile(name, value);
        yield return null;
    }

    void MakeObject(int i, int j, int v)
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
    }

    void CreateTextFile(string name, string value)
    {
        string path = "Assets/TextMap/";
        path += name + ".txt";

        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);

        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

        writer.WriteLine(value);
        writer.Close();
    }

}
