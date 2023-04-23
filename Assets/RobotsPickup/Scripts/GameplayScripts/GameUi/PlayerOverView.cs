using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using AsteroidsGame = Photon.Pun.Demo.Asteroids.AsteroidsGame;

namespace RobutPickUp
{
    public class PlayerOverView : MonoBehaviourPunCallbacks
    {
        public Transform panel;
        public GameObject PlayerOverviewEntryPrefab;

        private Dictionary<int, GameObject> playerListEntries;

        #region UNITY

        public void Awake()
        {
            playerListEntries = new Dictionary<int, GameObject>();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(PlayerOverviewEntryPrefab, panel);
                entry.transform.localScale = Vector3.one;
                var info = entry.GetComponent<PlayerInfo>();
                info.SetInfo(p.NickName, p.GetScore().ToString(), AsteroidsGame.GetColor(p.GetPlayerNumber()));

                playerListEntries.Add(p.ActorNumber, entry);
            }
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            GameObject go = null;
            if (this.playerListEntries.TryGetValue(otherPlayer.ActorNumber, out go))
            {
                Destroy(playerListEntries[otherPlayer.ActorNumber]);
                playerListEntries.Remove(otherPlayer.ActorNumber);
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            GameObject entry;
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                var info = entry.GetComponent<PlayerInfo>();
                info.SetInfo(targetPlayer.NickName, targetPlayer.GetScore().ToString());
            }
        }

        #endregion
    }
}
