using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 基础棋盘 : MonoBehaviour
{
	public float 单位长度;
    public int 棋盘横向数量;
    public float 棋盘长宽比;

    private Transform trans_plane;

	// Start is called before the first frame update
	void Start()
    {
        trans_plane=GameObject.Find("Plane").transform;
        updatePlane();
    }

    // Update is called once per frame
    void Update()
    {
        updatePlane();
        drawAxis();
    }

    void updatePlane()
    {
        trans_plane.localScale=new Vector3(单位长度*棋盘横向数量*0.2f,1*0.2f,单位长度*棋盘横向数量*棋盘长宽比*0.2f);
        trans_plane.position=new Vector3(单位长度 * 棋盘横向数量 , 0, 单位长度 * 棋盘横向数量 * 棋盘长宽比);
    }
    void drawAxis()
    {
        
		Vector3 xmax = new Vector3(999, 0, 0);
        Debug.DrawLine(Vector3.zero, xmax, Color.red, 1000, false);
		
		Vector3 zmax = new Vector3(0, 0, 999);
		Debug.DrawLine(Vector3.zero, zmax, Color.blue, 1000, false);

	}
}
