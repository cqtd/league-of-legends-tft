using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace CQ.LeagueOfLegends.TFT
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] LobbyPlayerCell me = default;
        [SerializeField] LobbyTeamCell[] teammates = new LobbyTeamCell[0];
        
        [SerializeField] Button matchStart = default;
        [SerializeField] TextMeshProUGUI roomTitle = default;

        string roomID = default;
        string playerName = default;

        RoomOptions options = default;
        CanvasGroup canvasGroup = default;
        
        public void CreateSession(string room, string player)
        {
            Debug.Log("Create Session");

            Prepare_Internal(room, player);
            
            // bool result = PhotonNetwork.CreateRoom(roomID, options);
            bool result = PhotonNetwork.JoinOrCreateRoom(roomID, options, TypedLobby.Default);
            Assert.IsTrue(result);
        }

        public void JoinSession(string room, string player)
        {
            Debug.Log("Join Session");

            Prepare_Internal(room, player);
            
            // bool result = PhotonNetwork.JoinRoom(roomID);
            bool result = PhotonNetwork.JoinOrCreateRoom(roomID, options, TypedLobby.Default);
            Assert.IsTrue(result);
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
            
            // 개인 설정
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                if (player.IsLocal)
                {
                    player.NickName = playerName;
                }
            }
            
            // 게임 시작 버튼
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                matchStart.onClick.RemoveAllListeners();
                matchStart.onClick.AddListener(OnMatchStart);
                
                matchStart.gameObject.SetActive(true);
            }
            
            // 룸 설정
            RepaintRoomList_Internal();
            

            FadeIn_Internal();
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            
            Debug.LogError(message);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            
            RepaintRoomList_Internal();
            Logger.Verbose($"플레이어 입장 : {newPlayer.ActorNumber}:{newPlayer.NickName}");
            
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            
            RepaintRoomList_Internal();
            Logger.Verbose($"플레이어 퇴장 : {otherPlayer.ActorNumber}:{otherPlayer.NickName}");
        }

        void OnMatchStart()
        {
            Debug.Log("Match Start");
            
            // 게임 시작을 통지
        }
        
        void Prepare_Internal(string room, string player)
        {
            this.canvasGroup = GetComponent<CanvasGroup>();
            this.canvasGroup.alpha = 0f;
            
            this.gameObject.SetActive(true);

            this.roomID = room;
            this.playerName = player;
            
            this.options = new RoomOptions {MaxPlayers = 8};
        }

        void RepaintRoomList_Internal()
        {
            Room room = PhotonNetwork.CurrentRoom;

            int index = 0;
            foreach (Player player in room.Players.Values)
            {
                if (player.IsLocal)
                {
                    me.Initialize(player);
                }
                else
                {
                    teammates[index].Initialize(player);
                    index++;
                }
            }

            for (int i = index; i < 7; i++)
            {
                teammates[i].SetVacant();
            }

            // 게임 시작 버튼
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                matchStart.gameObject.SetActive(true);
            }
            else
            {
                matchStart.gameObject.SetActive(false);
            }
            
            roomTitle.text = roomID;
        }


        void FadeIn_Internal()
        {
            canvasGroup.alpha = 0f;
            Tweener tween = canvasGroup.DOFade(1.0f, 0.5f);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //Debug.Log("OnPhotonSerializeView");
        }
    }
}