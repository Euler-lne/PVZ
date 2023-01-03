using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPanel : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject Progress;
    private GameObject Head;
    private GameObject LevelText;
    private GameObject Bg;
    private GameObject Flag;
    private GameObject FlagPrefab;
    private void Awake()
    {
        Progress = transform.Find("Progress").gameObject;
        Head = transform.Find("Head").gameObject;
        LevelText = transform.Find("LevelText").gameObject;
        Bg = transform.Find("Bg").gameObject;
        Flag = transform.Find("Flag").gameObject;
        //从Resources中加载预制件
        FlagPrefab = Resources.Load("Prefab/Flag") as GameObject;
    }
    void Start()
    {
        Flag.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void SetPercent(float per)
    {
        //进度条
        Progress.GetComponent<Image>().fillAmount = per;
        //进度条最右边的位置（初始位置）
        float originPosX = Bg.GetComponent<RectTransform>().position.x + Bg.GetComponent<RectTransform>().sizeDelta.x / 2;
        //进度条宽度
        float width = Bg.GetComponent<RectTransform>().sizeDelta.x;
        //这个是自定义参数，用来做偏移，可以更改
        float offset = 10f;
        //设置头的x轴位置；最右边的位置 - 进度宽度的一半 + 自定义的偏移量
        Head.GetComponent<RectTransform>().position = new Vector2(originPosX - per * width + offset, Head.GetComponent<RectTransform>().position.y);
    }

    public void SetFlagPercent(int num)
    {
        //进度条最右边的位置（初始位置）
        float originPosX = Bg.GetComponent<RectTransform>().position.x + Bg.GetComponent<RectTransform>().sizeDelta.x / 2;
        //进度条宽度
        float width = Bg.GetComponent<RectTransform>().sizeDelta.x;
        //这个是自定义参数，用来做偏移，可以更改
        float offset = 15f;
        //设置头的x轴位置；最右边的位置 - 进度宽度的一半 + 自定义的偏移量
        float per = 1.0f / num;
        for (int i = 1; i <= num; i++, per += 1.0f / num)
        {
            GameObject newFlag = Instantiate(FlagPrefab);
            newFlag.transform.SetParent(gameObject.transform, false);
            newFlag.GetComponent<RectTransform>().position = Flag.GetComponent<RectTransform>().position;
            newFlag.GetComponent<RectTransform>().position = new Vector2(originPosX - per * width + offset, newFlag.GetComponent<RectTransform>().position.y);
        }
        Head.transform.SetAsLastSibling();//头放到最下方
    }
}
