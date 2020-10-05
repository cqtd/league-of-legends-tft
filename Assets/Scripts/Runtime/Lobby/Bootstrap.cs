using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CQ.LeagueOfLegends.TFT
{
	public class Bootstrap : MonoBehaviour
	{
		[Header("Intro")] 
		
		[SerializeField] GameObject intro = default;
		[SerializeField] TMP_InputField displayName = default;
		[SerializeField] TMP_InputField roomIndex = default;
		[SerializeField] Button createSession = default;
		[SerializeField] Button joinSession = default;

		[Space] 
		[Header("Lobby")] 
		
		[SerializeField] LobbyManager lobby = default;

		void Awake()
		{
			intro.gameObject.SetActive(false);
			lobby.gameObject.SetActive(false);
		}

		void Start()
		{
			LoadLocalData();
			
			createSession.onClick.AddListener(OnCreateSession);
			joinSession.onClick.AddListener(OnJoinSession);
			
			intro.gameObject.SetActive(true);
		}

		void OnDisable()
		{
			SaveLocalData();
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

		void OnCreateSession()
		{
			SaveLocalData();

			var cg = intro.GetComponent<CanvasGroup>();
			var tween = cg.DOFade(0.0f, 0.5f);
			tween.onComplete += () =>
			{
				intro.gameObject.SetActive(false);

				lobby.CreateSession(roomIndex.text);
			};
		}

		void OnJoinSession()
		{
			SaveLocalData();
			
			var cg = intro.GetComponent<CanvasGroup>();
			var tween = cg.DOFade(0.0f, 0.5f);
			tween.onComplete += () =>
			{
				intro.gameObject.SetActive(false);

				lobby.JoinSession(roomIndex.text);
			};
		}

		[ContextMenu("Transfer")]
		void ContextMenu()
		{

		}
	}
}