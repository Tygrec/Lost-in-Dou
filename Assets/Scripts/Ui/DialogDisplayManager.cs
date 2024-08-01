using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogDisplayManager : MonoBehaviour
{
    public static DialogDisplayManager D;

    [SerializeField] Image _pnjAvatar;
    [SerializeField] Image _playerAvatar;
    [SerializeField] TextMeshProUGUI _characterName;
    [SerializeField] TextMeshProUGUI _replicaText;

    private void Awake() {
        if (D == null) {
            D = this;
            DontDestroyOnLoad(D);
        }
        else {
            Destroy(D.gameObject);
        }
    }

    public void DisplayReplica(Replica replica) {
        _playerAvatar.gameObject.SetActive(replica.Name == Game.G.Values.PLAYER_NAME);
        _pnjAvatar.gameObject.SetActive(replica.Name != Game.G.Values.PLAYER_NAME);

        if (replica.Name == Game.G.Values.PLAYER_NAME)
            _playerAvatar.sprite = Resources.Load<Sprite>($"Sprites/Characters/{replica.Name}/{replica.Emotion}");
        else if (replica.Name == Game.G.Values.PNJ_NAME)
            _pnjAvatar.sprite = Resources.Load<Sprite>($"Sprites/Characters/{replica.Name}/{replica.Emotion}");
        else
            Debug.LogError("Erreur : le prénom ne correspond à aucun personnage");

        _characterName.text = replica.Name;
        _replicaText.text = replica.Text;
    }
}