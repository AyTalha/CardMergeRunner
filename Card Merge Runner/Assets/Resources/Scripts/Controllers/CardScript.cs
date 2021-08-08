using DG.Tweening;
using Hyperlab.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class CardScript : MonoBehaviour
{

    public GameObject Player;
    private NavMeshAgent navmeshAgent;
    public Animator anim;

    public bool isFollow;
    private bool isBool=true;
    public bool isBack = true;
    private bool isIf = true;
    public List<Material> CardMaterials = new List<Material>();
    public SkinnedMeshRenderer skinnedMeshRenderer;
    private void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    void Start()
    {
        Player = GameObject.Find("Player");
        navmeshAgent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {

        Destination();
        if (GameManager.Instance.isFail==true&&isBool==true)
        {
            anim.SetBool("isFail", true) ;
            isBool = false;
        }


    }
    public void Animation()
    {
        anim.SetBool("Trigger", true);

    }
    public void Destination()
    {
        if (isFollow&&!GameManager.Instance.isComplete)
        {
          
            navmeshAgent.destination = Player.transform.position;
        }
        else if (GameManager.Instance.isComplete&&isIf)
        {
            anim.SetBool("isIdle", true);
            anim.SetBool("isFollow", false);
            isIf = false;
        }
    }

}
