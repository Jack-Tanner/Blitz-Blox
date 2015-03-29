using UnityEngine;
using System.Collections;

public class SpecialBlock : Block {

	public bool special = true;
	Vector2 blockPos;
	const int emitNum = 10;
	GameObject handler;

	void SpecialTrigger(int x, int y)
	{
        switch (colour)
		{
			case "Yellow":
				for (int i = -2; i <= 2; i++)
				{
					if (i != 0)
					{
						grid.RemoveBlock(x + i, y);
						this.EmitParticles(emitNum, new Vector2(this.transform.position.x + (blockSize * i), this.transform.position.y));
						grid.RemoveBlock(x, y + i);
						this.EmitParticles(emitNum, new Vector2(this.transform.position.x, this.transform.position.y + (blockSize * i)));
					}
				}

				grid.RemoveBlock (x + 1, y + 1);
				this.EmitParticles(emitNum, new Vector2(this.transform.position.x + blockSize, this.transform.position.y + blockSize));
				grid.RemoveBlock (x - 1, y - 1);
				this.EmitParticles(emitNum, new Vector2(this.transform.position.x - blockSize, this.transform.position.y - blockSize));
				grid.RemoveBlock (x - 1, y + 1);
				this.EmitParticles(emitNum, new Vector2(this.transform.position.x - blockSize, this.transform.position.y + blockSize));
				grid.RemoveBlock (x + 1, y - 1);
				this.EmitParticles(emitNum, new Vector2(this.transform.position.x + blockSize, this.transform.position.y - blockSize));
				grid.RemoveBlock(x, y);
				
				handler.GetComponent<GameHandlerScript>().rowsMoving = true;
				break;
		}

        DestroyObject(gameObject);
	}

	override public void Land()
	{
		isInAir = false;
		isMoving = false;
		
		enabled = false;


		handler = GameObject.FindGameObjectWithTag("GameHandler");
		GameHandlerScript hand = handler.GetComponent<GameHandlerScript>();
		hand.BlockLanded ();
		hand.spawner.shooting = false;

		grid.AddBlockToGrid (this);
		blockPos = grid.FindGridPosition(this);
        int x, y;
        x = (int)(blockPos.x);
        y = (int)(blockPos.y);
        SpecialTrigger (x, y);
	}

	public override void BlockCollide(bool horizontal, Collider2D collider)
	{
		if (colour == "Blue")
		{
			Vector3 emitPos = transform.position;
			emitPos.x = (emitPos.x+collider.transform.position.x)/2;
			emitPos.y = (emitPos.y+collider.transform.position.y)/2;
			EmitParticles(5, emitPos);
			
			blockPos = grid.FindGridPosition(collider.GetComponent<Block>());
			int x, y;
			x = (int)(blockPos.x);
			y = (int)(blockPos.y);
			
			grid.RemoveBlock(x, y);

		}
		else
		{
			base.BlockCollide(horizontal, collider);
		}
	}
}
