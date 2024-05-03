using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy_Boss_1 : Enemy
{
    [SerializeField]
    private GameObject test;


    [SerializeField]
    private State state;

    private Mover moverV;
    private MoverByTransform moverT;
    private Rigidbody2D rb;
    private BulletShooter shooter;

    private float originGravity;

    //�Է¿� ��ġ
    [Header("�Է¿�")]
    [SerializeField]
    private float wait_MaxTime;//��� �ִ� �ð�
    [SerializeField]
    private float move_MinDist;//�̵� �ּ� �Ÿ�
    [SerializeField]
    private float move_MaxTime;//�̵� �ִ� �ð�
    [SerializeField]
    private float meleeAttack1CheckDistance;//���������� ���� ���� �Ÿ�
    [SerializeField]
    private CollisionChecker meleeAttack1AreaChecker;

    [Header("���� ����")]
    [SerializeField]
    private float anyAttackCooltime;

    [Header("��������")]
    [SerializeField]
    private GameObject meleeAttackObject;
    [SerializeField]
    private float meleeAttack1EWaitTime = .5f;
    [SerializeField]
    private float meleeAttack1Time = .1f;
    [SerializeField]
    private float meleeAttack1LWaitTime = .4f;

    [Header("���Ÿ� ����")]
    [SerializeField]
    private float rangeAttack1EWaitTime = .3f;
    [SerializeField]
    private float rangeAttack1Time = .25f;
    [SerializeField]
    private int rangeAttack1RepeatCount = 4;
    [SerializeField]
    private float rangeAttack1LWaitTime = .3f;


    [Header("�ֺ� ź �߻�")]
    [SerializeField]
    private float barrageAttack2EWaitTime = 2f;
    [SerializeField]
    private float barrageAttack2AttackTime = 1.5f;
    [SerializeField]
    private int barrageAttack2BulletNum = 16;
    [SerializeField]
    private int barrageAttack2RepeatCount = 3;
    [SerializeField]
    private float barrageAttack2LWaitTime = 2f;
    [SerializeField]
    private float barrageAttack2Cooltime = 30f;



    //���� ��ġ
    private GameObject meleeAttackGO;
    private int rangeAttack1Counter = 0;

    //�� ��ġ
    [Header("�� ��ġ")]
    [SerializeField]
    private Transform platformL;
    [SerializeField]
    private Transform platformR;
    private Transform targetPlatform;

    //�� ����
    private Renderer renderer;
    private Color originColor;

    //Ÿ�̸�
    private float lastAttackTime;
    private float stateTime;//�� ���¿� ������ �ð�
    private float tick;
    private float time_BAttack;


    //�⺻�Լ�
    protected override void Start()
    {
        base.Start();
        moverV = gameObject.GetComponent<Mover>();
        moverT = gameObject.GetComponent<MoverByTransform>();
        renderer = gameObject.GetComponent<Renderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        shooter = gameObject.GetComponent<BulletShooter>();
        originGravity = rb.gravityScale;
        originColor = renderer.material.color;
        tick = Time.time;
    }

    protected override void Update()
    {
        PerformanceManager.StartTimer("TestEnemy_Boss_1.Update");

        base.Update();

        switch (state)
        {
            case State.Wait:

                if (TimeCheck(tick, 0.1f))
                {
                    tick = Time.time;

                    if (ChoiceAttack())
                    {
                        break;
                    }
                }
                if (TimeCheck(stateTime, wait_MaxTime))
                {
                    SetState(State.Move);
                }
                break;
            case State.Move:
                if (isLookLeft == (GameManager.Player.transform.position.x > transform.position.x + meleeAttack1CheckDistance * (isLookLeft ? -1 : 1)))
                {
                    SetState(State.Wait);
                }
                else if (TimeCheck(stateTime, move_MaxTime))
                {
                    SetState(State.Wait);
                }
                break;

            //����
            case State.MeleeAttack1_EWait:
                if (TimeCheck(stateTime, meleeAttack1EWaitTime))
                {
                    SetState(State.MeleeAttack1_Attack);
                }
                break;
            case State.MeleeAttack1_Attack:
                if (TimeCheck(stateTime, meleeAttack1Time))
                {
                    SetState(State.MeleeAttack1_LWait);
                }
                break;
            case State.MeleeAttack1_LWait:
                if (TimeCheck(stateTime, meleeAttack1LWaitTime))
                {
                    SetState(State.Wait);
                }
                break;

            //���Ÿ�
            case State.RangeAttack1_EWait:
                if (TimeCheck(stateTime, rangeAttack1EWaitTime))
                {
                    SetState(State.RangeAttack1_Attack);
                }
                break;
            case State.RangeAttack1_Attack:
                if (TimeCheck(stateTime, rangeAttack1Time))
                {
                    if (rangeAttack1Counter>=rangeAttack1RepeatCount)
                    {
                        SetState(State.RangeAttack1_LWait);
                    }
                    else
                    {
                        SetState(State.RangeAttack1_Attack);
                    }
                }
                break;
            case State.RangeAttack1_LWait:
                if (TimeCheck(stateTime, rangeAttack1LWaitTime))
                {
                    SetState(State.Wait);
                }
                break;


            case State.BarrageAttack2_EWait:
                if (TimeCheck(stateTime, barrageAttack2EWaitTime))
                {
                    SetState(State.BarrageAttack2_Attack);
                }
                break;
            case State.BarrageAttack2_Attack:
                break;
            case State.BarrageAttack2_LWait:
                if (TimeCheck(stateTime, barrageAttack2LWaitTime))
                {
                    SetState(State.Wait);
                }
                break;
        }
        PerformanceManager.StopTimer("TestEnemy_Boss_1.Update");
    }


    protected override void FixedUpdate()
    {
        PerformanceManager.StartTimer("TestEnemy_Boss_1.FixedUpdate");
        base.FixedUpdate();
        switch (state)
        {
            case State.Wait:
                break;
        }
        PerformanceManager.StopTimer("TestEnemy_Boss_1.FixedUpdate");

    }

    //���� ����
    private void SetState(State st)
    {
        PerformanceManager.StartTimer("TestEnemy_Boss_1.SetState");
        ExitState(st);
        stateTime = Time.time;
        state = st;
        EnterState(st);
        PerformanceManager.StopTimer("TestEnemy_Boss_1.SetState");
    }

    private void EnterState(State st)
    {
        switch (st)
        {
            case State.Wait:
                SetColor(Color.clear);
                tick = Time.time;
                StopMove();
                break;
            case State.Move://�÷��̾�� �ָ� �÷��̾�� �̵�
                MoveToPlayer();
                break;
                case State.MeleeAttack1_EWait:
                SetColor(Color.red * 0.8f);
                break;


            case State.MeleeAttack1_Attack:
                SetColor(Color.red);
                meleeAttackGO = Instantiate(meleeAttackObject, transform);
                meleeAttackGO.GetComponent<Attack>().Initialization(this, "Player", meleeAttackGO);
                Destroy(meleeAttackGO, meleeAttack1Time);
                break;


            case State.RangeAttack1_EWait:
                SetColor(Color.yellow*0.8f);
                rangeAttack1Counter = 0;
                shooter.shootType = BulletShootType.oneWay;
                shooter.BulletNum = 1;
                shooter.bulletSpeedMax = shooter.bulletSpeedMin = 8f;
                shooter.bulletDamageMax = shooter.bulletDamageMin = stats.attackPower; 
                break;

            case State.RangeAttack1_Attack:
                Vector2 temp = GetDist(GameManager.Player.transform.position + Vector3.up*0.5f);
                shooter.bulletAngleMax = shooter.bulletAngleMin =
                    Vector2.SignedAngle(Vector2.up, temp) * (isLookLeft? 1:-1);
                shooter.triger = true;
                rangeAttack1Counter++;
                break;

            case State.BarrageAttack2_EWait:
                shooter.shootType = BulletShootType.fan;
                shooter.BulletNum = barrageAttack2BulletNum;
                shooter.bulletAngleMax = 360;
                shooter.bulletAngleMin = 0;

                break;
            case State.BarrageAttack2_Attack:
                shooter.triger = true;
                break;
        }
    }

    private void ExitState(State st)
    {
        switch (st)
        {
            case State.Wait:
                break;
            case State.Move:
                StopMove();
                break;

            case State.MeleeAttack1_Attack:
                SetColor(Color.clear);
                break;
            case State.MeleeAttack1_LWait:
                lastAttackTime = Time.time;
                break;

            case State.RangeAttack1_EWait:
                SetColor(Color.yellow);
                break;
            case State.RangeAttack1_LWait:
                lastAttackTime = Time.time;
                break;
        }
    }

    //��Ÿ
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
    /// <summary>
    /// ���� ����. �� �ߴٸ� false ��ȯ
    /// </summary>
    /// <returns>���� �������</returns>
    private bool ChoiceAttack()
    {
        if (TimeCheck(lastAttackTime, anyAttackCooltime))
        {
            List<State> states = new();

            if (IsPlayerInMeleeAttack1Area())
            {
                states.Add(State.MeleeAttack1_EWait);
            }
            else
            {
                states.Add(State.RangeAttack1_EWait);
            }

            if (TimeCheck(time_BAttack, barrageAttack2Cooltime))
            {
                states.Add(State.BarrageAttack2_EWait);
            }

            if (states.Count >= 1)
            {
                int rand = Random.Range(0, states.Count);
                SetState(states[rand]);
                return true;
            }
        }

        return false;
    }

    //�̵�
    private void MoveToPlayer()
    {
        bool isPlayerLeft = (GameManager.Player.transform.position.x <= transform.position.x);
        if (isPlayerLeft != isLookLeft)
        {
            Turn();
        }

        //Vector2 targetPos = (Vector2)transform.position + move_MinDist * (isLookLeft ? Vector2.left : Vector2.right);
        float moveX = Stats.moveSpeed * (isLookLeft ? -1 : 1);
        moverV.SetVelocityX(moveX);
    }
    private void StopMove()
    {
        moverT.StopMove();
        moverV.StopMove();
        rb.gravityScale = originGravity;
    }
    private void MoveByTranceform(Vector2 target, float speedRate = 1f)
    {
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        moverT.StartMove(MoverByTransform.moveType.LinearByPosWithSpeed, target, speedRate * Stats.moveSpeed);
    }


    //�ð� üũ
    private bool TimeCheck(float timer, float targetTime)
    {
        return timer + targetTime < Time.time;
    }

    //������ ���⺤�� ��������
    private Vector2 GetDist(Vector2 target)
    {
        Vector2 temp = target - (Vector2)transform.position;
        return temp.normalized;
    }

    /// <summary>
    /// �÷��̾ �������� ��Ÿ� �ȿ� �ִ���
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerInMeleeAttack1Area()
    {
        var tempList = meleeAttack1AreaChecker.GetListOfClass<PlayerUnit>();
        if (tempList.Count >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private enum State
    {
        Wait,//�׳� ���
        Move,//�÷��̾�� �̵�
        MeleeAttack1_EWait, MeleeAttack1_Attack, MeleeAttack1_LWait,//�÷��̾ ������ ������ ���� ����
        RangeAttack1_EWait, RangeAttack1_Attack, RangeAttack1_LWait,//�÷��̾ �ָ� ������ ����ź �߻�
        AreaAttack1_EWait, AreaAttack1_Attack, AreaAttack1_LWait,//�÷��̾ �ִ� ���ǿ� ���� ����
        BarrageAttack1_EWait, BarrageAttack1_Attack, BarrageAttack1_LWait,//�÷��� �ϳ� ���ؼ� �̵� �� ź�� �߻�
        BarrageAttack2_EWait, BarrageAttack2_Attack, BarrageAttack2_LWait,//����� ź �߻�
    }
}
