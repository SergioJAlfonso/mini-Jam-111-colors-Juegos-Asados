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
            this.transform.position = nextTile.transform.position + new Vector3(0, 2, 0);
            currentTile = nextTile;
            //move player also
            player.DoMove(dir);
        }

    }





    // Update is called once per frame
    void Update()
    {
        currentTile = MyDetector.currentTile;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CalculateNextMove();
        }
    }
}
