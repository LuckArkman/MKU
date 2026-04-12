using System;
using UnityEngine;

namespace MKU.Scripts.HelthSystem
{
    [Serializable]
    public class _Stats
    {
        public _Stats(){}
        public int MaxHP;
        public int MaxSP;
        public int PhysicalAttack;
        public int MagicAttack;
        public float AttackSpeed;
        public int Flee;
        public int Hit;
        public int HpRegen;
        public int SpRegen;
        public float CriticalChance;
        public int PhysicalDefense;
        public int MagicDefense;
        public float CurrentHP;
        public float CurrentSP;

        public _Stats(_Stats other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            MaxHP = other.MaxHP;
            MaxSP = other.MaxSP;
            PhysicalAttack = other.PhysicalAttack;
            MagicAttack = other.MagicAttack;
            AttackSpeed = other.AttackSpeed;
            Flee = other.Flee;
            Hit = other.Hit;
            CriticalChance = other.CriticalChance;
            PhysicalDefense = other.PhysicalDefense;
            MagicDefense = other.MagicDefense;
        }
        public void RestoreOriginalValues(_Stats other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            MaxHP = other.MaxHP;
            MaxSP = other.MaxSP;
            PhysicalAttack = other.PhysicalAttack;
            MagicAttack = other.MagicAttack;
            AttackSpeed = other.AttackSpeed;
            Flee = other.Flee;
            Hit = other.Hit;
            CriticalChance = other.CriticalChance;
            PhysicalDefense = other.PhysicalDefense;
            MagicDefense = other.MagicDefense;
        }

        public _Stats(int maxHp, int maxSp, int physicalAttack, int magicAttack, float attackSpeed, int flee, int hit, float criticalChance, int physicalDefense, int magicDefense)
        {
            MaxHP = maxHp;
            MaxSP = maxSp;
            PhysicalAttack = physicalAttack;
            MagicAttack = magicAttack;
            AttackSpeed = attackSpeed;
            Flee = flee;
            Hit = hit;
            CriticalChance = criticalChance;
            PhysicalDefense = physicalDefense;
            MagicDefense = magicDefense;
        }

        public _Stats Clone()
            => new _Stats(this);
        public int CalculateMaxHP(int baseHP, int vitality) => baseHP + (vitality * 10);
        public int CalculateMaxSP(int baseSP, int intelligence) => baseSP + (intelligence * 8);
        public int CalculatePhysicalAttack(int baseATK, int strength) => baseATK + (strength * 2);
        public int CalculateMagicAttack(int baseMATK, int intelligence) => baseMATK + (intelligence * 2);
        public float CalculateAttackSpeed(float baseASPD, int agility, float penalty) => baseASPD + (agility * 0.1f) - penalty;
        public int CalculateFlee(int baseFlee, int agility) => baseFlee + agility;
        public float CalculateCriticalChance(int luck) => luck * 0.3f;
        public int CalculateDefense(int baseDefense, int vitality) => baseDefense + (vitality / 2);

        public void TakeDamage(int damage)
        => CurrentHP -= damage;
    }
}