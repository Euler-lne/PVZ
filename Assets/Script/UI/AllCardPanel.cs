using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//卡片集合
public class AllCardPanel : MonoBehaviour
{
    public GameObject bg;
    public GameObject beforeCardPrefab;
    // Start is called before the first frame update
    void Start()
    {
        //有40个卡片
        for (int i = 0; i < 40; i++)
        {
            GameObject beforeCard = Instantiate(beforeCardPrefab);
            beforeCard.transform.SetParent(bg.transform, false);
            beforeCard.name = "Card" + i.ToString();
        }
    }
    //不能放到Start函数中，因为要确保有数据
    public void InitCards()
    {
        foreach (PlantInfoItem plantInfo in GameManager.instance.plantInfo.PlantInfoList)
        {
            //注意锚点设置为中间，生成选卡栏中的卡片
            Transform cardParent = bg.transform.Find("Card" + plantInfo.plantId);
            GameObject reallyCard = Instantiate(plantInfo.prefab) as GameObject;
            reallyCard.transform.SetParent(cardParent, false);
            reallyCard.transform.localPosition = Vector2.zero;
            reallyCard.name = "BeforeCard";
            reallyCard.GetComponent<Card>().plantInfo = plantInfo;
        }
    }
    void Update()
    {

    }

    //点击方法
    public void OnBtStart()
    {
        GameManager.instance.GameStartBottomDown();
    }
}
