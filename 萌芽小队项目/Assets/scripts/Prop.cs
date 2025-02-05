using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Prop : MonoBehaviour
{
	public string name1;

	//这里本应该放一个函数//表示功能
	//这里用函数委托
	public delegate void functionDelegate();//函数委托的类型
	public functionDelegate functionToCall;//重要的就是设置这个functiontocall

	public void setUpFunction(functionDelegate func)
	{
		functionToCall = func;
	}

	public int price;
	public int availableTimes;//单个小道具的可用次数
	public float shipmentRate;//出货率
	public string notes;
	public Sprite sprite;

	public int num;//背包里有几个




	public void 显示名称()
	{
		Debug.Log(name1);
	}
	//点击显示物体名称
	
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


	//实例方法需要通过对象实例来访问，而静态方法可以直接通过类名访问
	//加上static就是静态方法
	public static void hpPlus1()
	{
		骰子的设定和控制 player = GameObject.Find("骰子").GetComponent<骰子的设定和控制>();
		player.nowUpAspect = player.sixAspects[player.nowUpAspect.num + 1 > 6 ? 6 : player.nowUpAspect.num];

		Debug.Log("hpPlus1"+"现在的上面是"+player.nowUpAspect.num);
	}
	public static void hpPlus2()
	{
		骰子的设定和控制 player = GameObject.Find("骰子").GetComponent<骰子的设定和控制>();
		player.nowUpAspect = player.sixAspects[player.nowUpAspect.num + 2 > 6 ? 6 : player.nowUpAspect.num];

		Debug.Log("hpPlus2" + "现在的上面是" + player.nowUpAspect.num);
	}
	public static void hpTo6()
	{
		骰子的设定和控制 player = GameObject.Find("骰子").GetComponent<骰子的设定和控制>();
		player.nowUpAspect = player.sixAspects[6];

		Debug.Log("hpTo6" + "现在的上面是" + player.nowUpAspect.num);
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

