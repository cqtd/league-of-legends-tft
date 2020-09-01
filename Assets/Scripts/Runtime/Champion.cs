using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
    [DefaultExecutionOrder(100)]
    public class Champion : AttackableUnit
    {
        Material mat;
        public double threshold = 0.1f;

        protected override void Start()
        {
            base.Start();
            
            mat = Instantiate(GetComponent<MeshRenderer>().sharedMaterial);
            GetComponent<MeshRenderer>().sharedMaterial = mat;

            mat.color = Color.white;
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

            Move(); 
            FindSkillTarget();

            if (Time.time - lastAttacked > threshold)
            {
                mat.color = Color.white;
            }
        }

        void SkillLogic()
        {
            if (CanSkill())
            {
                if (HasSkillTarget())
                {
                    DoSkill();
                }
            }
        }

        bool CanSkill()
        {
            return false;
        }

        void DoSkill()
        {
            
        }

        bool HasSkillTarget()
        {
            return false;
        }

        void FindSkillTarget()
        {
            
        }

        public override void OnAttacked(float damage)
        {
            base.OnAttacked(damage);

            lastAttacked = Time.time;
            mat.color = Color.red;
        }
    }
}