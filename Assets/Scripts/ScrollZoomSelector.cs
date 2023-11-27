using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollZoomSelector : MonoBehaviour
{
    public List<ScrollZoomAnimation> scrollableOptions;
    [SerializeField] ScrollZoomAnimation selectedOption;
    [SerializeField] TextMeshProUGUI debugText;
    private void Update() {
        GetClosestOption();
    }

    public void  GetClosestOption() {
        // Initialize with a large value to ensure any distance is smaller
        float closestDistance = float.MaxValue;
        ScrollZoomAnimation closestOption = null;

        foreach (ScrollZoomAnimation obj in scrollableOptions) {
            // Update the object with the smallest distance if necessary
            if (obj.DistanceFromOrigin < closestDistance) {
                closestDistance = obj.DistanceFromOrigin;
                closestOption = obj;
            }
        }

        selectedOption = closestOption;
        debugText.text = "Debug, selected option : " + selectedOption.gameObject.name;
    }
}
