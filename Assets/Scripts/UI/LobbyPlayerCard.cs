using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BreakoutStars
{
    public class LobbyPlayerCard : MonoBehaviour
    {
        [SerializeField] private GameObject playerDataPanel;

        [Header("Data Display")]
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private Image selectedCharacterImage;
        [SerializeField] private Toggle isReadyToggle;

        public void UpdateDisplay(LobbyPlayerState lobbyPlayerState)
        {
            playerNameText.text = lobbyPlayerState.PlayerName;
            isReadyToggle.isOn = lobbyPlayerState.IsReady;

            playerDataPanel.SetActive(true);
        }

        public void DisableDisplay()
        {
            playerDataPanel.SetActive(false);
        }

    }
}

