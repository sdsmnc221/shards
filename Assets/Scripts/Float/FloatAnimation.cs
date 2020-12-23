using UnityEngine;
using DG.Tweening;

public class FloatAnimation : MonoBehaviour
{
    public bool preFall = true;
    public float fallDuration = 0.6f;
    public Ease fallEase = Ease.OutBack;
    public float fallY = 10f;

    public float strength = 10f;
    public int vibrato = 1;
    public float duration = 2f;

    public bool idle = false;

    void Start()
    {
        if (preFall)
        {
            transform
                .DOMoveY(transform.position.y, duration)
                .From(fallY)
                .SetEase(fallEase)
                .OnComplete(() => TweenFloat());
        } else
        {
            TweenFloat();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (idle) transform.DOPause();
        else transform.DOPlay();
    }

    private void TweenFloat()
    {
        transform
            .DOShakePosition(duration, strength, vibrato)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
