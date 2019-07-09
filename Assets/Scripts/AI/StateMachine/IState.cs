using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for creating states
public interface IState
{
	void Enter();
	void Execute();
	void Exit();
}
