public class EnemyStateMachine
{
	public EnemyState currentState { get; private set; }
	protected EnemyController enemy;
	protected string aniBoolName;

	public void Initialize(EnemyState _startState)
	{
		currentState = _startState;
		currentState.Enter();
	}

	public void ChangeState(EnemyState _newState)
	{

		currentState.Exit();
		currentState = _newState;
		currentState.Enter();
	}

}
