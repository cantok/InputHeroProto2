/// <summary>
/// FSM���� �ϳ��� ���¸� ��Ÿ���� �⺻  Ŭ����
/// </summary>
public class State
{
    protected Unit unit;
    protected StateMachine machine;

    public State(StateMachine machine)
    {
        unit = machine.Unit;
        this.machine = machine;
    }

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void Exit() { }
}
