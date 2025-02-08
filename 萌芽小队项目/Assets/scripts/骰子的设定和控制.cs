using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ���ӵ��趨�Ϳ��� : MonoBehaviour
{
    public bool canUp = true;
    public bool canDown = true;
    public bool canLeft = true;
    public bool canRight = true;

    public int hp;
	public int atk;

	public int slotPosX;
	public int slotPosY;
	//�����֪�������м��м���
	public GameObject ����;
	private GameObject[,] allSlots; 
	private int ���̺�������;
	private int ������������;


	private bool bNowMoving = false;
	private float moveIntervalTime = 0;//����λ�Ʋ����ļ��ʱ��
	public bool bJustMoved = false;

	private Transform diceTransform;//=GameObject.Find("����").transform;
	//private Transform slotsParentTransform;//=GameObject.Find("����").transform;

    public class aspect
    {
        public int num;
		public aspect up;//��Ϊ��class������������������������ã�������ֱ�ӵ�ֵ����
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

	public Sprite ����ͼ��;
	GameObject ͼ��;//��ʵ�ǿ�����

	// Start is called before the first frame update
	void Start()
    {
		allSlots = ����.GetComponent<��������>().allSlots;

        initDice();
		nowUpAspect = sixAspects[3];//����ĳһ�����ϱ�
		//nowUpNumber = Random.Range(1, 7);//������ɳ�ʼֵ
        //nowUpAspect = sixAspects[nowUpNumber];

        diceTransform =GameObject.Find("����").transform;

		showOtherAspects();

		//slotsParentTransform= GameObject.Find("slotsParent").transform;//!!!ע��!!!���д�������������ɲ��֮����ܵ��ã����Ե����Ӻ�һ�±��ű���ִ��˳��
																	   //diceTransform.transform.SetParent(trans_plane,true);

		���̺������� = ����.GetComponent<��������>().���̺�������;
		������������ = (int)(����.GetComponent<��������>().���̺�������* ����.GetComponent<��������>().���̳����);

		diceTransform.position = new Vector3(allSlots[slotPosX -1,slotPosY -1].transform.position.x,0.5f,
			allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

		hp = nowUpAspect.num;
		atk = hp;

		enemy = GameObject.FindGameObjectsWithTag("Enemy");

		ͼ�� = new GameObject();
		ͼ��.AddComponent<SpriteRenderer>();
		ͼ��.GetComponent<SpriteRenderer>().sprite = ����ͼ��;
		ͼ��.transform.localScale *= 0.3f;
		ͼ��.transform.eulerAngles += new Vector3(90, 0, 0);
		
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
			//ִ���ƶ��Ĵ���
			diceMove();

			if (playerHas������)
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

		ͼ��.transform.position = new Vector3(diceTransform.position.x + new Vector3(-0.4f, 0, 0).x,1.5f, diceTransform.position.z + new Vector3(0, 0, 0.4f).z);
    }
    void initDice()
    {
		for (int i = 1; i <= 6; i++) //�ϱ�ֻ�Ǹ�����ʵ�����ˣ����ø������е�ÿ��Ԫ��ʵ����
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
		Debug.Log("���ڳ��ϵ���Ϊ��"+nowUpAspect.num+'\n'
			+"�ϱߵ���Ϊ"+nowUpAspect.up.num + "�±ߵ���Ϊ" + nowUpAspect.down.num 
			+ "��ߵ���Ϊ" + nowUpAspect.left.num  + "�ұߵ���Ϊ" + nowUpAspect.right.num );

	}
	void diceMove()
	{
		//canUp = true; canDown = true; canLeft = true; canRight = true;
		if (slotPosY < ������������)//��������
		{
			if (allSlots[slotPosX - 1, slotPosY + 1 - 1].name.Substring(0, 3) == "Obs")//�ϱ����ϰ���
			{
				if(allSlots[slotPosX - 1, slotPosY + 1 - 1].GetComponentInChildren<�ϰ����趨>() != null)//�ϱߵĲ����������				
					if(nowUpAspect.down.num < allSlots[slotPosX - 1, slotPosY + 1 - 1].GetComponentInChildren<�ϰ����趨>().hp)//�ϰ����hp����
					{
						canUp = false;
					}								
			}
		}
		if (slotPosY > 1)
		{
			if (allSlots[slotPosX - 1, slotPosY - 1 - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if(allSlots[slotPosX - 1, slotPosY - 1 - 1].GetComponentInChildren<�ϰ����趨>() != null)
					if(nowUpAspect.up.num <= allSlots[slotPosX - 1, slotPosY - 1 - 1].GetComponentInChildren<�ϰ����趨>().hp)
					{
						canDown = false;
					}
			}
		}
		if (slotPosX > 1)
		{
			if (allSlots[slotPosX - 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if(allSlots[slotPosX - 1 - 1, slotPosY - 1].GetComponentInChildren<�ϰ����趨>() != null)
					if(nowUpAspect.right.num <= allSlots[slotPosX - 1 - 1, slotPosY - 1].GetComponentInChildren<�ϰ����趨>().hp)
					{
						canLeft = false;
					}
			}
		}
		if (slotPosX < ������������)
		{
			if (allSlots[slotPosX + 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if(allSlots[slotPosX + 1 - 1, slotPosY - 1].GetComponentInChildren<�ϰ����趨>() != null)
					if(nowUpAspect.left.num <= allSlots[slotPosX + 1 - 1, slotPosY - 1].GetComponentInChildren<�ϰ����趨>().hp)
					{
						canRight = false;
					}
			}
		}

		if (slotPosX >=1 && slotPosY >=1 && slotPosX <= ���̺������� && slotPosY <= ������������)
		{
			if (Input.GetKey(KeyCode.W) && !canUp)
			{
				Debug.Log("�޷�ǰ��(" + slotPosX + ',' + (slotPosY + 1) + ")����Ϊ���ϰ����Ϊ��һ���ƶ��ķ�����");
				return;
			}
			else if (Input.GetKey(KeyCode.S) && !canDown)
			{
				Debug.Log("�޷�ǰ��(" + slotPosX + ',' + (slotPosY - 1) + ")����Ϊ���ϰ����Ϊ��һ���ƶ��ķ�����");
				return;
			}
			else if (Input.GetKey(KeyCode.A) && !canLeft)
			{
				Debug.Log("�޷�ǰ��(" + (slotPosX - 1) + ',' + slotPosY + ")����Ϊ���ϰ����Ϊ��һ���ƶ��ķ�����");
				return;
			}
			else if (Input.GetKey(KeyCode.D) && !canRight)
			{
				Debug.Log("�޷�ǰ��(" + (slotPosX + 1) + ',' + slotPosY + ")����Ϊ���ϰ����Ϊ��һ���ƶ��ķ�����");
				return;
			}

			if (Input.GetKey(KeyCode.W) && canUp && slotPosY < ������������  )
			{
                canUp = true;
                canDown = false;
                canLeft = true;
                canRight = true;

                slotPosY += 1;
				if(!isMoving)
				    StartCoroutine(���������˶�());
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
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.S) && canDown && slotPosY > 1 )
			{
                canUp = false;
                canDown = true;
                canLeft = true;
                canRight = true;

                slotPosY -= 1;
				if(!isMoving)
				    StartCoroutine(���������˶�());
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
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.A) && canLeft && slotPosX > 1 )
			{
                canUp = true;
                canDown = true;
                canLeft = true;
                canRight = false;

                slotPosX -= 1;
				if(!isMoving)
				    StartCoroutine(���������˶�());
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
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.D) && canRight && slotPosX < ���̺������� )
			{
                canUp = true;
                canDown = true;
                canLeft = false;
                canRight = true;

                slotPosX += 1;
				if(!isMoving)
				    StartCoroutine(���������˶�());
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
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
		}
		
	}
	/*private void OnValidate()
	{
		if (UnityEditor.EditorApplication.isPlaying)//ֻ���ڲ���ģʽ�������������Ҫ��ȻҲ�������
			if (Time.time > 1f) ;//��Ҫһ��ʼ�����У��������Ҳ���slotsParentTransform
			{
				diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);
			}
	}*/

	private void OnCollisionEnter(Collision collision)//��ʵ�ϣ��Ҹ����ڵ��˿��ƵĽű�д�������
	{
		if (collision == null) return;

		var enemy= collision.gameObject.GetComponent<���˿���>();
		if (enemy != null)
		{
			if (bRoundPlayerCanMove)//�ж��������
            {
				if(this.atk >= enemy.hp)//ս��ʤ�������
                {
                    if (enemy.GetComponent<���˿���>().M1 || enemy.GetComponent<���˿���>().M2)
                    {
                        GameObject.Find("�̵�ͱ������ű���").GetComponent<�̵�ͱ���>().coinAmount += 1;
                        Debug.Log("+1���");
                    }
                    else if (enemy.GetComponent<���˿���>().M3 || enemy.GetComponent<���˿���>().M4)
                    {
                        GameObject.Find("�̵�ͱ������ű���").GetComponent<�̵�ͱ���>().coinAmount += 2;
                        Debug.Log("+2���");
                    }
                    else if (enemy.GetComponent<���˿���>().M5 || enemy.GetComponent<���˿���>().M6)
                    {
                        GameObject.Find("�̵�ͱ������ű���").GetComponent<�̵�ͱ���>().coinAmount += 3;
                        Debug.Log("+3���");
                    }
                    Debug.Log("destroy��һ��Ŀ��");
                    Destroy(enemy.gameObject);
                }
				else//ս��ʧ�ܵ����
				{
					if (playerHas�ػ�����)//ʧ�ܵ��л���
					{
						GameObject shield = this.transform.GetChild(0).gameObject;
						Destroy(shield);

						playerHas�ػ����� = false;

						if (enemy.GetComponent<���˿���>().M1 || enemy.GetComponent<���˿���>().M2)
						{
							GameObject.Find("�̵�ͱ������ű���").GetComponent<�̵�ͱ���>().coinAmount += 1;
							Debug.Log("+1���");
						}
						else if (enemy.GetComponent<���˿���>().M3 || enemy.GetComponent<���˿���>().M4)
						{
							GameObject.Find("�̵�ͱ������ű���").GetComponent<�̵�ͱ���>().coinAmount += 2;
							Debug.Log("+2���");
						}
						else if (enemy.GetComponent<���˿���>().M5 || enemy.GetComponent<���˿���>().M6)
						{
							GameObject.Find("�̵�ͱ������ű���").GetComponent<�̵�ͱ���>().coinAmount += 3;
							Debug.Log("+3���");
						}
						Debug.Log("destroy��һ��Ŀ��");
						Destroy(enemy.gameObject);
					}
					else//ʧ�ܲ���û����
					{
						this.GetComponent<Collider>().enabled = false;//ȡ����ײ���Ա����ظ����
																	  //Destroy(this.gameObject);//�Ͳ�Ҫ�����ˣ�ʡ�ı���
						Debug.Log("���ѱ�����");

						//����Ⱦ�ˣ�˵��Ҫ������
						this.GetComponent<Renderer>().enabled = false;

						GameObject �غϼ����� = GameObject.Find("�غϼ�����");
						�غϼ�����.GetComponent<�غϼ�����>().���ʧ�� = true;
					}
                    
                }
			}
			else
			{
				if (this.hp > enemy.atk) 
				{
                    if (enemy.GetComponent<���˿���>().M1 || enemy.GetComponent<���˿���>().M2)
                    {
                        GameObject.Find("�̵�ͱ������ű���").GetComponent<�̵�ͱ���>().coinAmount += 1;
                        Debug.Log("+1���");
                    }
                    else if (enemy.GetComponent<���˿���>().M3 || enemy.GetComponent<���˿���>().M4)
                    {
                        GameObject.Find("�̵�ͱ������ű���").GetComponent<�̵�ͱ���>().coinAmount += 2;
                        Debug.Log("+2���");
                    }
                    else if (enemy.GetComponent<���˿���>().M5 || enemy.GetComponent<���˿���>().M6)
                    {
                        GameObject.Find("�̵�ͱ������ű���").GetComponent<�̵�ͱ���>().coinAmount += 3;
                        Debug.Log("+3���");
                    }
                    Debug.Log("��destroy��һ��Ŀ��");
                    Destroy(enemy.gameObject);
                }
				else
				{
                    this.GetComponent<Collider>().enabled = false;//ȡ����ײ���Ա����ظ����
                                                                  //Destroy(this.gameObject);//�Ͳ�Ҫ�����ˣ�ʡ�ı���
                    Debug.Log("�㱻������");

                    //����Ⱦ�ˣ�˵��Ҫ������
                    this.GetComponent<Renderer>().enabled = false;

                    GameObject �غϼ����� = GameObject.Find("�غϼ�����");
                    �غϼ�����.GetComponent<�غϼ�����>().���ʧ�� = true;
                }
			}
        }

		var obstacle = collision.gameObject.GetComponent<�ϰ����趨>();
		if(obstacle != null)
		{
			if(this.atk >= obstacle.hp)
			{
				Debug.Log("Destroy��һ���ϰ���");
				Destroy(obstacle.gameObject);
			}		
		}
		

		
	}


	//������дһЩ�������˶���Э��
	//public IEnumerator ���������˶�()
	//{
	//	float animTime = 0.5f;

	//	float speed = 1 / animTime;//ʵ���ٶȣ�����deltatime����ÿ֡�ߵľ���

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
	IEnumerator ���������˶�()
	{
		if (isMoving)
			yield break;

		isMoving = true;

		Vector3 startPos = transform.position;
		Vector3 endPos = startPos + new Vector3(0, 0, 1);

		//�����ת״̬������ֻ�������壬Ҳ����������
		//transform.rotation = Quaternion.identity;
		Quaternion startRot = transform.rotation;
		//Quaternion endRot = Quaternion.Euler(90, 0, 0); // ����
		//Quaternion endRot = startRot * Quaternion.Euler(90, 0, 0);//quaternion.elua���ص�����Ԫ������˼����ŷ����������Ԫ���������ң��Ƕ��������Ԫ��������˱�ʾ
		Quaternion endRot = Quaternion.AngleAxis(90, new Vector3(1, 0, 0)) * startRot;//quaternion.angleaxis����������ϵ�£���һ������תһ���Ƕ�
																			//���ң�������ҳ�startrot����ʾ��������ռ���Ӧ����ת����Ӧ��ԭ�еľֲ���ת
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
	IEnumerator ���������˶�()
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
	IEnumerator ���������˶�()
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
	IEnumerator ���������˶�()
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



	//һЩ����Ч��

	bool ʱ��ɳ©moved = false;
	void ʱ��ɳ©move()
	{
		//����Ϊ��dicemove�ļ򻯰棬���ƹ���
		if (slotPosY < ������������)//��������
		{
			if (allSlots[slotPosX - 1, slotPosY + 1 - 1].name.Substring(0, 3) == "Obs")//�ϱ����ϰ���
			{
				if (allSlots[slotPosX - 1, slotPosY + 1 - 1].GetComponentInChildren<�ϰ����趨>() != null)//�ϱߵĲ����������				
					if (nowUpAspect.down.num < allSlots[slotPosX - 1, slotPosY + 1 - 1].GetComponentInChildren<�ϰ����趨>().hp)//�ϰ����hp����
					{
						canUp = false;
					}
			}
		}
		if (slotPosY > 1)
		{
			if (allSlots[slotPosX - 1, slotPosY - 1 - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if (allSlots[slotPosX - 1, slotPosY - 1 - 1].GetComponentInChildren<�ϰ����趨>() != null)
					if (nowUpAspect.up.num <= allSlots[slotPosX - 1, slotPosY - 1 - 1].GetComponentInChildren<�ϰ����趨>().hp)
					{
						canDown = false;
					}
			}
		}
		if (slotPosX > 1)
		{
			if (allSlots[slotPosX - 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if (allSlots[slotPosX - 1 - 1, slotPosY - 1].GetComponentInChildren<�ϰ����趨>() != null)
					if (nowUpAspect.right.num <= allSlots[slotPosX - 1 - 1, slotPosY - 1].GetComponentInChildren<�ϰ����趨>().hp)
					{
						canLeft = false;
					}
			}
		}
		if (slotPosX < ������������)
		{
			if (allSlots[slotPosX + 1 - 1, slotPosY - 1].transform.name.Substring(0, 3) == "Obs")
			{
				if (allSlots[slotPosX + 1 - 1, slotPosY - 1].GetComponentInChildren<�ϰ����趨>() != null)
					if (nowUpAspect.left.num <= allSlots[slotPosX + 1 - 1, slotPosY - 1].GetComponentInChildren<�ϰ����趨>().hp)
					{
						canRight = false;
					}
			}
		}

		if (slotPosX >= 1 && slotPosY >= 1 && slotPosX <= ���̺������� && slotPosY <= ������������)
		{
			if (Input.GetKey(KeyCode.W) && !canUp)
			{
				Debug.Log("�޷�ǰ��(" + slotPosX + ',' + (slotPosY + 1) + ")����Ϊ���ϰ����Ϊ��һ���ƶ��ķ�����");
				return;
			}
			else if (Input.GetKey(KeyCode.S) && !canDown)
			{
				Debug.Log("�޷�ǰ��(" + slotPosX + ',' + (slotPosY - 1) + ")����Ϊ���ϰ����Ϊ��һ���ƶ��ķ�����");
				return;
			}
			else if (Input.GetKey(KeyCode.A) && !canLeft)
			{
				Debug.Log("�޷�ǰ��(" + (slotPosX - 1) + ',' + slotPosY + ")����Ϊ���ϰ����Ϊ��һ���ƶ��ķ�����");
				return;
			}
			else if (Input.GetKey(KeyCode.D) && !canRight)
			{
				Debug.Log("�޷�ǰ��(" + (slotPosX + 1) + ',' + slotPosY + ")����Ϊ���ϰ����Ϊ��һ���ƶ��ķ�����");
				return;
			}

			if (Input.GetKey(KeyCode.W) && canUp && slotPosY < ������������)
			{
				canUp = true;
				canDown = false;
				canLeft = true;
				canRight = true;

				slotPosY += 1;
				StartCoroutine(���������˶�());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

				int Num = nowUpAspect.down.num;
				nowUpAspect.up.num = nowUpAspect.num;
				nowUpAspect.down.num = 7 - nowUpAspect.num;
				nowUpAspect.num = Num;
				showOtherAspects();

				ʱ��ɳ©moved = true;

				if (debugLog)
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.S) && canDown && slotPosY > 1)
			{
				canUp = false;
				canDown = true;
				canLeft = true;
				canRight = true;

				slotPosY -= 1;
				StartCoroutine(���������˶�());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

				int Num = nowUpAspect.up.num;
				nowUpAspect.up.num = 7 - nowUpAspect.num;
				nowUpAspect.down.num = nowUpAspect.num;
				nowUpAspect.num = Num;
				showOtherAspects();

				ʱ��ɳ©moved = true;

				if (debugLog)
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.A) && canLeft && slotPosX > 1)
			{
				canUp = true;
				canDown = true;
				canLeft = true;
				canRight = false;

				slotPosX -= 1;
				StartCoroutine(���������˶�());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

				int Num = nowUpAspect.right.num;
				nowUpAspect.left.num = nowUpAspect.num;
				nowUpAspect.right.num = 7 - nowUpAspect.num;
				nowUpAspect.num = Num;
				showOtherAspects();

				ʱ��ɳ©moved = true;

				if (debugLog)
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
			else if (Input.GetKey(KeyCode.D) && canRight && slotPosX < ���̺�������)
			{
				canUp = true;
				canDown = true;
				canLeft = false;
				canRight = true;

				slotPosX += 1;
				StartCoroutine(���������˶�());
				//diceTransform.position = new Vector3(allSlots[slotPosX - 1, slotPosY - 1].transform.position.x, 0.5f,
				//allSlots[slotPosX - 1, slotPosY - 1].transform.position.z);

				int Num = nowUpAspect.left.num;
				nowUpAspect.left.num = 7 - nowUpAspect.num;
				nowUpAspect.right.num = nowUpAspect.num;
				nowUpAspect.num = Num;
				showOtherAspects();

				ʱ��ɳ©moved |= true;

				if (debugLog)
					Debug.Log("���ڵ�slotposx��slotposy�ֱ��ǣ�" + slotPosX + ',' + slotPosY);
			}
		}
	}//��Ȼ�غϼ�����ֻ�����Ƿ���bjustmoved���Ұ�������������Ĵ���ɾ�ˣ��Ͳ��ᴥ����
	public IEnumerator ʱ��ɳ©()
	{
		while(!ʱ��ɳ©moved)
		{
			 ʱ��ɳ©();
			yield return null;
		}
		yield return new WaitForSeconds(0.2f);
	}

	bool playerHas�ػ����� = false;
	public GameObject �ػ�����ģ��;
	public void �ػ�����()
	{
		GameObject shield=GameObject.Instantiate(�ػ�����ģ��, transform);
		shield.transform.parent = transform;
		shield.transform.position = transform.position;
		playerHas�ػ����� = true;
	}

	bool playerHas������ = false;
	public IEnumerator ������()
	{
		playerHas������ = true;

		GameObject roundCounter = GameObject.Find("�غϼ�����");
		int nowRoundCount= roundCounter.GetComponent<�غϼ�����>().roundCount;
		while (true)
		{
			if (roundCounter.GetComponent<�غϼ�����>().roundCount >= nowRoundCount + 2)//�������غϵ����
			{
				playerHas������ = false;
				break;
			}
			yield return null;
		}
		yield return null;
	}

	public IEnumerator ���;���()
	{
		bool hasMoved = false;

		List <GameObject> slots = new List<GameObject>();

		try
		{
			GameObject obj = allSlots[slotPosX - 1, slotPosY + 1 - 1];//��
			if(obj.name.Substring(0,3)!="Ene"&&obj.name.Substring(0,3)!="Obs")
				slots.Add(obj);

			obj = allSlots[slotPosX + 1 - 1, slotPosY + 1 - 1];//����
			if (obj.name.Substring(0, 3) != "Ene" && obj.name.Substring(0, 3) != "Obs")
				slots.Add(obj);

			obj = allSlots[slotPosX + 1 - 1, slotPosY - 1];//��
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
