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
    [SerializeField] GameObject _answersObj;

    private Dialog _dialog;
    private int i = 0;
    private Coroutine _typeCoroutine;
    public bool IsAnswering = false;

    private void Awake() {
        if (D == null) {
            D = this;
            DontDestroyOnLoad(D);
        }
        else {
            Destroy(D.gameObject);
        }
    }

    private void Start() {
        _replicaText.text = string.Empty;
    }

    public void DisplayDialog(Dialog dialog) {
        transform.GetChild(0).gameObject.SetActive(true);
        i = 0;
        _dialog = dialog;

        DisplayReplica(_dialog.Replicas[i]);

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {

            if (_typeCoroutine != null) {

                StopCoroutine(_typeCoroutine);
                _typeCoroutine = null;
                _replicaText.text = _dialog.Replicas[i].Text;
            }
            else {
                if (IsAnswering)
                    return;

                i++;
                if (i < _dialog.Replicas.Count) {
                    DisplayReplica(_dialog.Replicas[i]);
                }
                else {
                    EndDialog();
                }
            }
        }
    }

    public void DisplayReplica(Replica replica) {
        _replicaText.text = string.Empty;
        _answersObj.SetActive(replica.Answers != null);

        if (replica.Answers != null) {
            DisplayAnswers(replica.Answers);
            return;
        }

        _playerAvatar.gameObject.SetActive(replica.Name == Game.G.Values.PLAYER_NAME);
        _pnjAvatar.gameObject.SetActive(replica.Name != Game.G.Values.PLAYER_NAME);

        if (replica.Name == Game.G.Values.PLAYER_NAME)
            _playerAvatar.sprite = Resources.Load<Sprite>($"Sprites/Characters/{replica.Name}{replica.Emotion}");
        else if (replica.Name == Game.G.Values.PNJ_NAME)
            _pnjAvatar.sprite = Resources.Load<Sprite>($"Sprites/Characters/{replica.Name}{replica.Emotion}");
        else
            Debug.LogError("Erreur : le prénom ne correspond à aucun personnage");

        _characterName.text = replica.Name;

        _typeCoroutine = StartCoroutine(TypeLine(replica.Text));
    }

    private void DisplayAnswers(MultipleAnswers answers) {
        IsAnswering = true;
        foreach (var answer in answers.Answers) {
            AnswerDisplay display = Instantiate(Resources.Load<AnswerDisplay>("Prefabs/Ui/Answer"), _answersObj.transform);
            display.Set(answer);
        }
    }

    public void ClearAnswers() {
        foreach (Transform child in _answersObj.transform) {
            Destroy(child.gameObject);
        }
    }

    IEnumerator TypeLine(string line) {
        foreach (char c in line) {
            _replicaText.text += c;
            yield return new WaitForSeconds(Game.G.Values.TEXT_SPEED);
        }
        _typeCoroutine = null;
    }
    public void EndDialog() {
        transform.GetChild(0).gameObject.SetActive(false);
        Game.G.Dialog.StopDialog();
    }
}