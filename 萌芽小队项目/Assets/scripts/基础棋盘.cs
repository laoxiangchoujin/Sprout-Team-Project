using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class 基础棋盘 : MonoBehaviour
{
	public float 单位长度;
    public int 棋盘横向数量;
    public float 棋盘长宽比;

    //int 棋盘纵向数量 = 棋盘横向数量 * 棋盘长宽比;

    private Transform trans_plane;

    public GameObject 棋盘插槽;

	// Start is called before the first frame update
	void Start()
    {
        trans_plane=GameObject.Find("棋盘").transform;
        updatePlane();

		initAllSlots();
	}

    // Update is called once per frame
    void Update()
    {
        updatePlane();
        drawAxis();
    }

    void updatePlane()
    {
        trans_plane.localScale=new Vector3(单位长度*棋盘横向数量*0.1f,1*0.1f,单位长度*棋盘横向数量*棋盘长宽比*0.1f);
        trans_plane.position=new Vector3(单位长度 * 棋盘横向数量*0.5f , 0, 单位长度 * 棋盘横向数量 * 棋盘长宽比*0.5f);
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
		int 棋盘纵向数量 = (int)(棋盘横向数量 * 棋盘长宽比);
		//GameObject allSlots[10][10] = null;
		GameObject[,] allSlots = new GameObject[棋盘横向数量, 棋盘纵向数量];

		for (int i = 0; i < 棋盘横向数量; i++)
		{
			for (int j = 0; j < 棋盘纵向数量; j++)
			{
				Vector3 pos = new Vector3(0.5f + i, 0.07f, 0.5f + j);
				Vector3 rot = new Vector3(0, 0, 0);
				Quaternion rot2 = Quaternion.Euler(rot);//vec3的欧拉角转为quaternion的四元数
				allSlots[i, j] = GameObject.Instantiate(棋盘插槽, pos, rot2) as GameObject;
			}
		}
	}
}
