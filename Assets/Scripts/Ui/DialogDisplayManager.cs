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

    private Dialog _dialog = null;
    private ShowingDialog _sDialog = null;
    private int i = 0;
    private int s = 0;
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
        IsAnswering = false;
        transform.GetChild(0).gameObject.SetActive(true);
        i = 0;
        _dialog = dialog;
        UiManager.Instance.HideHud();

        DisplayReplica(_dialog.Replicas[i]);

    }
    public void DisplayDialog(ShowingDialog dialog) {
        running = true;
        _sDialog = dialog;
        transform.GetChild(0).gameObject.SetActive(true);
        s = 0;

        DisplayReplica(_sDialog.Replicas[s]);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && running) {
            // Si le texte est en cours d'écriture
            if (_typeCoroutine != null) {
                // S'il n'y a pas de réponses à donner, on set le texte au texte de la réplique
                if (_dialog.Replicas[i].Answers.Count == 0) {
                    _replicaText.text = _dialog.Replicas[i].Text;
                    StopTypingCoroutine();
                }

            }
            else {
                // Si le dialogue attend une réponse, on bloque le passage à la réplique suivante
                if (IsAnswering)
                    return;

                // Sinon, soit on passe à la réplique suivante, soit on arrête le dialogue
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

    public void StopTypingCoroutine() {
        if (_typeCoroutine == null) return;

        StopCoroutine(_typeCoroutine);
        _typeCoroutine = null;
    }
    public void DisplayReplica(Replica replica) {
        _replicaText.text = string.Empty;
        _answersObj.SetActive(replica.Answers.Count > 0);

        _playerAvatar.gameObject.SetActive(replica.Name == Name.Player);
        _pnjAvatar.gameObject.SetActive(replica.Name == Name.Pnj);

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

        if (replica.Answers.Count > 0) {
            DisplayAnswers(replica.Answers);
        }
    }
    private void DisplayReplica(ShowingReplica replica) {
        _replicaText.text = string.Empty;
        _answersObj.SetActive(replica.Answers.Count > 0);

        _pnjAvatar.gameObject.SetActive(true);
        _pnjAvatar.sprite = Resources.Load<Sprite>($"Sprites/Characters/Pnj{Emotion.Thinking}");

        if (replica.Answers.Count > 0)
            DisplayAnswers(replica.Answers);

        _typeCoroutine = StartCoroutine(TypeLine(replica.Text));
    }
    private void DisplayAnswers(List<Answer> answers) {
        IsAnswering = true;
        ClearAnswers();

        foreach (Answer answer in answers) {
            AnswerDisplay display = Instantiate(Resources.Load<AnswerDisplay>("Prefabs/Ui/Answer"), _answersObj.transform);
            display.Set(answer);
        }
    }
    private void DisplayAnswers(List<ShowingAnswer> answers) {
        IsAnswering = true;
        ClearAnswers();

        foreach (var answer in answers) {
            AnswerDisplay display = Instantiate(Resources.Load<AnswerDisplay>("Prefabs/Ui/Answer"), _answersObj.transform);
            display.Set(answer);
        }
    }

    // Dans le cas du mini-jeu showing
    public void ContinueShowingDialog() {
        s++;
        // S'il n'y a plus de réponses à afficher, c'est qu'on a atteint la fin du jeu
        if (_sDialog.Replicas[s].Answers.Count <= 0) {
            s += Game.G.Dialog.ShowingSuccess ? 0 : 1;

            DisplayReplica(_sDialog.Replicas[s]);
            Game.G.Dialog.StopShowingGame();
            IsAnswering = false;
            _sDialog = null;
        }
        // S'il reste des réponses à afficher, on affiche la suivante
        else if (s < _sDialog.Replicas.Count) {
            DisplayReplica(_sDialog.Replicas[s]);
            return;
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
        _dialog = null;
        _sDialog = null;
        running = false;
        transform.GetChild(0).gameObject.SetActive(false);
        UiManager.Instance.DisplayHud();
        Game.G.Dialog.StopDialog();
    }

    public bool IsAtLastReplica() {
        return i == _dialog.Replicas.Count - 1;
    }
    public bool IsFinished() {
        return _dialog == null || i >= _dialog.Replicas.Count;
    }
}