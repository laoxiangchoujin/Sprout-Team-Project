using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class 骰子的设定和控制 : MonoBehaviour
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

	private Transform diceTransform;//=GameObject.Find("骰子").transform;
	private Transform slotsParentTransform;//=GameObject.Find("棋盘").transform;

    public class aspect
    {
        public int num;
		public aspect up;//因为是class，所以这里的上下左右是引用，而不是直接的值传递
        public aspect down;
		public aspect left;
		public aspect right;
	};

    //aspect aspect1 = new aspect();
    private aspect[] sixAspects =new aspect[7];

	public aspect nowUpAspect;

	public bool bRoundPlayerCanMove;

	public bool debugLog = false;

	private GameObject[] enemy;

	// Start is called before the first frame update
	void Start()
    {
        initDice();
		nowUpAspect=sixAspects[5];//暂时先让5的那面在上边

		diceTransform=GameObject.Find("骰子").transform;

		showOtherAspects();

		slotsParentTransform= GameObject.Find("slotsParent").transform;//!!!注意!!!这行代码得在棋盘生成插槽之后才能调用，所以调整延后一下本脚本的执行顺序
																	   //diceTransform.transform.SetParent(trans_plane,true);

		棋盘横向数量 = 棋盘.GetComponent<基础棋盘>().棋盘横向数量;
		棋盘纵向数量 = (int)(棋盘.GetComponent<基础棋盘>().棋盘横向数量* 棋盘.GetComponent<基础棋盘>().棋盘长宽比);

		diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX-1+(slotPosY-1)*棋盘横向数量).position.x,0.5f,
			slotsParentTransform.GetChild(slotPosX-1+(slotPosY-1)*棋盘横向数量).position.z);

		hp = nowUpAspect.num;
		atk = hp;

		enemy = GameObject.FindGameObjectsWithTag("Enemy");
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyUp(KeyCode.W)|| Input.GetKeyUp(KeyCode.A)|| Input.GetKeyUp(KeyCode.S)|| Input.GetKeyUp(KeyCode.D))
		{ 
			bNowMoving = true;
		}
		moveIntervalTime += Time.deltaTime;
		if (moveIntervalTime > 0.2)
		{
			bJustMoved = false;
		}
		if (bNowMoving && !bJustMoved&&bRoundPlayerCanMove)
		{
			//执行移动的代码
			diceMove();

			hp = nowUpAspect.num;
			atk = hp;
		}
    }
    void initDice()
    {
		for (int i = 1; i <= 6; i++) //上边只是给数组实例化了，还得给数组中的每个元素实例化
		{
			sixAspects[i] = new aspect();
		}

		sixAspects[1].num = 1;
		sixAspects[1].up = sixAspects[5];
		sixAspects[1].down = sixAspects[2];
		sixAspects[1].left = sixAspects[4];
		sixAspects[1].right = sixAspects[3];

		sixAspects[2].num = 2;
		sixAspects[2].up = sixAspects[1];
		sixAspects[2].down = sixAspects[6];
		sixAspects[2].left = sixAspects[4];
		sixAspects[2].right = sixAspects[3];

		sixAspects[3].num = 3;
		sixAspects[3].up = sixAspects[6];
		sixAspects[3].down = sixAspects[1];
		sixAspects[3].left = sixAspects[5];
		sixAspects[3].right = sixAspects[2];

		sixAspects[4].num = 4;
		sixAspects[4].up = sixAspects[6];
		sixAspects[4].down = sixAspects[1];
		sixAspects[4].left = sixAspects[2];
		sixAspects[4].right = sixAspects[5];

		sixAspects[5].num = 5;
		sixAspects[5].up = sixAspects[6];//上边是6的边
		sixAspects[5].down = sixAspects[1];
		sixAspects[5].left = sixAspects[4];
		sixAspects[5].right = sixAspects[3];

		sixAspects[6].num = 6;
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
		if (slotPosX >=1 && slotPosY >=1 && slotPosX <= 棋盘横向数量 && slotPosY <= 棋盘纵向数量)
		{
			if (Input.GetKey(KeyCode.W) && slotPosY < 棋盘纵向数量)
			{
				slotPosY += 1;
				diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);

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
			else if (Input.GetKey(KeyCode.S) && slotPosY > 1)
			{
				slotPosY -= 1;
				diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);

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
			else if (Input.GetKey(KeyCode.A) && slotPosX > 1)
			{
				slotPosX -= 1;
				diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);

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
			else if (Input.GetKey(KeyCode.D) && slotPosX < 棋盘横向数量)
			{
				slotPosX += 1;
				diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);

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
	private void OnValidate()
	{
		if (UnityEditor.EditorApplication.isPlaying)//只有在播放模式才做这个操作，要不然也会空引用
			if (Time.time>1f)//不要一开始就运行，这样会找不到slotsParentTransform
			diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
			slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
	}

	private void OnCollisionEnter(Collision collision)//事实上，我更想在敌人控制的脚本写这个函数
	{
		if (collision == null) return;

		var enemy= collision.gameObject.GetComponent<敌人控制>();
		if (enemy == null) return;

		if (this.atk > enemy.hp)//战斗胜利的情况
		{
			Debug.Log("destroy了一个目标");
			Destroy(enemy.gameObject);
		}
		else if (this.hp < enemy.atk)//战斗失败的情况
		{
			Debug.Log("你已被击败");
			//Destroy(this.gameObject);//就不要销毁了，省的报错

			//不渲染了，说是要做动画
			this.GetComponent<Renderer>().enabled = false;

			GameObject 回合计数器 = GameObject.Find("回合计数器");
			回合计数器.GetComponent<回合计数器>().玩家失败 = true;
		}

		
	}
}
