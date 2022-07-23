using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{

    [SerializeField]
    TileDetector MyDetector;
    //[SerializeField]
    //Tablero table;
    private Tile currentTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //preguntar a su detector cual es su casilla, //actualizar por turno tal vez mejor, depende de como se queira el movimiento
        currentTile = MyDetector.currentTile;
        if (currentTile == null) return;
        //utilizar la infor de eso y el tablero o la de la tile para navegar, de momento le tepeo a la posicion

        Tile posible = null;
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            posible = currentTile.TryNextMove(direccion.Up);
        }
        else
        if (Input.GetKeyDown(KeyCode.A))
        {
            posible = currentTile.TryNextMove(direccion.Left);
            //left
        }
        else
        if (Input.GetKeyDown(KeyCode.S))
        {
            posible = currentTile.TryNextMove(direccion.Down);
            //down
        }
        else
        if (Input.GetKeyDown(KeyCode.D))
        {
            posible = currentTile.TryNextMove(direccion.Right);
            //right
        }

        if (posible != null)
        {
            //move
            this.transform.position = posible.transform.position + new Vector3(0, 2, 0);
            currentTile = posible;
        }

    }
}
