using System;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    //�̱���
    private static ProjectileManager instance;
    public static ProjectileManager Instance => instance;

    private List<Projectile> projectiles;

    private void Awake()
    {
        //�̱���
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    private void Start()
    {
        projectiles = new();
    }

    /// <summary>
    /// ����ü ���
    /// </summary>
    /// <param name="projectile">����� ����ü</param>
    public static void Enroll(Projectile projectile)
    {
        if (!instance.projectiles.Contains(projectile))
        {
            instance.projectiles.Add(projectile);
        }
    }

    /// <summary>
    /// ����ü ��� ����
    /// </summary>
    /// <param name="projectile">��� ������ ����ü</param>
    public static void Remove(Projectile projectile)
    {
        if (instance.projectiles.Contains(projectile))
        {
            instance.projectiles.Remove(projectile);
        }
    }

    /// <summary>
    /// ���ٽ� ������ �����ϴ� ����ü ã��
    /// </summary>
    /// <param name="func">ã�� ����</param>
    /// <returns>������ �����ϴ� ��� ����ü</returns>
    public static List<Projectile> FindByFunc(Func<Projectile, bool> func, bool isPlayers = false)
    {
        List<Projectile> result = new();

        foreach (Projectile p in instance.projectiles)
        {
            if (isPlayers != (p.AttackUnit==GameManager.Player))
            {
                continue;
            }
            if (func(p))
            {
                result.Add(p);
            }
        }

        return result;
    }

    /// <summary>
    /// ��� ��ġ���� ���� �Ÿ� ���� �ִ� ���� ����� źȯ ã��
    /// </summary>
    /// <param name="distance">ã�� �Ÿ�</param>
    /// <param name="origin">ã�� ����</param>
    /// <returns>ã�� źȯ, ���ٸ� null</returns>
    public static Projectile FindByDistance(Vector2 origin, float distance, bool isPlayers = false)
    {
        //�ֺ��� �ִ� ��� ź ��������
        List<Projectile> list = FindByFunc((Projectile) =>
        {
            return GameTools.IsAround(origin, Projectile.transform.position, distance);
        }, 
        isPlayers);

        if (list.Count > 0)
        {
            //ã�� ��� ź �� ���� ����� �� ã��
            Projectile closest = list[0];
            float sqrDist = ((Vector2)closest.transform.position - origin).sqrMagnitude;

            for (int i = 1; i < list.Count; i++)
            {
                float temp = ((Vector2)list[i].transform.position - origin).sqrMagnitude;
                if (temp < sqrDist)
                {
                    closest = list[i];
                    sqrDist = temp;
                }
            }

            return closest;
        }
        else
        {
            //ã�� ź ���ٸ� null ��ȯ
            return null;
        }
    }
}
