using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Pun.Demo.Asteroids;

namespace RobutPickUp
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance { get => instance; }
        private static GameManager instance;

        public GameObject infoPanel;
        public Text InfoText;

        public GameObject[] ItemPrefabs;
        [SerializeField] private Transform[] itemSpawnPoints;
        [SerializeField] private Transform[] playerSpawnPoints;
        [SerializeField] private GameObject godViewCam;
        private bool onPlayGame = false;
        private float remainTime = 30f;
        public TMPro.TMP_Text timeRemainTxt;

        #region UNITY

        public void Awake()
        {
            if(instance)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void Update()
        {
            if(onPlayGame)
            {
                CountdownGameplay();
            }
        }

        private void CountdownGameplay()
        {
            if (remainTime <= 0)
            {
                onPlayGame = false;
                remainTime = 0;
                CheckEndOfGame();
            }
            else
                remainTime -= Time.deltaTime;

            DisplayRemainTime();
        }

        private void DisplayRemainTime()
        {
            float minutes = Mathf.FloorToInt(remainTime / 60);
            float seconds = Mathf.FloorToInt(remainTime % 60);
            timeRemainTxt.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public override void OnEnable()
        {
            base.OnEnable();

            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        }

        public void Start()
        {
            Hashtable props = new Hashtable
            {
                {AsteroidsGame.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }

        #endregion

        #region COROUTINES

        private IEnumerator SpawnItem()
        {
            for(int i = 0; i < 10; i++)
            {
                RandomSpawnItem();
            }

            while (true)
            {
                yield return new WaitForSeconds(Random.Range(2f, 5f));

                RandomSpawnItem();
            }
        }

        private void RandomSpawnItem()
        {
            Vector3 position = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Length)].position;
            string itemName = ItemPrefabs[Random.Range(0, ItemPrefabs.Length)].name;
            PhotonNetwork.Instantiate(itemName, position, Quaternion.identity);
        }

        private IEnumerator EndOfGame(string winner, int score)
        {
            float timer = 5.0f;
            if(PlayerControler.player)
                PlayerControler.player.SetControlEnable(false);
            OpenInfoPanel();

            while (timer > 0.0f)
            {
                InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));


                yield return new WaitForEndOfFrame();

                timer -= Time.deltaTime;
            }

            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region PUN CALLBACKS


        public override void OnLeftRoom()
        {
            PhotonNetwork.JoinLobby();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                StartCoroutine(SpawnItem());
            }
        }


        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {

            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }


            // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
            int startTimestamp;
            bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

            if (changedProps.ContainsKey(AsteroidsGame.PLAYER_LOADED_LEVEL))
            {
                if (CheckAllPlayerLoadedLevel())
                {
                    if (!startTimeIsSet)
                    {
                        CountdownTimer.SetStartTime();
                    }
                }
                else
                {
                    // not all players loaded yet. wait:
                    Debug.Log("setting text waiting for players! ", this.InfoText);
                    InfoText.text = "Waiting for other players...";
                }
            }

        }

        #endregion

        private void StartGame()
        {
            onPlayGame = true;
            Debug.Log("StartGame!");

            var playerList = PhotonNetwork.CurrentRoom.Players;
            int playerNumber = -1;
            foreach(var p in playerList.Values)
            {
                playerNumber++;
                if (p.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    break;
            }

            var spawnPoint = playerSpawnPoints[playerNumber].position;
            var playerChar = Profile.character.prefab.name;

            PhotonNetwork.Instantiate(playerChar, spawnPoint, Quaternion.identity);

            if (PhotonNetwork.IsMasterClient)
            {
                StartCoroutine(SpawnItem());
            }

            godViewCam.SetActive(false);
            InfoText.text = "GO";
            Invoke(nameof(HideInfoPanel), 1.5f);
        }

        private bool CheckAllPlayerLoadedLevel()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedLevel;

                if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
                {
                    if ((bool)playerLoadedLevel)
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }

        private void CheckEndOfGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                StopAllCoroutines();
            }

            string winner = "";
            int score = -1;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.GetScore() > score)
                {
                    winner = p.NickName;
                    score = p.GetScore();
                }
            }

            StartCoroutine(EndOfGame(winner, score));
        }

        private void OnCountdownTimerIsExpired()
        {
            StartGame();
        }

        private void HideInfoPanel()
        {
            infoPanel.SetActive(false);
        }

        private void OpenInfoPanel()
        {
            infoPanel.SetActive(true);
        }
    }
}