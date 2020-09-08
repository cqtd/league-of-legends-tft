using System.Collections.Generic;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
    [DefaultExecutionOrder(100)]
    public class Champion : AttackableUnit
    {
        Material mat;
        public double threshold = 0.1f;
        public SkillDataBase skill;

        List<AttackableUnit> skillTargets;

        protected override void Start()
        {
            base.Start();
            
            mat = Instantiate(GetComponent<MeshRenderer>().sharedMaterial);
            GetComponent<MeshRenderer>().sharedMaterial = mat;
            mat.color = Color.white;

            if (unitData is ChampionData champData)
            {
                skill = Instantiate(champData.skill);
                skill.Initialize(this);
            }
            else
            {
                Debug.LogError("Invalid Champion Data", this);
            }
        }

        float lastAttacked;

        protected override void Update()
        {
            if (!roundStarted) return;
            
            SkillLogic();
            
            if (HasTarget())
            {
                AttackLogic();
                return;
            }

            ProcessMovement(); 
            // FindSkillTarget();

            if (Time.time - lastAttacked > threshold)
            {
                mat.color = Color.white;
            }
        }

        void SkillLogic()
        {
            if (CanSkill())
            {
                FindSkillTarget();
                if (HasSkillTarget())
                {
                    DoSkill();
                }
            }
        }

        bool CanSkill()
        {
            if (IsInvalid) return false;
            if (skill == null) return false;
            return GetMana() >= GetMaxMana();
        }

        void DoSkill()
        {
            // do skill
            skill?.Use(skillTargets);
            
            // release targets
            skillTargets = null;
            SetMana(0);
        }

        bool HasSkillTarget()
        {
            return skill != null && skillTargets != null && skillTargets.Count > 0;
        }

        void FindSkillTarget()
        {
            skillTargets = skill?.GetTargets();
        }

        public override void OnAttacked(float damage)
        {
            base.OnAttacked(damage);

            lastAttacked = Time.time;
            mat.color = Color.red;
        }

        public DamageContext GetSkillDamageContext()
        {
            var critPoss = unitData.criticalSkillPossibility.Get(Tier);
            bool isCrit = random.Next(100) <= critPoss * 100;
            float critMultiply = 1.0f;
            if (isCrit) critMultiply = unitData.criticalMultiplier.Get(Tier);

            var damage = new DamageContext()
            {
                damage = GetAttackDamage(),
                isCritical = isCrit,
                criticalMultiplier = critMultiply,
                damageType = EDamageType.AD,
            };

            return damage;
        }

        public float GetSkillCriticalPossibility()
        {
            var result = unitData.criticalSkillPossibility.Get(Tier);

            return result;
        }
    }
}