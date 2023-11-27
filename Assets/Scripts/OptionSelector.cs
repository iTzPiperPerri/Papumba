using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSelector : MonoBehaviour
{
    [SerializeField] int maxOptionsSelected;
    [SerializeField] List<Option> options;
    [SerializeField] List<Option> currentSelectedOptions;
    [SerializeField] int numOptionsSelected;
    [SerializeField] GameObject warning;
    [SerializeField] Button blockButton;
    CTAanimation cTAanimation;
    bool onlyOnce;


    private void Start()
    {
        cTAanimation =blockButton.GetComponent<CTAanimation>();
    }

    private void Update() {
        if (blockButton != null) {
            if (numOptionsSelected == 0) {
                if (onlyOnce) {
                    blockButton.interactable = false;
                    cTAanimation.StopAnimation();
                    onlyOnce = false;
                }
                
                
            } else {

                if(onlyOnce==false)
                {
                    blockButton.interactable = true;
                    cTAanimation.Animate();
                    onlyOnce = true;

                }
                
            }

        }
    }
    public void SelectOption(Option option) {

        //SINGLE OPTION SELECTION BEHAVIOR
        if (maxOptionsSelected == 1) {
            for (int i = 0; i < options.Count; i++) {
                options[i].Deselected();
                RemoveOptionFromSelected(options[i]);
                option.Selected();
                AddOptionToSelected(option);
                numOptionsSelected = 1;
            }


        } else {



            //NORMAL MULTI OPTION SELECTION
            if (!option.isSelected) {
                if (numOptionsSelected < maxOptionsSelected) {
                    option.Selected();
                    AddOptionToSelected(option);
                    numOptionsSelected++;
                } else {
                    if (warning != null) {
                        warning.SetActive(true);
                    }
                }
            } else {
                option.Deselected();
                RemoveOptionFromSelected(option);
                numOptionsSelected--;
                if (warning) {
                    warning.SetActive(false);
                }
            }

        }

    }
    //THIS ADDS OR REMOVE OPTIONS FROM SELECTED OPTIONS LIST
    void RemoveOptionFromSelected(Option option) {
        if (currentSelectedOptions.Contains(option)) {
            currentSelectedOptions.Remove(option);
        }
    }
    void AddOptionToSelected(Option option) {
        if (!currentSelectedOptions.Contains(option)) {
            currentSelectedOptions.Add(option);
        }
    }


}
