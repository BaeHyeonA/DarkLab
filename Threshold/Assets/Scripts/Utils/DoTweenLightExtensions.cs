using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public static class DoTweenLightExtensions
    {
        // Range ������ ���� Ȯ�� �޼���
        public static Tweener DORange(this Light light, float endValue, float duration)
        {
            return DOTween.To(() => light.range, x => light.range = x, endValue, duration);
        }

        // Spot angle ������ ���� Ȯ�� �޼���
        public static Tweener DOSpotAngle(this Light light, float endValue, float duration)
        {
            return DOTween.To(() => light.spotAngle, x => light.spotAngle = x, endValue, duration);
        }
    }
}