using UnityEngine;

namespace CQ.LeagueOfLegends.TFT
{
	public class Bootstrap : MonoBehaviour
	{
		[SerializeField] IntroManager intro = default;
		[SerializeField] LobbyManager lobby = default;

		void Awake()
		{
			intro.gameObject.SetActive(false);
			lobby.gameObject.SetActive(false);
			
			intro.RegisterEvents();
			
			intro.onCreateSession += OnCreateSession;
			intro.onJoinSession += OnJoinSession;

			Application.targetFrameRate = 60;
			Screen.SetResolution(1600, 900, false);
		}

		void Start()
		{
			// 인트로 패널 보이기
			intro.gameObject.SetActive(true);
		}

		/// <summary>
		/// 방 생성 버튼 콜백
		/// </summary>
		void OnCreateSession(string roomName, string displayName)
		{
			lobby.CreateSession(roomName, displayName);
		}

		/// <summary>
		/// 방 입장 버튼 콜백
		/// </summary>
		void OnJoinSession(string roomName, string displayName)
		{
			lobby.JoinSession(roomName, displayName);
		}

		[ContextMenu("Transfer")]
		void ContextMenu()
		{
			// var im = intro.GetComponent<IntroManager>();
			// im.displayName = this.displayName;
			// im.roomIndex = this.roomIndex;
			// im.createSession = this.createSession;
			// im.joinSession = this.joinSession;
		}
	}
}