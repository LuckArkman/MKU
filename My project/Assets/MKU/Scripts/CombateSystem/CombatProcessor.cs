using UnityEngine;
using MKU.Scripts.HelthSystem;
using MKU.Scripts.Interfaces;

namespace MKU.Scripts.CombateSystem
{
    public class CombatProcessor
    {
        private Base _attackerBase;
        private Transform _attackerTransform;
        private Animator _animator;
        private float _attackCooldownTimer = 0f;

        public CombatProcessor(Base attackerBase, Transform attackerTransform, Animator animator = null)
        {
            _attackerBase = attackerBase;
            _attackerTransform = attackerTransform;
            _animator = animator;
        }

        public void UpdateCooldown(float deltaTime)
        {
            if (_attackCooldownTimer > 0)
            {
                _attackCooldownTimer -= deltaTime;
            }
        }

        public bool CanAttack()
        {
             return _attackerBase != null && _attackerBase.IsAlive() && _attackCooldownTimer <= 0f;
        }

        public void ExecutePhysicsAttack(Vector3 origin, Vector3 direction, float range, float radius = 0.75f)
        {
            if (!CanAttack()) return;

            TriggerAttackAnimAndSetupCooldown();

            if (Physics.SphereCast(origin, radius, direction, out RaycastHit hit, range))
            {
                var damageable = hit.transform.GetComponent<IDamageable>();
                if (damageable != null && damageable.GetBaseStats().IsAlive() && damageable.GetTransform() != _attackerTransform)
                {
                    ApplyDamageTo(damageable);
                }
            }
        }

        public void ExecuteDirectAttack(IDamageable target)
        {
            if (!CanAttack()) return;

            TriggerAttackAnimAndSetupCooldown();

            if (target != null && target.GetBaseStats().IsAlive())
            {
                ApplyDamageTo(target);
            }
        }

        public void ApplyDamageDirectly(IDamageable target)
        {
            if (_attackerBase != null && _attackerBase.IsAlive() && target != null && target.GetBaseStats().IsAlive())
            {
                ApplyDamageTo(target);
            }
        }

        private void TriggerAttackAnimAndSetupCooldown()
        {
            if (_animator != null) _animator.SetTrigger("Attack");

            float aspd = _attackerBase.Status.AttackSpeed;
            if (aspd <= 0.1f) aspd = 0.1f;
            _attackCooldownTimer = 1f / aspd;
        }

        private void ApplyDamageTo(IDamageable target)
        {
            int damage = Mathf.Max(1, _attackerBase.Status.PhysicalAttack - target.GetBaseStats().Status.PhysicalDefense);
            target.TakeDamage(damage);
        }
    }
}
