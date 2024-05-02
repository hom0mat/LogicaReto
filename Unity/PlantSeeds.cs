using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlantSeeds : MonoBehaviour
{
    [SerializeField] private GameObject objectToReplace;
    [SerializeField] private GameObject replacementObject;

    private bool seedingAllowed;

    public UIController controller;
    public Text seedsCountText; // Referencia al elemento de texto para mostrar la cantidad de semillas

    private ToolSelection farmingToolSelector; // Referencia al script de selección de herramienta
    public static int plantedSeeds = 0;
    private void Start()
    {
        replacementObject.gameObject.SetActive(false);
        farmingToolSelector = FindObjectOfType<ToolSelection>();
    }

    // Update is called once per frame
	private void Update () {
        if (seedingAllowed && ((Input.GetKeyDown(KeyCode.Space) || controller.SeedsAction == 1) && farmingToolSelector.currentTool == ToolSelection.FarmingTool.Plant))
        {
            if (DeleteWheat.seedsCount > 0)
            {
                Seeding();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Tractor"))
        {
            seedingAllowed = true;
        }        
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Tractor"))
        {
            seedingAllowed = false;
        }
    }

    private void Seeding()
    {
        plantedSeeds++;
        DeleteWheat.seedsCount--;
        // Actualizar el texto en la interfaz de usuario para mostrar la cantidad actual de maíz y semillas
        if (seedsCountText != null)
        {
            seedsCountText.text = "Seeds: " + DeleteWheat.seedsCount.ToString();
        }

        objectToReplace.gameObject.SetActive(false) ;
        replacementObject.gameObject.SetActive(true) ;
    }

}