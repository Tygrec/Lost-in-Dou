using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Showing Dialog", menuName = "New Showing")]
public class ShowingDialog : ScriptableObject {
    public List<ShowingReplica> Replicas;
    public ItemData Item;
}
[Serializable]
public class ShowingReplica {
    public string Text;
    public List<ShowingAnswer> Answers;
}
[Serializable]
public class ShowingAnswer {
    public string Text;
    public bool RightOrWrong;
}