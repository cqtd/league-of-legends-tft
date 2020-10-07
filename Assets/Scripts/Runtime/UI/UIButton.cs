using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.LeagueOfLegends.TFT.UI
{
	public class UIButton : Button
	{
		[SerializeField] TextMeshProUGUI text = default;
		[SerializeField] ColorBlock textColors = default;
		[SerializeField] Transition trans = Transition.ColorTint;
		
		// Placeholders
		private AnimationTriggers animTriggers = new AnimationTriggers();
		private SpriteState _spriteState;


		protected override void DoStateTransition(SelectionState state, bool instant)
		{
			base.DoStateTransition(state, instant);

			if (!gameObject.activeInHierarchy)
				return;

			Color tintColor;
			Sprite transitionSprite;
			string triggerName;

			switch (state)
			{
				case SelectionState.Normal:
					tintColor = textColors.normalColor;
					transitionSprite = null;
					triggerName = animTriggers.normalTrigger;
					break;
				case SelectionState.Highlighted:
					tintColor = textColors.highlightedColor;
					transitionSprite = _spriteState.highlightedSprite;
					triggerName = animTriggers.highlightedTrigger;
					break;
				case SelectionState.Pressed:
					tintColor = textColors.pressedColor;
					transitionSprite = _spriteState.pressedSprite;
					triggerName = animTriggers.pressedTrigger;
					break;
				case SelectionState.Selected:
					tintColor = textColors.selectedColor;
					transitionSprite = _spriteState.selectedSprite;
					triggerName = animTriggers.selectedTrigger;
					break;
				case SelectionState.Disabled:
					tintColor = textColors.disabledColor;
					transitionSprite = _spriteState.disabledSprite;
					triggerName = animTriggers.disabledTrigger;
					break;
				default:
					tintColor = Color.black;
					transitionSprite = null;
					triggerName = string.Empty;
					break;
			}

			switch (trans)
			{
				case Transition.ColorTint:
					StartColorTween(tintColor * textColors.colorMultiplier, instant);
					break;
				case Transition.SpriteSwap:
					DoSpriteSwap(transitionSprite);
					break;
				case Transition.Animation:
					TriggerAnimation(triggerName);
					break;
			}
		}
		
		void StartColorTween(Color targetColor, bool instant)
		{
			if (text == null)
				return;
		
			// text.CrossFadeColor(targetColor, instant ? 0f : textColors.fadeDuration, true, true);
			text.DOColor(targetColor, textColors.fadeDuration);
		}

		void DoSpriteSwap(Sprite newSprite)
		{
			if (image == null)
				return;

			image.overrideSprite = newSprite;
		}

		void TriggerAnimation(string triggername)
		{
#if PACKAGE_ANIMATION
            if (transition != Transition.Animation || animator == null || !animator.isActiveAndEnabled || !animator.hasBoundPlayables || string.IsNullOrEmpty(triggername))
                return;

            animator.ResetTrigger(m_AnimationTriggers.normalTrigger);
            animator.ResetTrigger(m_AnimationTriggers.highlightedTrigger);
            animator.ResetTrigger(m_AnimationTriggers.pressedTrigger);
            animator.ResetTrigger(m_AnimationTriggers.selectedTrigger);
            animator.ResetTrigger(m_AnimationTriggers.disabledTrigger);

            animator.SetTrigger(triggername);
#endif
		}
	}
}