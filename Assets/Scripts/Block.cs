using UnityEngine;
using System.Collections;
using System;

public class Block : MonoBehaviour {

    public Transform block;

    public float blockSize = 0.64f;
    public string colour = "";
	public GameObject emitter;
    float _speed; //speed of the projectile

    //public GameObject scorer;
    public bool isMoving = true;
	public bool isInAir = true;

	public float _angle = 0;
	public Vector3 firingVector;
	//float storeY;
	Transform collidedBlock;

	Spawner _spawner;
	//bool sliding = false;

    public Grid grid;
	
	public float GetAngle()
	{
		return _angle;
	}
	
	public void SetAngle(float f)
	{
		_angle = f;
	}
	
	public void Move(Vector3 transformation)
	{
		block.position += transformation;
	}
	
	// Use this for initialization
	void Start () {
		GameObject spawner = GameObject.FindWithTag("Spawner");
		_spawner = spawner.GetComponent<Spawner> ();
		_speed = _spawner.speed/120f;
		firingVector = CalculateFiringVector (spawner.transform.eulerAngles.z);

        GameObject gridObject = GameObject.FindWithTag("Grid");
        grid = gridObject.GetComponent<Grid>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (isMoving) 
		{
			if(enteringCorridor())
			{
				
				firingVector.x = 0;
				/*if(sliding)
				{
					firingVector.y= storeY;
					sliding=false;
				}
			}
			if(sliding && Mathf.Abs(transform.position.x - collidedBlock.position.x)<0.1f)
			{
				grid.AddBlockToGrid(this);
				Land ();*/
			}
			moveBlock (firingVector);
		} 
	}
	
	void HorizontalHit(Collider2D collider)
	{
		if(block.position.x > collider.transform.position.x)
		{
			firingVector.x = Mathf.Abs(firingVector.x);
		}
		else
		{
			firingVector.x = Mathf.Abs(firingVector.x) * -1;
		}
	}

	public void EmitParticles(int count, Vector3 location)
	{
		GameObject emitObj1 = (GameObject)Instantiate (emitter,location,Quaternion.identity);
		emitObj1.GetComponent<EmitterKiller>().SetColour(colour);
		emitObj1.GetComponent<ParticleSystem>().Emit(count);
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (!isMoving)
		{
			return;
		}
		/*
		if(sliding)
		{
			sliding = false;
			grid.AddBlockToGrid(this);
			float signX = transform.position.x - collider.transform.position.x / Mathf.Abs (transform.position.x - collider.transform.position.x);
			EmitParticles(5, transform.position+new Vector3(0,(blockSize/2)*signX,0));
			Land ();
			return;
		}*/
		GetComponent<AudioSource>().Play ();
		bool horizontal = false;
		if(Mathf.Abs(block.position.x-collider.transform.position.x)> Mathf.Abs(block.position.y -collider.transform.position.y))
		{
			horizontal = true;
		}
		switch (collider.tag) 
		{
		case "Wall":
			WallCollide(horizontal, collider);
			break;
		case "Floor":
			FloorCollide();
			break;
		case "Block":
			BlockCollide(horizontal, collider);
			break;
		}
	}
	
	void WallCollide(bool horizontal, Collider2D collider)
	{
		HorizontalHit(collider);
		float signX = transform.position.y - collider.transform.position.y / Mathf.Abs (transform.position.y - collider.transform.position.y);
		EmitParticles(5, transform.position+new Vector3(0,(blockSize/2)*signX,0));
	}
	
	void FloorCollide()
	{
		grid.AddBlockToGrid(this);
		EmitParticles(5, transform.position+new Vector3(0,-0.5f*blockSize,0));
		Land ();
	}
	
	public virtual void BlockCollide(bool horizontal, Collider2D collider)
	{
		if (!collider.gameObject.GetComponent<Block>().isInAir)
		{
			
			Vector3 emitPos = transform.position;
			emitPos.x = (emitPos.x+collider.transform.position.x)/2;
			emitPos.y = (emitPos.y+collider.transform.position.y)/2;
			EmitParticles(5, emitPos);
			if (horizontal)
			{
				//Hit horizontal so bounce
				HorizontalHit(collider);
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
					HorizontalHit(collider);
				}
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D collider)
	{/*
		if (sliding) 
		{
			grid.SnapBlockToGrid(this);
			firingVector.y = storeY;
			sliding = false;
		}*/
	}
	
	bool enteringCorridor()
	{
		Vector3 rightVector = block.position;
		Vector3 leftVector = block.position;
		
		rightVector.y -= (blockSize / 2f) + 0.01f;
		rightVector.x += (blockSize / 2f) + 0.03f;
		
		leftVector.y -= (blockSize / 2f) + 0.01f;
		leftVector.x -= (blockSize / 2f) + 0.03f;
		
		Collider2D rightCollider = Physics2D.Raycast (rightVector, -Vector2.up, 0.01f).collider;
		Collider2D leftCollider = Physics2D.Raycast (leftVector, -Vector2.up, 0.01f).collider;
		//check if the item is landed or still falling
		if (rightCollider != null && leftCollider != null)
		{
			return true;
		}
		return false;
	}
	
	virtual public void Land()
	{
       
        isInAir = false;
		isMoving = false;
       
        enabled = false;

		GameObject handler = GameObject.FindGameObjectWithTag("GameHandler");
		GameHandlerScript hand = handler.GetComponent<GameHandlerScript>();
		hand.BlockLanded ();
		hand.spawner.shooting = false;
	}
	
	//Decide whether or not the block is still falling
	public bool CheckBelow(/*Collider2D collider*/)
	{
        Vector3 rightVector = block.position;
		Vector3 leftVector = block.position;
		
		rightVector.y -= (blockSize / 2f)+0.01f;
		rightVector.x += (blockSize / 2f) - 0.03f;
		
		leftVector.y -= (blockSize / 2f)+0.01f;
		leftVector.x -= (blockSize / 2f) - 0.03f;
		
		Collider2D rightCollider = Physics2D.Raycast (rightVector, -Vector2.up, 0.01f).collider;
		Collider2D leftCollider = Physics2D.Raycast (leftVector, -Vector2.up, 0.01f).collider;
		//check if the item is landed or still falling
		if (leftCollider != null || rightCollider != null /*&& !collider.gameObject.GetComponent<Block>().isInAir*/)
		{
			return true;
		}
		return false;
	}
	
	//Calculate the fired vector
	Vector3 CalculateFiringVector(float angle)
	{
		Vector3 firedVector;
		Quaternion quat;
		
		quat = Quaternion.AngleAxis (angle, Vector3.forward);
		firedVector = quat * Vector3.down;
		
		return firedVector;
	}
	
	//Call this once per Update to move the block
	void moveBlock(Vector3 movementVector)
	{
		Vector3 blockPos = block.position;
		blockPos += movementVector *_speed;
		block.position = blockPos;
	}
}
