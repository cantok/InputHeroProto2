using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTools
{
    /// <summary>
    /// �� �� ���� ���̰� �־��� �Ÿ����� ������� üũ
    /// </summary>
    /// <param name="start">���� ��ǥ</param>
    /// <param name="end">�� ��ǥ</param>
    /// <param name="distance">üũ�� �Ÿ�</param>
    /// <returns>�־��� �Ÿ����� ����?</returns>
    public static bool IsInDistance(Vector2 start, Vector2 end, float distance)
    {
        PerformanceManager.StartTimer("GameTools.IsInDistance");
        Vector2 distanceVector = end - start;
        float sqrVector = distanceVector.sqrMagnitude;
        float sqrDistance = distance * distance;
        PerformanceManager.StopTimer("GameTools.IsInDistance");
        return sqrDistance > sqrVector;
    }
}

public delegate void Act();