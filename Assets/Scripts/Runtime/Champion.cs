using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
    [DefaultExecutionOrder(100)]
    public class Champion : AttackableUnit
    {

        protected override void Update()
        {
            SkillLogic();
            
            if (HasTarget())
            {
                AttackLogic();
                return;
            }

            Move(); 
            FindSkillTarget();
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
    }
}