using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedCardScript : MonoBehaviour
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
            materials[i] = CardMaterials[playerScript.currentNumber-1];

        }
        skinnedMeshRenderer.materials = materials;





    }
}
