using Dreamteck.Splines;
using HedgehogTeam.EasyTouch;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using Hyperlab.Managers;
using UnityEngine.AI;
using Cinemachine;

public class PlayerScript : MonoBehaviour
{
    [Range(0, 2)]
    public float swipeSensivity;
  
    [Range(0, 20)]
    public float playerSpeed;
    public float shotSwipeSensivity;

    private SplineFollower splineFollower;

    private bool isRight,isLeft;

    
    public int currentNumber=1;

    public ParticleSystem spawnFx;
    
    private Animator playerAnim;

    public List<GameObject> Cards = new List<GameObject>();
    public List<GameObject> AddedCardsA = new List<GameObject>();
    public List<GameObject> AddedCards2 = new List<GameObject>();
    public List<GameObject> AddedCards3 = new List<GameObject>();
    public List<GameObject> AddedCards4 = new List<GameObject>();
    public List<GameObject> AddedCards5 = new List<GameObject>();
    public List<GameObject> AddedCards6 = new List<GameObject>();
    public List<GameObject> AddedCards7 = new List<GameObject>();
    public List<GameObject> AddedCards8 = new List<GameObject>();
    public List<GameObject> AddedCards9 = new List<GameObject>();
    public List<GameObject> AddedCards10 = new List<GameObject>();
    public List<GameObject> AddedCardsJ = new List<GameObject>();
    public List<GameObject> AddedCardsQ = new List<GameObject>();
    public List<GameObject> AddedCardsK = new List<GameObject>();
    public List<GameObject> AddedCards13 = new List<GameObject>();
    public List<GameObject> InstantiateCards = new List<GameObject>();

    public float triggerAnimationSpeed;

    public Vector3 smallSize = new Vector3(1f, 1f, 1f);
    public Vector3 XsmallSize = new Vector3(0.5f, 0.5f, 0.5f);
    public Vector3 bigSize = new Vector3(1.3f, 1.3f, 1.3f);
 

    public Transform targetObstacle;
    private bool isCollision=false;
    public Transform rotationPlayer;

    public GameObject droppedCard;
    public GameObject enterCard;
    public Transform entryCard,finishLine;
    private bool isStart;
    private bool isBool=true;

    public GameObject finalCardObject;
    public GameObject finalCardThrow;
    public GameObject trash;
    public GameObject baseLevel;

    public int followCardNumber;
    public CinemachineVirtualCamera shotCamera;
    public CinemachineVirtualCamera gameCamera;
    public bool isControl=false;

    GameObject mergeCardHigh;

    public float FinalcardThrowSpeed;
    public CardHouseManager cardHouseManager;
    public bool isShooting;
    public Transform muzzle;
    private bool isTouch;

    private void Start()
    {
        splineFollower = GetComponent<SplineFollower>();


    }
    private void Awake()
    {
        cardHouseManager = FindObjectOfType<CardHouseManager>();
    }
    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onStartPlay -= OnStartPlay;
            GameManager.Instance.onLevelSetup -= OnLevelSetup;
            GameManager.Instance.onLevelFinish -= OnLevelFinish;
        }
        EasyTouch.On_Swipe -= On_Swipe;
        //EasyTouch.On_SwipeStart += On_SwipeStart;

        EasyTouch.On_TouchUp -= On_TouchUp;
        EasyTouch.On_TouchStart -= On_TouchStart;
        EasyTouch.On_TouchDown -= On_TouchDown;
    }
    private void OnEnable()
    {
        EasyTouch.On_Swipe += On_Swipe;
        GameManager.Instance.onStartPlay += OnStartPlay;
        GameManager.Instance.onLevelSetup += OnLevelSetup;
        GameManager.Instance.onLevelFinish += OnLevelFinish;
        //EasyTouch.On_SwipeStart += On_SwipeStart;
        EasyTouch.On_TouchUp += On_TouchUp;
        EasyTouch.On_TouchStart += On_TouchStart;
        EasyTouch.On_TouchDown += On_TouchDown;
    }

    void OnLevelFinish()
    {
       
    }

    void OnLevelSetup()
    {

        shotCamera.Priority = 9;
        gameCamera.Priority = 10;
        isControl = false;
        foreach (GameObject item in Cards)
        {
            item.SetActive(false);
        }
        Cards[0].SetActive(true);
        Debug.Log("onleevelfinal");

        if (cardHouseManager.houseLevelNumber>0)
        {
            if (cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber-1].destroyBuild)
            {
                cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber-1].gameObject.SetActive(false);
            
            }
        }
       
    }



    void OnStartPlay()
    {
        splineFollower.followSpeed = playerSpeed;
     
        Cards[currentNumber - 1].GetComponent<Animator>().SetBool("isStart", true);
        isStart = true;
      
    }

    private void On_Swipe(Gesture gesture)
    {
        if (isShooting)
        {
            CardHouseScript cardHouseScript = cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber];
             muzzle = cardHouseScript.shotTransform;
            Vector3 target = new Vector3(muzzle.transform.localEulerAngles.x +
(-gesture.swipeVector.y * gesture.deltaTime * shotSwipeSensivity), muzzle.transform.localEulerAngles.y +
(gesture.swipeVector.x * gesture.deltaTime * shotSwipeSensivity), muzzle.transform.localEulerAngles.z);


            //Limits
            //if (target.x > 180)
            //    target.x = target.x < 300 ? 300 : target.x;
            //else if (target.x < 180 && target.x > 0)
            //    target.x = target.x > 30 ? 30 : target.x;
            muzzle.transform.localEulerAngles = target;
        }
        else
        {

            if (gesture.swipe == EasyTouch.SwipeDirection.Right)
            {
                Vector3 target = new Vector3(transform.position.x + (gesture.deltaPosition.x * swipeSensivity), transform.position.y, transform.position.z);
                if (target.x > 2.5f)
                {
                    target.x = 2.5f;
                }
                splineFollower.motion.offset = target;
                if (!isRight)
                {
                    isRight = true;
                    isLeft = false;

                    StartCoroutine(RightRotation());



                }

            }
            if (gesture.swipe == EasyTouch.SwipeDirection.Left)
            {
                Vector3 target = new Vector3(transform.position.x - (gesture.deltaPosition.x * swipeSensivity * -1), transform.position.y, transform.position.z);
                if (target.x < -2.5f)
                {
                    target.x = -2.5f;
                }
                splineFollower.motion.offset = target;

                if (!isLeft)
                {
                    isRight = false;
                    isLeft = true;


                    StartCoroutine(LeftRotation());


                }

            }
        }


     



        //Vector3 target = new Vector3(transform.position.x + (gesture.deltaPosition.x * swipeSensivity), transform.position.y, transform.position.z);

     
    }

    private Vector3 DOLocalRotate(Vector3 vector3, float v)
    {
        throw new NotImplementedException();
    }

    private void On_TouchStart(Gesture gesture)
    {
        isTouch = true;
    }

    private void On_TouchUp(Gesture gesture)
    {

        StartCoroutine(BaseRotation());

        isTouch = false;

    }
    private void On_TouchDown(Gesture gesture)
    {

    }

    private void Update()
    {
       
        CardHouseScript cardHouseScript = cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber];

        if (cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber].isFull && !isControl)
        {
            Debug.Log("levelnumber 0");
            StartCoroutine(ShotMechanic(cardHouseScript));
            isControl = true;

        }
        //if (cardHouseManager.houseLevelNumber==0)
        //{
            
        //}
        //else if(cardHouseManager.houseLevelNumber > 0)
        //{
        //    if (cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber].isFull && !isControl)
        //    {

        //        Debug.Log("levelnumber 0dan büyük");
        //        StartCoroutine(ShotMechanic(cardHouseScript));
        //        isControl = true;

        //    }
        //}
     
  


        if (GameManager.Instance.isFail==true&&isBool==true)
        {
            Cards[currentNumber-1].GetComponent<Animator>().SetBool("isFail", true);
            isBool = false;
        }
        if (isStart)
        {
            Cards[currentNumber - 1].GetComponent<Animator>().SetBool("isStart", true);
        }




        if (AddedCardsA.Count >= 2 )
        {
            var mergeCard= Instantiate(InstantiateCards[1], AddedCardsA[0].transform.position, Quaternion.identity);
            AddedCards2.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
          
            AddedCardsA[0].transform.DOMove(AddedCardsA[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCardsA[1].gameObject);
            AddedCardsA.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCardsA[0].gameObject);
            
                AddedCardsA.RemoveAt(0);
             
            });



        }
        else if (AddedCards2.Count >= 2)
        {
            var mergeCard = Instantiate(InstantiateCards[2], AddedCards2[0].transform.position, Quaternion.identity);
            AddedCards3.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCards2[0].transform.DOMove(AddedCards2[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCards2[1].gameObject);
            AddedCards2.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards2[0].gameObject);

                AddedCards2.RemoveAt(0);

            });

            followCardNumber -= 1;

        }  
        else if (AddedCards3.Count >= 2 )
        {
            var mergeCard = Instantiate(InstantiateCards[3], AddedCards3[0].transform.position, Quaternion.identity);
            AddedCards4.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCards3[0].transform.DOMove(AddedCards3[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCards3[1].gameObject);
            AddedCards3.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards3[0].gameObject);

                AddedCards3.RemoveAt(0);

            });
            followCardNumber -= 2;

        }
        else if (AddedCards4.Count >= 2 )
        {
            var mergeCard = Instantiate(InstantiateCards[4], AddedCards4[0].transform.position, Quaternion.identity);
            AddedCards5.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCards4[0].transform.DOMove(AddedCards4[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCards4[1].gameObject);
            AddedCards4.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards4[0].gameObject);

                AddedCards4.RemoveAt(0);

            });
            followCardNumber -= 3;
        }
        else if (AddedCards5.Count >= 2 )
        {
            var mergeCard = Instantiate(InstantiateCards[5], AddedCards5[0].transform.position, Quaternion.identity);
            AddedCards6.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCards5[0].transform.DOMove(AddedCards5[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCards5[1].gameObject);
            AddedCards5.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards5[0].gameObject);

                AddedCards5.RemoveAt(0);

            });
            followCardNumber -= 4;
        }    
        else if (AddedCards6.Count >= 2 )
        {
            var mergeCard = Instantiate(InstantiateCards[6], AddedCards6[0].transform.position, Quaternion.identity);
            AddedCards7.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCards6[0].transform.DOMove(AddedCards6[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCards6[1].gameObject);
            AddedCards6.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards6[0].gameObject);

                AddedCards6.RemoveAt(0);

            });
            followCardNumber -= 5;
        }   
        else if (AddedCards7.Count >= 2 )
        {
            var mergeCard = Instantiate(InstantiateCards[7], AddedCards7[0].transform.position, Quaternion.identity);
            AddedCards8.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCards7[0].transform.DOMove(AddedCards7[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCards7[1].gameObject);
            AddedCards7.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards7[0].gameObject);

                AddedCards7.RemoveAt(0);

            });
            followCardNumber -= 6;
        }
        else if (AddedCards8.Count >= 2 )
        {
            var mergeCard = Instantiate(InstantiateCards[8], AddedCards8[0].transform.position, Quaternion.identity);
            AddedCards9.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCards8[0].transform.DOMove(AddedCards8[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCards8[1].gameObject);
            AddedCards8.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards8[0].gameObject);

                AddedCards8.RemoveAt(0);

            });
            followCardNumber -= 7;
        }   
        else if (AddedCards9.Count >= 2)
        {
            var mergeCard = Instantiate(InstantiateCards[9], AddedCards9[0].transform.position, Quaternion.identity);
            AddedCards10.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCards9[0].transform.DOMove(AddedCards9[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCards9[1].gameObject);
            AddedCards9.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards9[0].gameObject);

                AddedCards9.RemoveAt(0);

            });
            followCardNumber -= 8;
        }  
        else if (AddedCards10.Count >= 2)
        {
            Debug.Log("eror");
  
            var mergeCard = Instantiate(InstantiateCards[10], AddedCards10[0].transform.position, Quaternion.identity);
            AddedCardsJ.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCards10[0].transform.DOMove(AddedCards10[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCards10[1].gameObject);
            AddedCards10.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards10[0].gameObject);

                AddedCards10.RemoveAt(0);

            });
            followCardNumber -= 9;
        }
            
        else if (AddedCardsJ.Count >= 2 )
        {
            var mergeCard = Instantiate(InstantiateCards[11], AddedCardsJ[0].transform.position, Quaternion.identity);
            AddedCardsQ.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCardsJ[0].transform.DOMove(AddedCardsJ[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCardsJ[1].gameObject);
            AddedCardsJ.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCardsJ[0].gameObject);

                AddedCardsJ.RemoveAt(0);

            });
            followCardNumber -= 10;
        }  
        else if (AddedCardsQ.Count >= 2 )
        {
            var mergeCard = Instantiate(InstantiateCards[12], AddedCardsQ[0].transform.position, Quaternion.identity);
            AddedCardsK.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCardsQ[0].transform.DOMove(AddedCardsQ[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCardsQ[1].gameObject);
            AddedCardsQ.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCardsQ[0].gameObject);

                AddedCardsQ.RemoveAt(0);

            });
            followCardNumber -= 11;
        }  
        else if (AddedCardsK.Count >= 2 )
        {
            Debug.Log("eror12");
            var mergeCard = Instantiate(InstantiateCards[13], AddedCardsK[0].transform.position, Quaternion.identity);
            AddedCards13.Add(mergeCard);
            mergeCard.GetComponent<CardScript>().isBack = false;
            mergeCard.GetComponent<CardScript>().isFollow = true;
            mergeCard.GetComponent<CardScript>().anim.SetBool("isFollow", true);
            mergeCard.transform.SetParent(baseLevel.transform);
            mergeCard.transform.DOScale(bigSize, 0.5f).OnComplete(() => mergeCard.transform.DOScale(smallSize, 0.5f));
            AddedCardsK[0].transform.DOMove(AddedCardsK[1].transform.position, triggerAnimationSpeed);
            Destroy(AddedCardsK[1].gameObject);
            AddedCardsK.RemoveAt(1);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCardsK[0].gameObject);

                AddedCardsK.RemoveAt(0);

            });
            
            followCardNumber -= 12;
        }







        if (AddedCardsA.Count > 0 && currentNumber == 1)
        {
            
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[1].SetActive(true);

            Cards[1].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[1].GetComponent<Animator>().SetBool("isMerge", false);
            });
            //Destroy(other.gameObject);
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));

            AddedCardsA[0].transform.DOMove(transform.position, triggerAnimationSpeed);

            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCardsA[0].gameObject);
                AddedCardsA.RemoveAt(0);
            });
      
            //other.GetComponent<CardScript>().Animation();
            //spawnFx.Play();

            currentNumber++;
            followCardNumber -= 1;
           
   

        }
        else if (AddedCards2.Count > 0 && currentNumber == 2)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[2].SetActive(true);
            Cards[2].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[2].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards2[0].gameObject);
                AddedCards2.RemoveAt(0);
            });
            followCardNumber -= 2;


        } 
        else if (AddedCards3.Count > 0 && currentNumber == 3)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[3].SetActive(true);
            Cards[3].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[3].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCards3[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards3[0].gameObject);
                AddedCards3.RemoveAt(0);
            });
            followCardNumber -= 3;

        } 
        else if (AddedCards4.Count > 0 && currentNumber == 4)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[4].SetActive(true);
            Cards[4].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[4].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCards4[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards4[0].gameObject);
                AddedCards4.RemoveAt(0);
            });
            followCardNumber -= 4;
        }
        else if (AddedCards5.Count > 0 && currentNumber == 5)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[5].SetActive(true);
            Cards[5].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[5].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCards5[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards5[0].gameObject);
                AddedCards5.RemoveAt(0);
            });
            followCardNumber -= 5;
        } 
        else if (AddedCards6.Count > 0 && currentNumber == 6)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[6].SetActive(true);
            Cards[6].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[6].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCards6[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards6[0].gameObject);
                AddedCards6.RemoveAt(0);
            });
            followCardNumber -= 6;
        }
        else if (AddedCards7.Count > 0 && currentNumber ==7)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[7].SetActive(true);
            Cards[7].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[7].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCards7[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards7[0].gameObject);
                AddedCards7.RemoveAt(0);
            });
            followCardNumber -= 7;
        }
          
        else if (AddedCards8.Count > 0 && currentNumber ==8)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[8].SetActive(true);
            Cards[8].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[8].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCards8[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards8[0].gameObject);
                AddedCards8.RemoveAt(0);
            });
            followCardNumber -= 8;
        }
        else if (AddedCards9.Count > 0 && currentNumber ==9)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[9].SetActive(true);
            Cards[9].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[9].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCards9[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards9[0].gameObject);
                AddedCards9.RemoveAt(0);
            });
            followCardNumber -= 9;
        }
        else if (AddedCards10.Count > 0 && currentNumber ==10)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[10].SetActive(true);
            Cards[10].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[10].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCards10[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCards10[0].gameObject);
                AddedCards10.RemoveAt(0);
            });
            followCardNumber -= 10;
        }
          
        else if (AddedCardsJ.Count > 0 && currentNumber ==11)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[11].SetActive(true);
            Cards[11].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[11].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCardsJ[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCardsJ[0].gameObject);
                AddedCardsJ.RemoveAt(0);
            });
            followCardNumber -= 11;
        } 
        else if (AddedCardsQ.Count > 0 && currentNumber ==12)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[12].SetActive(true);
            Cards[12].GetComponent<Animator>().SetBool("isMerge", true);
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[12].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCardsQ[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCardsQ[0].gameObject);
                AddedCardsQ.RemoveAt(0);
            });
            followCardNumber -= 12;
        }
        else if (AddedCardsK.Count > 0 && currentNumber ==13)
        {
            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[13].SetActive(true);
            Cards[13].GetComponent<Animator>().SetBool("isMerge", true);
           
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Cards[13].GetComponent<Animator>().SetBool("isMerge", false);
            });
            currentNumber++;
            transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            //AddedCards2[0].transform.DOMove(transform.position, triggerAnimationSpeed).OnComplete(() => Destroy(AddedCards2[0].gameObject));
            AddedCardsK[0].transform.DOMove(transform.position, triggerAnimationSpeed);
            
            DOVirtual.DelayedCall(0.5f, () =>
            {
                Destroy(AddedCardsK[0].gameObject);
                AddedCardsK.RemoveAt(0);
            });
            followCardNumber -= 13;
        }
    }


    public IEnumerator RightRotation()
    {
        //playerController transform scale
        Vector3 startPos = new Vector3(0, 0, 0);
        //Vector3 endPos = new Vector3(0, 45, 0);
        Quaternion endPos = Quaternion.Euler(0, 45, 0);


        //parenthole initial mesh collider scale



        float t = 0;
        while (t <= 0.4f)
        {
            t += Time.deltaTime;


            //splineFollower.motion.rotationOffset = Vector3.Lerp(splineFollower.motion.rotationOffset, endPos, t); 
            rotationPlayer.rotation = Quaternion.Slerp(rotationPlayer.rotation, endPos, t);

            yield return null;
        }
    }
      
    public IEnumerator LeftRotation()
    {
        //playerController transform scale
        Vector3 startPos = new Vector3(0, 0, 0);
        //Vector3 endPos = new Vector3(0, -45, 0);
        Quaternion endPos = Quaternion.Euler(0, -45, 0);
            //new Quaternion(0, -45, 0,20);

        //parenthole initial mesh collider scale



        float t = 0;
        while (t <= 0.4f)
        {
            t += Time.deltaTime;
        

            //splineFollower.motion.rotationOffset = Vector3.Lerp(splineFollower.motion.rotationOffset, endPos, t);
            rotationPlayer.rotation = Quaternion.Slerp(rotationPlayer.rotation, endPos, t);
            yield return null;
        }
    }    
    public IEnumerator BaseRotation()
    {
        //playerController transform scale
        //Vector3 startPos = new Vector3(0, 0, 0);
        Quaternion startPos = Quaternion.Euler(0, 0, 0);
 

        //parenthole initial mesh collider scale



        float t = 0;
        while (t <= 1f)
        {
            t += Time.deltaTime;

            rotationPlayer.rotation = Quaternion.Slerp(rotationPlayer.rotation, startPos, t);
            //splineFollower.motion.rotationOffset = Vector3.Lerp(splineFollower.motion.rotationOffset, startPos, t);
            yield return null;
        }
    }

  
    private void OnTriggerStay(Collider other)
    {
       
    }

    IEnumerator delayFor(CardHouseScript other)
    {
        CardHouseScript cardHouseScript = other.GetComponentInParent<CardHouseManager>().CardHouseObject[other.GetComponentInParent<CardHouseManager>().houseLevelNumber];
        int cardnumber = currentNumber;
        int cardHouseNumber = currentNumber + cardHouseScript.cardTotalNumber;
    

        for (int i = cardHouseScript.cardTotalNumber; i < cardHouseNumber; i++)
        {
            if (cardHouseScript.cardTotalNumber < cardHouseScript.CardsLocation.Count&& !other.GetComponentInParent<CardHouseManager>().CardHouseObject[other.GetComponentInParent<CardHouseManager>().houseLevelNumber].isFull)
            {
                cardnumber--;
                GameObject card = Instantiate(finalCardObject, transform.position, Quaternion.identity);


                card.transform.DORotateQuaternion(other.CardsLocation[i].transform.rotation, 0.5f);
                card.transform.DOMove(other.CardsLocation[i].transform.position, 0.5f);

                if (cardnumber > 0)
                {
                    foreach (GameObject item in Cards)
                    {
                        item.SetActive(false);
                    }
                    Cards[cardnumber - 1].SetActive(true);
                }
                else
                {
                    if (followCardNumber<14)
                    {
                        Cards[0].SetActive(false);
                    }
               
                }

                yield return new WaitForSeconds(0.5f);

                other.CardsLocation[i].SetActive(true);
                Destroy(card.gameObject);

                cardHouseScript.cardTotalNumber++;
         

            }
            else
            {
                //card house dolduğu zaman burası calısır
                foreach (GameObject item in Cards)
                {
                    item.SetActive(false);
                }
                Cards[0].SetActive(true);
                //cardHouseScript.gameObject.SetActive(false);
                other.GetComponentInParent<CardHouseManager>().CardHouseObject[other.GetComponentInParent<CardHouseManager>().houseLevelNumber].isFull = true;
                //other.GetComponentInParent<CardHouseManager>().houseLevelNumber++;

            }
        }

        StartCoroutine(FollowCarddelayFor(other));
       


    }
    IEnumerator FollowCarddelayFor(CardHouseScript other)
    {
        if (followCardNumber!=0)
        {
            int numMod = followCardNumber % 13;
            int numDivide = followCardNumber / 13;
            int distance = 0;
            Vector3 pos = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);

            foreach (GameObject item in Cards)
            {
                item.SetActive(false);
            }
            Cards[0].SetActive(true);

            //var mergeCardLow =  Instantiate(InstantiateCards[0], pos, Quaternion.identity);
            //mergeCardLow.GetComponent<CardScript>().isBack = false;
            //mergeCardLow.GetComponent<CardScript>().isFollow = false;
            //mergeCardLow.GetComponent<CardScript>().anim.SetBool("isIdle", true);
            //mergeCardLow.transform.SetParent(baseLevel.transform);


            CardHouseScript cardHouseScript = other.GetComponentInParent<CardHouseManager>().CardHouseObject[other.GetComponentInParent<CardHouseManager>().houseLevelNumber];
            int cardnumber = followCardNumber;
            int cardHouseNumber = followCardNumber + cardHouseScript.cardTotalNumber;
            bool houseIncrease = false;

            for (int i = cardHouseScript.cardTotalNumber; i < cardHouseNumber; i++)
            {
                if (cardHouseScript.cardTotalNumber < cardHouseScript.CardsLocation.Count && !other.GetComponentInParent<CardHouseManager>().CardHouseObject[other.GetComponentInParent<CardHouseManager>().houseLevelNumber].isFull)
                {
                    cardnumber--;
                    GameObject card = Instantiate(finalCardObject, transform.position, Quaternion.identity);


                    card.transform.DORotateQuaternion(other.CardsLocation[i].transform.rotation, 0.5f);
                    card.transform.DOMove(other.CardsLocation[i].transform.position, 0.5f);

                    if (cardnumber > 0)
                    {

                    }
                    else
                    {
                        Cards[0].SetActive(false);


                    }

                    yield return new WaitForSeconds(0.5f);

                    other.CardsLocation[i].SetActive(true);
                    Destroy(card.gameObject);

                    cardHouseScript.cardTotalNumber++;


                }
                else
                {

                    //card house dolduğu zaman burası calısır

                    //cardHouseScript.gameObject.SetActive(false);
                    other.GetComponentInParent<CardHouseManager>().CardHouseObject[other.GetComponentInParent<CardHouseManager>().houseLevelNumber].isFull = true;
                    //if (!houseIncrease)
                    //{
                    //    other.GetComponentInParent<CardHouseManager>().houseLevelNumber++;
                    //    houseIncrease = true;

                    //}

                }
            }
        }
     
        if (!other.GetComponentInParent<CardHouseManager>().CardHouseObject[other.GetComponentInParent<CardHouseManager>().houseLevelNumber].isFull)
        {
            GameManager.Instance.CompleteLevel();
        }

    



    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Final"))
        {
            Cards[0].GetComponent<Animator>().SetBool("isFinish", true);
            if (AddedCardsA.Count > 0)
            {
                AddedCardsA[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCardsA[0].gameObject.SetActive(false));
            }
            if (AddedCards2.Count > 0)
            {
                AddedCards2[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCards2[0].gameObject.SetActive(false));
            }
            if (AddedCards3.Count > 0)
            {
                AddedCards3[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCards3[0].gameObject.SetActive(false));
            }
            if (AddedCards4.Count > 0)
            {
                AddedCards4[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCards4[0].gameObject.SetActive(false));
            }
            if (AddedCards5.Count > 0)
            {
                AddedCards5[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCards5[0].gameObject.SetActive(false));
            }
            if (AddedCards6.Count > 0)
            {
                AddedCards6[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCards6[0].gameObject.SetActive(false));
            }
            if (AddedCards7.Count > 0)
            {
                AddedCards7[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCards7[0].gameObject.SetActive(false));
            }
            if (AddedCards8.Count > 0)
            {
                AddedCards8[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCards8[0].gameObject.SetActive(false));
            }
            if (AddedCards9.Count > 0)
            {
                AddedCards9[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCards9[0].gameObject.SetActive(false));
            }
            if (AddedCards10.Count > 0)
            {
                AddedCards10[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCards10[0].gameObject.SetActive(false));
            }
            if (AddedCardsJ.Count > 0)
            {
                AddedCardsJ[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCardsJ[0].gameObject.SetActive(false));
            }
            if (AddedCardsQ.Count > 0)
            {
                AddedCardsQ[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCardsQ[0].gameObject.SetActive(false));
            }
            if (AddedCardsK.Count > 0)
            {
                AddedCardsK[0].transform.DOMove(transform.position, 0.5f).OnComplete(() => AddedCardsK[0].gameObject.SetActive(false));
            }
            if (AddedCards13.Count > 0)
            {
                foreach (var item in AddedCards13)
                {
                    item.transform.DOMove(transform.position,0.5f).OnComplete(() =>item.gameObject.SetActive(false));
                }

            }


         
            CardHouseManager cardHouseManager = other.GetComponentInParent<CardHouseManager>();
            StartCoroutine(delayFor(cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber]));

            //if (!cardHouseManager.CardHouseObject[cardHouseManager.houseLevelNumber].isFull)
            //{

            //    GameManager.Instance.CompleteLevel();

            //}

     

            splineFollower.followSpeed = 0f;
          
        }


        if (other.CompareTag("As"))
        {
            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCardsA.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;

      
                followCardNumber += 1;

            }
        }
        else if (other.CompareTag("Two"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCards2.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;

          
                followCardNumber += 2;
            }
        }
        else if (other.CompareTag("Three"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCards3.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;

         
                followCardNumber += 3;
            }
        }
        else if (other.CompareTag("Four"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCards4.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;


                followCardNumber += 4;
            }
        }
        else if (other.CompareTag("Five"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCards5.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;

                followCardNumber += 5;

            }
        }
        else if (other.CompareTag("Six"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCards6.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;
                followCardNumber += 6;
      
                
            }
        }
        else if (other.CompareTag("Seven"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCards7.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;
                followCardNumber += 7;
         
               
            }
        }
        else if (other.CompareTag("Eight"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCards8.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;
                followCardNumber += 8;
       
            }
        }
        else if (other.CompareTag("Nine"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCards9.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;

                followCardNumber += 9;
      
            }
        }
        else if (other.CompareTag("Ten"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCards10.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;
   
                followCardNumber += 10;
            }
        }
        else if (other.CompareTag("Jack"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCardsJ.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;
           
                followCardNumber += 11;
            }
        }
        else if (other.CompareTag("Queen"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCardsQ.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;
         
                followCardNumber += 12;
            }
        }
        else if (other.CompareTag("King"))
        {

            if (other.GetComponent<CardScript>().isBack)
            {
                AddedCardsK.Add(other.gameObject);
                other.GetComponent<CardScript>().isFollow = true;
                other.GetComponent<CardScript>().anim.SetBool("isFollow", true);
                other.GetComponent<CardScript>().isBack = false;
       
                followCardNumber += 13;
            }
        }

        if (other.CompareTag("Floor"))
        {
            //zemine bastığı zaman kart fırlatma fonksiyonu 
            ObstacleCuttingAnimation(other);
            other.GetComponentInParent<DoorScript>().isCut = true;
            //DecreaseCard();

        }
        if (other.CompareTag("Wall"))
        {
            if (other.GetComponentInParent<DoorScript>() != null)
            {
                if (!other.GetComponentInParent<DoorScript>().isCut)
                {
                    DropCard(other);
                }
            }
        }
        if (other.CompareTag("LockedDoorTrigger"))
        {
          
            
            if (!other.GetComponentInParent<LockedDoorScript>().isUse)
            {
                splineFollower.followSpeed = 0f;
                if (other.GetComponentInParent<LockedDoorScript>().passedNumber <= currentNumber)
                {
                    var droppedCardTemp = Instantiate(enterCard, transform.position, Quaternion.identity);
                    droppedCardTemp.transform.parent = trash.transform;
                    entryCard = other.GetComponentInParent<LockedDoorScript>().entryCard;
                    finishLine = other.GetComponentInParent<LockedDoorScript>().finishLine;
                    //transform.DOScale(XsmallSize, 0.75f).OnComplete(() => transform.DOScale(smallSize, 0.75f));
                    DOVirtual.DelayedCall(1f, () =>
                    {
                        currentNumber--;
                        if (currentNumber > 0)
                        {

                            foreach (GameObject item in Cards)
                            {
                                item.SetActive(false);
                            }
                            splineFollower.followSpeed = 3f;
                            Cards[currentNumber - 1].SetActive(true);

                        }
                        else
                        {
                            GameManager.Instance.FailLevel();
                            splineFollower.followSpeed = 0;
                        }
                       
                        other.GetComponentInParent<LockedDoorScript>().isPassed = true;
                    });
                }
                else
                {
                    GameManager.Instance.FailLevel();
                    splineFollower.followSpeed = 0;
                }
                other.GetComponentInParent<LockedDoorScript>().isUse = true;
            }
            //transform.DOMove(other.GetComponentInParent<LockedDoorScript>().entryCard.position, 1f);
        }

        if (other.CompareTag("ObstacleHand"))
        {
            
            currentNumber--;
            other.GetComponent<HandScript>().hand.DOMoveY(0.5f, 0.5f).OnComplete(() => other.GetComponent<HandScript>().hand.DOMoveY(3, 1f)) ;

            if (currentNumber > 0)
            {
                Cards[currentNumber].GetComponent<Animator>().SetBool("isStart", true);
                var droppedCardTemp = Instantiate(droppedCard, transform.position, Quaternion.identity);
                droppedCardTemp.transform.parent = trash.transform;
                transform.DOScale(XsmallSize, 0.75f).OnComplete(() => transform.DOScale(smallSize, 0.75f));
                foreach (GameObject item in Cards)
                {
                    item.SetActive(false);
                }
                Cards[currentNumber - 2].SetActive(true);

            }
            else
            {
                GameManager.Instance.FailLevel();
                splineFollower.followSpeed = 0;
            }

        }
        if (other.CompareTag("Obstacle"))
        {
            if (currentNumber > 0)
            {
                var droppedCardTemp=Instantiate(droppedCard, transform.position, Quaternion.identity);
                droppedCardTemp.transform.parent = trash.transform;
                transform.DOScale(XsmallSize, 0.75f).OnComplete(() => transform.DOScale(smallSize, 0.75f));
                foreach (GameObject item in Cards)
                {
                    item.SetActive(false);
                }
                Cards[currentNumber - 2].SetActive(true);
   
            }
            else
            {
                GameManager.Instance.FailLevel();
                splineFollower.followSpeed = 0;
            }
            currentNumber--;
        }


    }
    private void OnTriggerExit(Collider other)
    {
        //Cards[currentNumber].GetComponent<Animator>().SetBool("isMerge", false);
    }

    public void ObstacleCuttingAnimation(Collider other)
    {
      
        targetObstacle = other.GetComponentInParent<DoorScript>().targetObject;
 
   
    }

    public void DropCard(Collider other)
    {
        if (!other.GetComponentInParent<DoorScript>().isCollision)
        {
            currentNumber--;
            if (currentNumber > 0)
            {
                Cards[currentNumber].GetComponent<Animator>().SetBool("isStart", true);
                var droppedCardTemp = Instantiate(droppedCard, transform.position, Quaternion.identity);
                //droppedCardTemp.transform.SetParent(trash.transform);
                droppedCardTemp.transform.parent = trash.transform;


                foreach (GameObject item in Cards)
                {
                    item.SetActive(false);
                }
                Cards[currentNumber - 2].SetActive(true);
                //Cards[currentNumber-1].GetComponent<Animator>().SetBool("isMerge", true);
                //DOVirtual.DelayedCall(0.3f, () =>
                //{
                //    Cards[currentNumber-1].GetComponent<Animator>().SetBool("isMerge", false);
                //});
                transform.DOScale(bigSize, 0.5f).OnComplete(() => transform.DOScale(smallSize, 0.5f));
            }
            else
            {
                splineFollower.followSpeed = 0;
                foreach (GameObject item in Cards)
                {
                    item.SetActive(false);
                }
                var droppedCardTemp=Instantiate(droppedCard, transform.position, Quaternion.identity);
                droppedCardTemp.transform.parent = trash.transform;
                GameManager.Instance.FailLevel();
            }
            other.GetComponentInParent<DoorScript>().isCollision = true;
        }
      

    }

   IEnumerator ShotMechanic(CardHouseScript other)
    {
        isShooting = true;
        Cards[0].SetActive(false);
        shotCamera.Priority = 11;
        gameCamera.Priority = 10;
        shotCamera.GetComponent<Transform>().position = new Vector3(other.shotTransform.position.x, other.shotTransform.position.y + 0.5f, other.shotTransform.position.z - 3);
        for (int i = 0; i < 1000; i++)
        {
            if (isTouch)
            {
                var tempCard = Instantiate(finalCardThrow, other.shotTransform.position, other.shotTransform.rotation);
                tempCard.GetComponent<Rigidbody>().velocity = other.shotTransform.transform.forward * FinalcardThrowSpeed;
                tempCard.transform.parent = trash.transform;
            }
            if (other.destroyCount == 0)
            {
                other.destroyBuild = true;
                GameManager.Instance.CompleteLevel();
            }

            //shotCamera.LookAt = other.shotTransform;

            //tempCard.GetComponent<Rigidbody>().AddForce(other.transform.forward * 5);
            yield return new WaitForSeconds(0.25f);

            //Destroy(tempCard);
        }
        

    }
    public void IsFullControlFunction(CardHouseScript other)
    {

    }
 
}
