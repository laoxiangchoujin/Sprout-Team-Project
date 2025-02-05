using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class 敌人控制 : MonoBehaviour
{
	public bool canUp = true;
	public bool canDown = true;
	public bool canLeft = true;
	public bool canRight = true;

    //以下两种类型的小怪位移方式与其他小怪不同
    public bool M4;
	public bool M6;
	private int 远程攻击控制 = 0;//每三回合攻击一次
    public bool canUpLeft = false;
    public bool canUpRight = false;
    public bool canDownLeft = false;
    public bool canDownRight = false;

    public int hp;
	public int atk;

	public int slotPosX;
	public int slotPosY;
	//另外得知道棋盘有几行几列
	public GameObject 棋盘;
	private GameObject[,] allSlots; 
	private int 棋盘横向数量;
	private int 棋盘纵向数量;


	private bool bNowMoving = false;
	private float moveIntervalTime = 0;//骰子位移操作的间隔时间
	public bool bJustMoved = false;

	private Transform enemyTransform;//=GameObject.Find("骰子").transform;
	//private Transform slotsParentTransform;//=GameObject.Find("棋盘").transform;

	public GameObject 骰子;
	private int dicePosX;
	private int dicePosY;

	public bool bRoundEnemyCanMove;

    private GameObject player;
    public GameObject rangeAttackTag; //远程攻击标记

    // Start is called before the first frame update
    void Start()
	{
		allSlots=棋盘.GetComponent<基础棋盘>().allSlots;

		enemyTransform = this.transform;//测试一下直接这样行不行

		棋盘横向数量 = 棋盘.GetComponent<基础棋盘>().棋盘横向数量;
		棋盘纵向数量 = (int)(棋盘.GetComponent<基础棋盘>().棋盘横向数量 * 棋盘.GetComponent<基础棋盘>().棋盘长宽比);

		enemyTransform.position = new Vector3(allSlots[slotPosX -1,slotPosY -1].transform.position.x, 0.5f,
			allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

		allSlots[slotPosX - 1, slotPosY - 1].name = "Enemy" + allSlots[slotPosX - 1, slotPosY - 1].transform.name;
		enemyTransform.SetParent(allSlots[slotPosX - 1, slotPosY - 1].transform);

		dicePosX = 骰子.GetComponent<骰子的设定和控制>().slotPosX;
		dicePosY = 骰子.GetComponent<骰子的设定和控制>().slotPosY;

        player = GameObject.FindWithTag("Player");
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

		validateIntervalTime += Time.deltaTime;
	}

	void enemyMove()
	{
        if (M6)
        {
            canUp = false; canDown = false; canLeft = false; canRight = false;
            canUpLeft = false; canUpRight = false; canDownLeft = false; canDownRight = false;
			if (远程攻击控制 == 0) 
            {
				Instantiate(rangeAttackTag, player.transform.position, player.transform.rotation);
				Debug.Log("生成了一个远程攻击标记");
				远程攻击控制 = 3;
            }
			远程攻击控制--;
            bJustMoved = true;
            moveIntervalTime = 0;
            return;
        }
        canUp = true; canDown = true; canLeft = true; canRight = true;
        canUpLeft = true; canUpRight = true; canDownLeft = true; canDownRight = true;
        if (slotPosY < 棋盘纵向数量) 
		{
			if(allSlots[slotPosX - 1, slotPosY + 1 - 1].transform.name.Substring(0, 3) == "Obs" || allSlots[slotPosX - 1, slotPosY + 1 - 1].transform.name.Substring(0, 3) == "Ene")
				canUp = false;
		}
		if (slotPosY > 1) 
		{
			if(allSlots[slotPosX - 1, slotPosY - 1 - 1].transform.name.Substring(0, 3) == "Obs" || allSlots[slotPosX - 1, slotPosY - 1 - 1].transform.name.Substring(0, 3) == "Ene")
				canDown = false;
		}
		if (slotPosX > 1) 
		{
			if(allSlots[slotPosX - 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Obs" || allSlots[slotPosX - 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Ene")
				canLeft = false;
		}
		if (slotPosX < 棋盘纵向数量) 
		{
			if(allSlots[slotPosX + 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Obs" || allSlots[slotPosX + 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Ene")
				canRight = false;
		}

        if (slotPosY < 棋盘纵向数量 && slotPosX > 1)
        {
            if (allSlots[slotPosX - 1 - 1, slotPosY + 1 - 1].transform.name.Substring(0, 3) == "Obs" || allSlots[slotPosX - 1 - 1, slotPosY + 1 - 1].transform.name.Substring(0, 3) == "Ene")
                canUpLeft = false;
        }
        if (slotPosY < 棋盘纵向数量 && slotPosX < 棋盘纵向数量)
        {
            if (allSlots[slotPosX + 1 - 1, slotPosY + 1 - 1].transform.name.Substring(0, 3) == "Obs" || allSlots[slotPosX + 1 - 1, slotPosY + 1 - 1].transform.name.Substring(0, 3) == "Ene")
                canUpRight = false;
        }
        if (slotPosY > 1 && slotPosX > 1)
        {
            if (allSlots[slotPosX - 1 - 1, slotPosY - 1 - 1].transform.name.Substring(0, 3) == "Obs" || allSlots[slotPosX - 1 - 1, slotPosY - 1 - 1].transform.name.Substring(0, 3) == "Ene")
                canDownLeft = false;
        }
        if (slotPosY > 1 && slotPosX < 棋盘纵向数量)
        {
            if (allSlots[slotPosX + 1 - 1, slotPosY - 1 - 1].transform.name.Substring(0, 3) == "Obs" || allSlots[slotPosX + 1 - 1, slotPosY - 1 - 1].transform.name.Substring(0, 3) == "Ene")
                canDownRight = false;
        }
        //Debug.Log(canUp+","+canDown+","+canLeft+","+canRight);

        if (slotPosX >= 1 && slotPosY >= 1 && slotPosX <= 棋盘横向数量 && slotPosY <= 棋盘纵向数量)
		{
            if (M4)//可斜向移动
            {
                if (dicePosY > slotPosY && slotPosY < 棋盘纵向数量 && dicePosX < slotPosX && slotPosX > 1 && canUpLeft)
                {
                    bJustMoved = true;
                    moveIntervalTime = 0;
                    changeEnemyState(slotPosX, slotPosY, slotPosX - 1, slotPosY + 1);
                    slotPosY += 1;
                    slotPosX -= 1;
                    return;
                }
                else if (dicePosY > slotPosY && slotPosY < 棋盘纵向数量 && dicePosX > slotPosX && slotPosX < 棋盘横向数量 && canUpRight)
                {
                    bJustMoved = true;
                    moveIntervalTime = 0;
                    changeEnemyState(slotPosX, slotPosY, slotPosX + 1, slotPosY + 1);
                    slotPosY += 1;
                    slotPosX += 1;
                    return;
                }
                else if (dicePosY < slotPosY && slotPosY > 1 && dicePosX < slotPosX && slotPosX > 1 && canDownLeft)
                {
                    bJustMoved = true;
                    moveIntervalTime = 0;
                    changeEnemyState(slotPosX, slotPosY, slotPosX - 1, slotPosY - 1);
                    slotPosY -= 1;
                    slotPosX -= 1;
                    return;
                }
                else if (dicePosY < slotPosY && slotPosY > 1 && dicePosX > slotPosX && slotPosX < 棋盘横向数量 && canDownRight)
                {
                    bJustMoved = true;
                    moveIntervalTime = 0;
                    changeEnemyState(slotPosX, slotPosY, slotPosX + 1, slotPosY - 1);
                    slotPosY -= 1;
                    slotPosX += 1;
                    return;
                }
            }
            if (dicePosY>slotPosY && slotPosY < 棋盘纵向数量 && canUp)
			{
				
				//enemyTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,[slotPosX - 1, slotPosY - 1].transform.position.z);
				
				bJustMoved = true;
				moveIntervalTime = 0;
				changeEnemyState(slotPosX, slotPosY, slotPosX, slotPosY + 1);
				slotPosY += 1;
			}
			else if (dicePosY<slotPosY && slotPosY > 1 && canDown)
			{
				
				//enemyTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);
				
				bJustMoved = true;
				moveIntervalTime = 0;
				changeEnemyState(slotPosX, slotPosY, slotPosX, slotPosY - 1);
				slotPosY -= 1;
			}
			else if (dicePosX<slotPosX && slotPosX > 1 && canLeft)
			{
				
				//enemyTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);
				
				bJustMoved = true;
				moveIntervalTime = 0;
				changeEnemyState(slotPosX, slotPosY, slotPosX - 1, slotPosY);
				slotPosX -= 1;
			}
			else if (dicePosX>slotPosX && slotPosX < 棋盘横向数量 && canRight)
			{
				
				//enemyTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);
				
				bJustMoved = true;
				moveIntervalTime = 0;
				changeEnemyState(slotPosX, slotPosY, slotPosX + 1, slotPosY);
				slotPosX += 1;
			}
			else
			{
				Debug.Log("敌人想动，却动不了");
				bJustMoved = true;
				moveIntervalTime = 0;
			}
		}
	}

	//这以下的三个函数使得可以通过onvalidate更改障碍物和插槽状态
	void changeEnemyState(int originalX, int originalY, int newX, int newY)
	{
		allSlots[originalX - 1, originalY - 1].name = originalX.ToString() + ',' + originalY.ToString();

		//enemyTransform.SetParent(allSlots[newX - 1, newY - 1].transform);
		enemyTransform.position = new Vector3(allSlots[newX - 1, newY - 1].transform.position.x, 0.5f,
				allSlots[newX - 1, newY - 1].transform.position.z);

		allSlots[newX - 1, newY - 1].name = "Enemy" + newX.ToString() + ',' + newY.ToString();
	}

	float validateIntervalTime = 0f;
	int originalX, originalY;
	private void OnValidate()
	{
		if (validateIntervalTime > 1f)
		{
			if (allSlots[slotPosX - 1, slotPosY - 1].transform.childCount == 0)
			{
				changeEnemyState(originalX, originalY, slotPosX, slotPosY);
				validateIntervalTime = 0f;

				originalX = slotPosX; originalY = slotPosY;

				#if UNITY_EDITOR
				UnityEditor.EditorApplication.delayCall += () =>
				{
					if (this == null) return;
					enemyTransform.SetParent(allSlots[slotPosX - 1, slotPosY - 1].transform);//设置障碍物在插槽的子级，就可以从插槽找到障碍物
				};
				#endif
			}
		}
	}
}
