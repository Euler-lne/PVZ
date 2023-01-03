using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Euler/PlantInfo",fileName ="PlantInfo",order =1)]
public class PlantInfo : ScriptableObject
{
    public List<PlantInfoItem> PlantInfoList = new List<PlantInfoItem>();
}

[System.Serializable]
public class PlantInfoItem
{
    public int plantId;
    public string plantName;
    public string description;
    public GameObject prefab;

    override
    public string ToString()
    {
        return "[id]: " + plantId.ToString();
    }

}
