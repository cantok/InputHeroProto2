using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� �������� ��޵Ǵ� ���� ��ü
/// </summary>
public class Attack : MonoBehaviour
{
    /// <summary>
    /// �����ϴ� ����
    /// </summary>
    private Unit attackUnit;
    /// <summary>
    /// �����ϴ� ����
    /// </summary>
    public Unit AttackUnit
    {
        get
        {
            return attackUnit;
        }
    }
    private string targetTag;//���ظ� ���� ����� �±�
    private List<DamageArea> damageAreaList = new();//��ϵ� �����
    private List<Unit> damagedUnitList = new();//����� ���� ���� ����Ʈ
    private bool isInitialized = false;//�ʱ�ȭ �Ǿ��°�?
    
    public bool dealDamageOnEnter = true;//���� �� ������� �����°�?
    public bool isDestroySelfAuto = true;//������� ���� ������� �ı��Ǵ°�?

    protected virtual void Update()
    {
        //�ʱ�ȭ ������ �� ����
        if (!isInitialized)
        {
            return;
        }

        //isDestroySelfAuto Ȱ��ȭ �� ��ϵ� ��� ������� ������� �ı�
        if (isDestroySelfAuto && damageAreaList.Count == 0)
        {
            Destroy(gameObject);
        }

        //dealDamageOnEnter Ȱ��ȭ �� ������ �� �����
        if (dealDamageOnEnter)
        {
            foreach (var contactedUnit in GetContactedUnits())
            {
                if (!damagedUnitList.Contains(contactedUnit.Key))
                {
                    damagedUnitList.Add(contactedUnit.Key);
                    contactedUnit.Value.DealDamage(contactedUnit.Key);
                }
            }
        }
    }

    /// <summary>
    /// ���� ����. �ʱ�ȭ�� ���ÿ� �̷����.
    /// ��ũ��Ʈ���� ��ġ�� ������ �� �� ����ϸ� �� ��
    /// �ַ� ����ü �߻�
    /// </summary>
    /// <param name="unit">������ ����</param>
    /// <param name="targetTag">�ǰ� ����� �±�</param>
    /// <param name="parent">�θ� �� Transform. null�̸� �θ� ����</param>
    /// <param name="DamageAreaObjects">�ʱ⿡ ����� ������� ���� ������Ʈ</param>
    /// <returns>������ Attack</returns>
    public static Attack MakeGameObject(Unit unit, string targetTag, Transform parent = null, params GameObject[] DamageAreaObjects)
    {
        GameObject go = new GameObject();
        if (parent != null)
        {
            go.transform.parent = parent;
        }
        Attack result = go.AddComponent<Attack>();
        result.Initialization(unit, targetTag, DamageAreaObjects);
        return result;
    }

    /// <summary>
    /// �ʱ�ȭ
    /// ��� ���� �ݵ�� �ʱ�ȭ�Ͽ��� ��
    /// </summary>
    /// <param name="unit">������ ����</param>
    /// <param name="targetTag">�ǰ� ����� �±�</param>
    /// <param name="DamageAreaObjects">�ʱ⿡ ����� ������� ���� ������Ʈ</param>
    public void Initialization(Unit unit, string targetTag, params GameObject[] DamageAreaObjects)
    {
        attackUnit = unit;
        this.targetTag = targetTag;
        damageAreaList.Clear();
        damagedUnitList.Clear();
        foreach (var obj in DamageAreaObjects)
        {
            EnrollDamage(obj);
        }
        isInitialized = true;
    }

    /// <summary>
    /// ����� ���
    /// </summary>
    /// <param name="damageArea">����� �����</param>
    public void EnrollDamage(DamageArea damageArea)//����� �ϳ� ���
    {
        if (damageAreaList.Contains(damageArea))
        {
            return;
        }
        damageAreaList.Add(damageArea);
        damageArea.Source = this;
    }

    /// <summary>
    /// ���� Ȥ�� �ڽ��� ��� ����� ���
    /// </summary>
    /// <param name="go">������ �� ������Ʈ</param>
    public void EnrollDamage(GameObject go)//�� ���� ���� ����� ���
    {
        foreach (DamageArea damageArea in go.GetComponentsInChildren<DamageArea>())
        {
            EnrollDamage(damageArea);
        }
    }

    /// <summary>
    /// ����� ����
    /// </summary>
    /// <param name="damageArea">������ �����</param>
    public void WithdrawDamage(DamageArea damageArea)//����� ����
    {
        if (damageAreaList.Contains(damageArea))
        {
            damageAreaList.Remove(damageArea);
        }
    }

    /// <summary>
    /// ���� Ȥ�� �ڽ��� ��� ����� ����
    /// </summary>
    /// <param name="go">������ �� ������Ʈ</param>
    public void WithdrawDamage(GameObject go)
    {
        foreach (DamageArea damageArea in go.GetComponentsInChildren<DamageArea>())
        {
            WithdrawDamage(damageArea);
        }
    }

    /// <summary>
    /// ������ ���� Ž��
    /// ��ϵ� Damage�� ���� ���� ��� ������ �о, �켱���� ����Ͽ� ��� ������� ����� �� �˷���
    /// </summary>
    /// <returns>Key: ������ ����, Value: ������ DamageArea</returns>
    private Dictionary<Unit, DamageArea> GetContactedUnits()
    {
        Dictionary<Unit, DamageArea> dict = new();//�켱���� ����Ͽ�, ��� ���ֿ��� � ���� ������ �浹�� ������ ���� ���
        //dict ä���
        foreach (DamageArea damageArea in damageAreaList)//��� ���� ������ ����
        {
            List<Unit> unitList = new();//�ش� ���� ������ �浹 ���� ����
            foreach (HitBox hitBox in damageArea.HitBoxList)//�浹 ���� ��� ��Ʈ�ڽ��� ����
            {
                if (!unitList.Contains(hitBox.Unit) &&//ó�� �浹�� �����̸�
                    hitBox.CompareTag(targetTag))//��ǥ �����̸�
                {
                    unitList.Add(hitBox.Unit);//����Ѵ�
                }
            }

            foreach (Unit unit in unitList)//�浹 ���� ��� ���ֿ� ����
            {
                if (!dict.ContainsKey(unit))//�ش� ���� �浹�� ó���̶��
                {
                    dict.Add(unit, damageArea);//����Ѵ�
                }
                else//�� ��° ���Ķ��
                {
                    if (dict[unit] != damageArea &&//������ �ƴϰ�
                        dict[unit].Priority < damageArea.Priority)//�ڱ� �켱���� �� ���ٸ�
                    {
                        dict[unit] = damageArea;//��ü�Ѵ�
                    }
                }
            }
        }

        return dict;
    }

}