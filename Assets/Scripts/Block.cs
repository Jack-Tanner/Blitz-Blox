using UnityEngine;
using System.Collections;
using System;

public class Block : MonoBehaviour {

	public Transform block;	

	public string colour = "";
	
	//particle effect stuff. Tom might kill me if I fuck with it.
	public GameObject emitter;


	float _speed; //speed of the block
	Vector3 firingVector; //block translational direction

	public bool isInAir = true;

	Collider2D whatAmIinsideNow;
		
	public Grid grid;
	public GameHandlerScript gameHandler;

	//Used to know whether or not to swap vectors with a collided block
	public bool vectorUpdated = false;

	// Use this for initialization
	void Start () 
	{		
		GameObject gridObject = GameObject.FindWithTag("Grid");
		grid = gridObject.GetComponent<Grid>();

		GameObject gameHandleObject = GameObject.FindWithTag("GameHandler");
		gameHandler = gameHandleObject.GetComponent<GameHandlerScript>();
	}
		
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (isInAir) 
		{
			moveBlock();
		} 
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (!isInAir)
		{
			//ignore any collision if the block is not falling
			return;
		}

		whatAmIinsideNow = collider;
		GetComponent<AudioSource>().Play ();
		
		switch (collider.tag) 
		{
		case "Wall":
			Debug.Log ("Collision between " + colour + "and " + collider.name);
			WallCollide(collider);
			break;
		case "Floor":
			Debug.Log ("Collision between " + colour + "and " + collider.name);
			FloorCollide();
			break;
		case "Block":
			Debug.Log ("Collision between " + colour + "and " + collider.GetComponent<Block>().colour);
			BlockCollide(Mathf.Abs(block.position.x - collider.transform.position.x) >= Mathf.Abs(block.position.y - collider.transform.position.y),
			             collider);
			break;
		}

	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (!isInAir)
		{
			//ignore any collision if the block is not falling
			return;
		}
		
		GetComponent<AudioSource>().Play ();
		
		switch (collider.tag) 
		{
		case "Wall":
			Debug.Log ("Collision exited between " + colour + "and " + collider.name);
			break;
		case "Floor":
			Debug.Log ("Collision exited between " + colour + "and " + collider.name);
			break;
		case "Block":
			Debug.Log ("Collision exited between " + colour + "and " + collider.GetComponent<Block>().colour);
			break;
		}
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		if(whatAmIinsideNow == collider)
		{
			return;
		}
		whatAmIinsideNow = collider;

		if (!isInAir)
		{
			//ignore any collision if the block is not falling
			return;
		}
		
		GetComponent<AudioSource>().Play ();
		
		switch (collider.tag) 
		{
		case "Wall":
			Debug.Log ("Collision between " + colour + "and " + collider.name);
			WallCollide(collider);
			break;
		case "Floor":
			Debug.Log ("Collision between " + colour + "and " + collider.name);
			FloorCollide();
			break;
		case "Block":
			Debug.Log ("Collision between " + colour + "and " + collider.GetComponent<Block>().colour);
			BlockCollide(Mathf.Abs(block.position.x - collider.transform.position.x) >= Mathf.Abs(block.position.y - collider.transform.position.y),
			             collider);
			break;
		}

	}
	//------------------------------------------------------react to collisions------------------------------------------------------------------

	void WallCollide(Collider2D collider)
	{
		if(firingVector.x != 0)
		{
			float direction = firingVector.x / Mathf.Abs (firingVector.x);
			if(collider.name == "Right Wall")
			{
				//go left(negative)
				firingVector.x *= direction * -1;
			}
			else
			{
				//go right(positive)
				firingVector.x *= direction;
			}
			float signX = transform.position.y - collider.transform.position.y / Mathf.Abs (transform.position.y - collider.transform.position.y);

			EmitParticles(5, transform.position+new Vector3(0,(gameHandler.blockSize/2)*signX,0));
		}
	}
	
	void FloorCollide()
	{
		grid.AddBlockToGrid(this);
		EmitParticles(5, transform.position+new Vector3(0,-0.5f*gameHandler.blockSize,0));
		Land ();
	}
	
	public virtual void BlockCollide(bool horizontal, Collider2D collider)
	{
		//PARTICULATES
		Vector3 emitPos = transform.position;
		emitPos.x = (emitPos.x+collider.transform.position.x)/2;
		emitPos.y = (emitPos.y+collider.transform.position.y)/2;
		EmitParticles(5, emitPos);

		if (horizontal)
		{
			grid.SnapHorizontally(this);
			if(collider.GetComponent<Block>().isInAir)
			{
				if(!vectorUpdated)
				{
					float tempX = collider.GetComponent<Block>().firingVector.x;
					collider.GetComponent<Block>().firingVector.x = firingVector.x;
					firingVector.x = tempX;
					collider.GetComponent<Block>().vectorUpdated = true;
				}
				else
				{
					vectorUpdated = false;
				}
			}
			else
			{
				//Hit horizontal so bounce
				firingVector.x = (block.position.x > collider.transform.position.x ? Mathf.Abs(firingVector.x) : Mathf.Abs(firingVector.x) * -1);
			}
		}
		else
		{
			grid.SnapVertically(this);
			if(collider.GetComponent<Block>().isInAir)
			{
				if(!vectorUpdated)
				{
					float tempY = collider.GetComponent<Block>().firingVector.y;
					collider.GetComponent<Block>().firingVector.y = firingVector.y;
					firingVector.y = tempY;
					collider.GetComponent<Block>().vectorUpdated = true;
				}
				else
				{
					vectorUpdated = false;
				}
			}
			else
			{
				grid.SnapBlockToGrid (this);
				if (CheckBelow ())
				{
					grid.AddBlockToGrid(this);
					Land();
					
				}
				else if(enteringCorridor())
				{
					firingVector.x =0;
				}
				else
				{
					firingVector.x = (block.position.x > collider.transform.position.x ? Mathf.Abs(firingVector.x) : Mathf.Abs(firingVector.x) * -1);
				}
			}
		}
	}

	//-----------------------------------------------------COLLISION FUNCTIONS---------------------------------------------------------------------------------

	virtual public void Land()
	{
		
		isInAir = false;
		
		GameObject handler = GameObject.FindGameObjectWithTag("GameHandler");
		GameHandlerScript hand = handler.GetComponent<GameHandlerScript>();
		hand.BlockLanded ();
		hand.spawner.shooting = false;
	}

	bool enteringCorridor()
	{
		Vector3 rightVector = block.position;
		Vector3 leftVector = block.position;
		
		rightVector.y -= (gameHandler.blockSize / 2f) + 0.01f;
		rightVector.x += (gameHandler.blockSize / 2f) + 0.03f;
		
		leftVector.y -= (gameHandler.blockSize / 2f) + 0.01f;
		leftVector.x -= (gameHandler.blockSize / 2f) + 0.03f;
		
		Collider2D rightCollider = Physics2D.Raycast (rightVector, -Vector2.up, 0.01f).collider;
		Collider2D leftCollider = Physics2D.Raycast (leftVector, -Vector2.up, 0.01f).collider;
		//check if the item is landed or still falling
		if (rightCollider != null && leftCollider != null)
		{
			return true;
		}
		return false;
	}

	//Decide whether or not the block is still falling
	public bool CheckBelow(/*Collider2D collider*/)
	{
		Vector3 rightVector = block.position;
		Vector3 leftVector = block.position;
		
		rightVector.y -= (gameHandler.blockSize / 2f)+0.01f;
		rightVector.x += (gameHandler.blockSize / 2f) - 0.03f;
		
		leftVector.y -= (gameHandler.blockSize / 2f)+0.01f;
		leftVector.x -= (gameHandler.blockSize / 2f) - 0.03f;
		
		Collider2D rightCollider = Physics2D.Raycast (rightVector, -Vector2.up, 0.01f).collider;
		Collider2D leftCollider = Physics2D.Raycast (leftVector, -Vector2.up, 0.01f).collider;
		//check if the item is landed or still falling
		if (leftCollider != null || rightCollider != null /*&& !collider.gameObject.GetComponent<Block>().isInAir*/)
		{
			return true;
		}
		return false;
	}
	//-------------------------------------------------------USEFURU FUNCTIONERUこんいちは-------------------------------------------------------------------------------------------------
	public void EmitParticles(int count, Vector3 location)
	{
		if(colour != "Grey") 
		{
			GameObject emitObj1 = (GameObject)Instantiate (emitter,location,Quaternion.identity);
			emitObj1.GetComponent<EmitterKiller>().SetColour(colour);
			emitObj1.GetComponent<ParticleSystem>().Emit(count);
		}
	}
	
	public void SetFiringVectorByAngle(float angle)
	{
		Vector3 firedVector;
		Quaternion quat;
		
		quat = Quaternion.AngleAxis (angle, Vector3.forward);
		firedVector = quat * Vector3.down;
		
		firingVector = firedVector;
	}
	
	public void SetSpeed(float newSpeed)
	{
        if (newSpeed > (50.0f / 120.0f))
            _speed = 50.0f / 120.0f;
        else
		    _speed = newSpeed;
	}
	
	//Call this once per Update to move the block
	void moveBlock()
	{
		block.position += firingVector *_speed;
	}

	public void Move(Vector3 transformation)
	{
		block.position += transformation;
	}
}
