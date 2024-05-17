using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class PlayerUnit : Unit, IGroundChecker, IMoveReceiver
{
    [Header("����Ű")]
    //[SerializeField]
    //private KeyCode MoveL = KeyCode.A;
    //[SerializeField]
    //private KeyCode MoveR = KeyCode.D;
    //[SerializeField]
    //private KeyCode MoveU = KeyCode.W;
    //[SerializeField]
    //private KeyCode MoveD = KeyCode.S;
    //[SerializeField]
    //private KeyCode Jump = KeyCode.Space;
    //[SerializeField]
    //private KeyCode Attack = KeyCode.Z;
    //[SerializeField]
    //private KeyCode Attack2 = KeyCode.Mouse0;
    //[SerializeField]
    //private KeyCode Dash = KeyCode.LeftShift;
    //[SerializeField]
    //private KeyCode Slow = KeyCode.LeftShift;
    protected Dictionary<InputType, bool> keyStay = new();

    [Header("��Ÿ")]
    [SerializeField]
    private Animator animator;
    private int canJumpCounter;
    private bool isJumping = false;
    private bool canMove = true;
    [SerializeField, Range(1f, 5f)]
    private float dashSpeedRate = 2f;
    [SerializeField]
    private float dashTime = 1f;
    private bool isDashing = false;

    [SerializeField]
    protected Transform groundCheckerLT;
    [SerializeField]
    protected Transform groundCheckerRD;
    [SerializeField]
    private GameObject groundChecker;
    private Collider2D groundCheckerCollider;
    [SerializeField]
    protected float groundCheckRadius = 0;
    [SerializeField]
    protected string groundLayer = "";
    [SerializeField]
    protected string halfGroundLayer = "";
    [SerializeField]
    private GameObject targetter;

    [SerializeField]
    private BulletShooter shooter;
    [SerializeField]
    private float shootCooltime;
    private bool canShoot = true;
    private bool isDownJumping = false;
    private PlatformEffector2D effector2D;

    [SerializeField]
    private List<PlayerSkill> skillList;
    public List<PlayerSkill> SkillList => skillList;


    protected override void Start()
    {
        base.Start();
        effector2D = GetComponent<PlatformEffector2D>();
        if (groundChecker != null)
        {
            groundCheckerCollider = groundChecker.GetComponent<Collider2D>();
        }
        GameManager.SetPlayer(this);
        skillList = new List<PlayerSkill>
        {
            new PSkill_TestAreaAtk(),
            new PSkill_TestDash(),
            new PSkill_TestRangeAtk()
        };
    }

    protected override void Update()
    {
        PerformanceManager.StartTimer("PlayerUnit.Update");
        RotateTargetter();
        JumpCheck();
        if (canMove)
        {
            if (IsKeyPushing(InputType.MoveLeft))
            {
                MoverV.SetVelocityX(Mathf.Max(0, Speed) * -1);
                if (!isLookLeft)
                {
                    Turn();
                }
            }
            else if (IsKeyPushing(InputType.MoveRight))
            {
                MoverV.SetVelocityX(Mathf.Max(0, Speed));
                if (isLookLeft)
                {
                    Turn();
                }
            }
            else
            {
                MoverV.StopMoveX();
            }
        }
        if (IsKeyPushing(InputType.Shoot) && canShoot)
        {
            StartCoroutine(DoShoot());
        }

        animator.SetFloat("MoveSpeedRate", Mathf.Abs(MoverV.Velocity.x) / stats.moveSpeed);
        PerformanceManager.StopTimer("PlayerUnit.Update");
    }


    public void KeyDown(InputType inputType)
    {
        //�Է� �˻�
        if (keyStay.ContainsKey(inputType))
        {
            keyStay[inputType] = true;
        }
        else
        {
            keyStay.Add(inputType, true);
        }

        if (TimeManager.IsSlowed)
        {

        }

        //���
        if (inputType == InputType.Dash)
        {
            StartCoroutine(DoDash());
            return;
        }

        //����
        if (!isDownJumping && ((inputType == InputType.Jump && IsKeyPushing(InputType.MoveDown)) || (inputType == InputType.MoveDown && IsKeyPushing(InputType.Jump))))
        {
            SetHalfDownJump(true);
        }
        else if (inputType == InputType.Jump)
        {
            if (canJumpCounter > 0 && canMove)
            {
                Jump();
            }
        }

        //�ް���
        if (GroundCheck() == false && IsKeyPushing(InputType.MoveDown))
        {
            //�ް���
            MoverV.SetVelocityY(-MoverV.MaxSpeedY, true);
        }

        //����
        if (canMove && !animator.GetBool("IsJumping"))
        {
            if (inputType == InputType.MeleeAttack)
            {
                animator.Play("mixamo_com");
                canMove = false;
            }
        }
        if (inputType == InputType.Shoot && canShoot)
        {
            StartCoroutine(DoShoot());
        }

        //���ο�
        if (inputType == InputType.Slow && TimeManager.IsSlowed == false)
        {
            TimeManager.StartSlow();
        }
    }


    public void KeyUp(InputType inputType)
    {
        //�Է� �˻�
        if (keyStay.ContainsKey(inputType))
        {
            keyStay[inputType] = false;
        }
        else
        {
            keyStay.Add(inputType, false);
        }

        //����
        if (isDownJumping && (inputType == InputType.Jump || inputType == InputType.MoveDown))
        {
            SetHalfDownJump(false);
        }
    }

    public void KeyReset()
    {
        foreach (var item in keyStay)
        {
            KeyUp(item.Key);
        }
        keyStay.Clear();
    }

    //���콺 ���� ǥ�ñ� ȸ��(�ӽ�)
    private void RotateTargetter()
    {
        PerformanceManager.StartTimer("PlayerUnit.RotateTargetter");
        //Ÿ���� ȸ��

        //���콺 ��ġ �о����
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0;

        //�Ÿ� ���ϰ� ����
        Vector2 dir = (worldMousePos - (transform.position + (Vector3)Vector2.up * .5f)).normalized * 2;
        targetter.transform.position = (Vector3)dir + transform.position + (Vector3)Vector2.up * .5f;
        PerformanceManager.StopTimer("PlayerUnit.RotateTargetter");
    }

    //���콺 �������� �߻�(�ӽ�)
    private void ShootToMouse()
    {
        PerformanceManager.StartTimer("PlayerUnit.ShootToMouse");

        //�� ������ ���ؿ´�
        Vector2 dir = (Vector2)targetter.transform.position - ((Vector2)transform.position + Vector2.up * .5f);
        float angle = Vector2.SignedAngle(dir, Vector2.up) * -1;


        //������ ���
        shooter.BulletAngle = angle;
        shooter.Triger();
        PerformanceManager.StopTimer("PlayerUnit.ShootToMouse");
    }

    public void AttackEnd()
    {
        canMove = true;
    }

    //���� ���� üũ
    private void JumpCheck()
    {
        PerformanceManager.StartTimer("PlayerUnit.JumpCheck");
        if (isJumping && MoverV.Velocity.y <= -0.01f)
        {
            isJumping = false;
        }
        if (GroundCheck() && MoverV.Velocity.y <= 0.01f && !isJumping)
        {
            canJumpCounter = stats.jumpCount;
            animator.SetBool("IsJumping", false);
        }
        else if (!GroundCheck() && canJumpCounter == stats.jumpCount)
        {
            canJumpCounter = stats.jumpCount - 1;
        }
        PerformanceManager.StopTimer("PlayerUnit.JumpCheck");
    }

    //�� üũ
    public bool GroundCheck()
    {
        PerformanceManager.StartTimer("PlayerUnit.GroundCheck");

        if (groundChecker == null)
        {
            PerformanceManager.StopTimer("PlayerUnit.GroundCheck");
            return false;
        }

        int layer = LayerMask.GetMask(groundLayer, halfGroundLayer);
        if (groundCheckerCollider.IsTouchingLayers(layer))
        {
            PerformanceManager.StopTimer("PlayerUnit.GroundCheck");
            return true;
        }

        PerformanceManager.StopTimer("PlayerUnit.GroundCheck");
        return false;
    }

    //Ű�� ������ �ִ���
    private bool IsKeyPushing(InputType inputType)
    {
        return keyStay.ContainsKey(inputType) && keyStay[inputType];
    }


    //�Ʒ� ����
    private void SetHalfDownJump(bool isDownJumping)
    {
        int layerIndex = LayerMask.NameToLayer(halfGroundLayer);
        this.isDownJumping = isDownJumping;
        if (isDownJumping)
        {
            effector2D.colliderMask &= ~(1 << layerIndex);
        }
        else
        {
            effector2D.colliderMask |= (1 << layerIndex);
        }
    }

    //����
    private void Jump()
    {
        canJumpCounter--;
        MoverV.SetVelocityY(0, true);
        MoverV.AddForceY(JumpPower);
        isJumping = true;
        animator.SetBool("IsJumping", true);
    }
    //���
    private IEnumerator DoDash()
    {
        canMove = false;
        float speed = Mathf.Max(0, Speed) * dashSpeedRate * (isLookLeft ? -1 : 1);
        MoverV.SetVelocityX(speed);

        yield return new WaitForSeconds(dashTime);
        canMove = true;
    }


    //���Ÿ� ��Ÿ
    private IEnumerator DoShoot()
    {
        canShoot = false;
        ShootToMouse();
        yield return new WaitForSecondsRealtime(shootCooltime);
        canShoot = true;
    }
}
