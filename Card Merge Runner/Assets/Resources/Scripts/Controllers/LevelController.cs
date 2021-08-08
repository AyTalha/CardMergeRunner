using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject CardHouseTransformSphere;
    public CardHouseManager cardHouseManager;
    void Start()
    {
        cardHouseManager = FindObjectOfType<CardHouseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
