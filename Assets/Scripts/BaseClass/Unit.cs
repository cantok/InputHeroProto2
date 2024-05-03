using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 유닛 공용으로 사용하는 부모 클래스
/// 능력치 및 대미지 적용 등을 관리
/// </summary>
public class Unit : MonoBehaviour, IMoveReceiver
{
    /// <summary>
    /// 유닛 아이디
    /// </summary>
    [SerializeField]
    protected int unitID = -1;
    /// <summary>
    /// 유닛 아이디
    /// </summary>
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
        //예외
        if (damage <= 0f)
        {
            return true;
        }

        //피해적용
        stats.health -= damage;

        //사망처리
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
    public bool IsKeyPushing(KeyCode keyCode)
    {
        return keyStay.ContainsKey(keyCode) && keyStay[keyCode];
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

    public void Turn(bool lookLeft)
    {
        if (isLookLeft!=lookLeft)
        {
            Turn();
        }
    }
}


[Serializable]
public struct Stats
{
    /// <summary>
    /// 현재 체력
    /// </summary>
    public float health;
    /// <summary>
    /// 최대 체력
    /// </summary>
    public float maxHealth;
    /// <summary>
    /// 공격력
    /// </summary>
    public float attackPower;
    /// <summary>
    /// 방어력
    /// </summary>
    public float defencePower;
    /// <summary>
    /// 방어율
    /// </summary>
    public float defenceRate;
    /// <summary>
    /// 이동속도
    /// </summary>
    public float moveSpeed;
    /// <summary>
    /// 점프력
    /// </summary>
    public float jumpPower;
    /// <summary>
    /// 쿨타임 배율
    /// </summary>
    public float cooldownRate;
    /// <summary>
    /// 점프 횟수
    /// </summary>
    public int jumpCount;


    /// <summary>
    /// 유닛 공용으로 사용하는 능력치 종류
    /// </summary>
    public enum StatType
    {
        /// <summary>
        /// 현재 체력
        /// </summary>
        health,
        /// <summary>
        /// 최대 체력
        /// </summary>
        maxHealth,
        /// <summary>
        /// 공격력
        /// </summary>
        attackPower,
        /// <summary>
        /// 방어력
        /// </summary>
        defencePower,
        /// <summary>
        /// 방어율
        /// </summary>
        defenceRate,
        /// <summary>
        /// 이동속도
        /// </summary>
        moveSpeed,
        /// <summary>
        /// 점프력
        /// </summary>
        jumpPower,
        /// <summary>
        /// 쿨타임 배율
        /// </summary>
        cooldownRate,
        /// <summary>
        /// 점프 횟수
        /// </summary>
        jumpCount,
    }
}