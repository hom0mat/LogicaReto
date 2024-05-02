using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject wheatPrefab; // Prefab del trigo
    public Transform spawnPoint; // Punto de aparición del trigo

    private List<GameObject> wheatList = new List<GameObject>(); // Lista de trigos
    private List<bool> wheatHarvestedStates = new List<bool>(); // Lista de estados de cosecha de los trigos

    void Start()
    {
        // Cargar el estado de la lista de trigos
        LoadWheatStates();

        // Generar los trigos según su estado de cosecha
        for (int i = 0; i < wheatList.Count; i++)
        {
            if (!wheatHarvestedStates[i])
            {
                Instantiate(wheatPrefab, wheatList[i].transform.position, Quaternion.identity);
            }
        }
    }

    public void AddWheat(GameObject wheat)
    {
        // Añadir el trigo a la lista y establecer su estado inicial como no cosechado
        wheatList.Add(wheat);
        wheatHarvestedStates.Add(false);
    }

    public void HarvestWheat(GameObject wheat)
    {
        // Marcar el trigo como cosechado
        int index = wheatList.IndexOf(wheat);
        wheatHarvestedStates[index] = true;

        // Eliminar el trigo de la lista si lo deseas
        // wheatList.RemoveAt(index);
        // wheatHarvestedStates.RemoveAt(index);
    }

    private void LoadWheatStates()
    {
        // Cargar el estado de la lista de trigos desde algún lugar persistente (por ejemplo, PlayerPrefs)
        // Por simplicidad, aquí simplemente inicializamos todas las cosechas como no realizadas
        foreach (GameObject wheat in wheatList)
        {
            wheatHarvestedStates.Add(false);
        }
    }
}