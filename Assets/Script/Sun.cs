using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    int boundary_y = 250;
    int boundary_l = -500;
    int boundary_r = 350;
    float idel_time = 5.0f;
    float dis_time = 4.0f;
    float timer = 0;
    public bool isDown = false;
    bool isCollect = false;
    Vector3 target;
    Rigidbody2D sun_rigid;
    private void Awake()
    {
        //用Awake，而不是Start，因为Awake在初始的时候就获得了Awake->OnEable->Start->FixedUpdate->Update->LateUpdate->OnGUI->Reset->OnDisable->OnDestroy
        sun_rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        target = UIManager.instance.transform.Find("ChooseCardPanel").Find("Sun").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollect)//飞向UI
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 500);
            float distance = Vector3.Distance(transform.position, target);
            if (Mathf.Approximately(distance, 0f))
            {
                GameManager.instance.ChangeSunNum(25);
                GameObject.Destroy(gameObject); 
            }
            return;
        }
        //使用计时器，而不是用Add Event 是因为这个游戏对象是克隆而来的，有相同的名字，会产生多个事件
        timer += Time.deltaTime;
        if (timer >= idel_time && !animator.GetBool("Disappear"))
        {
            animator.SetBool("Disappear", true);
        }
        if (timer > idel_time + dis_time)
            Destroy(gameObject);
        if (boundary_y - transform.position.y <= 50 && !isDown)//不上越界
            sun_rigid.gravityScale = 2f;
        if (transform.position.y + boundary_y <= 10)//不越下界
        {
            sun_rigid.gravityScale = 0;
            sun_rigid.drag = 1f;
        }
        if (transform.position.x - boundary_l <= 10 || boundary_r - transform.position.x <= 10)
        {
            sun_rigid.drag = 1f;
        }
    }
    public void SunMove(Vector2 direction)
    {
        float x = direction.x, y = direction.y;
        float x_force, y_force;
        if (y < 0)
        {
            sun_rigid.gravityScale = 0;
            x_force = Random.Range(20, 60);
            y_force = 50f;
        }
        else
        {
            sun_rigid.gravityScale = 0.5f;
            x_force = Random.Range(40, 80);
            y_force = Random.Range(100, 150);
        }
        Vector2 force = new Vector2(x_force, y_force);
        sun_rigid.velocity = direction * force;
    }
    private void OnMouseDown()
    {
        if (isCollect)
            return;
        //点击后：增加阳光数量  
        SoundManager.instance.PlaySound(Globals.S_SunCollect);
        isCollect = true;
        MoveToCollect();
    }
    private void MoveToCollect()
    {
        sun_rigid.drag = 0;
        sun_rigid.gravityScale = 0;
        sun_rigid.velocity = new Vector2(0, 0);
        animator.SetBool("Disappear", false);
    }
}
