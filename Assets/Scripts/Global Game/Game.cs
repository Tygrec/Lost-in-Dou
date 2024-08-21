using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    static public Game G;

    public GameManager GameManager;
    public SceneSystem Scene;
    public ConstGlobalValues Values;
    public TimeManager Time;
    public CraftManager Craft;
    public CookManager Cook;
    public DataBase Db;
    public Player Player;
    public PnjManager Pnj;
    public InventoryManager Inv;
    public DialogSystem Dialog;
    public SoundManager Sound;
    public CameraManager Camera;

    private void Awake() {

        if (G == null) {
            G = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
