using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> woodHits;
    [SerializeField] private float forceNeededForWoodSound;
    [SerializeField] private float forceNeededForFullWoodSound;
    [SerializeField] private List<AudioClip> scoringSounds;
    [SerializeField] private float timeBeforeScoreSoundReset;
    [SerializeField] private AudioClip mistakeSound;
    [SerializeField] private AudioClip playSound;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip levelSuccessSound;
    [SerializeField] private AudioClip levelFailureSound;

    private float _timeSinceLastScoreSound;
    private int _scoreSoundLevel;

    public void WoodHit(float relVelMag)
    {
        if (relVelMag >= forceNeededForWoodSound)
        {
            float volume = Mathf.Lerp (0.2f, 1f, Mathf.InverseLerp (forceNeededForWoodSound, forceNeededForFullWoodSound, relVelMag));
            int randomIndex = Random.Range(0, woodHits.Count);
            audioSource.PlayOneShot(woodHits[randomIndex], volume);
        }
    }

    public void LevelComplete(bool success)
    {
        audioSource.PlayOneShot(success ? levelSuccessSound : levelFailureSound);
    }

    public void Score()
    {
        if (Time.time - _timeSinceLastScoreSound > timeBeforeScoreSoundReset)
        {
            _scoreSoundLevel = 0;
        }
        else
        {
            // Limit level to max index
            _scoreSoundLevel = Math.Min(scoringSounds.Count - 1, _scoreSoundLevel + 1);
        }

        AudioClip scoreSound = scoringSounds[_scoreSoundLevel];
        float volume = Mathf.Lerp (0.5f, 1f, Mathf.InverseLerp (0, scoringSounds.Count - 1, _scoreSoundLevel));
        audioSource.PlayOneShot(scoreSound, volume);
        
        _timeSinceLastScoreSound = Time.time;
    }

    public void Mistake()
    {
        audioSource.PlayOneShot(mistakeSound);
    }

    public void Play()
    {
        audioSource.PlayOneShot(playSound);
    }

    public void ButtonPressed()
    {
        audioSource.PlayOneShot(buttonSound);
    }
}
