using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class Convenience : EditorWindow {

    //By 현수 - 에디터 편의성 / 개발속도 향상 22.01.09
    //Edior에 메뉴를 만들라는 의미. 
    // % == Ctrl
    // & == Alt
    [MenuItem("My project/Convenience/One &1")]
    public static void GoToSceneOne()
    {
        if(EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
    }

    [MenuItem("My project/Convenience/Two &2")]
    public static void GoToSceneTwo()
    {
        if(EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        EditorSceneManager.OpenScene("Assets/Scenes/Loading.unity");
    }

    [MenuItem("My project/Convenience/Three &3")]
    public static void GoToSceneThree()
    {
        if(EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        EditorSceneManager.OpenScene("Assets/Scenes/Game1.unity");
    }

    [MenuItem("My project/Convenience/Four &4")]
    public static void GoToSceneFour()
    {
        if(EditorSceneManager.GetActiveScene().isDirty)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        EditorSceneManager.OpenScene("Assets/Scenes/Testing.unity");
    }
    
}
