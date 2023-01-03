using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jalapeno : Plant
{
    private GameObject JalapenoAttactPrefb;
    private float x = -77f;
    protected override void Start()
    {
        base.Start();
        JalapenoAttactPrefb = Resources.Load("Prefab/JalapenoAttack") as GameObject;
    }
    public void CreateFire()
    {
        GameObject JalapenoAttact = Instantiate(JalapenoAttactPrefb, new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z), Quaternion.identity);
        SoundManager.instance.PlaySound(Globals.S_Jalapeno);
        GameObject.Destroy(gameObject);
    }
    public override float ChangeHealth(float num)
    {
        return health;
    }
}
