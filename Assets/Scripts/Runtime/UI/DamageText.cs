using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CQ.LeagueOfLegends.TFT.UI
{
	public sealed class DamageText : UIContent
	{
		DamageContext context;
		Text text;
		int size;
		
		public void Init(DamageContext context)
		{
			this.context = context;
			text = GetComponent<Text>();
			this.size = text.fontSize;
			
			Refresh();
			Repaint();

			Vector3 direction = Vector3.zero;
			direction.x = Random.Range(-20, 20);
			direction.y = Random.Range(-20, 20);


		}

		public override void Repaint()
		{
			Vector3 direction = Vector3.zero;
			
			direction.x = Random.Range(-1.2f, 1.5f);
			direction.z = Random.Range(-1.3f, 1.2f);
			
			var startPos = Camera.main.WorldToScreenPoint(context.pos);
			var endPos = Camera.main.WorldToScreenPoint(context.pos + direction);
			
			this.transform.position = startPos;
			(transform as RectTransform).DOMove(endPos, 1.4f).OnComplete(() =>
			{
				Destroy(gameObject);
			});

			text.text = context.damage.ToString();

			switch (context.damageType)
			{
				case EDamageType.AD:
					text.color = DamageTextCanvas.AttackDamage;
					break;
				case EDamageType.AP:
					text.color = DamageTextCanvas.MagicDamage;
					break;
				case EDamageType.Fixed:
					text.color = DamageTextCanvas.FixedDamage;
					break;
				default:
					break;
			}

			if (context.isCritical)
			{
				text.fontSize = (int) (size * 1.2f);
				text.fontStyle = FontStyle.Bold;
			}
			else
			{
				text.fontSize = size;
				text.fontStyle = FontStyle.Normal;
			}
		}

		public override void Refresh()
		{
			
		}
	}
}