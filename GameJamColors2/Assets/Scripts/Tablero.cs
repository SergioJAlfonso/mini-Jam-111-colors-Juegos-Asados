using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablero : MonoBehaviour
{
    [SerializeField]
    GameObject tile;
    [SerializeField]
    int x, y;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = new Vector2(0,0);
        Vector3 sc = tile.transform.localScale;
        pos.x = sc.x * (x/2.0f);
        for(int i=0; i<x; i++)
        {
            pos.z = sc.z * (y/2.0f);
            for(int j=0; j<y; j++)
            {
                pos.z += sc.z;
                Instantiate(tile, pos, Quaternion.identity, this.transform);
            }
            pos.x += sc.x;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
