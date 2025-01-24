using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class 敌人控制 : MonoBehaviour
{
	private int hp;
	private int atk;

	public int slotPosX;
	public int slotPosY;
	//另外得知道棋盘有几行几列
	public GameObject 棋盘;
	private int 棋盘横向数量;
	private int 棋盘纵向数量;


	private bool bNowMoving = false;
	private float moveIntervalTime = 0;//骰子位移操作的间隔时间
	private bool bJustMoved = false;

	private Transform enemyTransform;//=GameObject.Find("骰子").transform;
	private Transform slotsParentTransform;//=GameObject.Find("棋盘").transform;

	public GameObject 骰子;
	private int dicePosX;
	private int dicePosY;

	// Start is called before the first frame update
	void Start()
	{
		enemyTransform = this.transform;//测试一下直接这样行不行

		slotsParentTransform = GameObject.Find("slotsParent").transform;//!!!注意!!!这行代码得在棋盘生成插槽之后才能调用，所以调整延后一下本脚本的执行顺序
																		//diceTransform.transform.SetParent(trans_plane,true);

		棋盘横向数量 = 棋盘.GetComponent<基础棋盘>().棋盘横向数量;
		棋盘纵向数量 = (int)(棋盘.GetComponent<基础棋盘>().棋盘横向数量 * 棋盘.GetComponent<基础棋盘>().棋盘长宽比);

		enemyTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
			slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);

		dicePosX = 骰子.GetComponent<骰子的设定和控制>().slotPosX;
		dicePosY = 骰子.GetComponent<骰子的设定和控制>().slotPosY;
	}

	// Update is called once per frame
	void Update()
	{
		dicePosX = 骰子.GetComponent<骰子的设定和控制>().slotPosX;
		dicePosY = 骰子.GetComponent<骰子的设定和控制>().slotPosY;
		if (true)
		{
			bNowMoving = true;
		}
		moveIntervalTime += Time.deltaTime;
		if (moveIntervalTime > 0.5)
		{
			bJustMoved = false;
		}
		if (bNowMoving && !bJustMoved)
		{
			//执行移动的代码
			enemyMove();
		}
	}

	//void showOtherAspects()
	//{
	//	Debug.Log("现在朝上的面为：" + nowUpAspect.num + '\n'
	//		+ "上边的面为" + nowUpAspect.up.num + "下边的面为" + nowUpAspect.down.num
	//		+ "左边的面为" + nowUpAspect.left.num + "右边的面为" + nowUpAspect.right.num);

	//}
	void enemyMove()
	{
		if (slotPosX >= 1 && slotPosY >= 1 && slotPosX <= 棋盘横向数量 && slotPosY <= 棋盘纵向数量)
		{
			if (dicePosY>slotPosY && slotPosY < 棋盘纵向数量)
			{
				slotPosY += 1;
				enemyTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
				
				bJustMoved = true;
				moveIntervalTime = 0;
			}
			else if (dicePosY<slotPosY && slotPosY > 1)
			{
				slotPosY -= 1;
				enemyTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
				
				bJustMoved = true;
				moveIntervalTime = 0;
			}
			else if (dicePosX<slotPosX && slotPosX > 1)
			{
				slotPosX -= 1;
				enemyTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
				
				bJustMoved = true;
				moveIntervalTime = 0;
			}
			else if (dicePosX>slotPosX && slotPosX < 棋盘横向数量)
			{
				slotPosX += 1;
				enemyTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
				
				bJustMoved = true;
				moveIntervalTime = 0;
			}
		}
	}
	private void OnValidate()
	{
		if (Time.time > 1f)//不要一开始就运行，这样会找不到slotsParentTransform
			enemyTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
			slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
	}
}
