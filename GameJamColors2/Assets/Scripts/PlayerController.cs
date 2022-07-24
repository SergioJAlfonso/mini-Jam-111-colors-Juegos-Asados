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

    // Rotation
    [SerializeField]
    Transform obstacles;

    [SerializeField]
    float rotateLerpSpeed = 0.05f, posLerpSpeed = 0.5f;
    float rotateTime = 0, movementTime = 0;
    bool rotate = false, rotating = false, moving = false;
    Quaternion targetRotation;
    Vector3 targetPosition;

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

        // Input
        if (GameManager.instance.playerTurn)
            manageInput();

        // Movimiento en curso
        checkMovement();
        // Rotacion en curso
        checkRotation();
    }

    void startRotation(int rotDir)
    {
        targetRotation = Quaternion.Euler(0, obstacles.rotation.eulerAngles.y + rotDir, 0);
        rotateTime = 0;
        rotating = true;
        rotate = false;
        //desocupar todas las casillas de obstaculos
        foreach (Transform a in obstacles)
        {
            a.gameObject.GetComponent<Obstacle>().ChangeWalkable(true);
        }

    }
    void checkRotation()
    {
        if (rotating)
        {
            rotateTime += Time.deltaTime * rotateLerpSpeed;
            obstacles.rotation = Quaternion.Lerp(obstacles.rotation, targetRotation, rotateTime);
            if (rotateTime > 0.1f)
            {
                rotating = false;
                //actualizar obstaculos
                foreach (Transform a in obstacles)
                {
                    a.gameObject.GetComponent<Obstacle>().refrexCurrentTile();
                    a.gameObject.GetComponent<Obstacle>().ChangeWalkable(false);
                }
            }
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
    public void DoMove(direccion dir, bool enem)
    {
        Tile posible = currentTile.TryNextMove(dir);
        if (posible != null)
        {
            //move
            //transform.position = posible.transform.position + new Vector3(0, 0.71f, 0);
            targetPosition = posible.transform.position + new Vector3(0, 0.71f, 0);
            moving = true;
            movementTime = 0;
            currentTile = posible;
            if(!enem)
                GameManager.instance.decreaseActions();

            EndTile eT = posible.gameObject.GetComponent<EndTile>();
            if (eT != null && eT.heaven)
            {
                GameManager.instance.won = true;
            }
        }
    }
    void manageInput()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Rotate map - Indicar por UI que A izqda D dcha
            rotate = !rotate;
        }

        if (!rotating && !moving && Input.GetKeyDown(KeyCode.W))
        {
            if (!rotate)
                DoMove(direccion.Up, false);
        }
        else if (!rotating && !moving && Input.GetKeyDown(KeyCode.A))
        {
            //left
            if (!rotate)
                DoMove(direccion.Left, false);
            else
            {
                startRotation(90);
                GameManager.instance.decreaseActions();
            }
        }
        else if (!rotating && !moving && Input.GetKeyDown(KeyCode.S))
        {
            if (!rotate)
                DoMove(direccion.Down, false);
        }
        else if (!rotating && !moving && Input.GetKeyDown(KeyCode.D))
        {
            //right
            if (!rotate)
                DoMove(direccion.Right, false);
            else
            {
                startRotation(-90);
                GameManager.instance.decreaseActions();
            }
        }
    }
}
