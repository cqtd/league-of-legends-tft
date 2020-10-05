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

        string roomID;
        string playerName;
        
        RoomOptions options;
        CanvasGroup cg;
        
        public void CreateSession(string roomName, string playerName)
        {
            cg = GetComponent<CanvasGroup>();
            cg.alpha = 0f;
            
            gameObject.SetActive(true);

            roomID = roomName;
            this.playerName = playerName;
            
            Debug.Log("Create Session");

            options = new RoomOptions {MaxPlayers = 8};
            bool result = PhotonNetwork.CreateRoom(roomID, options);
            Assert.IsTrue(result);
        }

        public void JoinSession(string roomName, string playerName)
        {
            cg = GetComponent<CanvasGroup>();
            cg.alpha = 0f;
            
            gameObject.SetActive(true);
            roomID = roomName;
            this.playerName = playerName;
            
            Debug.Log("Join Session");
            

            bool result = PhotonNetwork.JoinRoom(roomID);
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
            
            // 룸 설정
            Room room = PhotonNetwork.CurrentRoom;

            int index = 0;
            foreach (var pair in room.Players)
            {
                Debug.Log($"{pair.Key} : {pair.Value.NickName}");
                
                Player player = pair.Value;
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

            roomTitle.text = roomID;
            
            matchStart.onClick.RemoveAllListeners();
            matchStart.onClick.AddListener(OnMatchStart);

            FadeIn();
        }

        void OnMatchStart()
        {
            Debug.Log("Match Start");
            
            // 게임 시작을 통지
        }

        void FadeIn()
        {
            Tweener tween = cg.DOFade(1.0f, 0.5f);
            tween.onComplete += () =>
            {
                Debug.Log("OnComplete");
            };
        }
    }
}