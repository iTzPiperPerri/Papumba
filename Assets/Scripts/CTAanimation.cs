using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CTAanimation : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Animate()
    {
        transform.DOScale(1.05f, 0.4f).SetLoops(-1, LoopType.Yoyo);
    }
}
