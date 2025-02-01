using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI对应的函数 : MonoBehaviour
{
    public GameObject 回合计数器;

    public void OnMouseEnter(GameObject ui)
    {
        ui.transform.localScale = ui.transform.localScale / 0.9f;
    }

    public void OnMouseExit(GameObject ui)
    {
        ui.transform.localScale = ui.transform.localScale * 0.9f;
    }

    public void StartGame()
    {
        //点击后跳转至关卡地图选择界面
    }

    public void ExitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
