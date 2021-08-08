using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowCardScript : MonoBehaviour
{
    public PlayerScript playerScript;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    public List<Material> CardMaterials = new List<Material>();

    public float cardThrowSpeed;
    public float cardRotationSpeed;
    private void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
     
        var materials = skinnedMeshRenderer.materials;
        for (int i = 0; i < 3; i++)
        {
           materials[i] = CardMaterials[playerScript.currentNumber-1];

        }
        skinnedMeshRenderer.materials = materials;


       


    }
    void Start()
    {

        Vector3 target = new Vector3(playerScript.targetObstacle.position.x, playerScript.targetObstacle.position.y, playerScript.targetObstacle.position.z+3);
        transform.DORotate(new Vector3(90, 0, 0), 1f).OnComplete(() => transform.DOMove(target, cardThrowSpeed)) ;
  

    }

    // Update is called once per frame
    void Update()
    {
   
   



    }
    private void FixedUpdate()
    {
        transform.Rotate(0, 0, cardRotationSpeed);
        //skinnedMeshRenderer.material = CardMaterials[3];

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            DOVirtual.DelayedCall(0.3f, () =>
            {
                Destroy(gameObject);
            });
    
        }
       
    }
}
