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

        Vector3 pos = new Vector2(0,0);
        Vector3 sc = tile.transform.localScale;
        pos.x = -sc.x * (x/2.0f) - margin*((x-1)/2.0f);
        for(int i=0; i<x; i++)
        {
            pos.z = -sc.z * (y/2.0f) - margin * ((y - 1) / 2.0f);
            for(int j=0; j<y; j++)
            {
                Instantiate(tile, pos, Quaternion.identity, this.transform);
                pos.z += sc.z + margin;


                //asignación de indices

                indexes[i + j].left = i + j - 1;
                indexes[i + j].right = i + j + 1;
                if ((i+j)%y == 0)
                {
                    indexes[i + j].left = -1;
                }
                else if ((i + j) % y == y - 1)
                {
                    indexes[i + j].right = -1;
                }

                indexes[i + j].down = i + j + y;
                indexes[i + j].up = i + j - y;

                if ((i+j) < y)
                {
                    indexes[i + j].up = -1;
                }
                else if ((i+j) >= y * (x - 1))
                {
                    indexes[i + j].down = -1;
                }

                indexes[i + j].transitable = true;

            }
            pos.x += sc.x + margin;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
