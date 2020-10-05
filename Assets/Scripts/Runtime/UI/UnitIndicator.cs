using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.LeagueOfLegends.TFT.UI
{
	public sealed class UnitIndicator : UIContent
	{
		public Text tier;
		public Text healthPoint;

		public Image prevHealthBar;
		public Image healthBar;
		public Image manaBar;

		public CanvasGroup group;

		const string format = "<size=32>{0}</size> <size=22>/ {1} </size>";

		int currentHealth;
		int MaxHealth;
		int prevHealth;

		AttackableUnit target;

		public void SetHealth(float cur, float max)
		{
			bool dirty = currentHealth != (int) cur ||
			             MaxHealth != (int) max;
			
			if (dirty)
			{
				prevHealth = currentHealth;
				currentHealth = (int) cur;
				MaxHealth = (int) max;
				
				healthPoint.text = string.Format(format, cur, max);
				healthBar.fillAmount = (float)currentHealth / MaxHealth;

				if (!animating)
				{
					StartCoroutine(AnimateHealth());
				}
			}

			if (cur < 1 && !fading)
			{
				StartCoroutine(FadeOut());
			}
		}

		bool fading;
		bool animating;
		
		public bool beforeDestroy;

		IEnumerator FadeOut()
		{
			fading = true;
			while (!beforeDestroy)
			{
				yield return null;
				@group.alpha -= 1.0f / Application.targetFrameRate;
			}
		}

		IEnumerator AnimateHealth()
		{
			animating = true;
			while (currentHealth <= prevHealth)
			{
				prevHealth -= (int) ((prevHealth - currentHealth) / 50 + 1);
				prevHealthBar.fillAmount = (float) prevHealth / MaxHealth;
				yield return null;
			}

			animating = false;
		}

		public override void Init(AttackableUnit unit)
		{
			this.target = unit;

			base.Init(unit);
		}

		public override void Repaint()
		{
			SetHealth(target.GetHealth(), target.GetMaxHealth());
			
			manaBar.fillAmount = target.GetMana() / target.GetMaxMana();
			tier.text = target.Tier.ToString();
		}

		public override void Refresh()
		{
			
		}
	}
}