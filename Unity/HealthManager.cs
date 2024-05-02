using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static int health = 3;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Text winLoseText;
    public Button menuBoton;
    public Image decoracion;

    //public SerialPort serialPort = new SerialPort("COM4", 115200);

    void Update()
    {
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }

        for (int i = 0; i < health; i++)
        {
            hearts[i].sprite = fullHeart;
        }

        if (HealthManager.health <= 0)
        {
            winLoseText.gameObject.SetActive(true);
            menuBoton.gameObject.SetActive(true);
            decoracion.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        /*
        if (serialPort.IsOpen)
        {
            //serialPort.Write(HealthManager.health.ToString());
        }

        */
    }
}
