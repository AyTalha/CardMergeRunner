using Hyperlab.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHouseScript : MonoBehaviour
{
    public List<GameObject> CardsLocation = new List<GameObject>();
    public int cardTotalNumber;
    public bool isFull;
    public CardHouseManager cardHouseManager;
    private bool isIf=false;
    public Transform shotTransform;
    public int destroyCount;
    public bool destroyBuild;
    void Start()
    {
        cardHouseManager = GetComponentInParent<CardHouseManager>();
        destroyCount = CardsLocation.Count;
    }

    // Update is called once per frame
    void Update()
    {

        //if (destroyBuild)
        //{
        //    gameObject.SetActive(false);
        //}
        if (!isIf&&destroyBuild)
        {
            cardHouseManager.houseLevelNumber++;
            isIf = true;
        }
   
    }
}
