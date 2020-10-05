using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CQ.LeagueOfLegends.TFT
{
	public class IntroManager : MonoBehaviour
	{
		[SerializeField] TMP_InputField displayName = default;
		[SerializeField] TMP_InputField roomIndex = default;
		[SerializeField] Button createSession = default;
		[SerializeField] Button joinSession = default;
		[SerializeField] CanvasGroup canvasGroup = default;
		[SerializeField] float duration = 0.6f;

		[NonSerialized] public UnityAction<string, string> onCreateSession;
		[NonSerialized] public UnityAction<string, string> onJoinSession;

		void OnEnable()
		{
			LoadLocalData();
		}

		void OnDisable()
		{
			SaveLocalData();
		}

		public void RegisterEvents()
		{
			createSession.onClick.AddListener(OnTryCreateSession);
			joinSession.onClick.AddListener(OnTryJoinSession);
		}
		
		void OnTryCreateSession()
		{
			FadeOut(() =>
			{
				gameObject.SetActive(false);

				onCreateSession?.Invoke(roomIndex.text, displayName.text);
			});
		}

		/// <summary>
		/// 방 입장 버튼 콜백
		/// </summary>
		void OnTryJoinSession()
		{
			FadeOut(() =>
			{
				gameObject.SetActive(false);

				onJoinSession?.Invoke(roomIndex.text, displayName.text);
			});
		}

		public void FadeIn(Action onComplete = null, float defaultvalue = 0)
		{
			canvasGroup.alpha = defaultvalue;
			gameObject.SetActive(true);
			
			Tweener tween = canvasGroup.DOFade(1.0f, duration);

			if (onComplete != null)
				tween.onComplete += new TweenCallback(onComplete);
		}

		public void FadeOut(Action onComplete = null, float defaultValue = 1)
		{
			canvasGroup.alpha = defaultValue;
			Tweener tween = canvasGroup.DOFade(0.0f, duration);

			if (onComplete != null)
				tween.onComplete += new TweenCallback(onComplete);
		}
		
		void SaveLocalData()
		{
			PlayerPrefs.SetString("cq.tft.bootstrap.display.name", displayName.text);
			PlayerPrefs.SetString("cq.tft.bootstrap.room.index", roomIndex.text);
		}

		void LoadLocalData()
		{
			displayName.text = PlayerPrefs.GetString("cq.tft.bootstrap.display.name", "기본닉네임");
			roomIndex.text = PlayerPrefs.GetString("cq.tft.bootstrap.room.index", "5324");
		}
	}
}