using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{

    #region Hints
    public GameObject hintInteractionUI;
    public Text taskHintUI;

    #endregion

    private static UserInterface userInterface;
    public static UserInterface GetInstance()
    {
        return userInterface;
    }

    private void Awake()
    {
        userInterface = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InteractionHintUIState(bool state)
    {
        hintInteractionUI.SetActive(state);
    }

    public void ShowTaskHint(string description){
        taskHintUI.text = description;
        taskHintUI.gameObject.SetActive(true);
    }

    public void HideTaskHint(){
        taskHintUI.gameObject.SetActive(false);
    }
}
