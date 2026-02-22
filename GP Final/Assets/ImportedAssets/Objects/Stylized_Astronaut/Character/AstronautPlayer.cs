using UnityEngine;
using System.Collections;

namespace AstronautPlayer
{

	public class AstronautPlayer : MonoBehaviour {

		private Animator anim;
		private CharacterController controller;

		public float speed = 600.0f;
		public float turnSpeed = 400.0f;
		private Vector3 moveDirection = Vector3.zero;
		public float gravity = 20.0f;
		public float jumpForce = 8.0f;
		private bool hasDoubleJumped = false;

		void Start () {
			controller = GetComponent <CharacterController>();
			anim = gameObject.GetComponentInChildren<Animator>();
		}

		void Update (){
			float vertical = Input.GetAxisRaw("Vertical");
			float horizontal = Input.GetAxisRaw("Horizontal");

			if (vertical != 0 || horizontal != 0) {
				anim.SetInteger ("AnimationPar", 1);
			} else {
				anim.SetInteger ("AnimationPar", 0);
			}

			float ySpeed = moveDirection.y;

			Vector3 forward = transform.forward * vertical;
			Vector3 right = transform.right * horizontal;
			moveDirection = (forward + right).normalized * speed;

			if(controller.isGrounded){
				hasDoubleJumped = false;
			}

			if(controller.isGrounded && ySpeed < 0){
				ySpeed = -2f;
			}

			if(Input.GetKeyDown(KeyCode.Space)){
				if(controller.isGrounded){
					ySpeed = jumpForce;
				} else if(!hasDoubleJumped){
					ySpeed = jumpForce;
					hasDoubleJumped = true;
				}
			}

			ySpeed -= gravity * Time.deltaTime;
			moveDirection.y = ySpeed;

			controller.Move(moveDirection * Time.deltaTime);

			//Resets character position if it falls
			if (transform.position.y < -15)
			{
				transform.position = Vector3.zero;
			}
		}
	}
}