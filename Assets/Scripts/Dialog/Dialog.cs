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
