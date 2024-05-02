using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{

    AudioManager audioManager;
    // Start is called before the first frame update
    public void Jugar()
    {
        PlayerPrefs.SetInt("CameFromMainMenu", 1);
        PlayerPrefs.Save();

        PlayerPrefs.SetInt("SelectedTool", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        HealthManager.health = 3;
        DeleteWheat.wheatCount = 0;
        DeleteWheat.seedsCount = 0;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    public void Salir()
    {
        Debug.Log("Salir...");
        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
