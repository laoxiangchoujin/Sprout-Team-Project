using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class 障碍物设定 : MonoBehaviour
{
    public int hp = 0;

    public int slotPosX;
    public int slotPosY;
    //另外得知道棋盘有几行几列
    public GameObject 棋盘;
	private GameObject[,] allSlots;
	private int 棋盘横向数量;
    private int 棋盘纵向数量;

    public bool canBeDestroyed;

    private Transform obstacleTransform;//=GameObject.Find("骰子").transform;

    void Start()
    {
        obstacleTransform = this.transform;

        allSlots=棋盘.GetComponent<基础棋盘>().allSlots;
        originalX=slotPosX; originalY=slotPosY;

        StartCoroutine(延时());
	}

    IEnumerator 延时()
    {
        yield return new WaitForSeconds(0.01f);

        棋盘横向数量 = 棋盘.GetComponent<基础棋盘>().棋盘横向数量;
        棋盘纵向数量 = (int)(棋盘.GetComponent<基础棋盘>().棋盘横向数量 * 棋盘.GetComponent<基础棋盘>().棋盘长宽比);

        obstacleTransform.position = new Vector3(allSlots[slotPosX -1, slotPosY -1].transform.position.x, 0.5f,
			allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

		allSlots[slotPosX - 1, slotPosY - 1].transform.name = "Obstacle"+ allSlots[slotPosX - 1, slotPosY - 1].transform.name;
        
        obstacleTransform.SetParent(allSlots[slotPosX - 1, slotPosY - 1].transform);//设置障碍物在插槽的子级，就可以从插槽找到障碍物
	}
    
    //这以下的三个函数使得可以通过onvalidate更改障碍物和插槽状态
    void Update()
    {
        changeIntervalTime += Time.deltaTime;
	}  

    void changeObstacleState(int originalX,int originalY,int newX,int newY)
    {
        allSlots[originalX - 1, originalY - 1].name = originalX.ToString() + ',' + originalY.ToString();

		obstacleTransform.position = new Vector3(allSlots[newX - 1, newY - 1].transform.position.x, 0.5f,
				allSlots[newX - 1, newY - 1].transform.position.z);

		allSlots[newX - 1, newY - 1].name = "Obstacle" + newX.ToString() + ',' + newY.ToString();
	}

    float changeIntervalTime = 0f;
	int originalX, originalY;
	private void OnValidate()
    {
        if(changeIntervalTime > 0.5f)
        {
            if (allSlots[slotPosX -1,slotPosY - 1].transform.childCount == 0)
            {
				changeObstacleState(originalX, originalY, slotPosX, slotPosY);
				changeIntervalTime = 0f;

				originalX = slotPosX; originalY = slotPosY;

                #if UNITY_EDITOR
				UnityEditor.EditorApplication.delayCall += () =>
				{
					if (this == null) return;
					obstacleTransform.SetParent(allSlots[slotPosX - 1, slotPosY - 1].transform);//设置障碍物在插槽的子级，就可以从插槽找到障碍物
				};
                #endif
				}
			}	
	}
}
