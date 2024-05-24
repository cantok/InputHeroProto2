using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTools
{
    #region �Ÿ� ��
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
    #endregion

    #region ����Ʈ ��
    /// <summary>
    /// �־��� �� ����Ʈ�� ������ ������ üũ
    /// </summary>
    /// <typeparam name="T">Ÿ��</typeparam>
    /// <returns>������?</returns>
    public static bool CompareList<T>(List<T> listA, List<T> listB) where T : class
    {
        if (listA.Count != listB.Count)
        {
            return false;
        }

        for (int i = 0; i < listA.Count; i++)
        {
            if (listA[i] != listB[i])
            {
                return false;
            }
        }
        return true;
    }
    public static bool CompareEnumList<T>(List<T> listA, List<T> listB) where T : Enum
    {
        if (listA.Count != listB.Count)
        {
            return false;
        }

        for (int i = 0; i < listA.Count; i++)
        {
            if (listA[i].Equals(listB[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CompareList<T>(List<T> list, T[] array) where T : class
    {
        if (list.Count != array.Length)
        {
            return false;
        }

        T[] temp = list.ToArray();

        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i] != array[i])
            {
                return false;
            }
        }
        return true;
    }
    public static bool CompareEnumList<T>(List<T> list, T[] array) where T : Enum
    {
        if (list.Count != array.Length)
        {
            return false;
        }

        T[] temp = list.ToArray();

        for (int i = 0; i < temp.Length; i++)
        {
            if (!temp[i].Equals(array[i]))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CompareList<T>(T[] arrayA, T[] arrayB) where T : class
    {
        if (arrayA.Length != arrayB.Length)
        {
            return false;
        }

        for (int i = 0; i < arrayA.Length; i++)
        {
            if (arrayA[i] != arrayB[i])
            {
                return false;
            }
        }
        return true;
    }
    public static bool CompareEnumList<T>(T[] arrayA, T[] arrayB) where T : Enum
    {
        if (arrayA.Length != arrayB.Length)
        {
            return false;
        }

        for (int i = 0; i < arrayA.Length; i++)
        {
            if (!arrayA[i].Equals(arrayB[i]))
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region �׷���

    /// <summary>
    /// Ư�� ������ �������� ���̴� �׷���
    /// </summary>
    /// <param name="graph">key ������� ���ĵ� �׷���</param>
    /// <param name="delta">��</param>
    /// <returns></returns>
    public static float GetNonlinearGraph(Dictionary<float, float> graph, float delta)
    {
        //����ó��
        if (graph == null || graph.Count <= 0)
        {
            Debug.LogError("GetNonlinearGraph: �߸��� �׷��� ����");
            return 0f;
        }

        //�迭 ���� ����(���� ȣ���ؾ� �ϹǷ� ���ɻ� ���� ����). �����ؼ� �־��ֱ�

        //��� ���
        var tempArr = graph.Keys.ToArray();//graph�� Ű �迭

        if (graph.Count == 1 || delta <= tempArr[0])//�ּ�ġ���� ������ or 1����
        {
            return graph[tempArr[0]];//�ּ�ġ ��ȯ
        }
        else if (delta >= tempArr[tempArr.Length - 1])//�ִ�ġ���� ������
        {
            return graph[tempArr[tempArr.Length - 1]];//�ִ�ġ ��ȯ
        }
        else//�� ���̸�
        {
            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                float aKey = tempArr[i];
                float aValue = graph[aKey];
                float bKey = tempArr[i + 1];
                float bValue = graph[bKey];

                if (aKey < delta && delta <= bKey)
                {
                    float rate = Mathf.InverseLerp(aKey, bKey, delta);
                    Debug.Log($"aK:{aKey}, aV:{aValue}, bK:{bKey}, bV:{bValue}, del:{delta}, rate:{rate}");
                    return Mathf.Lerp(aValue, bValue, rate);
                }
            }
        }

        return 0f;
    }

    public static float GetNonlinearGraph(Dictionary<float, Func<float, float>> graph, float delta)
    {
        //����ó��
        if (graph == null || graph.Count <= 0)
        {
            Debug.LogError("GetNonlinearGraph: �߸��� �׷��� ����");
            return 0f;
        }

        //��� ���
        var tempArr = graph.Keys.ToArray();//graph�� Ű �迭

        if (graph.Count == 1 || delta <= tempArr[0])//�ּ�ġ���� ������ or 1����
        {
            return graph[tempArr[0]](delta);//�ּ�ġ ��ȯ
        }
        else if (delta >= tempArr[tempArr.Length - 1])//�ִ�ġ���� ������
        {
            return graph[tempArr[tempArr.Length - 1]](delta);//�ִ�ġ ��ȯ
        }
        else//�� ���̸�
        {
            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                float aKey = tempArr[i];
                float aValue = graph[aKey](delta);
                float bKey = tempArr[i + 1];
                float bValue = graph[bKey](delta);
                //���� üũ�غ��� ������ ��
                if (aKey < delta && delta <= bKey)
                {
                    float rate = Mathf.InverseLerp(aKey, bKey, delta);
                    Debug.Log($"aK:{aKey}, aV:{aValue}, bK:{bKey}, bV:{bValue}, del:{delta}, rate:{rate}");
                    return Mathf.Lerp(aValue, bValue, rate);
                }
            }
        }

        return 0f;
    }

    /// <summary>
    /// Ư�� ������ �������� ���̴� �׷���
    /// </summary>
    /// <param name="graph">key ������� ���ĵ� �׷���</param>
    /// <param name="delta">��</param>
    /// <returns></returns>
    public static Vector2 GetNonlinearGraph(Dictionary<float, Vector2> graph, float delta)
    {
        //����ó��
        if (graph == null || graph.Count <= 0)
        {
            Debug.LogError("GetNonlinearGraph: �߸��� �׷��� ����");
            return Vector2.zero;
        }


        //��� ���
        var tempArr = graph.Keys.ToArray();//graph�� Ű �迭

        if (graph.Count == 1 || delta <= tempArr[0])//�ּ�ġ���� ������ or 1����
        {
            return graph[tempArr[0]];//�ּ�ġ ��ȯ
        }
        else if (delta >= tempArr[tempArr.Length - 1])//�ִ�ġ���� ������
        {
            return graph[tempArr[tempArr.Length - 1]];//�ִ�ġ ��ȯ
        }
        else//�� ���̸�
        {
            for (int i = 0; i < tempArr.Length - 1; i++)
            {
                float aKey = tempArr[i];
                Vector2 aValue = graph[aKey];
                float bKey = tempArr[i + 1];
                Vector2 bValue = graph[bKey];

                if (aKey < delta && delta <= bKey)
                {
                    float rate = Mathf.InverseLerp(aKey, bKey, delta);
                    Debug.Log($"aK:{aKey}, aV:{aValue}, bK:{bKey}, bV:{bValue}, del:{delta}, rate:{rate}");
                    return Vector2.Lerp(aValue, bValue, rate);
                }
            }
        }

        return Vector2.zero;
    }

    #endregion
}
