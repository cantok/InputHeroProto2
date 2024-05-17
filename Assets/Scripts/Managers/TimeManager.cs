using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    //�̱���
    private static TimeManager instance;
    public static TimeManager Instance => instance;

    //���� ������
    private const int framePerSec = 50;

    //�ð� ���� �� �̺�Ʈ
    private static event EventHandler<float> onTimeScaleChanged;
    public static event EventHandler<float> OnTimeScaleChanged
    {
        add { onTimeScaleChanged += value; }
        remove { onTimeScaleChanged -= value; }
    }

    //���ο�
    [SerializeField]
    private float slowRate = .5f;

    [SerializeField]
    private float slowTime = 4f;

    private bool isSlowed = false;
    public static bool IsSlowed => instance.isSlowed;
    private bool isUsingSkills = false;
    public static bool IsUsingSkills => instance.isUsingSkills;


    private TickTimer slowTimer;
    [SerializeField]
    private Slider slowSlider;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
        Time.fixedDeltaTime = Time.timeScale / framePerSec;
        slowTimer = new(unscaledTime: true);
    }

    private void Update()
    {
        if (isSlowed && !isUsingSkills)
        {
            float remainTime = slowTimer.GetRemain(slowTime);
            slowSlider.value = remainTime / instance.slowTime;
            if (remainTime <= 0)
            {
                StartCoroutine(StartSkillQueue());
            }
        }
    }

    public static void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = Time.timeScale / framePerSec;
        onTimeScaleChanged?.Invoke(instance, Time.timeScale);
    }

    public static void StartSlow()
    {
        instance.isSlowed = true;
        SetTimeScale(instance.slowRate);
        instance.slowTimer.Reset();
    }

    private IEnumerator StartSkillQueue()
    {
        isUsingSkills = true;

        //��ų �����
        ComboManager.
        yield return null;


        isUsingSkills = false;
    }

    public static void EndSlow()
    {
        SetTimeScale(1);
        instance.isSlowed = false;
    }
}
