using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class 骰子的设定和控制 : MonoBehaviour
{
    public bool canUp = true;
    public bool canDown = true;
    public bool canLeft = true;
    public bool canRight = true;

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

	private Transform diceTransform;//=GameObject.Find("骰子").transform;
	//private Transform slotsParentTransform;//=GameObject.Find("棋盘").transform;

    public class aspect
    {
        public int num;
		public aspect up;//因为是class，所以这里的上下左右是引用，而不是直接的值传递
        public aspect down;
		public aspect left;
		public aspect right;
	};

	//aspect aspect1 = new aspect();
	public aspect[] sixAspects = new aspect[7];

	public aspect nowUpAspect;
	public int nowUpNumber;

    public bool bRoundPlayerCanMove;

	public bool debugLog = false;

	private GameObject[] enemy;

	public Sprite 主角图像;
	GameObject 图像;//其实是空物体

	// Start is called before the first frame update
	void Start()
    {
		allSlots = 棋盘.GetComponent<基础棋盘>().allSlots;

        initDice();
		nowUpAspect = sixAspects[3];//先让某一面在上边
		//nowUpNumber = Random.Range(1, 7);//随机生成初始值
        //nowUpAspect = sixAspects[nowUpNumber];

        diceTransform =GameObject.Find("骰子").transform;

		showOtherAspects();

		//slotsParentTransform= GameObject.Find("slotsParent").transform;//!!!注意!!!这行代码得在棋盘生成插槽之后才能调用，所以调整延后一下本脚本的执行顺序
																	   //diceTransform.transform.SetParent(trans_plane,true);

		棋盘横向数量 = 棋盘.GetComponent<基础棋盘>().棋盘横向数量;
		棋盘纵向数量 = (int)(棋盘.GetComponent<基础棋盘>().棋盘横向数量* 棋盘.GetComponent<基础棋盘>().棋盘长宽比);

		diceTransform.position = new Vector3(allSlots[slotPosX -1,slotPosY -1].transform.position.x,0.5f,
			allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

		hp = nowUpAspect.num;
		atk = hp;

		enemy = GameObject.FindGameObjectsWithTag("Enemy");

		图像 = new GameObject();
		图像.AddComponent<SpriteRenderer>();
		图像.GetComponent<SpriteRenderer>().sprite = 主角图像;
		图像.transform.localScale *= 0.3f;
		图像.transform.eulerAngles += new Vector3(90, 0, 0);
		
	}

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyUp(KeyCode.W) && canUp) || (Input.GetKeyUp(KeyCode.A) && canLeft) || (Input.GetKeyUp(KeyCode.S) && canDown) || (Input.GetKeyUp(KeyCode.D) && canRight))
        {
            bNowMoving = true;
		}
		moveIntervalTime += Time.deltaTime;
		if (moveIntervalTime > 0.2)
		{
			bJustMoved = false;
		}
		if (bNowMoving && !bJustMoved && bRoundPlayerCanMove) 
		{
			//执行移动的代码
			diceMove();

			if (playerHas狂暴骰子)
			{
				hp = 8;
				atk = 8;
			}
			else
			{
				hp = nowUpAspect.num;
				atk = hp;
			}		
		}

		图像.transform.position = new Vector3(diceTransform.position.x + new Vector3(-0.4f, 0, 0).x,1.5f, diceTransform.position.z + new Vector3(0, 0, 0.4f).z);
    }
    void initDice()
    {
		for (int i = 1; i <= 6; i++) //上边只是给数组实例化了，还得给数组中的每个元素实例化
		{
			sixAspects[i] = new aspect();
            sixAspects[i].num = i;
        }

		sixAspects[1].up = sixAspects[5];
		sixAspects[1].down = sixAspects[2];
		sixAspects[1].left = sixAspects[4];
		sixAspects[1].right = sixAspects[3];

		sixAspects[2].up = sixAspects[1];
		sixAspects[2].down = sixAspects[6];
		sixAspects[2].left = sixAspects[4];
		sixAspects[2].right = sixAspects[3];

		sixAspects[3].up = sixAspects[6];
		sixAspects[3].down = sixAspects[1];
		sixAspects[3].left = sixAspects[5];
		sixAspects[3].right = sixAspects[2];

		sixAspects[4].up = sixAspects[6];
		sixAspects[4].down = sixAspects[1];
		sixAspects[4].left = sixAspects[2];
		sixAspects[4].right = sixAspects[5];

		sixAspects[5].up = sixAspects[6];
		sixAspects[5].down = sixAspects[1];
		sixAspects[5].left = sixAspects[4];
		sixAspects[5].right = sixAspects[3];

		sixAspects[6].up = sixAspects[2];
		sixAspects[6].down = sixAspects[5];
		sixAspects[6].left = sixAspects[4];
		sixAspects[6].right = sixAspects[3];
	}

	void showOtherAspects()
	{
		if(debugLog)
		Debug.Log("现在朝上的面为："+nowUpAspect.num+'\n'
			+"上边的面为"+nowUpAspect.up.num + "下边的面为" + nowUpAspect.down.num 
			+ "左边的面为" + nowUpAspect.left.num  + "右边的面为" + nowUpAspect.right.num );

	}
	void diceMove()
	{
		//canUp = true; canDown = true; canLeft = true; canRight = true;
		if (slotPosY < 棋盘纵向数量)//想向上走
		{
			if (allSlots[slotPosX - 1, slotPosY + 1 - 1].name.Substring(0, 3) == "Obs")//上边有障碍物
			{
				if(allSlots[slotPosX - 1, slotPosY + 1 - 1].GetComponentInChildren<障碍物设定>() != null)//上边的插槽有子物体				
					if(nowUpAspect.down.num < allSlots[slotPosX - 1, slotPosY + 1 - 1].GetComponentInChildren<障碍物设定>().hp)//障碍物的hp更大
					{
						canUp = false;
					}								
			}
		}
		if (slotPosY > 1)
		{
			if (allSlots[slotPosX - 1, slotPosY - 1 - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if(allSlots[slotPosX - 1, slotPosY - 1 - 1].GetComponentInChildren<障碍物设定>() != null)
					if(nowUpAspect.up.num <= allSlots[slotPosX - 1, slotPosY - 1 - 1].GetComponentInChildren<障碍物设定>().hp)
					{
						canDown = false;
					}
			}
		}
		if (slotPosX > 1)
		{
			if (allSlots[slotPosX - 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if(allSlots[slotPosX - 1 - 1, slotPosY - 1].GetComponentInChildren<障碍物设定>() != null)
					if(nowUpAspect.right.num <= allSlots[slotPosX - 1 - 1, slotPosY - 1].GetComponentInChildren<障碍物设定>().hp)
					{
						canLeft = false;
					}
			}
		}
		if (slotPosX < 棋盘纵向数量)
		{
			if (allSlots[slotPosX + 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if(allSlots[slotPosX + 1 - 1, slotPosY - 1].GetComponentInChildren<障碍物设定>() != null)
					if(nowUpAspect.left.num <= allSlots[slotPosX + 1 - 1, slotPosY - 1].GetComponentInChildren<障碍物设定>().hp)
					{
						canRight = false;
					}
			}
		}

		if (slotPosX >=1 && slotPosY >=1 && slotPosX <= 棋盘横向数量 && slotPosY <= 棋盘纵向数量)
		{
			if (Input.GetKey(KeyCode.W) && !canUp)
			{
				Debug.Log("无法前往(" + slotPosX + ',' + (slotPosY + 1) + ")，因为有障碍物或为上一次移动的反方向");
				return;
			}
			else if (Input.GetKey(KeyCode.S) && !canDown)
			{
				Debug.Log("无法前往(" + slotPosX + ',' + (slotPosY - 1) + ")，因为有障碍物或为上一次移动的反方向");
				return;
			}
			else if (Input.GetKey(KeyCode.A) && !canLeft)
			{
				Debug.Log("无法前往(" + (slotPosX - 1) + ',' + slotPosY + ")，因为有障碍物或为上一次移动的反方向");
				return;
			}
			else if (Input.GetKey(KeyCode.D) && !canRight)
			{
				Debug.Log("无法前往(" + (slotPosX + 1) + ',' + slotPosY + ")，因为有障碍物或为上一次移动的反方向");
				return;
			}

			if (Input.GetKey(KeyCode.W) && canUp && slotPosY < 棋盘纵向数量  )
			{
                canUp = true;
                canDown = false;
                canLeft = true;
                canRight = true;

                slotPosY += 1;
				if(!isMoving)
				    StartCoroutine(骰子向上运动());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

				int Num = nowUpAspect.down.num;
                nowUpAspect.up.num = nowUpAspect.num;
                nowUpAspect.down.num = 7 - nowUpAspect.num;
                nowUpAspect.num = Num;
                showOtherAspects();

				bJustMoved = true;
				moveIntervalTime = 0;
				if (debugLog)
					Debug.Log("现在的slotposx和slotposy分别是：" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.S) && canDown && slotPosY > 1 )
			{
                canUp = false;
                canDown = true;
                canLeft = true;
                canRight = true;

                slotPosY -= 1;
				if(!isMoving)
				    StartCoroutine(骰子向下运动());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

                int Num = nowUpAspect.up.num;
                nowUpAspect.up.num = 7 - nowUpAspect.num;
                nowUpAspect.down.num = nowUpAspect.num;
                nowUpAspect.num = Num;
                showOtherAspects();

				bJustMoved = true;
				moveIntervalTime = 0;
				if (debugLog)
					Debug.Log("现在的slotposx和slotposy分别是：" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.A) && canLeft && slotPosX > 1 )
			{
                canUp = true;
                canDown = true;
                canLeft = true;
                canRight = false;

                slotPosX -= 1;
				if(!isMoving)
				    StartCoroutine(骰子向左运动());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

                int Num = nowUpAspect.right.num;
                nowUpAspect.left.num = nowUpAspect.num;
                nowUpAspect.right.num = 7 - nowUpAspect.num;
                nowUpAspect.num = Num;
                showOtherAspects();

				bJustMoved = true;
				moveIntervalTime = 0;
				if (debugLog)
					Debug.Log("现在的slotposx和slotposy分别是：" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.D) && canRight && slotPosX < 棋盘横向数量 )
			{
                canUp = true;
                canDown = true;
                canLeft = false;
                canRight = true;

                slotPosX += 1;
				if(!isMoving)
				    StartCoroutine(骰子向右运动());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

                int Num = nowUpAspect.left.num;
                nowUpAspect.left.num = 7 - nowUpAspect.num;
                nowUpAspect.right.num = nowUpAspect.num;
				nowUpAspect.num = Num;
				showOtherAspects();

				bJustMoved = true;
				moveIntervalTime = 0;
				if (debugLog)
					Debug.Log("现在的slotposx和slotposy分别是：" + slotPosX + ',' + slotPosY);
			}
		}
		
	}
	/*private void OnValidate()
	{
		if (UnityEditor.EditorApplication.isPlaying)//只有在播放模式才做这个操作，要不然也会空引用
			if (Time.time > 1f) ;//不要一开始就运行，这样会找不到slotsParentTransform
			{
				diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);
			}
	}*/

	private void OnCollisionEnter(Collision collision)//事实上，我更想在敌人控制的脚本写这个函数
	{
		if (collision == null) return;

		var enemy= collision.gameObject.GetComponent<敌人控制>();
		if (enemy != null)
		{
			if (bRoundPlayerCanMove)//判断先手情况
            {
				if(this.atk >= enemy.hp)//战斗胜利的情况
                {
                    if (enemy.GetComponent<敌人控制>().M1 || enemy.GetComponent<敌人控制>().M2)
                    {
                        GameObject.Find("商店和背包（脚本）").GetComponent<商店和背包>().coinAmount += 1;
                        Debug.Log("+1金币");
                    }
                    else if (enemy.GetComponent<敌人控制>().M3 || enemy.GetComponent<敌人控制>().M4)
                    {
                        GameObject.Find("商店和背包（脚本）").GetComponent<商店和背包>().coinAmount += 2;
                        Debug.Log("+2金币");
                    }
                    else if (enemy.GetComponent<敌人控制>().M5 || enemy.GetComponent<敌人控制>().M6)
                    {
                        GameObject.Find("商店和背包（脚本）").GetComponent<商店和背包>().coinAmount += 3;
                        Debug.Log("+3金币");
                    }
                    Debug.Log("destroy了一个目标");
                    Destroy(enemy.gameObject);
                }
				else//战斗失败的情况
				{
					if (playerHas守护护盾)//失败但有护盾
					{
						GameObject shield = this.transform.GetChild(0).gameObject;
						Destroy(shield);

						playerHas守护护盾 = false;

						if (enemy.GetComponent<敌人控制>().M1 || enemy.GetComponent<敌人控制>().M2)
						{
							GameObject.Find("商店和背包（脚本）").GetComponent<商店和背包>().coinAmount += 1;
							Debug.Log("+1金币");
						}
						else if (enemy.GetComponent<敌人控制>().M3 || enemy.GetComponent<敌人控制>().M4)
						{
							GameObject.Find("商店和背包（脚本）").GetComponent<商店和背包>().coinAmount += 2;
							Debug.Log("+2金币");
						}
						else if (enemy.GetComponent<敌人控制>().M5 || enemy.GetComponent<敌人控制>().M6)
						{
							GameObject.Find("商店和背包（脚本）").GetComponent<商店和背包>().coinAmount += 3;
							Debug.Log("+3金币");
						}
						Debug.Log("destroy了一个目标");
						Destroy(enemy.gameObject);
					}
					else//失败并且没护盾
					{
						this.GetComponent<Collider>().enabled = false;//取消碰撞箱以避免重复检测
																	  //Destroy(this.gameObject);//就不要销毁了，省的报错
						Debug.Log("你已被击败");

						//不渲染了，说是要做动画
						this.GetComponent<Renderer>().enabled = false;

						GameObject 回合计数器 = GameObject.Find("回合计数器");
						回合计数器.GetComponent<回合计数器>().玩家失败 = true;
					}
                    
                }
			}
			else
			{
				if (this.hp > enemy.atk) 
				{
                    if (enemy.GetComponent<敌人控制>().M1 || enemy.GetComponent<敌人控制>().M2)
                    {
                        GameObject.Find("商店和背包（脚本）").GetComponent<商店和背包>().coinAmount += 1;
                        Debug.Log("+1金币");
                    }
                    else if (enemy.GetComponent<敌人控制>().M3 || enemy.GetComponent<敌人控制>().M4)
                    {
                        GameObject.Find("商店和背包（脚本）").GetComponent<商店和背包>().coinAmount += 2;
                        Debug.Log("+2金币");
                    }
                    else if (enemy.GetComponent<敌人控制>().M5 || enemy.GetComponent<敌人控制>().M6)
                    {
                        GameObject.Find("商店和背包（脚本）").GetComponent<商店和背包>().coinAmount += 3;
                        Debug.Log("+3金币");
                    }
                    Debug.Log("被destroy了一个目标");
                    Destroy(enemy.gameObject);
                }
				else
				{
                    this.GetComponent<Collider>().enabled = false;//取消碰撞箱以避免重复检测
                                                                  //Destroy(this.gameObject);//就不要销毁了，省的报错
                    Debug.Log("你被击败了");

                    //不渲染了，说是要做动画
                    this.GetComponent<Renderer>().enabled = false;

                    GameObject 回合计数器 = GameObject.Find("回合计数器");
                    回合计数器.GetComponent<回合计数器>().玩家失败 = true;
                }
			}
        }

		var obstacle = collision.gameObject.GetComponent<障碍物设定>();
		if(obstacle != null)
		{
			if(this.atk >= obstacle.hp)
			{
				Debug.Log("Destroy了一个障碍物");
				Destroy(obstacle.gameObject);
			}		
		}
		

		
	}


	//接下来写一些让骰子运动的协程
	//public IEnumerator 骰子向上运动()
	//{
	//	float animTime = 0.5f;

	//	float speed = 1 / animTime;//实际速度，乘上deltatime后是每帧走的距离

	//	float distance = 0;
	//	float maxDistance = 1f;

	//	while(distance < maxDistance)
	//	{
	//		diceTransform.Translate(new Vector3(0, 0, 1) * speed * Time.deltaTime);
	//		distance += speed * Time.deltaTime;

	//		yield return null;
	//	}

	//	yield return this;
	//}

	private bool isMoving;
	IEnumerator 骰子向上运动()
	{
		if (isMoving)
			yield break;

		isMoving = true;

		Vector3 startPos = transform.position;
		Vector3 endPos = startPos + new Vector3(0, 0, 1);

		//清空旋转状态（反正只是正方体，也看不出来）
		//transform.rotation = Quaternion.identity;
		Quaternion startRot = transform.rotation;
		//Quaternion endRot = Quaternion.Euler(90, 0, 0); // 向上
		//Quaternion endRot = startRot * Quaternion.Euler(90, 0, 0);//quaternion.elua返回的是四元数（意思是用欧拉角生成四元数），并且，角度相加在四元数是用相乘表示
		Quaternion endRot = Quaternion.AngleAxis(90, new Vector3(1, 0, 0)) * startRot;//quaternion.angleaxis是世界坐标系下，绕一个轴旋转一定角度
																			//并且，这里的右乘startrot，表示先在世界空间中应用旋转，再应用原有的局部旋转
		float t = 0;

		while (t <= 1f)
		{
			t += Time.deltaTime;
			float easeOut = 1 - Mathf.Pow(1 - t, 3);
			transform.position = Vector3.Lerp(startPos, endPos, easeOut);
			transform.rotation = Quaternion.Lerp(startRot, endRot, easeOut);
			yield return null;
		}

		isMoving= false;
		yield return null;
	}
	IEnumerator 骰子向下运动()
	{
		if (isMoving)
			yield break;

		isMoving = true;

		Vector3 startPos = transform.position;
		Vector3 endPos = startPos + new Vector3(0, 0, -1);

		Quaternion startRot = transform.rotation;
		Quaternion endRot = Quaternion.AngleAxis(-90, new Vector3(1, 0, 0)) * startRot;

		float t = 0;

		while (t <= 1f)
		{
			t += Time.deltaTime;
			float easeOut = 1 - Mathf.Pow(1 - t, 3);
			transform.position = Vector3.Lerp(startPos, endPos, easeOut);
			transform.rotation = Quaternion.Lerp(startRot, endRot, easeOut);
			yield return null;
		}

		isMoving = false;
		yield return null;
	}
	IEnumerator 骰子向左运动()
	{
		if (isMoving)
			yield break;

		isMoving = true;

		Vector3 startPos = transform.position;
		Vector3 endPos = startPos + new Vector3(-1, 0, 0);

		Quaternion startRot = transform.rotation;
		Quaternion endRot = Quaternion.AngleAxis(90, new Vector3(0, 0, 1)) * startRot;

		float t = 0;

		while (t <= 1f)
		{
			t += Time.deltaTime;
			float easeOut = 1 - Mathf.Pow(1 - t, 3);
			transform.position = Vector3.Lerp(startPos, endPos, easeOut);
			transform.rotation = Quaternion.Lerp(startRot, endRot, easeOut);
			yield return null;
		}

		isMoving=false; yield return null;
	}
	IEnumerator 骰子向右运动()
	{
		if (isMoving)
			yield break;

		isMoving = true;

		Vector3 startPos = transform.position;
		Vector3 endPos = startPos + new Vector3(1, 0, 0);

		Quaternion startRot = transform.rotation;
		Quaternion endRot = Quaternion.AngleAxis(-90, new Vector3(0, 0, 1)) * startRot;

		float t = 0;

		while (t <= 1f)
		{
			t += Time.deltaTime;
			float easeOut = 1 - Mathf.Pow(1 - t, 3);
			transform.position = Vector3.Lerp(startPos, endPos, easeOut);
			transform.rotation = Quaternion.Lerp(startRot, endRot, easeOut);
			yield return null;
		}
		isMoving=false;
		yield return null;
	}



	//一些技能效果

	bool 时间沙漏moved = false;
	void 时间沙漏move()
	{
		//就认为是dicemove的简化版，复制过来
		if (slotPosY < 棋盘纵向数量)//想向上走
		{
			if (allSlots[slotPosX - 1, slotPosY + 1 - 1].name.Substring(0, 3) == "Obs")//上边有障碍物
			{
				if (allSlots[slotPosX - 1, slotPosY + 1 - 1].GetComponentInChildren<障碍物设定>() != null)//上边的插槽有子物体				
					if (nowUpAspect.down.num < allSlots[slotPosX - 1, slotPosY + 1 - 1].GetComponentInChildren<障碍物设定>().hp)//障碍物的hp更大
					{
						canUp = false;
					}
			}
		}
		if (slotPosY > 1)
		{
			if (allSlots[slotPosX - 1, slotPosY - 1 - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if (allSlots[slotPosX - 1, slotPosY - 1 - 1].GetComponentInChildren<障碍物设定>() != null)
					if (nowUpAspect.up.num <= allSlots[slotPosX - 1, slotPosY - 1 - 1].GetComponentInChildren<障碍物设定>().hp)
					{
						canDown = false;
					}
			}
		}
		if (slotPosX > 1)
		{
			if (allSlots[slotPosX - 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if (allSlots[slotPosX - 1 - 1, slotPosY - 1].GetComponentInChildren<障碍物设定>() != null)
					if (nowUpAspect.right.num <= allSlots[slotPosX - 1 - 1, slotPosY - 1].GetComponentInChildren<障碍物设定>().hp)
					{
						canLeft = false;
					}
			}
		}
		if (slotPosX < 棋盘纵向数量)
		{
			if (allSlots[slotPosX + 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if (allSlots[slotPosX + 1 - 1, slotPosY - 1].GetComponentInChildren<障碍物设定>() != null)
					if (nowUpAspect.left.num <= allSlots[slotPosX + 1 - 1, slotPosY - 1].GetComponentInChildren<障碍物设定>().hp)
					{
						canRight = false;
					}
			}
		}

		if (slotPosX >= 1 && slotPosY >= 1 && slotPosX <= 棋盘横向数量 && slotPosY <= 棋盘纵向数量)
		{
			if (Input.GetKey(KeyCode.W) && !canUp)
			{
				Debug.Log("无法前往(" + slotPosX + ',' + (slotPosY + 1) + ")，因为有障碍物或为上一次移动的反方向");
				return;
			}
			else if (Input.GetKey(KeyCode.S) && !canDown)
			{
				Debug.Log("无法前往(" + slotPosX + ',' + (slotPosY - 1) + ")，因为有障碍物或为上一次移动的反方向");
				return;
			}
			else if (Input.GetKey(KeyCode.A) && !canLeft)
			{
				Debug.Log("无法前往(" + (slotPosX - 1) + ',' + slotPosY + ")，因为有障碍物或为上一次移动的反方向");
				return;
			}
			else if (Input.GetKey(KeyCode.D) && !canRight)
			{
				Debug.Log("无法前往(" + (slotPosX + 1) + ',' + slotPosY + ")，因为有障碍物或为上一次移动的反方向");
				return;
			}

			if (Input.GetKey(KeyCode.W) && canUp && slotPosY < 棋盘纵向数量)
			{
				canUp = true;
				canDown = false;
				canLeft = true;
				canRight = true;

				slotPosY += 1;
				StartCoroutine(骰子向上运动());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

				int Num = nowUpAspect.down.num;
				nowUpAspect.up.num = nowUpAspect.num;
				nowUpAspect.down.num = 7 - nowUpAspect.num;
				nowUpAspect.num = Num;
				showOtherAspects();

				时间沙漏moved = true;

				if (debugLog)
					Debug.Log("现在的slotposx和slotposy分别是：" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.S) && canDown && slotPosY > 1)
			{
				canUp = false;
				canDown = true;
				canLeft = true;
				canRight = true;

				slotPosY -= 1;
				StartCoroutine(骰子向下运动());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

				int Num = nowUpAspect.up.num;
				nowUpAspect.up.num = 7 - nowUpAspect.num;
				nowUpAspect.down.num = nowUpAspect.num;
				nowUpAspect.num = Num;
				showOtherAspects();

				时间沙漏moved = true;

				if (debugLog)
					Debug.Log("现在的slotposx和slotposy分别是：" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.A) && canLeft && slotPosX > 1)
			{
				canUp = true;
				canDown = true;
				canLeft = true;
				canRight = false;

				slotPosX -= 1;
				StartCoroutine(骰子向左运动());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

				int Num = nowUpAspect.right.num;
				nowUpAspect.left.num = nowUpAspect.num;
				nowUpAspect.right.num = 7 - nowUpAspect.num;
				nowUpAspect.num = Num;
				showOtherAspects();

				时间沙漏moved = true;

				if (debugLog)
					Debug.Log("现在的slotposx和slotposy分别是：" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.D) && canRight && slotPosX < 棋盘横向数量)
			{
				canUp = true;
				canDown = true;
				canLeft = false;
				canRight = true;

				slotPosX += 1;
				StartCoroutine(骰子向右运动());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

				int Num = nowUpAspect.left.num;
				nowUpAspect.left.num = 7 - nowUpAspect.num;
				nowUpAspect.right.num = nowUpAspect.num;
				nowUpAspect.num = Num;
				showOtherAspects();

				时间沙漏moved |= true;

				if (debugLog)
					Debug.Log("现在的slotposx和slotposy分别是：" + slotPosX + ',' + slotPosY);
			}
		}
	}//既然回合计数器只在意是否有bjustmoved，我把设置这个变量的代码删了，就不会触发它
	public IEnumerator 时间沙漏()
	{
		while(!时间沙漏moved)
		{
			 时间沙漏();
			yield return null;
		}
		yield return new WaitForSeconds(0.2f);
	}

	bool playerHas守护护盾 = false;
	public GameObject 守护护盾模型;
	public void 守护护盾()
	{
		GameObject shield=GameObject.Instantiate(守护护盾模型, transform);
		shield.transform.parent = transform;
		shield.transform.position = transform.position;
		playerHas守护护盾 = true;
	}

	bool playerHas狂暴骰子 = false;
	public IEnumerator 狂暴骰子()
	{
		playerHas狂暴骰子 = true;

		GameObject roundCounter = GameObject.Find("回合计数器");
		int nowRoundCount= roundCounter.GetComponent<回合计数器>().roundCount;
		while (true)
		{
			if (roundCounter.GetComponent<回合计数器>().roundCount >= nowRoundCount + 2)//超过两回合的情况
			{
				playerHas狂暴骰子 = false;
				break;
			}
			yield return null;
		}
		yield return null;
	}

	public IEnumerator 传送卷轴()
	{
		bool hasMoved = false;

		List <GameObject> slots = new List<GameObject>();

		try
		{
			GameObject obj = allSlots[slotPosX - 1, slotPosY + 1 - 1];//上
			if(obj.name.Substring(0,3)!="Ene"&&obj.name.Substring(0,3)!="Obs")
				slots.Add(obj);

			obj = allSlots[slotPosX + 1 - 1, slotPosY + 1 - 1];//右上
			if (obj.name.Substring(0, 3) != "Ene" && obj.name.Substring(0, 3) != "Obs")
				slots.Add(obj);

			obj = allSlots[slotPosX + 1 - 1, slotPosY - 1];//右
			if (obj.name.Substring(0, 3) != "Ene" && obj.name.Substring(0, 3) != "Obs")
				slots.Add(obj);

			obj = allSlots[slotPosX + 1 - 1, slotPosY - 1 - 1];
			if (obj.name.Substring(0, 3) != "Ene" && obj.name.Substring(0, 3) != "Obs")
				slots.Add(obj);

			obj = allSlots[slotPosX - 1, slotPosY - 1 - 1];
			if (obj.name.Substring(0, 3) != "Ene" && obj.name.Substring(0, 3) != "Obs")
				slots.Add(obj);

			obj = allSlots[slotPosX - 1 - 1, slotPosY - 1 - 1];
			if (obj.name.Substring(0, 3) != "Ene" && obj.name.Substring(0, 3) != "Obs")
				slots.Add(obj);

			obj = allSlots[slotPosX - 1 - 1, slotPosY - 1];
			if (obj.name.Substring(0, 3) != "Ene" && obj.name.Substring(0, 3) != "Obs")
				slots.Add(obj);

			obj = allSlots[slotPosX - 1 - 1, slotPosY + 1 - 1];
			if (obj.name.Substring(0, 3) != "Ene" && obj.name.Substring(0, 3) != "Obs")
				slots.Add(obj);
		}
		catch { }
		

		foreach (var slot in slots)
		{
			slot.GetComponent<Renderer>().material.SetColor("_Color", new Color(1, 0.77f, 0.17f, 1));
		}

		while (!hasMoved)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit))
				{
					Debug.Log(hit.collider.gameObject.name);

					foreach (var slot in slots)
					{
						if (hit.collider.gameObject == slot)
						{
							transform.position = new Vector3(slot.transform.position.x, slot.transform.position.y+1f, slot.transform.position.z);
							int posX, posY;
							posX = slot.gameObject.name[slot.gameObject.name.Length - 3] - '0';
							posY = slot.gameObject.name[slot.gameObject.name.Length - 1] - '0';

							slotPosX=posX; slotPosY=posY;

							hasMoved = true; break;
						}
					}
				}
			}
			yield return null;
		}

		foreach (var slot in slots)
		{
			slot.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
		}

		yield return null;
	}
}
