using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���� �������� ����ϴ� �θ� Ŭ����
/// �ɷ�ġ �� ����� ���� ���� ����
/// </summary>
public class Unit : MonoBehaviour, IMoveReceiver
{
    [SerializeField]
    protected int unitID = -1;
    public int UnitID
    {
        get
        {
            return unitID;
        }
        set
        {
            unitID = value;
        }
    }

    [SerializeField]
    protected Stats stats;
    public Stats Stats
    {
        get
        {
            return stats;
        }
        set
        {
            stats = value;
        }
    }


    public float Speed
    {
        get
        {
            return stats.moveSpeed;
        }
    }

    public float JumpPower
    {
        get
        {
            return stats.jumpPower;
        }
    }

    protected float movementX = 0;
    [SerializeField]
    protected Transform groundCheckerLT;
    [SerializeField]
    protected Transform groundCheckerRD;
    [SerializeField]
    protected float groundCheckRadius = 0;
    [SerializeField]
    protected string groundLayer = "";

    protected Dictionary<KeyCode, bool> keyStay = new();
    [SerializeField]
    protected bool isLookLeft = true;

    protected virtual void Start()
    {
        unitID = UnitManager.Instance.EnrollUnit(this);
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    public void Kill()
    {
        UnitManager.Instance.RemoveUnit(this);
        Destroy(gameObject);
    }

    public bool Damage(float damage)
    {
        //����
        if (damage <= 0f)
        {
            return true;
        }

        //��������
        stats.health -= damage;

        //���ó��
        if (stats.health > 0f)
        {
            return true;
        }
        Kill();
        return false;
    }

    public virtual void KeyDown(KeyCode keyCode)
    {
        if (keyStay.ContainsKey(keyCode))
        {
            keyStay[keyCode] = true;
        }
        else
        {
            keyStay.Add(keyCode, true);
        }
    }

    public virtual void KeyUp(KeyCode keyCode)
    {
        if (keyStay.ContainsKey(keyCode))
        {
            keyStay[keyCode] = false;
        }
        else
        {
            keyStay.Add(keyCode, false);
        }
    }

    public void KeyReset(KeyCode keyCode)
    {
        foreach (var item in keyStay)
        {
            KeyUp(item.Key);
        }
        keyStay.Clear();
    }

    public void Turn()
    {
        isLookLeft = !isLookLeft;
        transform.Rotate(new(0, 180, 0));
    }

    /// <summary>
    /// ���� �ִ��� üũ
    /// </summary>
    /// <returns>���� �ִ°�?</returns>
    protected bool GroundCheck()
    {
        if (groundCheckerLT == null || groundCheckerRD == null)
        {
            return false;
        }
        return Physics2D.OverlapArea(groundCheckerLT.position, groundCheckerRD.position, LayerMask.GetMask(groundLayer));
        //return Physics2D.OverlapCircle(groundChecker.transform.position, groundCheckRadius, LayerMask.GetMask("Ground")) ||
        //Physics2D.OverlapCircle(groundChecker2.transform.position, groundCheckRadius, LayerMask.GetMask("Ground"));
    }

    private void OnDrawGizmos()
    {
        if (groundCheckerLT != null && groundCheckerRD != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(groundCheckerLT.position.x, groundCheckerLT.position.y, 0), new Vector3(groundCheckerRD.position.x, groundCheckerLT.position.y, 0));
            Gizmos.DrawLine(new Vector3(groundCheckerLT.position.x, groundCheckerLT.position.y, 0), new Vector3(groundCheckerLT.position.x, groundCheckerRD.position.y, 0));
            Gizmos.DrawLine(new Vector3(groundCheckerRD.position.x, groundCheckerLT.position.y, 0), new Vector3(groundCheckerRD.position.x, groundCheckerRD.position.y, 0));
            Gizmos.DrawLine(new Vector3(groundCheckerLT.position.x, groundCheckerRD.position.y, 0), new Vector3(groundCheckerRD.position.x, groundCheckerRD.position.y, 0));
        }
    }
}


[Serializable]
public struct Stats
{
    /// <summary>
    /// ���� ü��
    /// </summary>
    public float health;
    /// <summary>
    /// �ִ� ü��
    /// </summary>
    public float maxHealth;
    /// <summary>
    /// ���� ����
    /// </summary>
    public float magicPoint;
    /// <summary>
    /// �ִ� ����
    /// </summary>
    public float maxMagicPoint;
    /// <summary>
    /// ���ݷ�
    /// </summary>
    public float attackPower;
    /// <summary>
    /// ����
    /// </summary>
    public float defencePower;
    /// <summary>
    /// �����
    /// </summary>
    public float defenceRate;
    /// <summary>
    /// �̵��ӵ�
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// ������
    /// </summary>
    public float jumpPower;
    /// <summary>
    /// ��Ÿ�� ����
    /// </summary>
    public float cooldownRate;
    public int jumpCount;


    /// <summary>
    /// ���� �������� ����ϴ� �ɷ�ġ ����
    /// </summary>
    public enum StatType
    {
        /// <summary>
        /// ���� ü��
        /// </summary>
        health,
        /// <summary>
        /// �ִ� ü��
        /// </summary>
        maxHealth,
        /// <summary>
        /// ���� ����
        /// </summary>
        magicPoint,
        /// <summary>
        /// �ִ� ����
        /// </summary>
        maxMagicPoint,
        /// <summary>
        /// ���ݷ�
        /// </summary>
        attackPower,
        /// <summary>
        /// ����
        /// </summary>
        defencePower,
        /// <summary>
        /// �����
        /// </summary>
        defenceRate,
        /// <summary>
        /// �̵��ӵ�
        /// </summary>
        moveSpeed,
        /// <summary>
        /// ������
        /// </summary>
        jumpPower,
        /// <summary>
        /// ��Ÿ�� ����
        /// </summary>
        cooldownRate,
        jumpCount,
    }
}