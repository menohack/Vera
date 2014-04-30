using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	public Animator animator;

	public void SetWalkSpeed(float speed)
	{
		if (Network.connections.Length > 0)
			networkView.RPC("SetWalkSpeedRPC", RPCMode.AllBuffered, speed);
		else
			SetWalkSpeedRPC(speed);
	}

	public void Attack()
	{
		if (Network.connections.Length > 0)
			networkView.RPC("AttackRPC", RPCMode.AllBuffered);
		else
			AttackRPC();
	}

	public bool GetAttacking()
	{
		return animator.GetCurrentAnimatorStateInfo(0).IsName("Attack");
	}

	public void Jump()
	{
		if (Network.connections.Length > 0)
			networkView.RPC("JumpRPC", RPCMode.AllBuffered);
		else
			JumpRPC();
	}

	[RPC]
	void JumpRPC()
	{
		animator.SetTrigger("Jump");
	}

	[RPC]
	void SetWalkSpeedRPC(float speed)
	{
		animator.SetFloat("Speed", speed);
	}

	[RPC]
	void AttackRPC()
	{
		animator.SetTrigger("Attack");
	}
}
