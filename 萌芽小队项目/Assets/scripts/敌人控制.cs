using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class 敌人控制 : MonoBehaviour
{
	public int hp;
	public int atk;

	public int slotPosX;
	public int slotPosY;
	//另外得知道棋盘有几行几列
	public GameObject 棋盘;
	private int 棋盘横向数量;
	private int 棋盘纵向数量;


	private bool bNowMoving = false;
	private float moveIntervalTime = 0;//骰子位移操作的间隔时间
	public bool bJustMoved = false;

	private Transform enemyTransform;//=GameObject.Find("骰子").transform;
	private Transform slotsParentTransform;//=GameObject.Find("棋盘").transform;

	public GameObject 骰子;
	private int dicePosX;
	private int dicePosY;

	public bool bRoundEnemyCanMove;

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

		atk = hp;
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
		if (bNowMoving && !bJustMoved&&bRoundEnemyCanMove)
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
		bool canUp = true;
		if (slotPosY < 棋盘纵向数量) 
		{
			if(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY + 1 - 1) * 棋盘横向数量).name.Substring(0, 3) == "Obs")
				canUp = false;
		}

		bool canDown = true;
		if (slotPosY > 1) 
		{
			if(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1 - 1) * 棋盘横向数量).name.Substring(0, 3) == "Obs")
				canDown = false;
		}

		bool canLeft = true;
		if (slotPosX > 1) 
		{
			if(slotsParentTransform.GetChild(slotPosX - 1 - 1 + (slotPosY - 1) * 棋盘横向数量).name.Substring(0, 3) == "Obs")
				canLeft = false;
		}

		bool canRight = true;
		if (slotPosX < 棋盘纵向数量) 
		{
			if(slotsParentTransform.GetChild(slotPosX + 1 - 1 + (slotPosY - 1) * 棋盘横向数量).name.Substring(0, 3) == "Obs")
				canRight = false;
		}


		if (slotPosX >= 1 && slotPosY >= 1 && slotPosX <= 棋盘横向数量 && slotPosY <= 棋盘纵向数量)
		{
			if (dicePosY>slotPosY && slotPosY < 棋盘纵向数量 && canUp)
			{
				slotPosY += 1;
				enemyTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
				
				bJustMoved = true;
				moveIntervalTime = 0;
			}
			else if (dicePosY<slotPosY && slotPosY > 1 && canDown)
			{
				slotPosY -= 1;
				enemyTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
				
				bJustMoved = true;
				moveIntervalTime = 0;
			}
			else if (dicePosX<slotPosX && slotPosX > 1 && canLeft)
			{
				slotPosX -= 1;
				enemyTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
				
				bJustMoved = true;
				moveIntervalTime = 0;
			}
			else if (dicePosX>slotPosX && slotPosX < 棋盘横向数量 && canRight)
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
		if(UnityEditor.EditorApplication.isPlaying)//只有在播放模式才做这个操作，要不然也会空引用
			if (Time.time > 1f)//不要一开始就运行，这样会找不到slotsParentTransform
			enemyTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
			slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
	}

    /*private void OnCollisionEnter(Collision collision)//事实上，我更想在敌人控制的脚本写这个函数
    {
        if (collision == null) return;

		var player = collision.gameObject.GetComponent<骰子的设定和控制>();
        if (player == null) return;

        if (this.atk < player.hp)//被玩家击败的情况
        {
            Debug.Log("destroy了一个目标");
            Destroy(player.gameObject);
        }
        else if (this.hp >= player.atk)//击败玩家的情况
        {
            Debug.Log("你已被击败");
            //Destroy(this.gameObject);//就不要销毁了，省的报错

            //不渲染了，说是要做动画
            this.GetComponent<Renderer>().enabled = false;

            GameObject 回合计数器 = GameObject.Find("回合计数器");
            回合计数器.GetComponent<回合计数器>().玩家失败 = true;
        }


    }*/
}
