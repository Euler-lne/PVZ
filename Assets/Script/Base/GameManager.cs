using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance;
    public int sunNum;
    public GameObject bornParent;
    public float creatZombieTime;
    private int zOrderIndex = 0;
    public LevelData levelData;
    [HideInInspector]
    public PlantInfo plantInfo;
    public bool gameStart;
    private int curWorld;//第几个世界
    private int curLevelId;//第几关
    public int curProgressId;//第几波次
    public int ProgressId;//这一关一共有几波
    public int countInfo = 0;
    private int count = 0;//代表levelDataList excel有几行，下标从0开始

    private float sunDowmTimer;
    private float sunDowmTime;
    private bool isSunDownRandom;
    private bool isSunDown;
    public List<GameObject> curProgressZombie;//僵尸容器用于查看僵尸的剩余数量，来判断当前运行到什么程度
    private void Awake()
    {
        instance = this;//单例管理器初始化
    }
    void Start()
    {
        curProgressZombie = new List<GameObject>();
        count = 0;
        ReadData();
        sunDowmTime = Random.Range(8, 16);
        isSunDownRandom = false;
        isSunDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSunDown)
        {
            sunDowmTimer += Time.deltaTime;
            if (sunDowmTimer >= sunDowmTime)
            {
                CreateSunDown();
                sunDowmTimer = 0;
                isSunDownRandom = true;
            }
            if (isSunDownRandom)
            {
                sunDowmTime = Random.Range(8, 16);
                isSunDownRandom = false;
            }
        }
    }
    private void GameStart()
    {
        curProgressId = levelData.levelDataList[count].progressId;
        curLevelId = levelData.levelDataList[count].levelId[4] - '0';
        curWorld = levelData.levelDataList[count].levelId[0] - '0';
        ProgressId = curProgressId;
        UIManager.instance.InitUI();
        UIManager.instance.InitProgressPanel();
    }
    public void GameStartBottomDown()
    {
        CreateZombie();
        //游戏开始播放BGM
        SoundManager.instance.PlayBGM(Globals.BGM1);
        isSunDown = true;
        gameStart = true;
    }
    void ReadData()
    {
        // StartCoroutine(LoadTable());
        LoadTableNew();
    }
    public void LoadTableNew()
    {
        levelData = Resources.Load("TableData/Level") as LevelData;
        plantInfo = Resources.Load("TableData/PlantInfo") as PlantInfo;
        GameStart();
    }
    // IEnumerator LoadTable()
    // {
    //     ResourceRequest request = Resources.LoadAsync("Level");
    //     yield return request;
    //     levelData = request.asset as LevelData;//强制类型转换
    //     //读取表格成功之后才会调用整GameStart()来启动整个游戏
    //     GameStart();
    // }
    //数量发生改变就调用这个方法
    public void ChangeSunNum(int changeNum)
    {
        sunNum += changeNum;
        if (sunNum <= 0)
            sunNum = 0;
        UIManager.instance.UpdateUI();
    }

    public void CreateZombie()
    {
        TableCreateZombie();
    }
    private void TableCreateZombie()
    {
        //判断是否为最后一波，如果表格中当前波次没有可以创建的僵尸，则游戏胜利
        bool canCreate = false;
        //不需要每一次都从0开始
        for (int i = count; i < levelData.levelDataList.Count; i++)
        {
            LevelItem levelItem = levelData.levelDataList[i];
            //如果curProgressId不存在于data中那么久不会让canCreate为True
            if (levelItem.levelId[4] - '0' == curLevelId && levelItem.progressId == curProgressId && curWorld == levelItem.levelId[0] - '0')
            {
                StartCoroutine(ITableCreateZombie(levelItem));
                canCreate = true;
                count++;
            }
        }
        //不能创建僵尸则游戏胜利
        if (!canCreate)
        {
            //相等则则证明到了最后一关
            if (count != levelData.levelDataList.Count)
            {
                curProgressId = levelData.levelDataList[count].progressId;
            }
            //结束协程
            StopAllCoroutines();
            curProgressZombie = new List<GameObject>();
            //胜利后的一些表现
            gameStart = false;
        }
    }
    IEnumerator ITableCreateZombie(LevelItem levelItem)
    {
        yield return new WaitForSeconds(levelItem.createTime);
        //加载预制件：从Resoures文件中加载
        GameObject zombiePrefab = Resources.Load("Prefab/Zombie" + levelItem.zombieType.ToString()) as GameObject;//强制类型转换
        GameObject zombie = Instantiate(zombiePrefab);
        Transform zombieline = bornParent.transform.Find("born" + levelItem.bornPos.ToString());
        zombie.transform.parent = zombieline;
        zombie.transform.localPosition = Vector3.zero;
        zombie.GetComponent<SpriteRenderer>().sortingOrder = zOrderIndex;
        zOrderIndex++;
        curProgressZombie.Add(zombie);
    }
    public void ZombieDead(GameObject gameObject)
    {
        if (curProgressZombie.Contains(gameObject))
        {
            curProgressZombie.Remove(gameObject);
            UIManager.instance.UpdateProgressPanel();
        }
        if (curProgressZombie.Count == 0)//这一波打完
        {
            curProgressId -= 1;
            countInfo++;
            zOrderIndex = 0;
            //TODO：最后一波要做特殊化处理
            TableCreateZombie();
        }
    }

    public void CreateSunDown()
    {
        //左下角
        Vector3 leftBottom = Camera.main.ViewportToWorldPoint(Vector2.zero);
        //右上角
        Vector3 rightTop = Camera.main.ViewportToWorldPoint(Vector2.one);
        GameObject sunPrefab = Resources.Load("Prefab/Sun") as GameObject;
        float x = Random.Range(leftBottom.x + 30, rightTop.x - 30);
        Vector3 bornPos = new Vector3(x, rightTop.y, 0);
        GameObject sun = Instantiate(sunPrefab, bornPos, Quaternion.identity);
        sun.GetComponent<Sun>().isDown = true;
        float force_x = Random.Range(-40, 40);
        float force_y = Random.Range(-120, -80);
        sun.GetComponent<Rigidbody2D>().velocity = new Vector2(force_x, force_y);
    }
    public int GetPlantLine(GameObject plant)
    {
        GameObject lineObject = plant.transform.parent.parent.gameObject;
        string lineStr = lineObject.name;
        //int.Parse()字符串转换为int，由于切割的是前方的元素所以，line0.Split("line") 为 （空，0）
        int line = int.Parse(lineStr.Split("line")[1]);
        return line;
    }
}
