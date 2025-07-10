using System;
using UnityEngine;

public class Movables : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Transform nextRoadSpawn;
    [SerializeField] private AudioSource audioSource;
    
    private bool _stopped;

    private void OnEnable()
    {
        GameManager.OnLowHealth += MonsterGrowling;
        GameManager.OnGameOver += Pauser;
    }
    private void OnDisable()
    {
        GameManager.OnLowHealth -= MonsterGrowling;
        GameManager.OnGameOver -= Pauser;
    }

    void Update()
    {
        if (id == 0 && !_stopped)
            transform.Translate(-Vector3.forward * (Time.deltaTime * 15f));
        if (id == 1)
        {
            transform.Translate(-Vector3.forward * (Time.deltaTime * 20f));
            transform.Translate(-Vector3.right * (Time.deltaTime * 1.5f));
        }
    }

    public Transform GetNextRoad()
    {
        return nextRoadSpawn;
    }

    void MonsterGrowling(int i)
    {
        if (id == 1) audioSource.Play();
    }

    void Pauser() => _stopped = true;
}
