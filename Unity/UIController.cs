using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//****************************
//UART interface
using System;
using System.IO.Ports;
using Unity.VisualScripting.Dependencies.NCalc;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Text;
using System.Drawing;
//****************************

public class UIController : MonoBehaviour
{
    //************************** 
    //UART interface

    //Variables to store if methods are activated
    public int Action = 0;
    public int HarvestTool = 0;
    public int PlantTool = 0;
    public int LowSpeed = 0;
    public int NoTool = 0;

    public int moveRight = 0;
    public int moveLeft = 0;
    public int moveUp = 0;
    public int moveDown = 0;

    public int HarvestAction = 0;
    public int SeedsAction = 0;

    public int value;
    public int speedValue;
    public int toolValue;
    public int movementValue;
    public SerialPort serialPort = new SerialPort("COM3", 115200);
    //**************************

    void Start()
    {
        //UART interface
        if (!serialPort.IsOpen)
        {
            serialPort.Open();          //Abrimos una nueva conexión de puerto serie
            serialPort.ReadTimeout = 1; //Establecemos el tiempo de espera cuando una operación de lectura no finaliza
        }
    }

    void Update()
    {
        //UART interface
        if (!serialPort.IsOpen)
        {
            serialPort.Open();          //Abrimos una nueva conexión de puerto serie
            serialPort.ReadTimeout = 1; //Establecemos el tiempo de espera cuando una operación de lectura no finaliza
        }

        if (serialPort.IsOpen)
        {
            try
            {
                if (serialPort.BytesToRead > 0)             //Verificamos si existe un nuevo dato recibido
                {
                    value = serialPort.ReadByte();          //Leemos el dato de 8 bits
                    speedValue = serialPort.ReadByte();
                    toolValue = serialPort.ReadByte();
                    movementValue = serialPort.ReadByte();  //Direccion de la marcha
                    serialPort.DiscardInBuffer();

                }
                else
                {
                    value = 0;
                }

                Debug.Log(movementValue);
                Debug.Log(value);

                //----ELECCIÓN DE HERRAMIENTA----
                //Both swithces on, no tool on
                if (toolValue == 0x03)
                {
                    NoTool = 1;
                    PlantTool = 0;
                    HarvestTool = 0;
                }
                //Switch 0 up, harvest tool on
                else if ((toolValue & 1) != 0)
                {
                    HarvestTool = 1;
                    NoTool = 0;
                    PlantTool = 0;
                }

                //Switch 1 up, seeds tool on
                else if ((toolValue & 2) != 0)
                {
                    PlantTool = 1;
                    NoTool = 0;
                    HarvestTool = 0;
                }

                //No tool selected
                else
                {
                    NoTool = 1;
                    PlantTool = 0;
                    HarvestTool = 0;
                }

                //----ACCIONES----
                if (value  == 0x08)
                {
                    Action = 1;
                }

                //(25 & 41 & 73 & 137)
                //      
                else if (value == 0x09 || value == 0x0D || value == 0x19 || value == 0x1D || value == 0x29 || value == 0x2D || value == 0x49 || value == 0x4D || value == 0x89 || value == 0x8D)
                {
                    HarvestAction = 1;
                    SeedsAction = 0;
                }

                else if (value == 0x55 || value == 0x51 || value == 0x95 || value == 0x91 || value == 0xA5 || value == 0xA1 || value == 0x65 || value == 0x61)
                {
                    HarvestAction = 1;
                    SeedsAction = 0;
                }

                //(26 & 42 & 74 & 138)
                else if (value == 0x0A || value == 0x0E || value == 0x1A || value == 0x1E || value == 0x2A || value == 0x2E || value == 0x4A || value == 0x4E || value == 0x8A || value == 0x8E)
                {
                    SeedsAction = 1;
                    HarvestAction = 0;
                }

                else if (value == 0x56 || value == 0x52 || value == 0x96 || value == 0x92 || value == 0xA6 || value == 0xA2 || value == 0x66 || value == 0x62)
                {
                    HarvestAction = 0;
                    SeedsAction = 1;
                }

                else
                {
                    HarvestAction = 0;
                    SeedsAction = 0;
                    Action = 0;
                }

                //----VELOCIDAD DEL TRACTOR----
                int bitPosition = 2;            // Posición del bit que quieres verificar (empezando desde 0)
                int mask = 1 << bitPosition;    // Crear una máscara desplazando 1 hacia la izquierda por la posición del bit
                if ((speedValue & mask) == 0)
                {
                    // Bit 2 está activado
                    LowSpeed = 0;
                }
                else
                {
                    LowSpeed = 1;
                }


                if ((movementValue & 16) != 0)
                {
                    moveRight = 1;
                    moveLeft = 0;
                    moveUp = 0;
                    moveDown = 0;

                }
                else if((movementValue & 32) != 0)
                {
                    moveRight = 0;
                    moveLeft = 1;
                    moveUp = 0;
                    moveDown = 0;
                }
                else if ((movementValue & 64) != 0)
                {
                    moveRight = 0;
                    moveLeft = 0;
                    moveUp = 1;
                    moveDown = 0;
                }
                else if ((movementValue & 128) != 0)
                {
                    moveRight = 0;
                    moveLeft = 0;
                    moveUp = 0;
                    moveDown = 1;
                }
                else
                {
                    moveRight = 0;
                    moveLeft = 0;
                    moveUp = 0;
                    moveDown = 0;
                }

                serialPort.Write(HealthManager.health.ToString());  

            }
            catch { }
        }
    }

    //private byte[] DataToBin (int data)
    //{
    //    byte[] dataByte = new Byte[1];
    //    dataByte[0] = (byte)(data & 0xFF);

    //    return dataByte;

    //}
}

