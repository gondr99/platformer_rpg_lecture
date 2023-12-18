using System;
using System.Collections.Generic;


public class EnemyStateMachine<T> where T : Enum
{
    public EnemyState<T> CurrentState { get; private set; }
    public Dictionary<T, EnemyState<T>> StateDictionary = new Dictionary<T, EnemyState<T>>();

    public Enemy _enemyBase;

    public void Initialize(T startState, Enemy enemy)
    {
        _enemyBase = enemy;
        CurrentState = StateDictionary[startState];
        CurrentState.Enter();
    }

    public void ChangeState(T newState, bool forceMode = false)
    {
        if (!_enemyBase.CanStateChangeable && !forceMode) return;

        CurrentState.Exit();
        CurrentState = StateDictionary[newState];
        CurrentState.Enter();
    }

    public void AddState(T state, EnemyState<T> playerState)
    {
        StateDictionary.Add(state, playerState);
    }
}
