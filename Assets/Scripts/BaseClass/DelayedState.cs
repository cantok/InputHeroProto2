using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��/�ĵ��� ������ �ð��� ���� ����Ǵ� ����. ���ϴ� ��ŭ �� ȿ���� �ݺ��� �� ����.
/// </summary>
public class DelayedState : State
{
    protected float earlyDelay;
    protected float mainDelay;
    protected float lateDelay;
    protected int repeatNum;
    protected int repeatCounter;
    protected TickTimer timer;
    protected DelayCondition delayCondition;

    public DelayedState(StateMachine machine) : base(machine)
    {
        timer = new();
        delayCondition = DelayCondition.idle;
    }

    public override void Enter()
    {
        timer.Reset();
        delayCondition = DelayCondition.early;
        repeatCounter = 0;
        OnEnterEarly();
    }

    public override void Execute()
    {
        switch (delayCondition)
        {
            case DelayCondition.early:
                if (timer.Check(earlyDelay))
                {
                    delayCondition = DelayCondition.main;
                    timer.Reset();
                    OnEnterMain();
                }
                break;
            case DelayCondition.main:
                if (timer.Check(mainDelay))
                {
                    repeatCounter++;
                    if (repeatCounter >= repeatNum)
                    {
                        delayCondition = DelayCondition.late;
                        timer.Reset();
                        OnEnterLate();
                    }
                    else
                    {
                        timer.Reset();
                        OnEnterMain();
                    }
                }
                else
                {
                    MainAct();
                }
                break;
            case DelayCondition.late:
                if (timer.Check(lateDelay))
                {
                    timer.Reset();
                    delayCondition = DelayCondition.idle;
                    OnEnterIdle();
                }
                break;
        }
    }

    protected virtual void OnEnterEarly()
    {

    }

    protected virtual void OnEnterMain()
    {

    }

    protected virtual void MainAct()
    {

    }

    protected virtual void OnEnterLate()
    {

    }


    protected virtual void OnEnterIdle()
    {

    }


    protected enum DelayCondition
    {
        early,
        main,
        late,
        idle
    }
}