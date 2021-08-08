using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCardThrowScript : MonoBehaviour
{
    public float cardRotationSpeed=15;
    public float cardSpeed=5;
    public GameObject mesh;
    public Transform muzzle;
    private CardHouseManager cardHouseManager;
    private void Awake()
    {
        cardHouseManager = FindObjectOfType<CardHouseManager>();
        muzzle = cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber].shotTransform;
    }
    void Start()
    {

        //mesh.transform.DORotate(new Vector3(90, 0, 0), 0.2f);
        //gameObject.GetComponent<Rigidbody>().velocity = muzzle.transform.forward * cardSpeed;
    }

   
    private void FixedUpdate()
    {
        mesh.transform.Rotate(0, 0, cardRotationSpeed);
        gameObject.GetComponent<Rigidbody>().velocity = transform.forward * cardSpeed;

    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.CompareTag("HouseCard"))
        {
            if (!collision.gameObject.GetComponent<HouseCardPolygonScript>().isCollision )
            {
                cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber].destroyCount--;
                collision.gameObject.GetComponent<HouseCardPolygonScript>().isCollision = true;
            }
          
            collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.gameObject.transform.position);
           
            Destroy(this.gameObject);
        }
 
    }
}
