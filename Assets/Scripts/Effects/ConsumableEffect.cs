using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName ="ConsumableEffect", menuName ="Effects/ConsumableEffect", order=1)]
public class ConsumableEffect : ScriptableObject
{
    [Serializable]
    public enum ConsumableEffectType
    {
        HUNGER,
        THIRST,
        HEALTH,
        STAMINA
    }

    [SerializeField]
    public int effectIntensity;
    [SerializeField]
    public ConsumableEffectType effectType;
}
