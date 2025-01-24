using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 骰子的设定和控制 : MonoBehaviour
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

	private Transform diceTransform;//=GameObject.Find("骰子").transform;
	private Transform slotsParentTransform;//=GameObject.Find("棋盘").transform;

    class aspect
    {
        public int num;
		public aspect up;//因为是class，所以这里的上下左右是引用，而不是直接的值传递
        public aspect down;
		public aspect left;
		public aspect right;
	};

    //aspect aspect1 = new aspect();
    private aspect[] sixAspects =new aspect[6];

	private aspect nowUpAspect;

    // Start is called before the first frame update
    void Start()
    {
        initDice();
		nowUpAspect=sixAspects[4];//暂时先让5的那面在上边

		diceTransform=GameObject.Find("骰子").transform;

		showOtherAspects();

		slotsParentTransform= GameObject.Find("slotsParent").transform;//!!!注意!!!这行代码得在棋盘生成插槽之后才能调用，所以调整延后一下本脚本的执行顺序
																	   //diceTransform.transform.SetParent(trans_plane,true);

		棋盘横向数量 = 棋盘.GetComponent<基础棋盘>().棋盘横向数量;
		棋盘纵向数量 = (int)(棋盘.GetComponent<基础棋盘>().棋盘横向数量* 棋盘.GetComponent<基础棋盘>().棋盘长宽比);

		diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX-1+(slotPosY-1)*棋盘横向数量).position.x,0.5f,
			slotsParentTransform.GetChild(slotPosX-1+(slotPosY-1)*棋盘横向数量).position.z);
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
		if (bNowMoving && !bJustMoved)
		{
			//执行移动的代码
			diceMove();
			
		}
    }
    void initDice()
    {
		for (int i = 0; i < 6; i++) //上边只是给数组实例化了，还得给数组中的每个元素实例化
		{
			sixAspects[i] = new aspect();
		}

		sixAspects[0].num = 1;
		sixAspects[0].up = sixAspects[4];
		sixAspects[0].down = sixAspects[2];
		sixAspects[0].left = sixAspects[3];
		sixAspects[0].right = sixAspects[5];

		sixAspects[1].num = 2;
		sixAspects[1].up = sixAspects[5];
		sixAspects[1].down = sixAspects[0];
		sixAspects[1].left = sixAspects[4];
		sixAspects[1].right = sixAspects[2];

		sixAspects[2].num = 3;
		sixAspects[2].up = sixAspects[5];
		sixAspects[2].down = sixAspects[0];
		sixAspects[2].left = sixAspects[1];
		sixAspects[2].right = sixAspects[3];

		sixAspects[3].num = 4;
		sixAspects[3].up = sixAspects[0];
		sixAspects[3].down = sixAspects[5];
		sixAspects[3].left = sixAspects[1];
		sixAspects[3].right = sixAspects[3];

		sixAspects[4].num = 5;
		sixAspects[4].up = sixAspects[5];//上边是6的边
		sixAspects[4].down = sixAspects[0];
		sixAspects[4].left = sixAspects[3];
		sixAspects[4].right = sixAspects[2];

		sixAspects[5].num = 6;
		sixAspects[5].up = sixAspects[2];
		sixAspects[5].down = sixAspects[5];
		sixAspects[5].left = sixAspects[3];
		sixAspects[5].right = sixAspects[0];
	}

	void showOtherAspects()
	{
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
				nowUpAspect = nowUpAspect.up;
				showOtherAspects();
				bJustMoved = true;
				moveIntervalTime = 0;
			}
			else if (Input.GetKey(KeyCode.S) && slotPosY > 1)
			{
				slotPosY -= 1;
				diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
				nowUpAspect = nowUpAspect.up;
				showOtherAspects();
				bJustMoved = true;
				moveIntervalTime = 0;
			}
			else if (Input.GetKey(KeyCode.A) && slotPosX > 1)
			{
				slotPosX -= 1;
				diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
				nowUpAspect = nowUpAspect.up;
				showOtherAspects();
				bJustMoved = true;
				moveIntervalTime = 0;
			}
			else if (Input.GetKey(KeyCode.D) && slotPosX < 棋盘横向数量)
			{
				slotPosX += 1;
				diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
				nowUpAspect = nowUpAspect.up;
				showOtherAspects();
				bJustMoved = true;
				moveIntervalTime = 0;
			}
		}
		Debug.Log("现在的slotposx和slotposy分别是：" + slotPosX + slotPosY);
	}
	private void OnValidate()
	{
		if(Time.time>1f)//不要一开始就运行，这样会找不到slotsParentTransform
			diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.x, 0.5f,
			slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * 棋盘横向数量).position.z);
	}
}
