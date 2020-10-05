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
		[SerializeField] public TMP_InputField displayName = default;
		[SerializeField] public TMP_InputField roomIndex = default;
		[SerializeField] public Button createSession = default;
		[SerializeField] public Button joinSession = default;
		[SerializeField] CanvasGroup canvasGroup = default;
		[SerializeField] float duration = 0.6f;

		public UnityAction<string, string> onCreateSession;
		public UnityAction<string, string> onJoinSession;

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

		public void FadeOut(Action onComplete)
		{
			Tweener tween = canvasGroup.DOFade(0.0f, duration);
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