using System.Collections.Generic;

public enum PlayerStateEnum
{
    Idle,
    Move,
    Jump,
    Fall,
    WallSlide,
    WallJump,
    Dash,
    PrimaryAttack,
    CounterAttack
}

public class PlayerStateMachine 
{
    public PlayerState CurrentState { get; private set; }
    public Dictionary<PlayerStateEnum, PlayerState> stateDictionary;

    private Player _player;

    public PlayerStateMachine()
    {
        stateDictionary = new Dictionary<PlayerStateEnum, PlayerState>();
    }

    //run once at first time!
    public void Initialize(PlayerStateEnum startState, Player player)
    {
        _player = player;
        CurrentState = stateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(PlayerStateEnum newState)
    {
        if (!_player.CanStateChangeable) return;

        CurrentState.Exit();
        CurrentState = stateDictionary[newState];
        CurrentState.Enter();
    }

    public void AddState(PlayerStateEnum state, PlayerState playerState)
    {
        stateDictionary.Add(state, playerState);
    }
}
