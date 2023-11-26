using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform main, welcome, brings, pInterest, customize, birth, month, lang, favorite, cInterest, cName, create;
    // Start is called before the first frame update
    void Start()
    {
        main.DOAnchorPos(this.transform.position, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
