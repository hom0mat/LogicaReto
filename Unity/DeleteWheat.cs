using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteWheat : MonoBehaviour
{
    public Text wheatCountText; // Referencia al elemento de texto para mostrar la cantidad de maíz
    public Text seedsCountText; // Referencia al elemento de texto para mostrar la cantidad de semillas
    public static int wheatCount = 0; // Contador de la cantidad de maíz cosechado
    public static int seedsCount = 0;

    private GameObject currentCorn = null; // Referencia al objeto de maíz con el que se ha colisionado
    private ToolSelection farmingToolSelector; // Referencia al script de selección de herramienta

    public UIController action;

    private GameManager gameManager; // Referencia al GameManager

    private void Start()
    {
        wheatCountText.text = "Wheat: " + wheatCount.ToString();
        seedsCountText.text = "Seeds: " + seedsCount.ToString();

        // Obtener referencia al script FarmingToolSelector
        farmingToolSelector = FindObjectOfType<ToolSelection>();

        // Obtener referencia al GameManager
        gameManager = FindObjectOfType<GameManager>(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wheat")) // Verifica si colisionó con un objeto de maíz
        {
            currentCorn = collision.gameObject; // Almacena la referencia al maíz colisionado
            gameManager.AddWheat(collision.gameObject); // Informa al GameManager sobre el trigo colisionado
        }

    
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wheat")) // Verifica si salió de la colisión con un objeto de maíz
        {
            currentCorn = null; // Borra la referencia al maíz colisionado
        }


    }
    private void Update()
    {
        if (farmingToolSelector.currentTool == ToolSelection.FarmingTool.Harvest && currentCorn != null && ((Input.GetKeyDown(KeyCode.Space)) || action.HarvestAction == 1))
        {
            HarvestCorn(currentCorn);
            currentCorn = null; // Borra la referencia después de interactuar con el maíz
        }
    }

    private void HarvestCorn(GameObject cornObject)
    {
        // Destruir el objeto de maíz específico al cosecharlo (quitar el sprite)
        Destroy(cornObject); // Destruye el objeto de maíz con el que se colisionó
        PlayerPrefs.SetInt("WheatHarvested", 1);

        // Aumentar la cantidad de maíz y semillas
        wheatCount++;
        seedsCount++;

        // Actualizar la interfaz de usuario para mostrar la cantidad actual de maíz y semillas
        UpdateCountUI();
    }


    public void UpdateCountUI()
    {
        // Actualizar el texto en la interfaz de usuario para mostrar la cantidad actual de maíz y semillas
        if (wheatCountText != null && seedsCountText != null)
        {
            wheatCountText.text = "Wheat: " + wheatCount.ToString();
            seedsCountText.text = "Seeds: " + seedsCount.ToString();
        }
    }
}
