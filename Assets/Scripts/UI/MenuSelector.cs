using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelector : MonoBehaviour
{
    public Animator animatorTablet;
    public StartMenu startMenu;
    public NextMenu nextMenu;

    // Start is called before the first frame update
    void Start()
    {
        ShowStartMenu();
    }

    public void ShowStartMenu()
    {
        startMenu.gameObject.SetActive(true);
        nextMenu.gameObject.SetActive(false);
    }

    public void ShowNextMenu()
    {
        startMenu.gameObject.SetActive(false);
        nextMenu.gameObject.SetActive(true);
    }

    public void ShowTablet()
    {
        animatorTablet.SetTrigger("ShowTablet");
    }

    public void HideTablet()
    {
        animatorTablet.SetTrigger("HideTablet");
    }
}
