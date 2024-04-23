using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    //�̱���
    private static UnitManager instance;
    public static UnitManager Instance => instance;

    private SerializedDictionary<int, Unit> unitList = new();
    private int lastUnitNum = 0;

    //�̱���
    private void Awake()
    {
        //�̱���
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    /// <summary>
    /// ���� ���
    /// </summary>
    /// <param name="unit">����Ϸ��� ����</param>
    /// <returns>�Ҵ�� ���� ID</returns>
    public int EnrollUnit(Unit unit)
    {
        lastUnitNum++;
        unitList.Add(lastUnitNum, unit);
        return lastUnitNum;
    }

    /// <summary>
    /// ���� ��� ����
    /// </summary>
    /// <param name="unit"></param>
    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit.UnitID);
    }

    /// <summary>
    /// ������ֱ�
    /// </summary>
    /// <param name="target">�޴� ���</param>
    /// <param name="source">�ִ� ���</param>
    /// <param name="damage">����� Ŭ����</param>
    /// <returns>��������</returns>
    public bool DamageUnitToUnit(Unit target, Unit source, DamageArea damageArea)
    {
        if (target == null || source == null || damageArea == null)
        {
            return true;
        }
        target.Damage(damageArea.damage);
        Debug.Log($"���� ����:{source.name}�� {target.name}����, {damageArea.damage} ����");
        return true;
        //�ӽ�
    }
}
