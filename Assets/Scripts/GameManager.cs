using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instence;

    [Header("interaction")]
    [SerializeField] private GameObject _interactUI;
    [Header("dialogue")]
    [SerializeField] private GameObject _dialogueUI;
    [SerializeField] private Animator _dialogueAnimation;
    
    private void Awake()
    {
        instence = this;
    }

    private void Start()
    {
        ShowInteractUI(false);
    }

    public void ShowInteractUI(bool show)
    {
        _interactUI.SetActive(show);
    }

    public void ShowDialogue()
    {
        _dialogueAnimation.Play("DialogueShow");
    }

    public void HideDialogue()
    {
        _dialogueAnimation.Play("DialogueHide");
    }

}
