using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy_Air_R_1 : Enemy
{
    private Rigidbody2D rb;
    [SerializeField]
    private float patrolSpeedRate = 1f;
    [SerializeField]
    private float patrolDistance;
    [SerializeField]
    private float patrolTick = 1f;
    [SerializeField]
    private float patrolMinRange = 2f;//�� �� �����϶� �ּ�
    [SerializeField]
    private float patrolMaxRange = 4f;//�� �� ������ �� �ִ�
    [SerializeField]
    private float patrolRangeTotal = 5f;//ù ��ġ
    private Vector2 patrolDist;
    private Vector2 patrolOrigin;
    [SerializeField]
    private int state = 0;
    private Renderer renderer;
    private Color originColor;

    private float timer1;
    private float timer2;
    private float timer3;
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<Renderer>();
        originColor = renderer.material.color;
        timer1 = Time.time;
        timer2 = Time.time;
        timer3 = Time.time;
        patrolDist = transform.position;
        patrolOrigin = transform.position;
    }
    protected override void Update()
    {
        base.Update();
        if (state >= 0 && state <= 2 && FindPlayer())//�÷��̾� �߰� ��
        {
            SetState(3);
            return;
        }
        switch (state)
        {
            case 0://���� ���
                if (timer1 + patrolTick <= Time.time)
                {
                    SetState(1);
                }
                break;
            case 1://���� �̵�
                if (Vector3.Distance(transform.position, (Vector3)patrolDist) <= .3f)
                {
                    SetState(0);
                }
                break;
            case 2://���� ����
                if (Vector3.Distance(transform.position, (Vector3)patrolDist) <= .3f)
                {
                    SetState(0);
                }
                break;
            case 3://�÷��̾�� ����

                break;
            default:
                break;
        }
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();


        var player = GameManager.Player;
        bool isRight = player.transform.position.x >= transform.position.x;//�÷��̾ �����ʿ� �ִ°�?

        switch (state)
        {
            case 0:
                rb.velocity = new Vector2(0, 0);
                break;
            case 1:
                {
                    float speed = Mathf.Max(0, Speed) * patrolSpeedRate;
                    rb.velocity = GetDist(patrolDist) * speed;
                }
                break;
            case 2:
                {
                    float speed = Mathf.Max(0, Speed);
                    rb.velocity = GetDist(patrolDist) * speed;
                }
                break;
            case 3:

                break;
            default:
                break;
        }
    }

    private void SetState(int num)
    {
        switch (state)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }

        state = num;

        switch (state)
        {
            case 0:
                timer1 = Time.time;
                break;
            case 1:
                SetRandomPatrolPosition();
                break;
            case 2:
                patrolDist = patrolOrigin;
                break;
            case 3:
                break;
            default:
                break;
        }
    }

    private Vector2 GetDist(Vector2 target)//������ ���⺤�� ��������
    {
        Vector2 temp = target - (Vector2)transform.position;
        return temp.normalized;
    }
    private void SetColor(Color color)
    {
        if (color == Color.clear)
        {
            renderer.material.color = originColor;
        }
        else
        {
            renderer.material.color = color;
        }
    }

    private void SetRandomPatrolPosition()
    {
        float randomVelue = Random.Range(1,1);
        float targetX = transform.position.x + Random.Range(-patrolDistance, patrolDistance);
        patrolDist = new Vector2(targetX, transform.position.y);
    }
}
