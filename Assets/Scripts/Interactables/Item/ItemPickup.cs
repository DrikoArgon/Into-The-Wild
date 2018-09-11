using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

	public Item item;
	public int amount = 1;

	public bool justDropped;
	public bool initialDropMotionActivated;
	public bool groundBumbMotionActivate;

	private Rigidbody2D myRigidBody;
	private float landingY;

	private float randomDropXLaunch;
	private float randomDropYLaunch;

	void Start(){
		myRigidBody = GetComponent<Rigidbody2D>();
		StartCoroutine(InitialDropLaunch());
	}

	void Update(){
		if(initialDropMotionActivated){
			if(transform.position.y <= landingY){
				initialDropMotionActivated = false;

				StartCoroutine(GroundBumpMotion());
			}
		}

		if(groundBumbMotionActivate){
			if(transform.position.y <= landingY){
				groundBumbMotionActivate = false;

				myRigidBody.velocity = Vector2.zero;
				myRigidBody.gravityScale = 0;
			}
		}
	}

	public void SetAsDroppedItem(){
		justDropped = true;
		StartCoroutine(SetJustDroppedOff());
	}

	void PickUp(){

		if(!justDropped){
			bool wasPickedUp = Inventory.instance.AddItem(item, amount);

			if(wasPickedUp){
				NotificationManager.instance.CreateNotificationOnPanel(item.icon, "Obtained " + item.itemName + ": " + amount);
				Destroy(gameObject);
			}
		}
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			PickUp();
		}
	}

	IEnumerator InitialDropLaunch(){


		float landingYModifier = Random.Range(-1f, 1f);

		landingY = transform.position.y + landingYModifier;

		randomDropXLaunch = Random.Range(-1f, 1f);
		randomDropYLaunch = Random.Range(3f, 4f);

		myRigidBody.gravityScale = 1;
		myRigidBody.AddForce(new Vector2(randomDropXLaunch, randomDropYLaunch), ForceMode2D.Impulse);

		yield return new WaitForSeconds(0.3f);

		initialDropMotionActivated = true;
	}

	IEnumerator GroundBumpMotion(){

		myRigidBody.velocity = Vector2.zero;
		myRigidBody.AddForce(new Vector2(randomDropXLaunch / 2.0f , randomDropYLaunch / 2.0f ), ForceMode2D.Impulse);

		yield return new WaitForSeconds(0.1f);

		groundBumbMotionActivate = true;
	}

	IEnumerator SetJustDroppedOff(){

		yield return new WaitForSeconds(3f);
		justDropped = false;
	}

}
