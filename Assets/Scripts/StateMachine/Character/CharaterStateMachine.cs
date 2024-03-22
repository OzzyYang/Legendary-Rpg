public class CharacterStateMachine
{
	public CharacterState currentState { get; private set; }


	public virtual void Initialize(CharacterState _startState)
	{
		currentState = _startState;
		currentState.Enter();
	}

	public virtual void ChangeState(CharacterState _newState)
	{
		currentState.Exit();
		currentState = _newState;
		currentState.Enter();
	}
}
