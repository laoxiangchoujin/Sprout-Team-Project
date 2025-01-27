using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class 回合计数器 : MonoBehaviour
{
    public int roundCount = 1;
    public bool bPlayerMoveFirst = true;

    //public bool bRoundPlayerCanMove;
	public bool bRoundPlayerMoved;
	//public bool bRoundEnemyCanMove;
    public int bRoundEnemyMoveCount=0;
    private bool bRoundAllEnemyMoved;

    private GameObject player;
    private GameObject []enemy;

    public float 敌人移动的时间间隔 = 0.3f;

    private float 玩家未操作的时长 = 0f;
	//private float 敌人单次移动总时长 = 1f;//相当于上边的敌人的未操作时长?不过玩家的静止循环靠按键打破，敌人的就靠经过一段时间
	//private float 敌人单次移动now时长 = 0f;//让它+deltatime，直到敌人单次移动总时长足够

	public bool debuglog = false;

	public bool 玩家失败 = false;

	//下边是一些和UI相关的变量
	private bool 回合暂停 = false;
	public GameObject canvas1;//这个是战斗界面的canvas

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemy = GameObject.FindGameObjectsWithTag("Enemy");

		//bRoundPlayerCanMove = true;
		//bRoundEnemyCanMove = false;
		if(player == null || enemy == null)
		{
			Debug.Log("没有敌人或玩家，快去加上");
		}
		else
		{
			StartCoroutine(GameLoop());//协程，在start中启动
		}
		//player.GetComponent<骰子的设定和控制>().bRoundPlayerCanMove = true;
	}

	IEnumerator GameLoop()
	{
		while (true)
		{
			if (回合暂停)
			{
				yield return new WaitForSeconds(10000000);
			}

			player = GameObject.FindGameObjectWithTag("Player");
			enemy = GameObject.FindGameObjectsWithTag("Enemy");//更新一下


			if (玩家失败)
			{
				player.GetComponent<骰子的设定和控制>().bRoundPlayerCanMove = false;

				GameObject.Find("Canvas2/关卡结算界面/失败图片").GetComponent<Image>().enabled = true;
				Debug.Log("玩家失败了");
				break;
			}

			if (player != null && enemy != null && enemy.Length>0)//正常运行的情况
			{
				if (bPlayerMoveFirst)
				{


					if (!bRoundPlayerMoved)//让玩家动
					{
						player.GetComponent<骰子的设定和控制>().bRoundPlayerCanMove = true;
						//应该设置一个计数器，否则加入玩家没动，直接跳过不执行了，不行
						while (true)//玩家不操作的时候，就卡在这里了
						{
							玩家未操作的时长 += Time.deltaTime;
							//Debug.Log("玩家未操作的时长"+ 玩家未操作的时长);
							if (player.GetComponent<骰子的设定和控制>().bJustMoved)
							{
								bRoundPlayerMoved = true;
								player.GetComponent<骰子的设定和控制>().bRoundPlayerCanMove = false;
								玩家未操作的时长 = 0;
								break;
							}
							yield return null;//等待下一帧，不会卡住游戏
						}
					}

					if (bRoundPlayerMoved && !bRoundAllEnemyMoved)//让敌人动
					{
						foreach (var item in enemy)
						{
							if (item == null) continue;

							//先间隔时间再动
							yield return new WaitForSeconds(敌人移动的时间间隔);

							if (item != null)
							{
								item.GetComponent<敌人控制>().bRoundEnemyCanMove = true;//我觉得不需要一段时间，只要把它设为true，敌人就会立即移动
								while (!item.GetComponent<敌人控制>().bJustMoved)
								{
									yield return null;
								}
								item.GetComponent<敌人控制>().bRoundEnemyCanMove = false;
							}

						}
						bRoundAllEnemyMoved = true;

					}

					if (bRoundPlayerMoved && bRoundAllEnemyMoved)//可以进行下个回合，复原变量先
					{
						bRoundPlayerMoved = false;
						bRoundAllEnemyMoved = false;
						bRoundEnemyMoveCount = 0;

						roundCount++;
						if (debuglog)
							Debug.Log("现在的回合数是：" + roundCount);
					}
				}
			}
			else if (player != null && enemy != null && enemy.Length == 0)
			{
				GameObject.Find("Canvas2/关卡结算界面/胜利图片").GetComponent<Image>().enabled = true;
				Debug.Log("敌人都被消灭，游戏胜利");
				break;
			}
			else
			{
				Debug.Log("其他情况？");
				break;
			}
		}
	}

	public void 暂停(bool bPause)
	{
		回合暂停 = bPause;
	}

	void 更新UI()
	{
		//从左1到左4，然后从右1到右4的顺序
		var lui1 = GameObject.Find("Canvas1/左半/ui1/文本").GetComponent<TextMeshProUGUI>();
		lui1.text = "回合：" + roundCount + '/' + "步数" + "?"
			+ '\n' + "剩余敌人：" + enemy.Length
			+ '\n' + "金币数目" + "?";

		var lui3 = GameObject.Find("Canvas1/左半/ui3");
		lui3.transform.GetChild(5).GetComponent<TextMeshProUGUI>().text=player.GetComponent<骰子的设定和控制>().nowUpAspect.num.ToString();
		lui3.transform.GetChild(6).GetComponent<TextMeshProUGUI>().text = player.GetComponent<骰子的设定和控制>().nowUpAspect.up.num.ToString();
		lui3.transform.GetChild(7).GetComponent<TextMeshProUGUI>().text = player.GetComponent<骰子的设定和控制>().nowUpAspect.down.num.ToString();
		lui3.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = player.GetComponent<骰子的设定和控制>().nowUpAspect.left.num.ToString();
		lui3.transform.GetChild(9).GetComponent<TextMeshProUGUI>().text = player.GetComponent<骰子的设定和控制>().nowUpAspect.right.num.ToString();
	}

	private void Update()
	{
		更新UI();
	}

}
