using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ���ӵ��趨�Ϳ��� : MonoBehaviour
{
	public int hp;
	public int atk;

	public int slotPosX;
	public int slotPosY;
	//�����֪�������м��м���
	public GameObject ����;
	private int ���̺�������;
	private int ������������;


	private bool bNowMoving = false;
	private float moveIntervalTime = 0;//����λ�Ʋ����ļ��ʱ��
	public bool bJustMoved = false;

	private Transform diceTransform;//=GameObject.Find("����").transform;
	private Transform slotsParentTransform;//=GameObject.Find("����").transform;

    public class aspect
    {
        public int num;
		public aspect up;//��Ϊ��class������������������������ã�������ֱ�ӵ�ֵ����
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
		nowUpAspect=sixAspects[5];//��ʱ����5���������ϱ�

		diceTransform=GameObject.Find("����").transform;

		showOtherAspects();

		slotsParentTransform= GameObject.Find("slotsParent").transform;//!!!ע��!!!���д�������������ɲ��֮����ܵ��ã����Ե����Ӻ�һ�±��ű���ִ��˳��
																	   //diceTransform.transform.SetParent(trans_plane,true);

		���̺������� = ����.GetComponent<��������>().���̺�������;
		������������ = (int)(����.GetComponent<��������>().���̺�������* ����.GetComponent<��������>().���̳����);

		diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX-1+(slotPosY-1)*���̺�������).position.x,0.5f,
			slotsParentTransform.GetChild(slotPosX-1+(slotPosY-1)*���̺�������).position.z);

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
			//ִ���ƶ��Ĵ���
			diceMove();

			hp = nowUpAspect.num;
			atk = hp;
		}
    }
    void initDice()
    {
		for (int i = 1; i <= 6; i++) //�ϱ�ֻ�Ǹ�����ʵ�����ˣ����ø������е�ÿ��Ԫ��ʵ����
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
		sixAspects[5].up = sixAspects[6];//�ϱ���6�ı�
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
		Debug.Log("���ڳ��ϵ���Ϊ��"+nowUpAspect.num+'\n'
			+"�ϱߵ���Ϊ"+nowUpAspect.up.num + "�±ߵ���Ϊ" + nowUpAspect.down.num 
			+ "��ߵ���Ϊ" + nowUpAspect.left.num  + "�ұߵ���Ϊ" + nowUpAspect.right.num );

	}
	void diceMove()
	{
		if (slotPosX >=1 && slotPosY >=1 && slotPosX <= ���̺������� && slotPosY <= ������������)
		{
			if (Input.GetKey(KeyCode.W) && slotPosY < ������������)
			{
				slotPosY += 1;
				diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * ���̺�������).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * ���̺�������).position.z);

                int Num = nowUpAspect.down.num;
                nowUpAspect.up.num = nowUpAspect.num;
                nowUpAspect.down.num = 7 - nowUpAspect.num;
                nowUpAspect.num = Num;
                showOtherAspects();

				bJustMoved = true;
				moveIntervalTime = 0;
				if (debugLog)
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.S) && slotPosY > 1)
			{
				slotPosY -= 1;
				diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * ���̺�������).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * ���̺�������).position.z);

                int Num = nowUpAspect.up.num;
                nowUpAspect.up.num = 7 - nowUpAspect.num;
                nowUpAspect.down.num = nowUpAspect.num;
                nowUpAspect.num = Num;
                showOtherAspects();

				bJustMoved = true;
				moveIntervalTime = 0;
				if (debugLog)
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.A) && slotPosX > 1)
			{
				slotPosX -= 1;
				diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * ���̺�������).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * ���̺�������).position.z);

                int Num = nowUpAspect.right.num;
                nowUpAspect.left.num = nowUpAspect.num;
                nowUpAspect.right.num = 7 - nowUpAspect.num;
                nowUpAspect.num = Num;
                showOtherAspects();

				bJustMoved = true;
				moveIntervalTime = 0;
				if (debugLog)
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.D) && slotPosX < ���̺�������)
			{
				slotPosX += 1;
				diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * ���̺�������).position.x, 0.5f,
				slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * ���̺�������).position.z);

                int Num = nowUpAspect.left.num;
                nowUpAspect.left.num = 7 - nowUpAspect.num;
                nowUpAspect.right.num = nowUpAspect.num;
				nowUpAspect.num = Num;
				showOtherAspects();

				bJustMoved = true;
				moveIntervalTime = 0;
				if (debugLog)
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
		}
		
	}
	private void OnValidate()
	{
		if (UnityEditor.EditorApplication.isPlaying)//ֻ���ڲ���ģʽ�������������Ҫ��ȻҲ�������
			if (Time.time>1f)//��Ҫһ��ʼ�����У��������Ҳ���slotsParentTransform
			diceTransform.position = new Vector3(slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * ���̺�������).position.x, 0.5f,
			slotsParentTransform.GetChild(slotPosX - 1 + (slotPosY - 1) * ���̺�������).position.z);
	}

	private void OnCollisionEnter(Collision collision)//��ʵ�ϣ��Ҹ����ڵ��˿��ƵĽű�д�������
	{
		if (collision == null) return;

		var enemy= collision.gameObject.GetComponent<���˿���>();
		if (enemy == null) return;

		if (this.atk > enemy.hp)//ս��ʤ�������
		{
			Debug.Log("destroy��һ��Ŀ��");
			Destroy(enemy.gameObject);
		}
		else if (this.hp < enemy.atk)//ս��ʧ�ܵ����
		{
			Debug.Log("���ѱ�����");
			//Destroy(this.gameObject);//�Ͳ�Ҫ�����ˣ�ʡ�ı���

			//����Ⱦ�ˣ�˵��Ҫ������
			this.GetComponent<Renderer>().enabled = false;

			GameObject �غϼ����� = GameObject.Find("�غϼ�����");
			�غϼ�����.GetComponent<�غϼ�����>().���ʧ�� = true;
		}

		
	}
}
