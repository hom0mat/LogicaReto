using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TractorCollision : MonoBehaviour
{

    public Text winLoseText;
    public Button menuBoton;
    public Image decoracion;
    public GameObject player;
    private bool canOpenChest = false;
    private bool chestOpened = false;

    public UIController controller;
    AudioManager audioManager;

    private void Awake()
    {
        //audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Rocks")
        {
            HealthManager.health--;
            //audioManager.PlaySFX(audioManager.rocksCollision);

            if (HealthManager.health <= 0)
            {
              
                winLoseText.gameObject.SetActive(true);
                menuBoton.gameObject.SetActive(true);
                decoracion.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }

        if (collision.gameObject.CompareTag("Win")) // Verifica si colisionó con el objeto del cofre
        {
            canOpenChest = true; // Habilita la posibilidad de abrir el cofre
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Win")) // Verifica si salió de la colisión con el cofre
        {
            canOpenChest = false; // Deshabilita la posibilidad de abrir el cofre
        }
    }

    private void Update()
    {
        if (canOpenChest && !chestOpened && (Input.GetKeyDown(KeyCode.Z) || controller.Action == 1))
        {
            // Abrir el cofre solo si está habilitada la apertura, no se ha abierto aún y se presionó la tecla Z
            OpenChest();
        }
    }

    private void OpenChest()
    {
        // Mostrar mensaje de victoria y activar elementos de la interfaz
        winLoseText.gameObject.SetActive(true);
        menuBoton.gameObject.SetActive(true);
        decoracion.gameObject.SetActive(true);
        Time.timeScale = 0; // Pausar el juego

        chestOpened = true; // Marcar el cofre como abierto para evitar abrirlo múltiples veces
    }
}


