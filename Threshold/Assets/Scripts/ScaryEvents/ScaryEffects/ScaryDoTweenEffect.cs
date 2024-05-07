using DG.Tweening;
using UnityEngine;

namespace ScaryEvents.ScaryEffects
{
    public enum DoTweenType
    {
        None,
        Move,
        Rotate,
        Scale,
        Shake,
        Fade
    }

    public class ScaryDoTweenEffect : ScaryEffect
    {
        public DoTweenType doTweenType;
    
        // DoTween variables
        // targetPosition and targetRotation is based on World Space. local Space �� Relative �� case ���� ����ϸ� ���� ��.
        public Vector3 targetPosition;
        public Vector3 targetRotation;
        public Vector3 targetScale;
        public float shakePosition;
    
        public override void StartEffectInternal()
        {
            switch (doTweenType)
            {
                case DoTweenType.Move:
                    Position();
                    break;
                case DoTweenType.Rotate:
                    Rotation();
                    break;
                case DoTweenType.Scale:
                    Scale();
                    break;
                case DoTweenType.Shake:
                    Shaking();
                    break;
                case DoTweenType.Fade:
                    Fade();
                    break;
            }
        }

        #region Dotween Functions
    
        public void Position()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            a.DOMove(new Vector3(targetPosition.x,targetPosition.y,targetPosition.z), duration)
                .SetEase(ease);
            DelayAndStopEffect();
        }

        public void Rotation()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            a.DORotate(new Vector3(targetRotation.x,targetRotation.y,targetRotation.z), duration, RotateMode.FastBeyond360)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Restart); 
            DelayAndStopEffect();
        }

        public void Scale()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            a.DOScale(new Vector3(targetScale.x, targetScale.y, targetScale.z), duration)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo);
            DelayAndStopEffect();
        }

        //�켱 ��ġ�� ��鸮�� �ߴµ�, rotate/scale�� �־ �̰� ���� �ϸ� ������!
        public void Shaking()
        {
            var a = targetSource.GetCurrentTarget<Transform>("transform");
            a.DOShakePosition(shakePosition, duration);
            DelayAndStopEffect();
        }

        public void Fade()
        {
            //��.. ���׸��� �����;��ϴµ�,, ObjectInfoHolder�� �߰��ұ�,,,?
        }

        #endregion

    
    
    
    }
}