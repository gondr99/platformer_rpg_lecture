
public class DBCastState : EnemyState<DeathBringerStateEnum>
{
    public DBCastState(Enemy enemyBase, EnemyStateMachine<DeathBringerStateEnum> stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
    }
}
