using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Prop : MonoBehaviour
{
	public string name1;

	//���ﱾӦ�÷�һ������//��ʾ����
	//�����ú���ί��
	public delegate void functionDelegate();//����ί�е�����
	public functionDelegate functionToCall;//��Ҫ�ľ����������functiontocall

	public void setUpFunction(functionDelegate func)
	{
		functionToCall = func;
	}

	public int price;
	public int availableTimes;//����С���ߵĿ��ô���
	public float shipmentRate;//������
	public string notes;
	public Sprite sprite;

	public int num;//�������м���




	public void ��ʾ����()
	{
		Debug.Log(name1);
	}
	//�����ʾ��������
	
	public static void copyValues(Prop target,Prop source)
	{
		target.name1 = source.name1;
		target.functionToCall = source.functionToCall;
		target.price = source.price;
		target.availableTimes = source.availableTimes;
		target.shipmentRate = source.shipmentRate;
		target.notes = source.notes;
		target.sprite = source.sprite;
		target.num = source.num;
	}


	//ʵ��������Ҫͨ������ʵ�������ʣ�����̬��������ֱ��ͨ����������
	//����static���Ǿ�̬����
	public static void hpPlus1()
	{
		���ӵ��趨�Ϳ��� player = GameObject.Find("����").GetComponent<���ӵ��趨�Ϳ���>();
		player.nowUpAspect = player.sixAspects[player.nowUpAspect.num + 1 > 6 ? 6 : player.nowUpAspect.num];

		Debug.Log("hpPlus1"+"���ڵ�������"+player.nowUpAspect.num);
	}
	public static void hpPlus2()
	{
		���ӵ��趨�Ϳ��� player = GameObject.Find("����").GetComponent<���ӵ��趨�Ϳ���>();
		player.nowUpAspect = player.sixAspects[player.nowUpAspect.num + 2 > 6 ? 6 : player.nowUpAspect.num];

		Debug.Log("hpPlus2" + "���ڵ�������" + player.nowUpAspect.num);
	}
	public static void hpTo6()
	{
		���ӵ��趨�Ϳ��� player = GameObject.Find("����").GetComponent<���ӵ��趨�Ϳ���>();
		player.nowUpAspect = player.sixAspects[6];

		Debug.Log("hpTo6" + "���ڵ�������" + player.nowUpAspect.num);
	}
	public static void flash()
	{

	}
	public static void oneMoreStep()
	{

	}
	public static void lightningHammer()
	{

	}
	public static void pierce()
	{

	}
	public static void shield()
	{

	}
	public static void rage()
	{

	}
	//public static void revive()
	//{

	//}
	public static void scrachCard()
	{

	}
}

