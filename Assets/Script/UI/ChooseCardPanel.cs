using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


//卡槽
public class ChooseCardPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cards;//父物体
    public GameObject beforeCardPrefab;//背景栏
    public List<GameObject> ChooseCard;//具体选择的卡的容器
    void Start()
    {
        ChooseCard = new List<GameObject>();
        //代码动态生成8个框
        for(int i = 0;i<8;i++)
        {
            GameObject beforeCard = Instantiate(beforeCardPrefab);
            beforeCard.transform.SetParent(cards.transform,false);
            beforeCard.name = "Card" + i.ToString();
            //原本的背景栏有自己的背景图了
            beforeCard.transform.Find("bg").gameObject.SetActive(false);
        }
    }

    //为了使卡片向左靠齐
    public void UpdateCarPositon()
    {
        for(int i = 0;i<ChooseCard.Count;i++)
        {
            GameObject useCard = ChooseCard[i];//取到循环中的卡片
            Transform targetObject = cards.transform.Find("Card" + i.ToString());
            useCard.GetComponent<Card>().isMove = true;
            //DOMove进行移动
            //下标第几就移动到几
            useCard.transform.DOMove(targetObject.position,0.3f).OnComplete(
                ()=>
                {
                    useCard.transform.SetParent(targetObject,false);
                    useCard.transform.localPosition = Vector3.zero;
                    useCard.GetComponent<Card>().isMove = false;
                }
            );
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
