using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BreakoutStars
{
    public class LobbyPlayerCard : MonoBehaviour
    {
        [Header("Data Display")]
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private Image selectedCharacterImage;
        [SerializeField] private Toggle isReadyToggle;
    }
}

