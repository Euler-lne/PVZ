using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//ScriptableObject主要用于将数据保存问项目中使用的资源，以便在运行中使用
public class LevelData : ScriptableObject
{
    //使用一个List容器来容纳所有的LevelItem行数据
    public List<LevelItem> levelDataList = new List<LevelItem>();
    public List<LevelItemInfo> levelInfoList = new List<LevelItemInfo>();
}

[System.Serializable]//表示这个类可以序列化
public class LevelItem
{
    public int id;
    public string levelId;
    public int progressId;
    public int createTime;
    public int zombieType;
    public int bornPos;
}

[System.Serializable]//表示这个类可以序列化
public class LevelItemInfo
{
    public string levelId;
    public int progressId;
    public int zombieNum;
}


