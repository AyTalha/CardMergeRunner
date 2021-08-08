using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterCardScript : MonoBehaviour
{

    public PlayerScript playerScript;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    public List<Material> CardMaterials = new List<Material>();


    private void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();

        var materials = skinnedMeshRenderer.materials;
        for (int i = 0; i < 3; i++)
        {
            materials[i] = CardMaterials[playerScript.currentNumber - 2];

        }
        skinnedMeshRenderer.materials = materials;





    }
    void Start()
    {

        transform.DORotate(new Vector3(0, -90, 0), 0.75f);

        transform.DOJump(playerScript.entryCard.position, 1, 1, 0.75f).OnComplete(() => transform.DOMove(playerScript.finishLine.position, 0.5f));
        DOVirtual.DelayedCall(1.4f, () =>
        {
            Destroy(this.gameObject);
        });

     
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
