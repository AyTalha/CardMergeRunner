using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorScript : MonoBehaviour
{
    public Transform rightDoor, LeftDoor,entryCard,finishLine;
    public bool isPassed;
    public bool isUse=false;
    public int passedNumber;
    public MeshRenderer doorColor;
    public Material greenColor;
    private PlayerScript playerScript;

    private void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DoorRotation();
    }
    public void DoorRotation()
    {
        if (isPassed)
        {
      
            if (playerScript.currentNumber>0)
            {
                var materials = doorColor.materials;
                materials[1] = greenColor;
                doorColor.materials = materials;
                LeftDoor.DOLocalMoveZ(1.7f, 1f);
                rightDoor.DOLocalMoveZ(-1.7f, 1f);
            }
        
        
            //LeftDoor.DORotate(new Vector3(0, -90, 0), 1f);
            //rightDoor.DORotate(new Vector3(0, 90, 0), 1f);
            isPassed = false;

        }

    }

}
