using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI对应的函数 : MonoBehaviour
{
    //以下两个方法控制鼠标悬停时UI大小动态变化（结合Event Trigger组件）
    public void OnMouseEnter(GameObject ui)//使用Pointer Enter
    {
        ui.transform.localScale = ui.transform.localScale / 0.95f;
    }
    public void OnMouseExit(GameObject ui)//使用Pointer Exit
    {
        ui.transform.localScale = ui.transform.localScale * 0.95f;
    }

    public void EnterLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void BackTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
