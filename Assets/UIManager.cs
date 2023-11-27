using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour

    

{
    public static UIManager instance;

    [SerializeField]float slideTime;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }
     

    //LEFT SLIDE
    public void SlideScreen(RectTransform id) {
        StartCoroutine(SlideScreenCoroutine(id));
    }
    public void SlideScreenOut(RectTransform id) {
        StartCoroutine(SlideScreenOutCoroutine(id));
    }
    IEnumerator SlideScreenCoroutine(RectTransform screen) {
       screen.transform.SetAsLastSibling();
        float time = 0;
            Vector2 startPosition = new Vector2(1533, screen.anchoredPosition.y);
            Vector2 targetPosition = new Vector2(428, screen.anchoredPosition.y);
            
            while (time < slideTime) {
            screen.anchoredPosition = Vector2.Lerp(startPosition,targetPosition , time / slideTime);

                time += Time.deltaTime;
                yield return null;
            }
            screen.anchoredPosition = targetPosition;
        
    }
    IEnumerator SlideScreenOutCoroutine(RectTransform screen) {
        
        float time = 0;
        Vector2 targetPosition = new Vector2(1533, screen.anchoredPosition.y);
        Vector2 startPosition = new Vector2(428, screen.anchoredPosition.y);


        while (time < slideTime) {
            screen.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time / slideTime);

            time += Time.deltaTime;
            yield return null;
        }
        screen.anchoredPosition = targetPosition;

    }

    //BOTTOM SLIDE
    public void BottomShowScreen(RectTransform id) {
        StartCoroutine(BottomShowScreenCoroutine(id));
    }
    public void BottomHideScreen(RectTransform id) {
        StartCoroutine(BottomHideScreenCoroutine(id));
    }

    IEnumerator BottomShowScreenCoroutine(RectTransform screen) {
        transform.SetAsLastSibling();
        float time = 0;
        Vector2 startPosition = new Vector2(screen.anchoredPosition.x, -1091);
        Vector2 targetPosition = new Vector2(screen.anchoredPosition.x, -447);

        while (time < slideTime) {
            screen.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time / slideTime);

            time += Time.deltaTime;
            yield return null;
        }
        screen.anchoredPosition = targetPosition;

    }
    IEnumerator BottomHideScreenCoroutine(RectTransform screen) {
       
        float time = 0;
        Vector2 startPosition = new Vector2(screen.anchoredPosition.x, -447);
        Vector2 targetPosition = new Vector2(screen.anchoredPosition.x, -1091);

        while (time < slideTime) {
            screen.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, time / slideTime);

            time += Time.deltaTime;
            yield return null;
        }
        screen.anchoredPosition = targetPosition;

    }

}
