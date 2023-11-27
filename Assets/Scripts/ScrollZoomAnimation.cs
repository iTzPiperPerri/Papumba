using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollZoomAnimation : MonoBehaviour
{
    // Define the minimum and maximum position values.
    public float effectFactor ;
    
    //when the text is at max scale
    public Vector2 origin;
    public float minScale = 0f;
    public float maxScale = 1f;
    RectTransform rectTransform;
    [SerializeField] RectTransform pivot;
    TextMeshProUGUI textMeshProUGUI;
    float distanceFromOrigin;

    public float DistanceFromOrigin { get => distanceFromOrigin; set => distanceFromOrigin = value; }

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    void Update() {
        // Get the object's position on the Y-axis.
        Vector2 positionY = new Vector2(pivot.anchoredPosition.x,  pivot.anchoredPosition.y);
        // Calculate the distance from the origin (0, 0, 0).
        distanceFromOrigin = Vector2.Distance(origin, positionY);

        // Map the position to the scale using linear interpolation (Lerp).
        float scaleValue = Mathf.Lerp(maxScale, minScale, Mathf.InverseLerp(0, effectFactor, distanceFromOrigin));

        // Set the object's scale on the X-axis.
        rectTransform.localScale= new Vector3(scaleValue, scaleValue, scaleValue);

        //set text alpha 
        textMeshProUGUI.color = new Color(0, 0, 0, scaleValue);



       
      
    }
}
