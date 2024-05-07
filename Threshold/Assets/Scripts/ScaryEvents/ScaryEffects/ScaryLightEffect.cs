using DG.Tweening;
using UnityEngine;

namespace ScaryEvents.ScaryEffects
{
    public enum LightEffectType
    {
        None,
        Flicker,
        ColorChange,
        IntensityChange
    }

    public class ScaryLightEffect : MonoBehaviour
    {
        public LightEffectType effectType;
        public Light lightComponent; //�̰� �� �ʿ��Ѱž�?? target���� �������� �ʳ�??!
        public Color targetColor;
        public float targetIntensity;
        public float targetIndirectMultiplier;
        public LightShadows targetShadowType;
        public bool targetDrawHalo;
        public float duration;

        public ScaryEvent targetSource;

        //Light�� DoTween �̿��ؼ� ���� �����Ұ���
        //�ƴϸ� �ڷ�ƾ �̿��ؼ� �����Ұ��� ���ؾ��� �� �����ϴ�!!!
        //�ٵ� �ڵ尡 ����ѰŴ� DoTween�� �� ���ƿ�,,��,,

        void Start()
        {
            targetSource = transform.parent.GetComponent<ScaryEvent>();
        }
    
        public void Flicker()
        {
            var a = targetSource.GetCurrentTarget<Light>("light");
            DOTween.To(() => a.intensity, x => a.intensity = x, targetIntensity, duration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo)
                .SetDelay(0.5f);
        }

        public void ColorChange()
        {
            var a = targetSource.GetCurrentTarget<Light>("light");
            a.DOColor(targetColor, duration);
        }

        //DoTween �̿�
        public void IntensityChange()
        {
            var a = targetSource.GetCurrentTarget<Light>("light");
            a.DOIntensity(targetIntensity, duration)
                .SetEase(Ease.InOutSine);
        }

    
    }
}