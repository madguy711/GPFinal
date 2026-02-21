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

			if(controller.isGrounded){
				Vector3 forward = transform.forward * vertical;
				Vector3 right = transform.right * horizontal;
				moveDirection = (forward + right).normalized * speed;
			}

			if(controller.isGrounded && ySpeed < 0){
				ySpeed = -2f;
			}

			ySpeed -= gravity * Time.deltaTime;
			moveDirection.y = ySpeed;

			controller.Move(moveDirection * Time.deltaTime);
		}
	}
}