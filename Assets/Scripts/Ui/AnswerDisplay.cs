using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnswerDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    public Replica NextReplica;
    private RelationshipModifier _modifier;

    [SerializeField] TextMeshProUGUI _replicaTxt;

    public void Set(Answer answer) {
        _replicaTxt.text = answer.Text;
        NextReplica = answer.Reaction;
        _modifier = answer.Modifier;
    }

    public void OnPointerDown(PointerEventData eventData) {
        DialogDisplayManager.D.IsAnswering = false;
        Game.G.GameManager.ChangeRelationship(_modifier);
        DialogDisplayManager.D.DisplayReplica(NextReplica);
        DialogDisplayManager.D.ClearAnswers();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GetComponent<Outline>().enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        GetComponent<Outline>().enabled = false;
    }
}