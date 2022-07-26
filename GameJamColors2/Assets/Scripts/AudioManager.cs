﻿using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    Sound[] sounds = null; // Array que contiene todos los sonidos del juego

    public static AudioManager instance;
    [SerializeField]
    AudioMixerGroup sfx;

    public enum ESounds { bajarPuesto, bate1, bate2, bate3, eoweo, eructo, escribiendo, golpeNiño1, golpeNiño2, golpeNiño3, 
                          golpe, pasos1, pasos2, pasos3, pasos4, pasos5, pasos6, pop, tintontin1, subirPuesto, Apedra, CoroSandokaniko, 
                          EuroBeat, Perder, GolpeObj1, GolpeObj2}; // Enum usado para acceder al array sounds

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else        
            Destroy(this.gameObject);
        

        foreach (Sound s in sounds) // Asigna a cada sonido un AudioSource con las características correspondientes
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip; 
            s.source.outputAudioMixerGroup = s.mixer; 
            s.source.loop = s.loop;
            s.source.volume = s.volume;
        }
    }

    public void Play (ESounds sound) // Hace sonar el sonido que corresponda
    {
        int i = (int)sound;
        Sound s = sounds[i];
        if(!s.source.isPlaying)
            s.source.Play();
    }

    public void Stop (ESounds sound) // Para el sonido que corresponda
    {
        int i = (int)sound;
        Sound s = sounds[i];
        if(s.source.isPlaying)
            s.source.Stop();
    }

    public void StopAll() // Para todos los sonidos
    {
        Sound s;
        for (int i = 0; i < sounds.Length; i++)
        {
            s = sounds[i];
            if (s.source.isPlaying)
                s.source.Stop();
        }
    }

    public void StopAllSFX() // Para todos los efectos de sonido
    {
        Sound s;
        for(int i = 0; i < sounds.Length; i++)
        {
            s = sounds[i];
            if (s.source.outputAudioMixerGroup == sfx) 
                s.source.Stop();
        }
    }

    public void StopAllMusic() // Para todos los efectos de sonido
    {
        Sound s;
        for (int i = 0; i < sounds.Length; i++)
        {
            s = sounds[i];
            if (s.source.outputAudioMixerGroup != sfx)
                s.source.Stop();
        }
    }

    public bool IsPlaying(ESounds sound) // Comprueba si un clip está sonando o no
    {
        int i = (int)sound;
        Sound s = sounds[i];
        if (s.source.isPlaying)
            return true;
        else
            return false;
    }
}


