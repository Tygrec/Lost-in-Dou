using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    private bool _simpleAnswer;
    
    public Replica NextReplica;
    private RelationshipModifier _modifier;
    private EventType _event = EventType.None;
    private bool _rightOrWrong;

    [SerializeField] TextMeshProUGUI _replicaTxt;

    public void Set(Answer answer) {
        _simpleAnswer = true;
        _replicaTxt.text = answer.Text;
        NextReplica = answer.Reaction;
        _modifier = answer.Modifier;
        _event = answer.eventType;
    }

    public void Set(ShowingAnswer answer) {
        _simpleAnswer = false;
        _replicaTxt.text = answer.Text;
        _rightOrWrong = answer.RightOrWrong;
    }

    public void OnPointerDown(PointerEventData eventData) {
        DialogDisplayManager.D.StopTypingCoroutine();

        // On vérifie qu'il n'y ait pas d'évènements à lancer. Si c'est le cas, on le lance
        if (_event != EventType.None) {
            EventHub.SendEvent(_event, null);
            DialogDisplayManager.D.ClearAnswers();
            return;
        }

        DialogDisplayManager.D.IsAnswering = false;

        // S'il s'agit d'un dialogue classique, on applique le modifier en fonction de la réponse choisie et on affiche la réaction en prochaine réplique
        if (_simpleAnswer) {
            Game.G.GameManager.ChangeRelationship(_modifier);
            DialogDisplayManager.D.DisplayReplica(NextReplica);
        }
        // S'il s'agit d'un dialogue du mini-jeu showing, on vérifie que la réponse est bonne, puis on continue le dialogue
        else {
            if (!_rightOrWrong)
                Game.G.Dialog.ShowingSuccess = false;
            DialogDisplayManager.D.ContinueShowingDialog();
        }

    }

    public void OnPointerEnter(PointerEventData eventData) {
        GetComponent<Outline>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponent<Outline>().enabled = false;
    }
}