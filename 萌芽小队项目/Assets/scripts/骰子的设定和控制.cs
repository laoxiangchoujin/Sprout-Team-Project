using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ���ӵ��趨�Ϳ��� : MonoBehaviour
{
	private int hp;
	private int atk;

	public int slotPosX;
	public int slotPosY;

	private bool bNowMoving = false;
	private float moveIntervalTime = 0;//����λ�Ʋ����ļ��ʱ��
	private bool bJustMoved = false;

	private Transform diceTransform;//=GameObject.Find("����").transform;
	private Transform slotsParentTransform;//=GameObject.Find("����").transform;

    class aspect
    {
        public int num;
		public aspect up;//��Ϊ��class������������������������ã�������ֱ�ӵ�ֵ����
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
		nowUpAspect=sixAspects[4];//��ʱ����5���������ϱ�

		diceTransform=GameObject.Find("����").transform;

		showOtherAspects();

		slotsParentTransform= GameObject.Find("slotsParent").transform;//!!!ע��!!!���д�������������ɲ��֮����ܵ��ã����Ե����Ӻ�һ�±��ű���ִ��˳��
		//diceTransform.transform.SetParent(trans_plane,true);

		diceTransform.position = new Vector3(slotsParentTransform.GetChild(Random.Range(0,slotsParentTransform.childCount)).position.x,0.5f,
			slotsParentTransform.GetChild(Random.Range(0, slotsParentTransform.childCount)).position.z);
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
			//ִ���ƶ��Ĵ���
			diceMove();
			
		}
    }
    void initDice()
    {
		for (int i = 0; i < 6; i++) //�ϱ�ֻ�Ǹ�����ʵ�����ˣ����ø������е�ÿ��Ԫ��ʵ����
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
		sixAspects[4].up = sixAspects[5];//�ϱ���6�ı�
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
		Debug.Log("���ڳ��ϵ���Ϊ��"+nowUpAspect.num+'\n'
			+"�ϱߵ���Ϊ"+nowUpAspect.up.num + "�±ߵ���Ϊ" + nowUpAspect.down.num 
			+ "��ߵ���Ϊ" + nowUpAspect.left.num  + "�ұߵ���Ϊ" + nowUpAspect.right.num );

	}
	void diceMove()
	{
		if (Input.GetKey(KeyCode.W))
		{
			diceTransform.position += new Vector3(0, 0, 1);
			nowUpAspect=nowUpAspect.up;
			showOtherAspects();
			bJustMoved = true;
			moveIntervalTime = 0;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			diceTransform.position += new Vector3(0, 0, -1);
			nowUpAspect = nowUpAspect.up;
			showOtherAspects();
			bJustMoved = true;
			moveIntervalTime = 0;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			diceTransform.position += new Vector3(-1, 0, 0);
			nowUpAspect = nowUpAspect.up;
			showOtherAspects();
			bJustMoved = true;
			moveIntervalTime = 0;
		}
		else if (Input.GetKey(KeyCode.D))
		{
			diceTransform.position += new Vector3(1, 0, 0);
			nowUpAspect = nowUpAspect.up;
			showOtherAspects();
			bJustMoved = true;
			moveIntervalTime = 0;
		}
	}
}
