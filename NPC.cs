using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    public string[] dialogueNpc;
    public int dialogueIndex;

    public GameObject dialoguePanel;
    public Text dialogueText;

    //Variable de Npc
    AudioSource myAudio;
    public AudioClip speakSound;
    public Image ImageNpc;

    public GameObject dialogue_mark;

    public bool readyToSpeak;
    public bool startDialogue;


    //Referencia del PJ
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        dialoguePanel.SetActive(false);
        myAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    public void npcCharacter(InputAction.CallbackContext callback)
    {
        if(callback.performed && readyToSpeak)
        {
            if (!startDialogue)
            {
                StartDialogue();
            }
            else if (dialogueText.text == dialogueNpc[dialogueIndex])
            {
                NextDialogue();
            }
        }
    }


    void NextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex < dialogueNpc.Length)
        {
            StartCoroutine(ShowDialogue());
        }
        else
        {
            dialoguePanel.SetActive(false);
            startDialogue = false;
            dialogueIndex = 0;
        }
    }


    void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        startDialogue = true;
        dialogueIndex = 0;
        StartCoroutine(ShowDialogue());
    }

    IEnumerator ShowDialogue()
    {
        dialogueText.text = "";
        foreach (char letter in dialogueNpc[dialogueIndex])
        {
            dialogueText.text += letter;
            myAudio.PlayOneShot(speakSound);
            yield return new WaitForSeconds(0.09f);

        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            readyToSpeak = true;
            dialogue_mark.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            readyToSpeak = false;
            dialogue_mark.SetActive(false);
        }
    }
}
