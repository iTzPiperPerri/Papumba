using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CTAanimation : MonoBehaviour
{
    Tween action;
    public bool startOnAwake;

    // Start is called before the first frame update
    void Start()
    {
        action = transform.DOScale(1.05f, 0.4f).SetLoops(-1, LoopType.Yoyo);
        if (!startOnAwake) {
            action.Pause();
        }
    }


    public void Animate()
    {

        
        action.Restart();
    }
    public void StopAnimation() {
        action.Pause();
        
    }
}
