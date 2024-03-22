public class EnemyStateMachine : CharacterStateMachine
{

	public string currentAnimBoolName { get; protected set; }
	public override void ChangeState(CharacterState _newState)
	{
		base.ChangeState(_newState);
		currentAnimBoolName = _newState.animBoolName;
	}

	public override void Initialize(CharacterState _startState)
	{
		base.Initialize(_startState);
		currentAnimBoolName = _startState.animBoolName;
	}
}
