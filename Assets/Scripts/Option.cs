using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    //the gamemeobjects that shows up when you select this option
    [SerializeField] GameObject selector;
    public bool isSelected;
    public void Selected() {
        selector.SetActive(true);
        isSelected = true;
    }
    public void Deselected() {
        selector.SetActive(false);
        isSelected = false;
    }
}
