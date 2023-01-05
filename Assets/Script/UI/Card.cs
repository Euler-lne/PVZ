using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

//IPointerClickHandler 要实现接口，鼠标点击逻辑
public class Card : MonoBehaviour, IPointerClickHandler
{
    public GameObject ObjectPrefab;//卡片对应的物体预制件
    private GameObject curGameObject;//记录当前创建出来的物体
    GameObject darkBg;
    GameObject progressBar;
    public float waitTime;
    public int useSun;
    private float timer;
    public PlantInfoItem plantInfo;
    public bool hasUse = false;
    public bool hasLock = false;
    public bool isMove = false;
    public bool isStart = false;//卡片是否初始化
    // Start is called before the first frame update
    void Start()
    {
        //transform.Find()找到物体身上的子物体
        darkBg = transform.Find("dark").gameObject;
        progressBar = transform.Find("process").gameObject;
        //游戏还没有开始，不压黑
        darkBg.SetActive(false);
        progressBar.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.gameStart)
            return;
        if (!isStart)
        {
            darkBg.SetActive(true);
            progressBar.SetActive(true);
            isStart = true;
        }
        timer += Time.deltaTime;
        UpdateProgress();
        UpdateDarkBg();
    }

    void UpdateProgress()
    {
        //限制value的值在min和max之间， 如果value小于min，返回min。 如果value大于max，返回max，否则返回value (value,min,max)
        float per = Mathf.Clamp(timer / waitTime, 0, 1);
        progressBar.GetComponent<Image>().fillAmount = 1 - per;
    }
    void UpdateDarkBg()
    {
        //todo : && useSun > 当前太阳数量
        //进度是否为0，太阳的数量是否足够
        if (progressBar.GetComponent<Image>().fillAmount == 0 && GameManager.instance.sunNum >= useSun)
        {
            darkBg.SetActive(false);//不压黑
        }
        else//否则背景压黑
        {
            darkBg.SetActive(true);
        }
    }
    //按下鼠标，拖拽开始
    public void OnBeginDrag(BaseEventData data)
    {
        //初始化拖拽失效
        if (!isStart)
            return;
        //判断是否可以种植，压黑则无法种植
        if (darkBg.activeSelf)
            return;
        PointerEventData pointerEventData = data as PointerEventData;//创建一个鼠标类型
        curGameObject = Instantiate(ObjectPrefab);//生成一个游戏对象
        curGameObject.transform.position = TranslateScreenToWorld(pointerEventData.position);//将鼠标的位置转换为世界坐标后赋予游戏对象
        //点击卡片播放音乐
        SoundManager.instance.PlaySound(Globals.S_Seedlift);
    }
    //按住鼠标，拖拽过程
    public void OnDrag(BaseEventData data)
    {
        if (curGameObject == null)
        {
            return;//一定要判断
        }
        PointerEventData pointerEventData = data as PointerEventData;//创建一个鼠标类型
        curGameObject.transform.position = TranslateScreenToWorld(pointerEventData.position);//将鼠标的位置转换为世界坐标后赋予游戏对象
    }
    //松开鼠标，种植
    public void OnEndDrag(BaseEventData data)
    {
        if (curGameObject == null)
        {
            return;//一定要判断
        }
        PointerEventData pointerEventData = data as PointerEventData;//创建一个鼠标类型
        //Physics2D.OverlapPointAll获取世界某点上所有碰撞体
        Collider2D[] col = Physics2D.OverlapPointAll(TranslateScreenToWorld(pointerEventData.position));
        foreach (Collider2D c in col)
        {
            //碰撞体的标签为Land的话就是我们需要的格子
            //如果该格子有植物那么久不可以种
            if (c.tag == "Land" && c.transform.childCount == 0)
            {
                //把当前物体添加为土地的子物体
                curGameObject.transform.parent = c.transform;
                curGameObject.transform.localPosition = Vector3.zero;//相对于父亲变换坐标，坐标变换为(0,0,0)
                curGameObject.GetComponent<Plant>().SetPlantStart();
                //测试
                GameManager.instance.GetPlantLine(curGameObject);
                //种植到土地上播放音乐
                SoundManager.instance.PlaySound(Globals.S_Plant);
                //重置默认值，生成结果
                curGameObject = null;
                GameManager.instance.ChangeSunNum(-useSun);
                //重置计时器
                timer = 0;
                break;
            }
        }
        //如果种植失败，那么curGameObject还存活着，那么要销毁他，重置curGameObject
        if (curGameObject != null)//种植失败
        {
            GameObject.Destroy(curGameObject);
            //或者Destroy(curGameObject);
            curGameObject = null;
        }
    }
    //将鼠标坐标（鼠标位于屏幕上）转换为世界坐标
    public static Vector3 TranslateScreenToWorld(Vector3 positon)
    {
        Vector3 cameraTranslatePos = Camera.main.ScreenToWorldPoint(positon);
        return new Vector3(cameraTranslatePos.x, cameraTranslatePos.y, 0);
    }






    //卡片移动
    //注意有个接口
    //IPointerClickHandler的接口
    public void OnPointerClick(PointerEventData eventData)
    {
        // throw new System.NotImplementedException(); 原来自带的
        if (isMove || hasLock || GameManager.instance.gameStart)
            return;
        if (hasUse)//出现在上方的卡片栏中
        {
            RemoveCard(gameObject);
        }
        else
        {
            AddCard();
        }
    }
    //都是一张卡，卡的位置不同，移除操作是对于上方卡槽中的卡的操作，但是haslock和 darkBg.SetActive要对下方所有卡进行操作
    public void RemoveCard(GameObject removeCard)
    {
        ChooseCardPanel chooseCardPanel = UIManager.instance.chooseCardPanel;
        if (chooseCardPanel.ChooseCard.Contains(removeCard))
        {
            //移除操作
            removeCard.GetComponent<Card>().isMove = true;
            chooseCardPanel.ChooseCard.Remove(removeCard);
            //移动完之后就回位
            chooseCardPanel.UpdateCarPositon();
            //移动到原来的位置
            Transform carParent = UIManager.instance.allCardPanel.bg.transform.Find("Card" + removeCard.GetComponent<Card>().plantInfo.plantId);
            Vector3 curPosition = removeCard.transform.position;
            //设置父亲
            removeCard.transform.SetParent(UIManager.instance.transform, false);
            //改变位置，因为设置了父亲后位置会改变
            removeCard.transform.position = curPosition;
            //DOMove
            removeCard.transform.DOMove(carParent.position, 0.3f).OnComplete(
                () =>
                {
                    // hasLock = false;
                    // darkBg.SetActive(false);
                    carParent.Find("BeforeCard").GetComponent<Card>().darkBg.SetActive(false);
                    carParent.Find("BeforeCard").GetComponent<Card>().hasLock = false;
                    removeCard.GetComponent<Card>().isMove = false;
                    Destroy(removeCard);//移动好后删除
                }
            );

        }
    }
    //对下方所有卡的操作，复制出来的卡是上方的卡
    public void AddCard()
    {
        ChooseCardPanel chooseCardPanel = UIManager.instance.chooseCardPanel;
        int curIndex = chooseCardPanel.ChooseCard.Count;
        if (curIndex >= 8)
        {
            return;
        }
        GameObject useCard = Instantiate(plantInfo.prefab);//创建一个新的卡片，这张卡要被移动
        useCard.transform.SetParent(UIManager.instance.transform);//父亲是最外层，还没有移动
        useCard.transform.position = transform.position;//在这张卡上复制一张新的卡
        useCard.name = "Card";//名字叫做Card
        useCard.GetComponent<Card>().plantInfo = plantInfo;//这张新卡有和旧卡一样的GamObject
        //下方卡被选，压黑
        hasLock = true;
        darkBg.SetActive(true);
        //移动到目标位置
        Transform targetObject = chooseCardPanel.cards.transform.Find("Card" + curIndex);
        useCard.GetComponent<Card>().isMove = true;
        useCard.GetComponent<Card>().hasUse = true;
        chooseCardPanel.ChooseCard.Add(useCard);
        //DoMove进行移动，使用回调函数
        //DoMove 目标位置和移动时间
        useCard.transform.DOMove(targetObject.position, 0.3f).OnComplete(
            () =>
            {
                useCard.transform.SetParent(targetObject, false);//重新设置父亲
                useCard.transform.localPosition = Vector3.zero;
                useCard.GetComponent<Card>().isMove = false;
            }
        );
    }
}
