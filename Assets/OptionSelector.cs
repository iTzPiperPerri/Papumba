using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSelector : MonoBehaviour
{
    [SerializeField] int maxOptionsSelected;
    [SerializeField] List<Option> options;
    [SerializeField] int currentOptionsSelected;
    [SerializeField] GameObject warning;


    public void SelectOption(Option option)
    {

        //SINGLE OPTION SELECTION BEHAVIOR
        if (maxOptionsSelected == 1) {
            for (int i = 0; i < options.Count; i++) {
                options[i].Deselected();
                option.Selected();
            }


        } else {



            //NORMAL MULTI OPTION SELECTION
            if (!option.isSelected) {
                if (currentOptionsSelected < maxOptionsSelected) {
                    option.Selected();
                    currentOptionsSelected++;
                } else {
                    if (warning != null) {
                        warning.SetActive(true);
                    }
                }
            } else {
                option.Deselected();
                currentOptionsSelected--;
                if (warning) {
                    warning.SetActive(false);
                }
            }

        }
        
    }
    
}
