using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectileMotion : MonoBehaviour
{
    public Vector2 targetPosition; // ���� ��ġ
    public float gravity = 9.81f;   // �߷� ���ӵ�
    public float time = 1f;

    private Vector2 initialPosition; // �ʱ� ��ġ
    private Vector2 initialVelocity; // �ʱ� �ӵ� ����

    void Start()
    {
        // �ʱ� ��ġ�� �����մϴ�.
        initialPosition = transform.position;

        // �ʱ� �ӵ� ���͸� ����մϴ�.
        CalculateInitialVelocity();
    }

    void CalculateInitialVelocity()
    {
        // �ʱ� ��ġ�� ���� ��ġ ������ �Ÿ��� ����մϴ�.
        Vector2 displacement = targetPosition - initialPosition;

        // ������ ����� ���� ���� �ӵ��� ������ �����Ƿ� �ʱ� �ӵ� ������ x ������ displacement�� x �����Դϴ�.
        initialVelocity.x = displacement.x / time;

        // ������ ����� ���� ���� �ӵ��� �߷��� ������ �����Ƿ� �ʱ� �ӵ� ������ y ������ ���� �������� �̵��ϱ� ���� �ʱ� �ӵ��Դϴ�.
        // �ʱ� �ӵ��� ����ϴ� ������ ����Ͽ� y ������ ����մϴ�.
        initialVelocity.y = (displacement.y - 0.5f * gravity * Mathf.Pow(displacement.x / initialVelocity.x, 2)) / (displacement.x / initialVelocity.x) / time;
    }

    void Update()
    {
        // ���� �ð��� �����ɴϴ�.
        float t = Time.time;

        // ������ ��� ��ġ�� ����մϴ�.
        Vector2 position = initialVelocity * t + 0.5f * new Vector2(0, -gravity) * t * t + initialPosition;

        // ������Ʈ�� ���� ��ġ�� �̵���ŵ�ϴ�.
        transform.position = position;
    }
}
