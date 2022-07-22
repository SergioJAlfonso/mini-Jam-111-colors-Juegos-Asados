using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablero : MonoBehaviour
{
    [SerializeField]
    GameObject tile;
    [SerializeField]
    int x, y;
    [SerializeField]
    float margin;
    // Start is called before the first frame update

    //colocar los nodos e indicies, lo que utilizaria la ia para moverse por turnos.
    nodo[] indexes;
    //la estructura del nodo
    public struct nodo
    {
        //indices adyadcentes
        public int up, left, right, down;
        public bool transitable;
        // public int coste;
    }


    void Start()
    {
        indexes = new nodo[x * y];

        Vector3 pos = new Vector2(0, 0);
        Vector3 sc = tile.transform.localScale;
        pos.x = -sc.x * (x / 2.0f) - margin * ((x - 1) / 2.0f);
        for (int i = 0; i < x; i++)
        {
            pos.z = -sc.z * (y / 2.0f) - margin * ((y - 1) / 2.0f);
            for (int j = 0; j < y; j++)
            {
                Instantiate(tile, pos, Quaternion.identity, this.transform);
                pos.z += sc.z + margin;


                //asignación de indices
                int id = i * x + j;

                indexes[id].left = id - 1;
                indexes[id].right = id + 1;
                if ((id) % y == 0)
                {
                    indexes[id].left = -1;
                }
                else if ((id) % y == y - 1)
                {
                    indexes[id].right = -1;
                }

                indexes[id].down = id + y;
                indexes[id].up = id - y;

                if (id < y)
                {
                    indexes[id].up = -1;
                }
                else if (id >= y * (x - 1))
                {
                    indexes[id].down = -1;
                }

                indexes[id].transitable = true;

            }
            pos.x += sc.x + margin;
        }

        this.transform.GetChild(0).gameObject.AddComponent<EndTile>();
        this.transform.GetChild(transform.childCount-1).gameObject.AddComponent<EndTile>().heaven = false;
    }

    public Vector2 IndexToCoord(int index)
    {
        Vector2 coord;

        coord.x = (int)(index % y);
        coord.y = (int)(index / y);

        return coord;
    }

    public int CoordToIndex(Vector2 coord)
    {
        return (int)(coord.x + (coord.y * y));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
