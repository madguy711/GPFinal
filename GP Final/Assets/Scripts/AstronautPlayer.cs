using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AstronautPlayer
{

	public class AstronautPlayer : MonoBehaviour {

		private Animator anim;
		private CharacterController controller;

		[Header("Player Settings")]
		public float speed = 600.0f;
		public float turnSpeed = 400.0f;
		private Vector3 moveDirection = Vector3.zero;
		public float gravity = 20.0f;
		public float jumpForce = 8.0f;
		
		[Header("Coyote Time")]
		public float coyoteTime = 0.15f;        // small grace period after not touching the ground where a jump still counts as grounded
		private float coyoteTimeRemaining = 0f;

		private bool hasDoubleJumped = false;

		[Header("Dash Settings")]
		public float dashSpeed = 40.0f;
		public float dashDuration = 0.2f;
		public float dashCooldown = 1.0f;
		private bool isDashing = false;
		private float dashTimeRemaining = 0f;
		private float dashCooldownRemaining = 0f;
		private Vector3 dashDirection = Vector3.zero;
		public ParticleSystem speedLines;
		public Slider dashCooldownSlider;

		public AudioSource dashSFX;
		public AudioSource jumpSFX;
		public Image doubleJumpIndicator;          
		public Color doubleJumpAvailableColor = Color.green;
		public Color doubleJumpUsedColor = Color.white;
		

		void Start () {
			controller = GetComponent <CharacterController>();
			anim = gameObject.GetComponentInChildren<Animator>();
		}

		void Update (){
			// Dash cooldown
			if (dashCooldownRemaining > 0f)
				dashCooldownRemaining -= Time.deltaTime;
			
			// Update the cooldown slider 
			if (dashCooldownSlider)
			{
				dashCooldownSlider.value = 1f - (dashCooldownRemaining / dashCooldown);
			}

			// Update the double jump indicator
			if (doubleJumpIndicator)
			{
				if (hasDoubleJumped)
				{
					doubleJumpIndicator.color = doubleJumpUsedColor;
				}
				else
				{
					doubleJumpIndicator.color = doubleJumpAvailableColor;
				}
			}

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
			Vector3 inputDirection = (forward + right).normalized;

			if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownRemaining <= 0f)
			{
				// Dash in movement direction, or forward if standing still
				if (inputDirection.magnitude > 0.1f)
				{
					dashDirection = inputDirection;
				}
    			else
				{
					dashDirection = transform.forward;
				}
				dashDirection.y = 0f;
				dashDirection.Normalize();

				isDashing = true;
				dashTimeRemaining = dashDuration;
				dashCooldownRemaining = dashCooldown;
				dashSFX.Play();
				speedLines.Play();
			}

			if (isDashing)
			{
				dashTimeRemaining -= Time.deltaTime;
				if (dashTimeRemaining <= 0f)
				{
					isDashing = false;
				}

				// Override movement with dash to preserve gravity
				moveDirection = dashDirection * dashSpeed;
				moveDirection.y = ySpeed - gravity * Time.deltaTime;
				controller.Move(moveDirection * Time.deltaTime);
			}
			else
			{
				moveDirection = inputDirection * speed;

				if(controller.isGrounded)
				{
					hasDoubleJumped = false;
					coyoteTimeRemaining = coyoteTime;   // refresh the grace timer when grounded again
				}
				else
				{
					coyoteTimeRemaining -= Time.deltaTime;
				}

				if(controller.isGrounded && ySpeed < 0)
				{
					ySpeed = -2f;
				}

				if(Input.GetKeyDown(KeyCode.Space)){
					if(controller.isGrounded || coyoteTimeRemaining > 0f){
						// Ground jump (or coyote-time jump just after walking off)
						ySpeed = jumpForce;
						coyoteTimeRemaining = 0f;   // Consume the grace so it can't be reused
					} 
					else if(!hasDoubleJumped)
					{
						ySpeed = jumpForce;
						jumpSFX.Play();
						hasDoubleJumped = true;
					}
				}

				ySpeed -= gravity * Time.deltaTime;
				moveDirection.y = ySpeed;

				controller.Move(moveDirection * Time.deltaTime);
			}

			// Resets character position if it falls
			if (transform.position.y < -15)
			{
				RestartLevel();
			}
		}

		// Restart the level if the player gets shot
		void OnCollisionEnter(Collision collision)
		{
			if (collision.transform.CompareTag("EnemyBullet"))
			{
				RestartLevel();
			}
		}

		// Reload the current scene to restart the level
		void RestartLevel()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}
}