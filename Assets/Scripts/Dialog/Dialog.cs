using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogId {
    TEST
}

[CreateAssetMenu(fileName = "Dialog", menuName = "New Dialog")]
public class Dialog : ScriptableObject {

    public List<Replica> Replicas = new List<Replica>();

    public TextAsset _replicasFile;
    public List<TextAsset> _answersFile;

    public DialogId Id;

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
    }
}

public class Replica {
    public string Name;
    public string Text;
    public string Emotion;

    public MultipleAnswers Answers;

    public Replica(string name, string emotion, string text, MultipleAnswers answers) {
        Name = name;
        Text = text;
        Emotion = emotion;
        Answers = answers;
    }
}

public class Reactions {
    public Replica Replica;
    public int NextReplicaId;
    public Reactions(Replica replica, int nextReplicaId) {
        Replica = replica;
        NextReplicaId = nextReplicaId;
    }
}

public class MultipleAnswers {
    public List<Answer> Answers = new List<Answer>();

    public MultipleAnswers(List<string> answers, List<Replica> reactions, List<RelationshipModifier> modifier) {
        for (int i = 0; i < answers.Count; i++) {
            Answers.Add(new Answer(reactions[i], answers[i], modifier[i]));
        }
    }
}

public enum RelationshipModifier {
    Positive,
    Negative,
    Neutral
}

public class Answer {
    public Replica Reaction;
    public string Text;
    public RelationshipModifier Modifier;

    public Answer(Replica reaction, string text, RelationshipModifier modifier) {
        Reaction = reaction;
        Text = text;
        Modifier = modifier;
    }
}