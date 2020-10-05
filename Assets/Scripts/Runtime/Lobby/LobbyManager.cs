using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Assertions;

namespace CQ.LeagueOfLegends.TFT
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public enum eConnectType
        {
            Create,
            Join,
        }
        
        [SerializeField] public LobbyPlayerCell me = default;
        [SerializeField] public LobbyTeamCell[] teammates = new LobbyTeamCell[0];

        eConnectType connectType;
        string roomID;
        RoomOptions options;
        CanvasGroup cg;
        
        public void CreateSession(string roomName)
        {
            cg = GetComponent<CanvasGroup>();
            cg.alpha = 0f;
            
            gameObject.SetActive(true);

            roomID = roomName;
            
            Debug.Log("Create Session");

            connectType = eConnectType.Create;
            options = new RoomOptions {MaxPlayers = 8};
            
            PhotonNetwork.ConnectUsingSettings();
        }

        public void JoinSession(string roomName)
        {
            cg = GetComponent<CanvasGroup>();
            cg.alpha = 0f;
            
            gameObject.SetActive(true);
            roomID = roomName;
            
            Debug.Log("Join Session");
            
            connectType = eConnectType.Join;
            
            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            
            Debug.Log("Joined Lobby");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            
            Debug.Log("Joined Room");
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            if (connectType == eConnectType.Create)
            {
                bool result = PhotonNetwork.CreateRoom(roomID, options);
                Assert.IsTrue(result);
            }
            else if (connectType == eConnectType.Join)
            {
                bool result = PhotonNetwork.JoinRoom(roomID);
                Assert.IsTrue(result);
            }

            Tweener tween = cg.DOFade(1.0f, 0.5f);
            tween.onComplete += () =>
            {
                Debug.Log("OnComplete");
            };
            
            Debug.Log("OnConnectedToMaster");
        }
    }
}