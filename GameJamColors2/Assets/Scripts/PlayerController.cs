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
    float lerpSpeed = 0.05f;
    float rotateTime = 0;
    bool rotate = false, rotating = false;
    Quaternion targetRotation;

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
        manageInput();

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
            rotateTime += Time.deltaTime * lerpSpeed;
            obstacles.rotation = Quaternion.Lerp(obstacles.rotation, targetRotation, rotateTime);
            if (rotateTime > lerpSpeed)
            {
                Debug.Log("end rot");
                rotating = false;
                //octualizar obstaculos
                foreach (Transform a in obstacles)
                {
                    a.gameObject.GetComponent<Obstacle>().refrexCurrentTile();
                    a.gameObject.GetComponent<Obstacle>().ChangeWalkable(false);
                }
            }
        }
    }

    public void DoMove(direccion dir)
    {
        Tile posible = currentTile.TryNextMove(dir);
        if (posible != null)
        {
            //move
            this.transform.position = posible.transform.position + new Vector3(0, 2, 0);
            currentTile = posible;
        }
    }

    void manageInput()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Rotate map
            // Indicar por UI que A izqda D dcha
            rotate = true;
        }

        if (!rotating && Input.GetKeyDown(KeyCode.W))
        {
            if (!rotate)
                DoMove(direccion.Up);
        }
        else if (!rotating && Input.GetKeyDown(KeyCode.A))
        {
            //left
            if (!rotate)
                DoMove(direccion.Left);
            else
                startRotation(90);
        }
        else if (!rotating && Input.GetKeyDown(KeyCode.S))
        {
            if (!rotate)
                DoMove(direccion.Down);
            //down
        }
        else if (!rotating && Input.GetKeyDown(KeyCode.D))
        {
            //right
            if (!rotate)
                DoMove(direccion.Right);
            else
                startRotation(-90);
        }
    }
}
