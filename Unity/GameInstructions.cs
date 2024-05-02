using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameInstructions : MonoBehaviour
{
    public Text messageText;
    public Text continueText; // Text element for "Click to continue"
    public string[] messages;
    public float messageSpeed = 0.05f; // Speed at which characters appear in the message
    public GameObject player;

    public Text bossName;
    public Image boss;
    public Image dialogBox;

    private int currentMessageIndex = 0;
    private bool showingMessage = false;
    private bool messageFullyDisplayed = false;

    public UIController controller;

    private void Start()
    {

        bool cameFromMainMenu = PlayerPrefs.GetInt("CameFromMainMenu", 0) == 1;

        // Disable player movement at the start
        if (player != null)
        {
            player.GetComponent<TractorMovement>().enabled = false;
        }

        // Check if the message has already been shown
        if (!cameFromMainMenu)
        {
            // If message has been shown, skip showing messages
            EnablePlayerMovement();
        }
        else
        {
           ShowNextMessage();
        }
    }

    private void Update()
    {
        // Check for input to display next message or enable player movement
        if (showingMessage && messageFullyDisplayed && (Input.GetMouseButtonDown(0) || controller.Action == 1)) // Change Input to desired key/button
        {
            if (currentMessageIndex < messages.Length - 1)
            {
                currentMessageIndex++;
                ShowNextMessage();
            }
            else
            {
                // Enable player movement after all messages are shown
                EnablePlayerMovement();
            }
        }
    }

    private void ShowNextMessage()
    {
        // Display the next message character by character
        showingMessage = true;
        messageFullyDisplayed = false;
        messageText.gameObject.SetActive(true); // Show message text
        continueText.gameObject.SetActive(false); // Hide "Click to continue" text
        messageText.text = "";

        // Start showing the next message
        StartCoroutine(ShowMessageCoroutine(messages[currentMessageIndex]));
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        for (int i = 0; i < message.Length; i++)
        {
            messageText.text += message[i];
            yield return new WaitForSeconds(messageSpeed);
        }

        // Message fully displayed, show "Click to continue" text
        messageFullyDisplayed = true;
        continueText.gameObject.SetActive(true);
        continueText.text = "Click to continue"; // Update "Click to continue" text after message is fully displayed
    }

    private void EnablePlayerMovement()
    {
        if (player != null)
        {
            player.GetComponent<TractorMovement>().enabled = true;
        }
        showingMessage = false;
        messageText.gameObject.SetActive(false); // Hide message text
        continueText.gameObject.SetActive(false);
        boss.gameObject.SetActive(false);
        dialogBox.gameObject.SetActive(false);
        bossName.gameObject.SetActive(false);
        PlayerPrefs.SetInt("CameFromMainMenu", 0);
        PlayerPrefs.Save();
    }
}
