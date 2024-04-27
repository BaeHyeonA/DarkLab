using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public enum DoTweenType
{
    None,
    Move,
    Rotate,
    Scale,
    Shake,
    Fade
}

public class ScaryDoTweenEffect : MonoBehaviour
{
    public DoTweenType doTweenType;
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    public Vector3 targetScale;
    public float shakePosition;
    public Ease ease;
    public float duration = 1f;

    public ScaryEvent targetSource;

    private void Start()
    {
        targetSource = transform.parent.GetComponent<ScaryEvent>();
    }

    public void Position()
    {
        var a = targetSource.GetCurrentTarget<Transform>("transform");
        a.DOMove(new Vector3(targetPosition.x,targetPosition.y,targetPosition.z), duration)
            .SetEase(ease);
    }

    public void Rotation()
    {
        var a = targetSource.GetCurrentTarget<Transform>("transform");
        a.DORotate(new Vector3(targetRotation.x,targetRotation.y,targetRotation.z), duration, RotateMode.FastBeyond360)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Restart); 
    }

    public void Scale()
    {
        var a = targetSource.GetCurrentTarget<Transform>("transform");
        a.DOScale(new Vector3(targetScale.x, targetScale.y, targetScale.z), duration)
            .SetEase(ease)
            .SetLoops(-1, LoopType.Yoyo);
    }

    //�켱 ��ġ�� ��鸮�� �ߴµ�, rotate/scale�� �־ �̰� ���� �ϸ� ������!
    public void Shaking()
    {
        var a = targetSource.GetCurrentTarget<Transform>("transform");
        a.DOShakePosition(shakePosition, duration);
    }

    public void Fade()
    {
        //��.. ���׸��� �����;��ϴµ�,, ObjectInfoHolder�� �߰��ұ�,,,?
    }
}
