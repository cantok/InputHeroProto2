using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MoverByTransform : MonoBehaviour
{
    /// <summary>
    /// ���� ��ǥ�� ����ϴ°�?
    /// </summary>
    [SerializeField]
    private bool isSetByLocal = true;
    private Vector2 Position
    {
        get
        {
            if (isSetByLocal)
            {
                return transform.localPosition;
            }
            else
            {
                return transform.position;
            }
        }
        set
        {
            if (isSetByLocal)
            {
                transform.localPosition = value;
            }
            else
            {
                transform.position = value;
            }
        }
    }

    /// <summary>
    /// �̵� Ÿ��
    /// </summary>
    public enum moveType
    {
        LinearByPosWithTime,
        LinearByPosWithSpeed,
        LinearBySpeed,
    }

    private moveType type;

    //��ġ ��� �̵� ��
    //��ǥ ��ġ
    private Vector2 targetPos;
    private float targetMoveTime = 0;
    private Vector2 posOrigin = Vector2.zero;

    //�ӵ� ��� �̵� ��
    //��ǥ �ӵ�
    private Vector2 targetSpeed;
    private float targetSpeedF = 0;

    [SerializeField]
    private float moveTimer;
    [SerializeField]
    private bool isMoving = false;
    public bool IsMoving => isMoving;


    private void Update()
    {
        if (isMoving)
        {
            switch (type)
            {
                case moveType.LinearByPosWithTime:
                    MoveLinearByPosWithTime();
                    break;

                case moveType.LinearByPosWithSpeed:
                    MoveLinearByPosWithSpeed();
                    break;

                case moveType.LinearBySpeed:
                    MoveLinearBySpeed();
                    break;

                default:
                    break;
            }
        }

    }

    /// <summary>
    /// ������ ����
    /// </summary>
    public void StartMove(moveType type, Vector2 target, params float[] options)
    {
        moveTimer = 0;
        this.type = type;
        posOrigin = Position;
        isMoving = true;

        switch (type)
        {
            case moveType.LinearByPosWithTime:
                targetPos = target;
                targetMoveTime = Mathf.Max(options[0], 0);

                break;

            case moveType.LinearByPosWithSpeed:
                targetPos = target;
                targetSpeedF = options[0];
                break;

            case moveType.LinearBySpeed:
                targetSpeed = target;
                targetMoveTime = options[0];

                break;
            default:
                break;
        }
    }

    public void StopMove()
    {
        moveTimer = 0;
        isMoving = false;
    }

    private void MoveLinearByPosWithTime()
    {
        //Ÿ�̸� üũ
        moveTimer += Time.deltaTime;
        if (moveTimer >= targetMoveTime)
        {
            moveTimer = targetMoveTime;
            isMoving = false;
        }

        //�ð� ���� ���
        float Ilerp = Mathf.InverseLerp(0, targetMoveTime, moveTimer);

        //�ð� ������ ���� ��ǥ ���
        float moveX = Mathf.Lerp(posOrigin.x, targetPos.x, Ilerp);
        float moveY = Mathf.Lerp(posOrigin.y, targetPos.y, Ilerp);

        //�̵�
        Position = new Vector2(moveX, moveY);
    }
    private void MoveLinearByPosWithSpeed()
    {
        //Ÿ�̸� üũ
        moveTimer += Time.deltaTime;
        float targetDist = (targetPos - posOrigin).magnitude;//�̵��ؾ� �ϴ� �Ÿ�
        float targetTime = targetDist / targetSpeedF;//�̵��� �ɸ��� �ð�


        if (moveTimer >= targetTime)
        {
            Position = targetPos;
            isMoving = false;
            return;
        }

        //�ð� ���� ���
        float Ilerp = Mathf.InverseLerp(0, targetTime, moveTimer);

        //�ð� ������ ���� ��ǥ ���
        float moveX = Mathf.Lerp(posOrigin.x, targetPos.x, Ilerp);
        float moveY = Mathf.Lerp(posOrigin.y, targetPos.y, Ilerp);

        //�̵�
        Position = new Vector2(moveX, moveY);
    }
    private void MoveLinearBySpeed()
    {
        float deltaTime = Time.deltaTime;
        if (targetMoveTime > 0)
        {
            moveTimer += deltaTime;
            if (moveTimer >= targetMoveTime)
            {
                moveTimer = targetMoveTime;
                isMoving = false;
            }
        }

        Vector2 temp = Position;
        temp.x += deltaTime * targetSpeed.x;
        temp.y += deltaTime * targetSpeed.y;
        Position = temp;
    }
}
