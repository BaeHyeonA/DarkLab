using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaryLightEffect : ScaryEffect
{
    // ����� �Ǵ� ����Ʈ
    public Light targetLight;

    // ��� ���� ��
    public Color targetColor;
    public float targetIntensity;
    public float targetIndirectMultiplier;
    public LightShadows targetShadowType;
    public bool targetDrawHalo;

    public void ColorOff()
    {
        this.GetComponent<Light>().intensity = 10;
    }
}
