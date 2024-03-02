using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : MonoBehaviour
{
    /*TODOS:
     
     - can't interact with items until completely stopped / not holding a move key
    
    FUTURE:
     - animation
     - h & w*/

    public Map map;
    Room currentRoom;

    //movement mechanics
    float xPos = 0.5f;
    float yPos = 0.5f;
    bool moving;
    bool movingInProgress = false;
    bool movingPlanSet = false;
    int xMovement = 0;
    int yMovement = 0;
    enum Direction { W, A, S, D, NONE }
    Direction facing = Direction.S;
    Direction movementDir = Direction.NONE;

    //movement VARS
    float walkSpeed = 0.2f; //how long it takes to move between tiles
    float movementTime = 0.0f;

    Vector3 old;
    Vector3 neww;

    //Sprites
    Sprite[] sprites = new Sprite[4];

    void Start()
    {
        sprites[0] = Resources.Load<Sprite>("Player/walk_back");
        sprites[1] = Resources.Load<Sprite>("Player/walk_left");
        sprites[2] = Resources.Load<Sprite>("Player/walk_front");
        sprites[3] = Resources.Load<Sprite>("Player/walk_right");

        RoomObjDoor.doorOpened += doorToOtherRoom;
        Map.generationCompleted += enablePlayer;
    }

    //when the map is finished generation, we can start using the player object
    //TODO: needs to be more refined. complete enabling / disabling?
    void enablePlayer()
    {
        transform.position = new Vector3(0.5f, 0.5f, transform.position.z);
        switchRoom(RoomGen.map.rooms[0]);
    }

    void doorToOtherRoom(Room r)
    {
        switchRoom(r);
    }

    void switchRoom(Room r)
    {
        currentRoom = r;
    }
    
    
    void Update()
    {
        //if moving right now to another spot on the grid, finish the move animation first
        if (movingInProgress)
        {
            if (!movingPlanSet)
            {
                old = transform.position;
                neww = new Vector3(old.x + xMovement, old.y + yMovement, old.z);
                movingPlanSet = true;
            }

            if (movementTime >= walkSpeed)
            {
                movingInProgress = false;

                xPos = transform.position.x;
                yPos = transform.position.y;

                transform.position = neww;
                movingPlanSet = false;
            } else
            {
                movementTime += Time.deltaTime;
                
                transform.position = Vector3.Lerp(old, neww, movementTime / walkSpeed);
            }
        }
        else
        {
            movementTime = 0.0f;

            //if key down of current facing direction, go in that direction.
            //else figure out where else you'd like to go.
            if (stillMovingInSameDirection())
            {
                moving = true;
            } else
            {
                xMovement = 0;
                yMovement = 0;
                if (Input.GetKey(KeyCode.W))
                {
                    yMovement += 1;
                    moving = true;
                    facing = Direction.W;
                    movementDir = Direction.W;
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    xMovement -= 1;
                    moving = true;
                    facing = Direction.A;
                    movementDir = Direction.A;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    yMovement -= 1;
                    moving = true;
                    facing = Direction.S;
                    movementDir = Direction.S;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    xMovement += 1;
                    moving = true;
                    facing = Direction.D;
                    movementDir = Direction.D;
                }
                else
                {
                    movementDir = Direction.NONE;
                }
                
            }

            if (moving)
            {
                //change facing direction of sprite
                resetFacingSprite(facing);
                //Last check: Are you allowed to move here?
                if (currentRoom.canMoveHere(new Vector2(xPos + xMovement, yPos + yMovement)))
                {
                    movingInProgress = true;
                    moving = false;
                }
            } else
            {
                //interact with an object if there is one in the direction you are facing
                if (Input.GetKeyDown(KeyCode.E))
                {
                    RoomObject obj = currentRoom.isObjectHere(new Vector2Int(Mathf.FloorToInt(xPos), Mathf.FloorToInt(yPos)) + (directionToCoords(facing)));
                    if (obj != null)
                    {
                        obj.interact();
                        if (obj is RoomObjDoor door) { 
                            transform.position = door.openDoor(currentRoom);
                            xPos = transform.position.x;
                            yPos = transform.position.y;
                        }
                    }
                }
            }
        }

        
    }

    //call whenever facing direction has potentially changed
    void resetFacingSprite(Direction face)
    {
        switch (face)
        {
            case Direction.W:
                this.GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
            case Direction.A:
                this.GetComponent<SpriteRenderer>().sprite = sprites[1];
                break;
            case Direction.S:
                this.GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;
            case Direction.D:
                this.GetComponent<SpriteRenderer>().sprite = sprites[3];
                break;
            default: break;
        }
    }

    bool stillMovingInSameDirection()
    {
        if (movementDir == Direction.W && Input.GetKey(KeyCode.W)) return true;
        if (movementDir == Direction.A && Input.GetKey(KeyCode.A)) return true;
        if (movementDir == Direction.S && Input.GetKey(KeyCode.S)) return true;
        if (movementDir == Direction.D && Input.GetKey(KeyCode.D)) return true;
        return false;
    }

    Vector2Int directionToCoords(Direction d)
    {
        switch (d)
        {
            case Direction.W: return new Vector2Int(0, 1);
            case Direction.A: return new Vector2Int(-1, 0);
            case Direction.S: return new Vector2Int(0, -1);
            case Direction.D: return new Vector2Int(1, 0);
            default: return new Vector2Int(0, 0);
        }
    }
}
