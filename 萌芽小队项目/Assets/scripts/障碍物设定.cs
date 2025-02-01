using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class 障碍物设定 : MonoBehaviour
{
    public int slotPosX;
    public int slotPosY;
    //另外得知道棋盘有几行几列
    public GameObject 棋盘;
    private int 棋盘横向数量;
    private int 棋盘纵向数量;

    public bool canDestroyed;

    private Transform obstacleTransform;//=GameObject.Find("骰子").transform;
    private Transform slotsParentTransform;//=GameObject.Find("棋盘").transform;

    void Start()
    {
        obstacleTransform = this.transform;
        StartCoroutine(延时());
    }

    IEnumerator 延时()
    {
        yield return new WaitForSeconds(0.05f);
        slotsParentTransform = GameObject.Find("slotsParent").transform;//!!!注意!!!这行代码得在棋盘生成插槽之后才能调用，所以调整延后一下本脚本的执行顺序
                                                                        //diceTransform.transform.SetParent(trans_plane,true);

        棋盘横向数量 = 棋盘.GetComponent<基础棋盘>().棋盘横向数量;
        棋盘纵向数量 = (int)(棋盘.GetComponent<基础棋盘>().棋盘横向数量 * 棋盘.GetComponent<基础棋盘>().棋盘长宽比);

        obstacleTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
            slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
    }

    void Update()
    {
        
    }

    private void OnValidate()
    {
        if (UnityEditor.EditorApplication.isPlaying)//只有在播放模式才做这个操作，要不然也会空引用
            if (Time.time > 1f)//不要一开始就运行，这样会找不到slotsParentTransform
                obstacleTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
                slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
    }
}
