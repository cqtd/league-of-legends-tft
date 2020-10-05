using System;
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

            connectType = eConnectType.Create;
            options = new RoomOptions {MaxPlayers = 8};

            var setting = new AppSettings()
            {
                
            };
            
            PhotonNetwork.ConnectUsingSettings();
        }

        public void JoinSession(string roomName, string playerName)
        {
            cg = GetComponent<CanvasGroup>();
            cg.alpha = 0f;
            
            gameObject.SetActive(true);
            roomID = roomName;
            this.playerName = playerName;
            
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

            FadeIn();
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
            else
            {
                throw new Exception("Undefined connect type");
            }
            
            Debug.Log("OnConnectedToMaster");
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