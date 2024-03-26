using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    Tile currentTile;
    Locations locationReference;

    [SerializeField]
    List<AudioClip> sounds = new List<AudioClip>();
    AudioSource aud;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        locationReference = FindObjectOfType<Locations>();
    }

    private void Update()
    {
        if(currentTile == null) { 
            MoveToNewTile();
        }
    }
    public void MoveToNewTile()
    {
        if (currentTile)
        {
            currentTile.SetFood(null);
            currentTile.isOccupied = false;
        }
        currentTile = FindObjectOfType<Locations>().GetRandomUnoccupiedTile();
        transform.position = currentTile.transform.position;
        //currentTile.SetFood(this);
        currentTile.food = this;
        int randomNum = Random.Range(0, sounds.Count - 1);
        aud.clip = sounds[randomNum];
        aud.Play();
        aud.loop = false;

    }

    public Tile GetCurrentTile()
    {
        return currentTile;
    }
}
