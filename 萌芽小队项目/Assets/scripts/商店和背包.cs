using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class 商店和背包 : MonoBehaviour//所有道具、商店的类型，还得做游戏中的道具界面
{
	//添加单例属性
	public static 商店和背包 Instance { get; private set; }


	//需要有获取道具，使用道具两个函数
	//获取是，在商店那点击，然后把对应的道具放入背包
	//使用是，在背包里点击，就出发对应的函数

	public Sprite 面包;
	public Sprite 炸鸡腿;
	public Sprite 生命药水;
	public Sprite 传送卷轴;
	public Sprite 时间沙漏;
	public Sprite 守护护盾;
	public Sprite 狂暴骰子;
	public Sprite 刮刮乐;

	//所有小道具的集合list
	public List<Prop> allPropsList = new List<Prop>();
    //还是需要一个start，初始化这些个小道具
    private void Start()
    {
		if (Instance == this)
		{
			initAllProps();
			start打开商店();
		}		
    }

	//添加awake来初始化单例
	private void Awake()
	{
		//单例初始化
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}


	void initAllProps()
    {
        Prop prop1 = new Prop();
        prop1.name1 = "面包";
        prop1.setUpFunction(Prop.hpPlus1);
        prop1.price = 1;
        prop1.availableTimes = 1;
        prop1.shipmentRate = 0.1f;
        prop1.notes="bread";
        prop1.sprite = 面包;
		prop1.num = 1;				allPropsList.Add(prop1);

		Prop prop2 = new Prop();
		prop2.name1 = "炸鸡腿";
		prop2.setUpFunction(Prop.hpPlus2);
		prop2.price = 2;
		prop2.availableTimes = 1;
		prop2.shipmentRate = 0.1f;
		prop2.notes = "好吃";
		prop2.sprite = 炸鸡腿;
		prop2.num = 1;				allPropsList.Add(prop2);

		Prop prop3 = new Prop();
		prop3.name1 = "生命药剂（攻击）";
		prop3.setUpFunction(Prop.hpTo6);
		prop3.price = 10;
		prop3.availableTimes = 1;
		prop3.shipmentRate = 0.1f;
		prop3.notes = "嗑药->爷们要战斗";
		prop3.sprite = 生命药水;
		prop3.num = 1;				allPropsList.Add(prop3);

		Prop prop4 = new Prop();
		prop4.name1 = "传送卷轴";
		prop4.setUpFunction(Prop.flash);
		prop4.price = 30;
		prop4.availableTimes = 1;
		prop4.shipmentRate = 0.1f;
		prop4.notes = "可用于快速调整位置，躲避危险或接近目标";
		prop4.sprite = 传送卷轴;
		prop4.num = 1;				allPropsList.Add(prop4);

		//Prop prop5 = new Prop();
		//prop5.name1 = "时间沙漏";
		//prop5.setUpFunction(Prop.oneMoreStep);
		//prop5.price = 30;
		//prop5.availableTimes = 2;
		//prop5.shipmentRate = 0.1f;
		//prop5.notes = "使本回合的时间暂停，玩家可以额外进行一次行动";
		//prop5.sprite = 时间沙漏;
		//prop5.num = 1;				allPropsList.Add(prop5);

		//Prop prop6 = new Prop();
		//prop6.name1 = "闪电链球";
		//prop6.setUpFunction(Prop.lightningHammer);
		//prop6.price = 30;
		//prop6.availableTimes = 2;
		//prop6.shipmentRate = 0.1f;
		//prop6.notes = "遇到一堆怪的时候能够快速清理";
		//prop6.sprite = null;
		//prop6.num = 1;				allPropsList.Add(prop6);

		//Prop prop7 = new Prop();
		//prop7.name1 = "穿刺之矛";
		//prop7.setUpFunction(Prop.pierce);
		//prop7.price = 30;
		//prop7.availableTimes = 2;
		//prop7.shipmentRate = 0.1f;
		//prop7.notes = "遇到一排怪时使用";
		//prop7.sprite = null;
		//prop7.num = 1;				allPropsList.Add(prop7);

		Prop prop8 = new Prop();
		prop8.name1 = "守护护盾";
		prop8.setUpFunction(Prop.shield);
		prop8.price = 30;
		prop8.availableTimes = 2;
		prop8.shipmentRate = 0.1f;
		prop8.notes = "在面对敌人较多或者躲避抵挡远程攻击时使用";
		prop8.sprite = 守护护盾;
		prop8.num = 1;				allPropsList.Add(prop8);

		Prop prop9 = new Prop();
		prop9.name1 = "狂暴骰子";
		prop9.setUpFunction(Prop.rage);
		prop9.price = 30;
		prop9.availableTimes = 1;
		prop9.shipmentRate = 0.1f;
		prop9.notes = "别惹平头哥！";
		prop9.sprite = 狂暴骰子;
		prop9.num = 1;				allPropsList.Add(prop9);

		//prop prop10 = new prop();
		//prop10.name = "复活卡？";
		//prop10.setUpFunction(revive);
		//prop10.price = 100;
		//prop10.availableTimes = 1;
		//prop10.shipmentRate = 0.1f;
		//prop10.notes = "不看广告也能复活";
		//prop10.sprite = null;
		//prop10.num = 0;				allPropsList.Add(prop10);

		Prop prop11 = new Prop();
		prop11.name1 = "刮刮乐？";
		prop11.setUpFunction(Prop.scrachCard);
		prop11.price = 8;
		prop11.availableTimes = 1;
		prop11.shipmentRate = 0f;
		prop11.notes = "0.01%666金币		0.99%50金币		9%30金币		30%10金币	40%6金币		20%1金币";
		prop11.sprite = 刮刮乐;
		prop11.num = 1;				allPropsList.Add(prop11);
	}







	//以下是游戏关卡内打开道具界面（背包）的函数
	public Image bagPage;
	public List<Prop> bagPropList = new List<Prop>(6);//因为装备栏最多6个
	private int MAX_SIZE = 6;
	public void start打开背包()
	{
		StartCoroutine(打开背包());
	}
    public IEnumerator 打开背包()
    {
		bagPage.gameObject.SetActive(true);

		刷新背包金币();

		刷新背包();


        yield return null;
    }

	public void 刷新背包金币()
	{
		//金币显示
		bagPage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = coinAmount.ToString();
	}

	public void 刷新背包()
	{
		//在货架中显示道具图片和描述
		Transform 货架 = bagPage.transform.GetChild(1);
		Transform 货物介绍父对象=货架.GetChild(6);
		for (int i = 0; i < bagPropList.Count; i++)
		{
			//不止要改图片和文本，要给图片上赋的Prop也改了
			Prop.copyValues(货架.GetChild(i).GetComponent<Prop>(), bagPropList[i]);
			//货架.GetChild(i).GetComponent<Prop>()=shopPropList[i];

			//货架.GetChild(i).gameObject.SetActive(true);
			货架.GetChild(i).GetComponent<Image>().sprite = bagPropList[i].sprite;
			//显示介绍和数量		
			货架.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = bagPropList[i].num.ToString();//介绍挪去别地了，数量的索引变成0了

			货物介绍父对象.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = bagPropList[i].name1 + ':' + bagPropList[i].notes;
		}
		for(int i = bagPropList.Count;i < 6; i++)//没有prop的那部分架子呢
		{
			//创建一个空prop
			Prop nullProp=new Prop();

			Prop.copyValues(货架.GetChild(i).GetComponent<Prop>(), nullProp);
			
			货架.GetChild(i).GetComponent<Image>().sprite = null;
			
			//货架.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = null;
			货架.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = null;
			货物介绍父对象.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = null;
		}
	}

	public void 使用prop(Prop uiProp)
	{
		//因为你在ui上看到的prop实际上是bag里面prop复制过来的，所以改ui的prop不影响bag的prop，所以
		//直接在bagPropList中查找对应名字并操作
		var item=bagPropList.Find(x=>x.name1==uiProp.name1);
		if (item!=null && item.num > 0)
		{
			item.functionToCall();
			item.num -= 1;

			if (item.num == 0)
			{
				bagPropList.Remove(item);
			}

			刷新背包();
		}	
	}
	public void 离开背包()
	{
		bagPage.gameObject.SetActive(false);
	}

	//public void 背包显示介绍(GameObject 介绍)
	//{
	//	介绍.gameObject.SetActive(true);
	//}













	//以下是在大地图中打开商店界面的函数
	public Image shopPage;//其实是商店的背景图
	public List<Prop> shopPropList = new List<Prop>();

	public int coinAmount = 100;
    public void start打开商店()
	{
		StartCoroutine(打开商店());
	}
    public IEnumerator 打开商店()
    {
		//刷新键
		//shopPage.transform.GetChild(1).
		刷新商店金币();

		刷新商店();
		

		yield return null;
    }

	public void 刷新商店金币()
	{
		//金币显示
		if(shopPage != null)
		shopPage.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = coinAmount.ToString();
	}

	public void 刷新商店()
	{
		shopPropList.Clear();
		//从allPropsList抽出4个放入shopPropList
		for (int i = 0; i < 4; i++)
		{
			shopPropList.Add(allPropsList[Random.Range(0, allPropsList.Count - 1)]);
		}
		//在货架中显示道具图片和描述
		Transform 货架 = shopPage.transform.GetChild(4);
		for (int i = 0; i < 4; i++)
		{
			//不止要改图片和文本，要给图片上赋的Prop也改了
			Prop.copyValues(货架.GetChild(i).GetComponent<Prop>(), shopPropList[i]);
			//货架.GetChild(i).GetComponent<Prop>()=shopPropList[i];

			货架.GetChild(i).GetComponent<Image>().sprite = shopPropList[i].sprite;
			//显示介绍和价格
			货架.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = shopPropList[i].name1 + ':' + shopPropList[i].notes;
			货架.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = shopPropList[i].price.ToString();

			//Debug.Log(货架.GetChild(i).GetChild(0).GetChild(0).name);
		}
		////展示推荐商品（最贵的那个）
		//prop recommed = shopPropList[0];
		//for(int i=0;i< 4; i++)
		//{
		//	if (shopPropList[i].price > recommed.price)
		//		recommed= shopPropList[i];
		//}
		//货架.GetChild(4).GetComponent<Image>().sprite=recommed.sprite;
		//货架.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = recommed.notes;
		//展示刮刮乐
		Prop scrach = allPropsList[allPropsList.Count - 1];
		Prop.copyValues(货架.GetChild(4).GetComponent<Prop>(), scrach);
		货架.GetChild(4).GetComponent<Image>().sprite = scrach.sprite;
		货架.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = scrach.name1 + ':' + scrach.notes;
		货架.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>().text = scrach.price.ToString();
	}//只影响4个商品+1个刮刮卡

	public void 购买商品(Prop aprop)
	{
		if (bagPropList.Count >= MAX_SIZE)
		{
			Debug.Log("背包已满，别再买了");
			return;
		}
		if (coinAmount >= aprop.price)
		{
			coinAmount -= aprop.price;
			if (aprop.name1 == "刮刮乐？")
			{
				//0.01%,666金币,0.99 % 50金币,9 % 30金币,30 % 10金币,40 % 6金币,20 % 1金币
				float scrachNumber = 0;
				scrachNumber = Random.Range(0f, 1f);

				int scrachCoin = 0;
				if (scrachNumber <= 0.0001)
				{
					scrachCoin = 666;
				}
				else if (scrachNumber > 0.0001 && scrachNumber <= 0.01)
				{
					scrachCoin = 50;
				}
				else if (scrachNumber > 0.01 && scrachNumber <= 0.1)
				{
					scrachCoin = 30;
				}
				else if (scrachNumber > 0.1 && scrachNumber <= 0.4)
				{
					scrachCoin = 10;
				}
				else if (scrachNumber > 0.4 && scrachNumber <= 0.8)
				{
					scrachCoin = 6;
				}
				else if (scrachNumber > 0.8 && scrachNumber <= 1)
				{
					scrachCoin = 1;
				}
				coinAmount += scrachCoin;
				Debug.Log("从刮刮乐中得到了"+scrachCoin+"枚金币");
			}
			else//正经买了能放入物品栏的商品
			{
				bool flag = false;//默认我当包里没有这个商品
				foreach (var item in bagPropList)
				{
					if (item.name1 == aprop.name1)
					{
						item.num += 1;
						Debug.Log("背包里已有" + item.name1 + item.num + "个");
						flag = true;
					}
				}
				if (!flag)
				{
					bagPropList.Add(aprop);
					Debug.Log("添加了" + aprop.name1);
				}
				
			}
		}
		else
		{
			Debug.Log("买不起别买");
		}
		
	}

	public void 点击老板娘()
	{
		StartCoroutine(夸夸老板娘());
	}
	public IEnumerator 夸夸老板娘()
	{
		Transform 老板娘 = shopPage.transform.GetChild(2);

		//留一段不写，区分普通关和boss战的if

		coinAmount += 1;

		老板娘.GetChild(0).gameObject.SetActive(true);
		老板娘.GetChild(1).gameObject.SetActive(true);

		yield return new WaitForSeconds(1f);

		老板娘.GetChild(0).gameObject.SetActive(false);
		老板娘.GetChild(1).gameObject.SetActive(false);

		yield return null;
	}

	public void 离开商店()
	{
		shopPage.gameObject.SetActive(false);
	} 

    // Update is called once per frame
    void Update()
    {
		刷新商店金币();

		if(Input.GetKeyDown(KeyCode.F))
			for(int i = 0; i < bagPropList.Count; i++)
			{
				Debug.Log(bagPropList[i].name1);
			}
    }

	public void 点击显示物体名称()
	{
		Debug.Log(this.name);
	}
}
