﻿using System;
using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
    public class DamageContext
    {
        public float damage;
    }

    public class BulletContext
    {
        public DamageContext damage;
        public AttackableUnit owner;
        public AttackableUnit target;
        public Vector3 startPos;
        public Vector3 direction;
        public float missileSpeed;
    }
    
	public class Bullet : MonoBehaviour
	{
        Vector3 direction = new Vector3(0, 0, 1);
        AttackableUnit followTarget = null;
        DamageContext context;
        float speed = 1.0f;

        public void SetDirection(Vector3 direction)
        {
            this.direction = direction;
        }


        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }

        public void SetTarget(AttackableUnit target)
        {
            this.followTarget = target;
        }

        public void SetContext(DamageContext context)
        {
            this.context = context;
        }

        void DetermineDirection()
        {
            if (this.followTarget != null)
            {
                // Calculate rotation to target.
                this.direction = (this.followTarget.transform.position - transform.position).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(this.direction);

                // This variable represents the quality of the missile.
                // If this value is small, it turns very slowly.
                // If this value is large, it turns very fast to target.
                float angularVelocity = 4.0f;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * angularVelocity);
            }
        }


        void Move()
        {
            Vector3 velocity = transform.forward * this.speed;
            transform.position += Time.deltaTime * velocity;
        }

        void Update()
        {
            DetermineDirection();
            Move();

            if (followTarget.IsInvalid)
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == followTarget.gameObject)
            {
                followTarget.GetComponent<AttackableUnit>().TakeDamage(context);
                Destroy(gameObject);
            }
        }
    }
}