using System;
using System.Threading.Tasks;
using MKU.Scripts.HelthSystem;

namespace MKU.Scripts.HelthSystem
{
    [Serializable]
    public class Base
    {
        public Base(){}
        private int _level;
        public _Attributs Attributes;
        public _Stats Status;

        public Base StartCalc(_Attributs attributes, _Stats _status, int level)
        {
            _level = level;
            Attributes = attributes;
            Status = _status;
            UpdateStatus();
            return this;
        }
        public void UpdateStatus()
        {
            Status.MaxHP = (int)(100 + (Attributes.Vitality * 10) * (_level *1.2f)); 
            Status.MaxSP = (int)(50 + (Attributes.Intelligence * 8) * (_level *1.2f));
            Status.CurrentHP = Status.MaxHP;
            Status.CurrentSP = Status.MaxSP;
            
            Status.PhysicalAttack = (int)(Attributes.Strength * 2 *  (_level *1.2f));
            Status.MagicAttack = (int)(Attributes.Intelligence * 2 * (_level *1.2f));
            
            Status.AttackSpeed = 1.0f + (Attributes.Agility * 0.1f) *  (_level *1.2f);
            Status.Flee = (int)(5 + Attributes.Agility *  (_level *1.2f));
            
            Status.Hit = (int)(10 + Attributes.Dexterity *  (_level *1.2f));
            Status.CriticalChance = Attributes.Luck * 0.3f *  (_level *1.2f);
            
            Status.PhysicalDefense = (int)(Attributes.Vitality / 2 *  (_level *1.2f));
            Status.MagicDefense = (int)(Attributes.Intelligence / 2 *  (_level *1.2f));
            
            Status.HpRegen = (int)(Attributes.Vitality / 5 + 1 * (_level *1.2f));
            Status.SpRegen = (int)(Attributes.Intelligence / 5 + 1 *  (_level *1.2f));
        }
        
        public void ApplyEquipmentBonus(_Attributs equipmentBonus)
        {
            Attributes.Strength += equipmentBonus.Strength;
            Attributes.Agility += equipmentBonus.Agility;
            Attributes.Vitality += equipmentBonus.Vitality;
            Attributes.Intelligence += equipmentBonus.Intelligence;
            Attributes.Dexterity += equipmentBonus.Dexterity;
            Attributes.Luck += equipmentBonus.Luck;

            UpdateStatus();
        }
        
        public void ApplyTemporaryBuff(_Attributs buff, int durationInSeconds)
        {
            Attributes.Strength += buff.Strength;
            Attributes.Agility += buff.Agility;

            UpdateStatus();
            
            Task.Delay(durationInSeconds * 1000).ContinueWith(_ =>
            {
                Attributes.Strength -= buff.Strength;
                Attributes.Agility -= buff.Agility;

                UpdateStatus();
            });
        }
        
        public bool IsAlive() => Status.CurrentHP > 0;

        public int TakeDamage(int damage)
        {
            if (Status.CurrentHP > 0)Status.CurrentHP -= damage;
            if (Status.CurrentHP < 0) Status.CurrentHP = 0;
            return damage;
        }
    }
}