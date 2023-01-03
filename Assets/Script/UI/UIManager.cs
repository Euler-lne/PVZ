using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public ProgressPanel progressPanel;//添加文本类型的参数，并且在游戏中将文本拖拽到相应位置
    public Text sunNumText;//添加文本类型的参数，并且在游戏中将文本拖拽到相应位置
    public AllCardPanel allCardPanel;
    public ChooseCardPanel chooseCardPanel;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    //在游戏开始之初，读取GameManager中的阳光数量，并且转换为字符串，然后显示在UI的文本控件上面
    public void InitUI()
    {
        sunNumText.text = GameManager.instance.sunNum.ToString();
        allCardPanel.InitCards();
    }
    //刷新UI显示
    public void UpdateUI()
    {
        sunNumText.text = GameManager.instance.sunNum.ToString();
    }
    public void InitProgressPanel()
    {
        progressPanel.SetFlagPercent(GameManager.instance.ProgressId);
        //从0.1开始
        progressPanel.SetPercent(0.1f);
        progressPanel.gameObject.SetActive(true);
    }
    public void UpdateProgressPanel()
    {
        int remainNum = GameManager.instance.curProgressZombie.Count;//目前场上的僵尸数
        //i为第几波正序
        int i = GameManager.instance.ProgressId - GameManager.instance.curProgressId + 1;
        //这一波的僵尸总数
        int totalNum = GameManager.instance.levelData.levelInfoList[GameManager.instance.countInfo].zombieNum;
        //算进度条
        progressPanel.SetPercent(Mathf.Clamp(1.0f * (totalNum - remainNum) / totalNum * (1f / GameManager.instance.ProgressId) + (i - 1) * 1f / GameManager.instance.ProgressId, 0.1f, 1f));
    }
}
