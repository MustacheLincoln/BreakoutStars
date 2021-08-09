using TMPro;
using UnityEngine;

namespace BreakoutStars
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private TMP_InputField IPInputField;
        [SerializeField] private TMP_InputField passwordInputField;

        private void Start()
        {
            nameInputField.text = PlayerPrefs.GetString("PlayerName");
            IPInputField.text = PlayerPrefs.GetString("IP");
            passwordInputField.text = PlayerPrefs.GetString("Password");
        }

        public void OnHostClicked()
        {
            PlayerPrefs.SetString("PlayerName", nameInputField.text);
            PlayerPrefs.SetString("Password", passwordInputField.text);
            PlayerPrefs.Save();

            GameNetPortal.Instance.StartHost();
        }

        public void OnJoinClicked()
        {
            PlayerPrefs.SetString("PlayerName", nameInputField.text);
            PlayerPrefs.SetString("IP", IPInputField.text);
            PlayerPrefs.SetString("Password", passwordInputField.text);
            PlayerPrefs.Save();

            ClientGameNetPortal.Instance.StartClient();
        }
    }
}