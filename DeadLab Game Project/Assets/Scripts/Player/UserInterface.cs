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

    #region Weapon
    public Text bulletCountUI;
    #endregion


    #region SINGLETON PATTERN
    private static UserInterface userInterface;
    public static UserInterface GetInstance()
    {
        return userInterface;
    }

    private void Awake()
    {
        userInterface = this;
    }

    #endregion

    public void InteractionHintUIState(bool state)
    {
        hintInteractionUI.SetActive(state);
    }

    public void ShowTaskHint(string description)
    {
        taskHintUI.text = description;
        taskHintUI.gameObject.SetActive(true);
    }

    public void HideTaskHint()
    {
        taskHintUI.gameObject.SetActive(false);
    }

    public void bulletCounteUpdate(int count)
    {
        bulletCountUI.text = "" + count;
    }
}
