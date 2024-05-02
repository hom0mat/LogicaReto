using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockWin : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject doorLeft;
    public GameObject doorRight;

    void Start()
    {
        //If the requirements are met, the door will unlock
        if (PlantSeeds.plantedSeeds >= 5 && DeleteWheat.wheatCount >= 5)
        {
            doorLeft.gameObject.SetActive(false);
            doorRight.gameObject.SetActive(false);
        }

    }
}
