using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {


	public float speed;
	private float currentSpeed;

	public KeyCode moveLeftKey;
	public KeyCode moveRightKey;
	public KeyCode moveUpKey;
	public KeyCode moveDownKey;

	public bool movingUp;
	public bool movingDown;
	public bool movingLeft;
	public bool movingRight;

	public bool diagonal;

	private Rigidbody2D myRigidBody;
	private Animator myAnimator;

	public PlayerDirection playerDirection = PlayerDirection.Down;
	public bool walking;


	void Start () {
		myRigidBody = GetComponent<Rigidbody2D>();
		myAnimator = GetComponent<Animator>();

		currentSpeed = speed;
	}
	
	// Update is called once per frame
	// Inputs should be registered here
	void Update () {

		if(GameManager.instance.uiActive){
			return;
		}

		if(Input.GetKey(moveLeftKey)){
			movingLeft = true;
			movingRight = false;
		}else{
			movingLeft = false;
		}

		if(Input.GetKey(moveRightKey)){
			movingRight = true;
			movingLeft = false;
		}else{
			movingRight = false;
		}

		if(Input.GetKey(moveUpKey)){
			movingUp = true;
			movingDown = false;
		}else{
			movingUp = false;
		}

		if(Input.GetKey(moveDownKey)){
			movingDown = true;
			movingUp = false;
		}else{
			movingDown = false;
		}

		if(movingRight && (movingUp || movingDown)){ //The player is moving diagonaly
			diagonal = true;
		}else if(movingLeft && (movingUp || movingDown)){ //The player is moving diagonaly
			diagonal = true;
		}else{
			diagonal = false;
		}


	}

	//Movement should occur here
	void FixedUpdate(){

		if(GameManager.instance.uiActive){
			if(movingLeft && movingRight && movingUp && movingDown){
				Idle();
			}
			return;
		}

		if(movingUp){
			MoveUp();
		}

		if(movingDown){
			MoveDown();
		}

		if(movingLeft){
			MoveLeft();
		}

		if(movingRight){
			MoveRight();
		}

		if(diagonal){
			currentSpeed = speed / 1.5f;
		}else{
			currentSpeed = speed;
		}

		if(!movingLeft && !movingRight && !movingUp && !movingDown){
			Idle();
		}
	}

	void MoveLeft(){

		if(!walking){
			myAnimator.Play("Walking");
			walking = true;
		}

		myRigidBody.transform.position += Vector3.left * currentSpeed * Time.deltaTime;
		myAnimator.SetFloat("directionHorizontal", -1);
		myAnimator.SetFloat("directionVertical", 0);

		if(playerDirection != PlayerDirection.Left){

			playerDirection = PlayerDirection.Left;

		}


	}

	void MoveRight(){

		if(!walking){
			myAnimator.Play("Walking");
			walking = true;
		}

		myRigidBody.transform.position += Vector3.right * currentSpeed * Time.deltaTime;
		myAnimator.SetFloat("directionHorizontal", 1);
		myAnimator.SetFloat("directionVertical", 0);

		if(playerDirection != PlayerDirection.Right){

			playerDirection = PlayerDirection.Right;

		}
	}

	void MoveUp(){

		if(!walking){
			myAnimator.Play("Walking");
			walking = true;
		}

		myRigidBody.transform.position += Vector3.up * currentSpeed * Time.deltaTime;
		myAnimator.SetFloat("directionHorizontal", 0);
		myAnimator.SetFloat("directionVertical", 1);

		if(playerDirection != PlayerDirection.Up && !diagonal){

			playerDirection = PlayerDirection.Up;

		}
	}

	void MoveDown(){

		if(!walking){
			myAnimator.Play("Walking");
			walking = true;
		}

		myRigidBody.transform.position += Vector3.down * currentSpeed * Time.deltaTime;
		myAnimator.SetFloat("directionHorizontal", 0);
		myAnimator.SetFloat("directionVertical", -1);

		if(playerDirection != PlayerDirection.Down && !diagonal){

			playerDirection = PlayerDirection.Down;

		}
	}

	void Idle(){

		myAnimator.Play("Idle");
		walking = false;
	
	}
}

public enum PlayerDirection{
	Up,
	Down,
	Left,
	Right
}
