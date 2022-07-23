using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDetector : MonoBehaviour
{
    [SerializeField]
    bool notExitTrigger;

    private Tile currentTile_;
    public Tile currentTile { 
        get { return currentTile_; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Tile>())
        {
            currentTile_ = other.GetComponent<Tile>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!notExitTrigger && currentTile == other.GetComponent<Tile>())
        {
            currentTile_ = null;
        }
    }
}
