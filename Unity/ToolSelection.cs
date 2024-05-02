using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolSelection : MonoBehaviour
{
    public Image toolSelection; // Referencia a la imagen para mostrar la herramienta seleccionada
    public Sprite harvestToolSprite; // Sprite para la herramienta de cosechar
    public Sprite plantToolSprite; // Sprite para la herramienta de plantar
    public Sprite noToolSprite;
    public enum FarmingTool { None, Harvest, Plant }; // Enumeración para las herramientas disponibles
    public FarmingTool currentTool = FarmingTool.None; // Herramienta inicialmente seleccionada

    public UIController action;

    private const string SelectedToolKey = "SelectedTool";

    private void Start()
    {
        // Cargar la herramienta seleccionada guardada (si existe)
        if (PlayerPrefs.HasKey(SelectedToolKey))
        {
            currentTool = (FarmingTool)PlayerPrefs.GetInt(SelectedToolKey);
        }

        UpdateToolSelection();
    }

    private void Update()
    {
        // Verifica las teclas presionadas para cambiar la herramienta
        if (Input.GetKeyDown(KeyCode.P) || action.HarvestTool == 1)
        {
            currentTool = FarmingTool.Harvest; // Selecciona la herramienta de cosechar (P)
            UpdateToolSelection();
            SaveSelectedTool();
        }

        if (Input.GetKeyDown(KeyCode.U) || action.PlantTool == 1)
        {
            currentTool = FarmingTool.Plant; // Selecciona la herramienta de plantar (U)
            UpdateToolSelection();
            SaveSelectedTool();
        }

        if (Input.GetKeyDown(KeyCode.I) || action.NoTool == 1)
        {
            currentTool = FarmingTool.None; // Selecciona ninguna herramienta (I)
            UpdateToolSelection();
            SaveSelectedTool();
        }
    }

    private void UpdateToolSelection()
    {
        // Actualiza la imagen de la herramienta seleccionada
        if (toolSelection != null)
        {
            switch (currentTool)
            {
                case FarmingTool.Harvest:
                    toolSelection.sprite = harvestToolSprite; // Asigna el sprite de cosechar
                    break;
                case FarmingTool.Plant:
                    toolSelection.sprite = plantToolSprite; // Asigna el sprite de plantar
                    break;
                case FarmingTool.None:
                    toolSelection.sprite = noToolSprite; // Asigna el sprite de ninguna herramienta
                    break;
                default:
                    break;
            }
        }
    }

    private void SaveSelectedTool()
    {
        // Guarda la herramienta seleccionada en PlayerPrefs
        PlayerPrefs.SetInt(SelectedToolKey, (int)currentTool);
        PlayerPrefs.Save();
    }
}
