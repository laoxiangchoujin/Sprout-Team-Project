using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class 基础棋盘 : MonoBehaviour
{
    public int 棋盘横向数量;
    public float 棋盘长宽比;

	private int 棋盘纵向数量;// = (int)(棋盘横向数量 * 棋盘长宽比);
	public GameObject[,] allSlots;// = new GameObject[棋盘横向数量, 棋盘纵向数量];

	private Transform trans_plane;

	public GameObject[] 地板砖;

	private Transform slotsParentTransform;

	private bool bHasChanged = false;
	private float changeIntervalTime = 0;//怎么说也1秒变一回，别太快
	private bool bJustChanged = false;//是看是不是刚刚改变了inspector的值，不要改了值以后立马变，会卡

	// Start is called before the first frame update
	void Start()
    {
		trans_plane = GameObject.Find("棋盘").transform;
        updatePlane();

		GameObject slotsParent = new GameObject("slotsParent");
		slotsParentTransform=slotsParent.transform;

		//slotsParent.transform.SetParent(trans_plane,true);

		initAllSlots();//需要先创建父物体，在生成插槽，否则先生成的插槽没有父级
	}

    // Update is called once per frame
  //  void Update()
  //  {
  //      updatePlane();
  //      drawAxis();

		//changeIntervalTime += Time.deltaTime;
		//if (changeIntervalTime > 3)
		//{
		//	bJustChanged = false;
		//}
		//if (bHasChanged && !bJustChanged)
		//{
		//	clearAllSlots();
		//	Debug.Log("清除并且重置了");
		//	initAllSlots();
		//	bHasChanged = false;
		//}
		////Debug.Log(changeIntervalTime);
		////Debug.Log(bJustChanged);
		////if (Input.GetKey(KeyCode.E))
		////{
		////	clearAllSlots();
		////}
  //  }

    void updatePlane()
    {
		trans_plane.localScale = new Vector3(棋盘横向数量 * 0.1f, 1 * 0.1f, 棋盘横向数量 * 棋盘长宽比 * 0.1f);
		trans_plane.position = new Vector3(棋盘横向数量 * 0.5f, 0, 棋盘横向数量 * 棋盘长宽比 * 0.5f);

		trans_plane.localScale *= 1.2f;
    }
    void drawAxis()
    {      
		Vector3 xmax = new Vector3(999, 0, 0);
        Debug.DrawLine(Vector3.zero, xmax, Color.red, 1000, false);
		
		Vector3 zmax = new Vector3(0, 0, 999);
		Debug.DrawLine(Vector3.zero, zmax, Color.blue, 1000, false);
	}

	
    void initAllSlots()
    {
		
		for (int j = 0; j < 棋盘纵向数量; j++)
		{
			for (int i = 0; i < 棋盘横向数量; i++)
			{
				Vector3 pos = new Vector3(0.5f + i, 0.07f, 0.5f + j);
				Vector3 rot = new Vector3(0, 0, 0);
				Quaternion rot2 = Quaternion.Euler(rot+new Vector3(0,90*Random.Range(0,3),0));//vec3的欧拉角转为quaternion的四元数
				allSlots[i, j] = GameObject.Instantiate(地板砖[Random.Range(0,7)], pos, rot2,slotsParentTransform) as GameObject;//在生成时直接指定父物体
				allSlots[i, j].name = (i + 1).ToString() + ',' + (j + 1).ToString();
			}
		}
	}
	private void OnValidate()//开始或inspector面板中的值改变时调用此函数
	{
		棋盘纵向数量 = (int)(棋盘横向数量 * 棋盘长宽比);
		allSlots = new GameObject[棋盘横向数量, 棋盘纵向数量];

		bHasChanged = true;
		changeIntervalTime = 0;
		bJustChanged = true;
	}

	void clearAllSlots()
	{
		if (allSlots != null)
		{
			foreach(Transform children in slotsParentTransform)
			{
				Destroy(children.gameObject);
			}
		}
	}
}
