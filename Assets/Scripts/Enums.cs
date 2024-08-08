public enum GAMESTATE {
    RUNNING,
    PAUSE
}
public enum RelationshipModifier {
    None,
    Positive,
    Negative,
    Neutral
}
public enum Name {
    Player,
    Pnj
}
public enum Emotion {
    Happy,
    Sad,
    Angry,
    Fearful,
    Neutral,
    Wondering,
    Surprised
}
public enum DialogId {
    TEST,
    Showing,
    Surprised,
    NotFood,
    ThankfulForEat,
    Daily,
    DiscoverCave,
    FirstEncounter
}
public enum InvTag {
    Player,
    Stock,
    Craft,
    Kitchen,
    Max,
    Prep1,
    Prep2,
    Prep3,
    Prep4
}
public enum DayTime {
    Morning,
    Afternoon,
    Evening,
    Night
}
public enum ItemType {
    Food,
    Resource,
    Tool
}
public enum SuccessRate {
    Fail,
    Success,
    Critical
}
public enum EventType {
    None,
    Test,
    GiveItemToPnj,
    ShowObjectToPnj
}