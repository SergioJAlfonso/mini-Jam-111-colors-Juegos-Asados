using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public float mainVolSlider = 0.5f,
                 SFXVolSlider = 0.5f,
                 musicVolSlider = 0.5f;
    public bool gameIsPaused, needToPause, needToResume, playerTurn, moveZombie, won = false, lost = false;
    int currActions, maxActions = 2;
    float zombieTurnTime1 = 1.0f, zombieTurnTime2 = 1.0f, zombieTimer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        Random.InitState((int)DateTime.Now.Ticks);
    }

    // Start is called before the first frame update
    void Start()
    {
        playerTurn = true;
        moveZombie = !playerTurn;
        currActions = maxActions;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerTurn)
        {
            zombieTimer -= Time.deltaTime;
            if(zombieTimer <= 0)
            {
                moveZombie = true;
                playerTurn = true;
            }
        }
        if (won)
        {
            Debug.Log("Has ganado");
        }
        else if (lost)
        {
            Debug.Log("Has perdido");
        }
    }

    public void MainSliderState(float volume)
    {
        mainVolSlider = volume;
    }
    public void MusicSliderState(float volume)
    {
        musicVolSlider = volume;
    }
    public void SFXSliderState(float volume)
    {
        SFXVolSlider = volume;
    }
    public void decreaseActions()
    {
        currActions--;
        if(currActions <= 0)
        {
            currActions = maxActions;
            playerTurn = false;
            Debug.Log("Fin de turno");

            zombieTimer = zombieTurnTime1;
        }
    }
}
