using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
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
	private GameObject[] enemy;
    private GameObject attackTag;//远程攻击标记
    private GameObject[] attackTag2;

    public float 敌人移动的时间间隔 = 0.3f;
    public float 远程攻击的时间间隔 = 0.1f;

    private float 玩家未操作的时长 = 0f;
	//private float 敌人单次移动总时长 = 1f;//相当于上边的敌人的未操作时长?不过玩家的静止循环靠按键打破，敌人的就靠经过一段时间
	//private float 敌人单次移动now时长 = 0f;//让它+deltatime，直到敌人单次移动总时长足够

	public bool debuglog = false;

	public bool 玩家失败 = false;

	//下边是一些和UI相关的变量
	private bool 回合暂停 = false;
	public GameObject canvas1;//这个是战斗界面的canvas


	public int maxEnemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        attackTag = GameObject.FindWithTag("AttackTag");
        attackTag2 = GameObject.FindGameObjectsWithTag("AttackTag2");

        //bRoundPlayerCanMove = true;
        //bRoundEnemyCanMove = false;
        if (player == null || enemy == null)
		{
			Debug.Log("没有敌人或玩家，快去加上");
		}
		else
		{
			maxEnemyCount = enemy.Length;

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
            attackTag = GameObject.FindWithTag("AttackTag");
            attackTag2 = GameObject.FindGameObjectsWithTag("AttackTag2");


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
                                yield return new WaitForSeconds(0.4f);//延迟一段时间确保击败敌人
                                player.GetComponent<骰子的设定和控制>().bRoundPlayerCanMove = false;
								玩家未操作的时长 = 0;
								break;
							}
							yield return null;//等待下一帧，不会卡住游戏
						}
					}

					if (bRoundPlayerMoved && !bRoundAllEnemyMoved)//让敌人动
					{
						//远程攻击标记
                        if (attackTag != null)
                        {
                            if (attackTag.GetComponent<Renderer>().enabled)
                            {
                                //爆炸攻击
                                yield return new WaitForSeconds(0.1f);//延迟一段时间确保击败敌人
                                Destroy(attackTag.gameObject);
                            }
                            attackTag.GetComponent<Renderer>().enabled = true;
                            //attackTag.GetComponent<Collider>().enabled = true;
                            Debug.Log("远程攻击在中心点造成伤害");
                        }


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
                    //远程攻击标记2
                    yield return new WaitForSeconds(0.2f);
                    foreach (var item in attackTag2)
                    {
                        if (item == null) continue;
                        if (item.GetComponent<Renderer>().enabled)
                        {
                            Destroy(item.gameObject);
                        }
                    }

                    if (bRoundPlayerMoved && bRoundAllEnemyMoved)//可以进行下个回合，复原变量先
					{
						yield return new WaitForSeconds(0.05f);//延迟一段时间确保击败敌人
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
                yield return new WaitForSeconds(0.5f);
                GameObject.Find("Canvas2/关卡结算界面/胜利图片").GetComponent<Image>().enabled = false;
                int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (sceneIndex > 10)
                {
                    SceneManager.LoadScene(0);
                }
				else
                {
                    SceneManager.LoadScene(sceneIndex);
                }
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
		var lui1 = GameObject.Find("Canvas1/局内ui 背景/左半/ui1").transform;
		lui1.GetChild(0).GetComponent<TextMeshProUGUI>().text = roundCount.ToString();//第一个，步数
		lui1.GetChild(1).GetComponent<TextMeshProUGUI>().text = enemy.Length.ToString();//第二个，敌人数量
		lui1.GetChild(2).GetComponent<TextMeshProUGUI>().text = GameObject.Find("商店和背包（脚本）").GetComponent<商店和背包>().coinAmount.ToString();//第三个，金币数量

		var lui2 = GameObject.Find("Canvas1/局内ui 背景/左半/ui2").transform;
		lui2.GetChild(0).GetComponent<TextMeshProUGUI>().text = SceneManager.GetActiveScene().name.ToString();//第一个，场景名
		lui2.GetChild(1).GetComponent<TextMeshProUGUI>().text = "消灭全部敌人" + '\n' + '\n' + "剩余：（" + enemy.Length.ToString() + '/' + maxEnemyCount.ToString() + ")";//第二个，当前目标
																							 //还得记录杀了几个敌人，原先总共有几个敌人
																							 //注意

		string path = "Assets/resources/Textures/局内ui/骰子点数图集.spriteatlas";
		Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(SpriteAtlas));
		SpriteAtlas atlas=asset as SpriteAtlas;
		if (atlas == null)
		{
			Debug.Log("atlas为null");
		}
		else
		{
			var lui3 = GameObject.Find("Canvas1/局内ui 背景/左半/ui3").transform;
			lui3.GetChild(0).GetComponent<Image>().sprite = atlas.GetSprite("局内ui 点数" + player.GetComponent<骰子的设定和控制>().nowUpAspect.num.ToString());//中间
			lui3.GetChild(1).GetComponent<Image>().sprite = atlas.GetSprite("局内ui 点数" + player.GetComponent<骰子的设定和控制>().nowUpAspect.up.num.ToString());//上
			lui3.GetChild(2).GetComponent<Image>().sprite = atlas.GetSprite("局内ui 点数" + player.GetComponent<骰子的设定和控制>().nowUpAspect.down.num.ToString());//下
			lui3.GetChild(3).GetComponent<Image>().sprite = atlas.GetSprite("局内ui 点数" + player.GetComponent<骰子的设定和控制>().nowUpAspect.left.num.ToString());//左
			lui3.GetChild(4).GetComponent<Image>().sprite = atlas.GetSprite("局内ui 点数" + player.GetComponent<骰子的设定和控制>().nowUpAspect.right.num.ToString());//右
			lui3.GetChild(5).GetComponent<Image>().sprite = atlas.GetSprite("局内ui 点数" + (7-player.GetComponent<骰子的设定和控制>().nowUpAspect.num).ToString());//背面
		}
		

		var lui4 = GameObject.Find("Canvas1/局内ui 背景/左半/ui4").transform;
		lui4.GetChild(0).GetComponent<TextMeshProUGUI>().text= player.GetComponent<骰子的设定和控制>().hp.ToString();
		lui4.GetChild(1).GetComponent<TextMeshProUGUI>().text = player.GetComponent<骰子的设定和控制>().atk.ToString();

		string buffString=null;
		if (player.GetComponent<骰子的设定和控制>().playerHas守护护盾)
			buffString += " 守护护盾 ";
		if (player.GetComponent<骰子的设定和控制>().playerHas狂暴骰子)
			buffString += " 狂暴骰子 ";
		lui4.GetChild(2).GetComponent<TextMeshProUGUI>().text = buffString;

    }

	private void Update()
	{
		更新UI();
        if (玩家失败)//可随时停止
        {
			StopCoroutine(GameLoop());
            player.GetComponent<骰子的设定和控制>().bRoundPlayerCanMove = false;
            StartCoroutine(ResetGame());
        }
    }

    IEnumerator ResetGame()
    {
        GameObject.Find("Canvas2/关卡结算界面/失败图片").GetComponent<Image>().enabled = true;
        Debug.Log("玩家失败了");
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("Canvas2/关卡结算界面/失败图片").GetComponent<Image>().enabled = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
