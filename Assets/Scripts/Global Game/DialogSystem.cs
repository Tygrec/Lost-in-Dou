using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    
}
[CreateAssetMenu(fileName="Dialog", menuName ="New Dialog")]
public class Dialog : ScriptableObject {
    public List<Replica> Replicas;
    public TextAsset _replicasFile;
    public TextAsset _answersFile;
}

public class Replica {
    public string Name;
    public string Text;
    public string Emotion;
}