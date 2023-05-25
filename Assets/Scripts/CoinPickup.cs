using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip playAudio;
    [SerializeField] int points = 100;
    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && !wasCollected){
            wasCollected = true;
            FindObjectOfType<GameSession>().AddScore(points);
            AudioSource.PlayClipAtPoint(playAudio, Camera.main.transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
