using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    [SerializeField]
    Tile currentTile;
    [SerializeField]
    TileDetector myDetector;


    public void ChangeWalkable(bool walkable)
    {
        currentTile.ChangeTransitable(walkable);
    }

    public void refrexCurrentTile()
    {
        currentTile = myDetector.currentTile;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
