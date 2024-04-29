using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Rigidbody2D rb;
    private float originGravity;

    //��ǥ �ӵ�
    private float targetSpeedX = 0;
    private float targetSpeedY = 0;

    //�ִ� �ӵ�
    [SerializeField]
    private float maxSpeedX = 0;
    public float MaxSpeedX
    { get { return maxSpeedX; } set { maxSpeedX = value; } }

    [SerializeField]
    private float maxSpeedY = 0;
    public float MaxSpeedY
    { get { return maxSpeedY; } set { maxSpeedY = value; } }

    //�ӵ� ����
    private bool fixSpeedX = false;
    private bool fixSpeedY = false;

    public Vector2 Velocity
    {
        get { return rb.velocity; }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originGravity = rb.gravityScale;
    }

    private void FixedUpdate()
    {
        if (fixSpeedX)
            SetVelocityX();
        if (fixSpeedY)
            SetVelocityY();
        MaxSpeedCheck();
    }

    private void MaxSpeedCheck()
    {
        if (maxSpeedX != 0)
        {
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeedX, maxSpeedX), rb.velocity.y);
        }
        if (maxSpeedY != 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -maxSpeedY, maxSpeedY));
        }
    }

    /// <summary>
    /// �ӵ� �缳��(X�ุ)
    /// </summary>
    private void SetVelocityX()
    {
        rb.velocity = new Vector2(targetSpeedX, rb.velocity.y);
        MaxSpeedCheck();
    }

    /// <summary>
    /// �ӵ� �缳��(X�ุ)
    /// </summary>
    /// <param name="speed">������ �ӵ�</param>
    public void SetVelocityX(float speed, bool once = false)
    {
        targetSpeedX = speed;
        SetVelocityX();
        if (!once)
        {
            fixSpeedX = true;
        }
    }

    /// <summary>
    /// �̵� ����(X�ุ)
    /// </summary>
    public void StopMoveX(bool instant = true)
    {
        fixSpeedX = false;
        targetSpeedX = 0;
        if (instant)
        {
            SetVelocityX();
        }
    }

    /// <summary>
    /// �ӵ� �缳��(Y�ุ)
    /// </summary>
    private void SetVelocityY()
    {
        rb.velocity = new Vector2(rb.velocity.x, targetSpeedY);
        MaxSpeedCheck();
    }

    /// <summary>
    /// �ӵ� �缳��(Y�ุ)
    /// </summary>
    /// <param name="speed">������ �ӵ�</param>
    public void SetVelocityY(float speed, bool once = false)
    {
        targetSpeedY = speed;
        SetVelocityY();
        if (!once)
        {
            fixSpeedY = true;
        }
    }

    /// <summary>
    /// �̵� ����(Y�ุ)
    /// </summary>
    public void StopMoveY(bool instant = true)
    {
        fixSpeedY = false;
        targetSpeedY = 0;
        if (instant)
        {
            SetVelocityY();
        }
    }

    /// <summary>
    /// �ӵ� �缳��
    /// </summary>
    /// <param name="velocity">�缳���� �ӵ�</param>
    public void SetVelocity(Vector2 velocity, bool once = false)
    {
        SetVelocityX(velocity.x, once);
        SetVelocityY(velocity.y, once);
    }

    /// <summary>
    /// �ӵ� �缳��
    /// </summary>
    /// <param name="velocity">�缳���� �ӵ�</param>
    public void SetVelocity(Vector3 velocity)
    {
        SetVelocity((Vector2)velocity);
    }

    /// <summary>
    /// �̵� ����
    /// </summary>
    public void StopMove(bool instant = true)
    {
        fixSpeedX = false;
        fixSpeedY = false;
        targetSpeedX = 0;
        targetSpeedY = 0;
        if (instant)
        {
            SetVelocity(Vector2.zero);
        }
    }

    /// <summary>
    /// AddForce ��ü
    /// </summary>
    /// <param name="force">���� ��(�� ����)</param>
    public void AddForceX(float force)
    {
        fixSpeedX = true;
        rb.AddForce(Vector2.right * force);
    }

    /// <summary>
    /// AddForce ��ü
    /// </summary>
    /// <param name="force">���� ��(������ ����)</param>
    public void AddForceY(float force)
    {
        fixSpeedY = false;
        rb.AddForce(Vector2.up * force);
    }

    /// <summary>
    /// AddForce ��ü
    /// </summary>
    /// <param name="force">���� ��</param>
    public void AddForce(Vector2 force)
    {
        fixSpeedX = false;
        fixSpeedY = false;
        rb.AddForce(force);
    }

    /// <summary>
    /// �߷� ��ȯ
    /// </summary>
    /// <param name="isOn">�߷��� �� ���ΰ�?</param>
    public void TurnGravity(bool isOn)
    {
        if (isOn)
        {
            rb.gravityScale = originGravity;
        }
        else
        {
            rb.gravityScale = 0;
        }
    }
}

public struct Move
{
    public float x;
    public bool isMoveX;
    public float y;
    public bool isMoveY;
    public float lifeTime;
    public bool isAdded;

    public Move(Vector2 move, bool isMoveX = true, bool isMoveY = true, float lifeTime = -1, bool isAdded = false)
    {
        x = move.x;
        y = move.y;
        this.isMoveX = isMoveX;
        this.isMoveY = isMoveY;
        this.isAdded = isAdded;

        this.lifeTime = lifeTime;
    }
}