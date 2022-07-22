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
    void Start()
    {
        Vector3 pos = new Vector2(0,0);
        Vector3 sc = tile.transform.localScale;
        //pos.x = -sc.x * (x/2.0f) - margin*((x-1)/2.0f);
        for(int i=0; i<x; i++)
        {
            //pos.z = -sc.z * (y/2.0f) - margin * ((y - 1) / 2.0f);
            pos.z = 0;
            for(int j=0; j<y; j++)
            {
                Instantiate(tile, pos, Quaternion.identity, this.transform);
                pos.z += sc.z + margin;
            }
            pos.x += sc.x + margin;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
