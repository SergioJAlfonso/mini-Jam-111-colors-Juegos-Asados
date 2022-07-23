using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    public int index;
    public Tablero table;

    public Tile TryNextMove(direccion dir)
    {
        int next = table.NextMove(index, dir);
        if (next != -1)
        {
            if (table.TransitableTile(next))
            {
                return table.getTileByIndex(next);
            }
        }
        return null;
    }
}
