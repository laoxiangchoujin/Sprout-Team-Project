using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI对应的函数 : MonoBehaviour
{
    public GameObject 回合计数器;

    //以下两个方法控制鼠标悬停时UI大小动态变化（结合Event Trigger组件）
    public void OnMouseEnter(GameObject ui)//使用Pointer Enter
    {
        ui.transform.localScale = ui.transform.localScale / 0.9f;
    }
    public void OnMouseExit(GameObject ui)//使用Pointer Exit
    {
        ui.transform.localScale = ui.transform.localScale * 0.9f;
    }

    public void StartGame()
    {
        //点击后跳转至关卡地图选择界面
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
