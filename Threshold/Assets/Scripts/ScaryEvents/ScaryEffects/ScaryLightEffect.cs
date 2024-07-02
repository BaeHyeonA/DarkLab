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

    public class ScaryLightEffect : ScaryEffect
    {
        
        public LightEffectType effectType;
        
        // Light variables
        public Color targetColor;
        public float targetIntensity;
        public float targetIndirectMultiplier;
        public LightShadows targetShadowType;
        public bool targetDrawHalo;

        public override void StartEffectInternal()
        {
            switch (effectType)
            {
                case LightEffectType.Flicker:
                    Flicker();
                    break;
                case LightEffectType.ColorChange:
                    ColorChange();
                    break;
                case LightEffectType.IntensityChange:
                    IntensityChange();
                    break;
            }
            
            DelayAndStopEffect();
        }
        
        //Light�� DoTween �̿��ؼ� ���� �����Ұ���
        //�ƴϸ� �ڷ�ƾ �̿��ؼ� �����Ұ��� ���ؾ��� �� �����ϴ�!!!
        //�ٵ� �ڵ尡 ����ѰŴ� DoTween�� �� ���ƿ�,,��,, => ���� ���� �� ���ƿ�!

        #region Light Functions
        
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
        
        #endregion
    }
}