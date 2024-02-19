#nullable enable
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stats {
    public SN<float> Health;
    public SN<float> Armor;
    public SN<float> MaxSpeed;
    public SN<float> AccMult;
    public SN<float> ShotDamage;
    public SN<float> ShotRate;
    public SN<float> ShotSpeed;
    public SN<float> PointValue;
    public SN<float> ContactDamage;
    public SN<float> AbsorbedDamage;
    public SN<float> RegenRate;
    public SN<float> Lifetime;
    public bool IsInvincible;
    public AudioClip? WhenShootSound;
    public AudioClip? WhenDieSound;
    public AudioClip? WhenHitSound;
    public AudioClip? WhenAbsorbSound;
    public ColorType Color = ColorType.None;
    public FactionType Faction = FactionType.None;
    public List<float> ItemList = new List<float> {0.0f, 0.0f, 0.0f, 0.0f, 0.0f};

    public void Merge(Stats s) {
        Health = s.Health.HasValue ? s.Health : Health;
        Armor = s.Armor.HasValue ? s.Armor : Armor;
        MaxSpeed = s.MaxSpeed.HasValue ? s.MaxSpeed : MaxSpeed;
        AccMult = s.AccMult.HasValue ? s.AccMult : AccMult;
        ShotDamage = s.ShotDamage.HasValue ? s.ShotDamage : ShotDamage;
        ShotRate = s.ShotRate.HasValue ? s.ShotRate : ShotRate;
        ShotSpeed = s.ShotSpeed.HasValue ? s.ShotSpeed : ShotSpeed;
        PointValue = s.PointValue.HasValue ? s.PointValue : PointValue;
        ContactDamage = s.ContactDamage.HasValue ? s.ContactDamage : ContactDamage;
        AbsorbedDamage = s.AbsorbedDamage.HasValue ? s.AbsorbedDamage : AbsorbedDamage;
        RegenRate = s.RegenRate.HasValue ? s.RegenRate : RegenRate;
        Lifetime = s.Lifetime.HasValue ? s.Lifetime : Lifetime;
        WhenShootSound = s.WhenShootSound == null ? WhenShootSound : s.WhenShootSound;
        WhenDieSound = s.WhenDieSound == null ? WhenDieSound : s.WhenDieSound;
        WhenHitSound = s.WhenHitSound == null ? WhenHitSound : s.WhenHitSound;
        WhenAbsorbSound = s.WhenAbsorbSound == null ? WhenAbsorbSound : s.WhenAbsorbSound;
        IsInvincible = s.IsInvincible;
        Color = s.Color;
        Faction = s.Faction;
        ItemList = s.ItemList;
    }
}

public enum ColorType{
    None = 0,
    Red = 1,
    Blue = 2,
    Green = 3
}

public enum FactionType{
    None = 0,
    Player = 1,
    Enemy = 2
}