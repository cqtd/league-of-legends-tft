using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CQ.LeagueOfLegends.TFT
{
	public class LobbyNetworkManager : MonoBehaviourPunCallbacks
	{
		[SerializeField] IntroManager intro = default;
		[SerializeField] LobbyManager lobby = default;

		static LobbyNetworkManager instance;

		void Awake()
		{
			instance = this;
			
			// 패널들 꺼놓기
			intro.gameObject.SetActive(false);
			lobby.gameObject.SetActive(false);
			
			// 포톤 접속 시도
			PhotonNetwork.ConnectUsingSettings();
			
			// 이벤트 등록
			intro.RegisterEvents();
			
			intro.onCreateSession += OnCreateSession;
			intro.onJoinSession += OnJoinSession;

			// 기초 세팅
			Application.targetFrameRate = 60;
			Screen.SetResolution(1600, 900, false);
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

		/// <summary>
		/// 포톤 네트워크 커넥션 콜백
		/// </summary>
		public override void OnConnectedToMaster()
		{
			base.OnConnectedToMaster();

			// 인트로 패널 활성화
			intro.FadeIn();
			
			Debug.Log("OnConnectedToMaster");
		}

		[PunRPC]
		private void NetworkedLoadScene(string sceneName)
		{
			SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		}

		public static void LoadLevel(string name)
		{
			PhotonNetwork.OpCleanRpcBuffer(instance.photonView);
			instance.photonView.RPC("NetworkedLoadScene", RpcTarget.All, name);
		}
	}
}