using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
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

    [Header("���� ����")]
    [SerializeField]
    private float areaAttack1EWaitTime = 2f;
    [SerializeField]
    private float areaAttack1AttackTime = 0.75f;
    [SerializeField]
    private float areaAttack1LWaitTime = 1.25f;
    [SerializeField]
    private float areaAttack1Cooltime = 45f;
    [SerializeField]
    private GameObject areaAttackObjectL;
    [SerializeField]
    private GameObject areaAttackObjectR;
    [SerializeField]
    private GameObject areaAttackObjectD;


    [Header("�ֺ� ź �߻�")]
    [SerializeField]
    private float barrageAttack1EWaitTime = 2f;
    [SerializeField]
    private float barrageAttack1AttackTime = .5f;
    [SerializeField]
    private int barrageAttack1BulletNum = 8;
    [SerializeField]
    private float barrageAttack1Angle = 45f;
    [SerializeField]
    private int barrageAttack1RepeatCount = 6;
    [SerializeField]
    private float barrageAttack1LWaitTime = 2f;
    [SerializeField]
    private float barrageAttack1Cooltime = 60f;


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
    private GameObject areaAttackGO;
    private int rangeAttack1Counter = 0;
    private int barrageAttack1Counter = 0;
    private int barrageAttack2Counter = 0;

    //�� ��ġ
    [Header("�� ��ġ")]
    [SerializeField]
    private Transform platformL;
    [SerializeField]
    private CollisionChecker collisionCheckerL;
    [SerializeField]
    private Transform platformR;
    [SerializeField]
    private CollisionChecker collisionCheckerR;
    [SerializeField]
    private Transform platformD;
    [SerializeField]
    private CollisionChecker collisionCheckerD;
    private Transform targetPlatform;

    //�� ����
    private Renderer renderer;
    private Color originColor;

    //Ÿ�̸�
    private float lastAttackTime;
    private float stateTime;//�� ���¿� ������ �ð�
    private float tick;
    private float time_BAttack;
    private float time_BAttack2;
    private float time_AreaAttack;


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
        lastAttackTime = stateTime = tick = Time.time;

        time_BAttack = -999f;
        time_AreaAttack = -999f;
        time_BAttack2 = -999f;
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
                    if (rangeAttack1Counter >= rangeAttack1RepeatCount)
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

            //����
            case State.AreaAttack1_EWait:
                if (TimeCheck(stateTime, areaAttack1EWaitTime))
                {
                    SetState(State.AreaAttack1_Attack);
                }
                break;
            case State.AreaAttack1_Attack:
                if (TimeCheck(stateTime, areaAttack1AttackTime))
                {
                    SetState(State.AreaAttack1_LWait);
                }
                break;
            case State.AreaAttack1_LWait:
                if (TimeCheck(stateTime, areaAttack1LWaitTime))
                {
                    SetState(State.Wait);
                }
                break;

            //ź��1
            case State.BarrageAttack1_EWait:
                if (TimeCheck(stateTime, barrageAttack1EWaitTime))
                {
                    SetState(State.BarrageAttack1_Attack);
                }
                break;
            case State.BarrageAttack1_Attack:
                if (TimeCheck(stateTime, barrageAttack1AttackTime))
                {
                    if (barrageAttack1Counter >= barrageAttack1RepeatCount)
                    {
                        SetState(State.BarrageAttack1_LWait);
                    }
                    else
                    {
                        SetState(State.BarrageAttack1_Attack);
                    }
                }
                break;
            case State.BarrageAttack1_LWait:
                if (TimeCheck(stateTime, barrageAttack1LWaitTime))
                {
                    SetState(State.Wait);
                }
                break;

            //ź��2
            case State.BarrageAttack2_EWait:
                if (TimeCheck(stateTime, barrageAttack2EWaitTime))
                {
                    SetState(State.BarrageAttack2_Attack);
                }
                break;
            case State.BarrageAttack2_Attack:
                if (TimeCheck(stateTime, barrageAttack2AttackTime))
                {
                    if (barrageAttack2Counter >= barrageAttack2RepeatCount)
                    {
                        SetState(State.BarrageAttack2_LWait);
                    }
                    else
                    {
                        SetState(State.BarrageAttack2_Attack);
                    }
                }
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
                SetColor(Color.yellow * 0.8f);
                rangeAttack1Counter = 0;
                shooter.shootType = BulletShootType.oneWay;
                shooter.BulletNum = 1;
                shooter.bulletSpeedMax = shooter.bulletSpeedMin = 8f;
                shooter.bulletDamageMax = shooter.bulletDamageMin = stats.attackPower;
                break;

            case State.RangeAttack1_Attack:
                {
                    ShootToPlayer();
                    rangeAttack1Counter++;
                }
                break;


            case State.AreaAttack1_EWait:
                {
                    SetColor(Color.red);
                }
                break;

            case State.AreaAttack1_Attack:
                {
                    if (ReferenceEquals(targetPlatform, platformD))
                    {
                        areaAttackGO = Instantiate(areaAttackObjectD);
                        areaAttackGO.GetComponent<Attack>().Initialization(this, "Player", areaAttackGO);
                    }
                    else if (ReferenceEquals(targetPlatform, platformL))
                    {
                        areaAttackGO = Instantiate(areaAttackObjectL);
                        areaAttackGO.GetComponent<Attack>().Initialization(this, "Player", areaAttackGO);
                    }
                    else if (ReferenceEquals(targetPlatform, platformR))
                    {
                        areaAttackGO = Instantiate(areaAttackObjectR);
                        areaAttackGO.GetComponent<Attack>().Initialization(this, "Player", areaAttackGO);
                    }
                    Destroy(areaAttackGO, areaAttack1AttackTime);

                    time_AreaAttack = Time.time;
                }
                break;

            case State.BarrageAttack1_EWait:
                //Debug.Log($"{targetPlatform.transform.position}, {targetPlatform}");
                transform.position = targetPlatform.transform.position + (Vector3.up * targetPlatform.lossyScale.y * 0.5f) + (Vector3.up * 2f);
                shooter.shootType = BulletShootType.fan;
                shooter.BulletNum = barrageAttack1BulletNum;
                shooter.bulletSpeedMax = shooter.bulletSpeedMin = 6f;
                moverV.SetVelocity(new Vector2(0, 0.2f));
                break;

            case State.BarrageAttack1_Attack:
                ShootToPlayer(barrageAttack1Angle);
                barrageAttack1Counter++;
                time_BAttack2 = Time.time;
                break;
            case State.BarrageAttack1_LWait:
                StopMove();
                break;

            case State.BarrageAttack2_EWait:
                {
                    SetColor(Color.cyan);
                    shooter.shootType = BulletShootType.fan;
                    shooter.BulletNum = barrageAttack2BulletNum;
                    shooter.bulletAngleMax = 360;
                    shooter.bulletAngleMin = 0;
                    shooter.bulletSpeedMax = shooter.bulletSpeedMin = 3f;
                    barrageAttack2Counter = 0;
                }
                break;
            case State.BarrageAttack2_Attack:
                {
                    float temp = (360 / barrageAttack2BulletNum) / 2;
                    shooter.bulletAngleMax += temp;
                    shooter.bulletAngleMin += temp;
                    time_BAttack = Time.time;
                    barrageAttack2Counter++;
                    shooter.triger = true;
                }
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


            case State.AreaAttack1_EWait:
                {
                    SetColor(Color.clear);
                }
                break;

            case State.BarrageAttack2_LWait:
                lastAttackTime -= Time.time;
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
                //�ٰŸ�
                states.Add(State.MeleeAttack1_EWait);
            }
            else
            {
                //���Ÿ�
                states.Add(State.RangeAttack1_EWait);
            }

            //���� �̵� �� ��ä�� ź
            if (TimeCheck(time_BAttack2, barrageAttack1Cooltime))
            {
                states.Add(State.BarrageAttack1_EWait);
            }

            //������ ź
            if (TimeCheck(time_BAttack, barrageAttack2Cooltime))
            {
                states.Add(State.BarrageAttack2_EWait);
            }

            //���ִ� ���� ����
            if (TimeCheck(time_AreaAttack, areaAttack1Cooltime))
            {
                if (collisionCheckerD.GetListOfClass<PlayerUnit>().Count >= 1 ||
                    collisionCheckerL.GetListOfClass<PlayerUnit>().Count >= 1 ||
                    collisionCheckerR.GetListOfClass<PlayerUnit>().Count >= 1)
                {
                states.Add(State.AreaAttack1_EWait);
                }
            }



            if (states.Count >= 1)
            {
                int rand = Random.Range(0, states.Count);
                var result = states[rand];

                switch(result)
                {
                    case State.BarrageAttack1_EWait:
                    targetPlatform = Random.Range(0, 2) == 0 ? platformL : platformR;
                        break;

                    case State.AreaAttack1_EWait:
                        if (collisionCheckerD.GetListOfClass<PlayerUnit>().Count >= 1)
                        {
                            targetPlatform = platformD;
                        }
                        else if (collisionCheckerL.GetListOfClass<PlayerUnit>().Count >= 1)
                        {
                            targetPlatform = platformL;
                        }
                        else if (collisionCheckerR.GetListOfClass<PlayerUnit>().Count >= 1)
                        {
                            targetPlatform = platformR;
                        }
                        break;
                }
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

    private void ShootToPlayer(float angleRange = 0f)
    {
        Vector2 temp = GetDist(GameManager.Player.transform.position + Vector3.up * 0.5f);
        float temp2 = Vector2.SignedAngle(Vector2.up, temp) * (isLookLeft ? 1 : -1);
        shooter.bulletAngleMax = temp2 + angleRange;
        shooter.bulletAngleMin = temp2 - angleRange;
        shooter.triger = true;
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
