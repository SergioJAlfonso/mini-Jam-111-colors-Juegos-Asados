using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField]
    Tablero table;
    [SerializeField]
    PlayerController player;
    [SerializeField]
    TileDetector MyDetector;

    private direccion dir;
    private Tile currentTile;
    private int objetive;

    float posLerpSpeed = 0.5f, movementTime = 0;
    bool moving = false;
    Vector3 targetPosition;

    // Start is called before the first frame update

    void Start()
    {
        
        currentTile = MyDetector.currentTile;
    }



    void CalculateNextMove()
    {
        objetive = table.ExitIndex(false);
        dir = table.DjistraAStarFromTo(currentTile.index, objetive);

        Tile nextTile = currentTile.TryNextMove(dir);
        if (nextTile != null)
        {
            //move
            targetPosition = nextTile.transform.position + new Vector3(0, 0.71f, 0);
            moving = true;
            movementTime = 0;

            currentTile = nextTile;
            //move player also
            player.DoMove(dir);
        }

    }
    void checkMovement()
    {
        if (moving)
        {
            movementTime += Time.deltaTime * posLerpSpeed;
            transform.position = Vector3.Lerp(transform.position, targetPosition, movementTime);
            if (movementTime > 0.2f)
            {
                moving = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTile = MyDetector.currentTile;

        if (GameManager.instance.moveZombie/*Input.GetKeyDown(KeyCode.Space)*/)
        {
            CalculateNextMove();
            GameManager.instance.moveZombie = false;
        }

        checkMovement();
    }
}
