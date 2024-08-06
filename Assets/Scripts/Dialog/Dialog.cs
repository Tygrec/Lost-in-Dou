using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dialog", menuName = "New Dialog")]
public class Dialog : ScriptableObject {
    public List<Replica> Replicas = new List<Replica>();
    public DialogId Id;
}

[Serializable]
public class Replica {
    public Name Name;
    public Emotion Emotion;
    public string Text;

    public List<Answer> Answers;
}

[Serializable]
public class Answer {
    public string Text;
    public Replica Reaction;
    public RelationshipModifier Modifier;
    public EventType eventType;
}

/*
    public TextAsset _replicasFile;
    public List<TextAsset> _answersFile;
    public void Initialize() {
        ParseReplicas();
    }
    // COLONNE 1 : NOM ou REPONSE
    // COLONNE 2 : EMOTION
    // COLONNE 3 : TEXTE ou REPONSE ID
    private void ParseReplicas() {
        string[] lines = _replicasFile.text.Split('\n');

        int i = 0;

        foreach (string line in lines) {
            string[] parts = line.Split('\t');

            if (parts[0] == "Réponses") {
                ParseAnswers(parts, i+1);
            }
            else {
                Replicas.Add(new Replica(parts[0], parts[1], parts[2], null));
            }
        }
    }

    private void ParseAnswers(string[] line, int nextReplica) {
        int id = int.Parse(line[2]);

        string[] lines = _answersFile[id].text.Split('\n');
        List<string> answersText = new List<string>();
        List<Replica> reactions = new List<Replica>();
        List<RelationshipModifier> modifiers = new List<RelationshipModifier>();

        foreach (var l in lines) {
            string[] parts = l.Split('\t');
            answersText.Add(parts[0]);
            reactions.Add(new Replica(Game.G.Values.PNJ_NAME, parts[1], parts[2], null));
            modifiers.Add(ParseModifier(parts[3]));
        }
        var answer = new MultipleAnswers(answersText, reactions, modifiers);
        var replica = new Replica(Game.G.Values.PLAYER_NAME, "", "", answer);
        Replicas.Add(replica);
    } 

    private RelationshipModifier ParseModifier(string text) {

        if (text.Contains("Positif"))
            return RelationshipModifier.Positive;
        else if (text.Contains("Neutre"))
            return RelationshipModifier.Neutral;
        else if (text.Contains("Négatif"))
            return RelationshipModifier.Negative;

        Debug.LogError("Modificateur de relation non reconnu");
        return RelationshipModifier.Neutral;
    }*/