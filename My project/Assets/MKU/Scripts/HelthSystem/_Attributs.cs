using System;
using UnityEngine;

namespace MKU.Scripts.HelthSystem
{
    [Serializable]
    public class _Attributs
    {
        public _Attributs(){}
        public int Strength;
        public int Agility;
        public int Vitality;
        public int Intelligence;
        public int Dexterity;
        public int Luck;
        
        public _Attributs(int strength = 0, int agility = 0, int vitality = 0, int intelligence = 0, int dexterity = 0, int luck = 0)
        {
            Strength = strength;
            Agility = agility;
            Vitality = vitality;
            Intelligence = intelligence;
            Dexterity = dexterity;
            Luck = luck;
        }
        public _Attributs(_Attributs other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            Strength = other.Strength;
            Agility = other.Agility;
            Vitality = other.Vitality;
            Intelligence = other.Intelligence;
            Dexterity = other.Dexterity;
            Luck = other.Luck;
        }
        
        public _Attributs SaveOriginalValues()
        => new _Attributs(this);
        
        public void RestoreOriginalValues(_Attributs _originalAttributes)
        {
            if (_originalAttributes == null) return;

            Strength = _originalAttributes.Strength;
            Agility = _originalAttributes.Agility;
            Vitality = _originalAttributes.Vitality;
            Intelligence = _originalAttributes.Intelligence;
            Dexterity = _originalAttributes.Dexterity;
            Luck = _originalAttributes.Luck;
        }
        
        public void _addAttributs(int strength, int agility, int vitality, int intelligence, int dexterity, int luck)
        {
            Strength += strength;
            Agility += agility;
            Vitality += vitality;
            Intelligence += intelligence;
            Dexterity += dexterity;
            Luck += luck;
        }
        
        

        public void AllocateAttributePoints(string attribute, int points)
        {
            switch (attribute)
            {
                case "STR":
                    Strength += points;
                    break;
                case "AGI":
                    Agility += points;
                    break;
                case "VIT":
                    Vitality += points;
                    break;
                case "INT":
                    Intelligence += points;
                    break;
                case "DEX":
                    Dexterity += points;
                    break;
                case "LUK":
                    Luck += points;
                    break;
            }
        }
        
        public int CalculatePhysicalAttack() => Strength * 2;
        public int CalculateMaxHP() => Vitality * 10 + 100;
        public int CalculateCriticalChance() => (int)(Luck * 0.3);
        public int CalculateFlee() => Agility + 5;
    }
}