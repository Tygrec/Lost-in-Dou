using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogDisplayManager : MonoBehaviour {
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

    private bool running;

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
        running = true;
        transform.GetChild(0).gameObject.SetActive(true);
        i = 0;
        _dialog = dialog;

        DisplayReplica(_dialog.Replicas[i]);

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && running) {

            if (_typeCoroutine != null) {

                if (_dialog.Replicas[i].Answers.Count == 0) {
                    _replicaText.text = _dialog.Replicas[i].Text;
                    StopCoroutine(_typeCoroutine);
                    _typeCoroutine = null;
                }

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
        _answersObj.SetActive(replica.Answers.Count > 0);

        _playerAvatar.gameObject.SetActive(replica.Name == Name.Player);
        _pnjAvatar.gameObject.SetActive(replica.Name == Name.Pnj);

        if (replica.Answers.Count > 0) {
            DisplayAnswers(replica.Answers);
            return;
        }

        if (replica.Name == Name.Player) {
            _playerAvatar.sprite = Resources.Load<Sprite>($"Sprites/Characters/{replica.Name}{replica.Emotion}");
            _characterName.text = Game.G.Values.PLAYER_NAME;
        }
        else if (replica.Name == Name.Pnj) {
            _pnjAvatar.sprite = Resources.Load<Sprite>($"Sprites/Characters/{replica.Name}{replica.Emotion}");
            _characterName.text = Game.G.Values.PNJ_NAME;
        }
        else
            Debug.LogError("Erreur : le prénom ne correspond à aucun personnage");

        _typeCoroutine = StartCoroutine(TypeLine(replica.Text));
    }

    private void DisplayAnswers(List<Answer> answers) {
        IsAnswering = true;

        foreach (Answer answer in answers) {
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
        running = false;
        transform.GetChild(0).gameObject.SetActive(false);
        Game.G.Dialog.StopDialog();
    }
}