using System;
using UnityEngine;

public class Movables : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private Transform nextRoadSpawn;
    [SerializeField] private AudioSource audioSource;

    private void OnEnable()
    {
        GameManager.onLowHealth += MonsterGrowling;
    }
    private void OnDisable()
    {
        GameManager.onLowHealth -= MonsterGrowling;
    }

    void Update()
    {
        if (id == 0)
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

    void MonsterGrowling()
    {
        if (id == 1) audioSource.Play();
    }
}
