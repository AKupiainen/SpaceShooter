using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
	private IState currentState;
	private IState previousState;

	/// <summary>
	/// Changes state and runs exit/enter methods
	/// </summary>
	/// <param name="newState"></param>
	public void ChangeState(IState newState)
	{
		if(CurrentState != null)
		{
			CurrentState.Exit();
		}

		PreviousState = CurrentState;
		CurrentState = newState;

		CurrentState.Enter();
	}

	/// <summary>
	/// Updates the state
	/// </summary>
	public void UpdateState()
	{
		if(CurrentState != null)
		{
			CurrentState.Execute();
		}
	}

	/// <summary>
	/// Swtiches back to previous state
	/// </summary>
	public void SwitchToPreviousState()
	{
		CurrentState.Exit();
		CurrentState = PreviousState;
		CurrentState.Enter();
	}

	public IState CurrentState
	{
		get
		{
			return currentState;
		}

		set
		{
			currentState = value;
		}
	}

	public IState PreviousState
	{
		get
		{
			return previousState;
		}

		set
		{
			previousState = value;
		}
	}
}