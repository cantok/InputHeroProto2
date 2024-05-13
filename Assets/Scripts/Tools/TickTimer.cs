using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ÿ�̸�. ���� Reset���κ��� ���� �̸� ����ߴ� �ð�(��) Ȥ�� ���ϴ� �ð�(��)�� �������� �˻��� �� ����
/// </summary>
public class TickTimer
{
    public float time;
    private float checkTime;
    private bool autoReset;

    public TickTimer(float checkTime = 1f, bool isTrigerInstant = false, bool autoReset = false)
    {
        this.checkTime = checkTime;
        if (isTrigerInstant)
        {
            time = float.MinValue;
        }
        else
        {
            Reset();
        }
        this.autoReset = autoReset;
    }

    public void Reset()
    {
        time = Time.time;
    }

    /// <summary>
    /// ������ ���ķ� �ش� �ð���ŭ ��������
    /// </summary>
    /// <param name="time">�ʿ��� ��� �ð�(s)</param>
    /// <returns>time��ŭ ����Ͽ��°�?</returns>
    public bool Check(float time)
    {
        PerformanceManager.StartTimer("TickTimer.Check");
        bool result = this.time + time <= Time.time;
        if (result && autoReset)
        {
            Reset();
        }
        PerformanceManager.StopTimer("TickTimer.Check");
        return result;
    }

    public bool Check()
    {
        return Check(checkTime);
    }
}
