using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class 道具设定 : MonoBehaviour//所有道具、商店的类型，还得做游戏中的道具界面
{
    public class prop//单个小道具的类型
    {
        public string name;

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
    }
    //接下来写各个道具对应的函数（道具的功能）
    void hpPlus1()
    {
        Debug.Log("hpPlus1");
    }
    void hpPlus2()
    {

    }
    void hpTo6()
    {

    }
    void flash()
    {

    }
    void oneMoreStep()
    {

    }
	void lightningHammer()
	{

	}
	void pierce()
	{

	}
	void shield()
	{

	}
	void rage()
	{

	}
	void revive()
	{
		
	}
	void scrachCard()
	{

	}






	//需要有获取道具，使用道具两个函数
	//获取是，在商店那点击，然后把对应的道具放入背包
	//使用是，在背包里点击，就出发对应的函数
	void getProp(prop aprop)
    {
        bagPropList.Add(aprop);
    }
    void useProp(prop aprop)
    {
        aprop.functionToCall();
        bagPropList.Remove(aprop);
    }

	//所有小道具的集合list
	public List<prop> allPropsList = new List<prop>();
    //还是需要一个start，初始化这些个小道具
    private void Start()
    {
		initAllProps();

		start打开商店();
    }
    void initAllProps()
    {
        prop prop1 = new prop();
        prop1.name = "面包";
        prop1.setUpFunction(hpPlus1);
        prop1.price = 1;
        prop1.availableTimes = 1;
        prop1.shipmentRate = 0.1f;
        prop1.notes="bread";
        prop1.sprite = null;
		prop1.num = 0;				allPropsList.Add(prop1);

		prop prop2 = new prop();
		prop2.name = "烤鸡";
		prop2.setUpFunction(hpPlus2);
		prop2.price = 2;
		prop2.availableTimes = 1;
		prop2.shipmentRate = 0.1f;
		prop2.notes = "好吃";
		prop2.sprite = null;
		prop2.num = 0;				allPropsList.Add(prop2);

		prop prop3 = new prop();
		prop3.name = "生命药剂（攻击）";
		prop3.setUpFunction(hpTo6);
		prop3.price = 10;
		prop3.availableTimes = 1;
		prop3.shipmentRate = 0.1f;
		prop3.notes = "嗑药->爷们要战斗";
		prop3.sprite = null;
		prop3.num = 0;				allPropsList.Add(prop3);

		prop prop4 = new prop();
		prop4.name = "传送卷轴";
		prop4.setUpFunction(flash);
		prop4.price = 30;
		prop4.availableTimes = 1;
		prop4.shipmentRate = 0.1f;
		prop4.notes = "可用于快速调整位置，躲避危险或接近目标";
		prop4.sprite = null;
		prop4.num = 0;				allPropsList.Add(prop4);

		prop prop5 = new prop();
		prop5.name = "时间沙漏";
		prop5.setUpFunction(oneMoreStep);
		prop5.price = 30;
		prop5.availableTimes = 2;
		prop5.shipmentRate = 0.1f;
		prop5.notes = "使本回合的时间暂停，玩家可以额外进行一次行动";
		prop5.sprite = null;
		prop5.num = 0;				allPropsList.Add(prop5);

		prop prop6 = new prop();
		prop6.name = "闪电链球";
		prop6.setUpFunction(lightningHammer);
		prop6.price = 30;
		prop6.availableTimes = 2;
		prop6.shipmentRate = 0.1f;
		prop6.notes = "遇到一堆怪的时候能够快速清理";
		prop6.sprite = null;
		prop6.num = 0;				allPropsList.Add(prop6);

		prop prop7 = new prop();
		prop7.name = "穿刺之矛";
		prop7.setUpFunction(pierce);
		prop7.price = 30;
		prop7.availableTimes = 2;
		prop7.shipmentRate = 0.1f;
		prop7.notes = "遇到一排怪时使用";
		prop7.sprite = null;
		prop7.num = 0;				allPropsList.Add(prop7);

		prop prop8 = new prop();
		prop8.name = "守护护盾";
		prop8.setUpFunction(shield);
		prop8.price = 30;
		prop8.availableTimes = 2;
		prop8.shipmentRate = 0.1f;
		prop8.notes = "在面对敌人较多或者躲避抵挡远程攻击时使用";
		prop8.sprite = null;
		prop8.num = 0;				allPropsList.Add(prop8);

		prop prop9 = new prop();
		prop9.name = "狂暴骰子";
		prop9.setUpFunction(rage);
		prop9.price = 30;
		prop9.availableTimes = 1;
		prop9.shipmentRate = 0.1f;
		prop9.notes = "别惹平头哥！";
		prop9.sprite = null;
		prop9.num = 0;				allPropsList.Add(prop9);

		prop prop10 = new prop();
		prop10.name = "复活卡？";
		prop10.setUpFunction(revive);
		prop10.price = 100;
		prop10.availableTimes = 1;
		prop10.shipmentRate = 0.1f;
		prop10.notes = "不看广告也能复活";
		prop10.sprite = null;
		prop10.num = 0;				allPropsList.Add(prop10);

		prop prop11 = new prop();
		prop11.name = "刮刮乐？";
		prop11.setUpFunction(scrachCard);
		prop11.price = 8;
		prop11.availableTimes = 1;
		prop11.shipmentRate = 0f;
		prop11.notes = "-----------------------------------------------";
		prop11.sprite = null;
		prop11.num = 0;				allPropsList.Add(prop11);
	}







	//以下是游戏关卡内打开道具界面（背包）的函数
	public Image bagPage;
	public List<prop> bagPropList = new List<prop>();
    public IEnumerator 打开背包()
    {



        return null;
    }






    //以下是在大地图中打开商店界面的函数
    public Image shopPage;//其实是商店的背景图
	public List<prop> shopPropList = new List<prop>();

	int coinAmount = 0;
    public void start打开商店()
	{
		StartCoroutine(打开商店());
	}
    public IEnumerator 打开商店()
    {
		//刷新键
		//shopPage.transform.GetChild(1).

		//金币显示
		shopPage.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = coinAmount.ToString();

		//从allPropsList抽出4个放入shopPropList
		for(int i = 0; i < 4; i++)
		{
			shopPropList.Add(allPropsList[Random.Range(0, allPropsList.Count - 1)]);
		}
		//在图片中显示
		Transform 货架 = shopPage.transform.GetChild(4);
		for(int i = 0; i < 4; i++)
		{
			货架.GetChild(i).GetComponent<Image>().sprite=shopPropList[i].sprite;
			货架.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = shopPropList[i].notes;

			//Debug.Log(货架.GetChild(i).GetChild(0).GetChild(0).name);
		}






		yield return null;
    }

	//public List<prop> random4Props()
	//{
	//	List<prop> 
	//} 

    // Update is called once per frame
    void Update()
    {
        
    }

	public void 点击显示物体名称()
	{
		Debug.Log(this.name);
	}
}
