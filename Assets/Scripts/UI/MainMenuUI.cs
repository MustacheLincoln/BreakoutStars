using MLAPI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BreakoutStars
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_InputField IPInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button cancelButton;
        [SerializeField] private TMP_Text statusText;

        private void Start()
        {
            nameInputField.text = PlayerPrefs.GetString("PlayerName");
            IPInputField.text = PlayerPrefs.GetString("IP");
            passwordInputField.text = PlayerPrefs.GetString("Password");
        }

        private void Update()
        {
            hostButton.interactable = !NetworkManager.Singleton.IsClient;
            joinButton.interactable = !NetworkManager.Singleton.IsClient;
            cancelButton.interactable = NetworkManager.Singleton.IsClient;

            if (!NetworkManager.Singleton.IsClient)
                if (statusText.text == "Joining lobby...")
                    statusText.text = "Connection timed out, check IP and Password";
        }

        public void OnHostClicked()
        {
            if (nameInputField.text == "")
            {
                statusText.text = "Invalid Name";
            }
            else
            {
                PlayerPrefs.SetString("PlayerName", nameInputField.text);
                PlayerPrefs.SetString("Password", passwordInputField.text);

                GameNetPortal.Instance.StartHost();

                statusText.text = "Creating lobby...";
            }

        }

        public void OnJoinClicked()
        {
            if (IPInputField.text == "")
            {
                statusText.text = "Invalid IP";
            }
            else if (nameInputField.text == "")
            {
                statusText.text = "Invalid Name";
            }
            else
            {
                PlayerPrefs.SetString("PlayerName", nameInputField.text);
                PlayerPrefs.SetString("IP", IPInputField.text);
                PlayerPrefs.SetString("Password", passwordInputField.text);

                ClientGameNetPortal.Instance.StartClient();

                statusText.text = "Joining lobby...";
            }
        }

        public void OnCancelClicked()
        {
            NetworkManager.Singleton.StopClient();
            GameNetPortal.Instance.RequestDisconnect();
            statusText.text = "";
        }
    }
}