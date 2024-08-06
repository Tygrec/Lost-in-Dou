using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private PlayerData _data;

    public ItemData Equipped() { return _data.EquippedItem; }

    float _speed; // Vitesse de déplacement
    private bool isRunning = false;

    private Coroutine _napCoroutine;

    [SerializeField] CharacterThoughts _characterThoughts;
    private void OnEnable() {
        EventHub.Listen(EventType.Test, OnStartDialog);
    }
    private void OnDisable() {
        EventHub.Unlisten(EventType.Test, OnStartDialog);
    }
    private void OnStartDialog(object payload) {
        Game.G.Dialog.StartDialog((DialogId)payload);
    }

    void Start() {
        _data = (PlayerData)Game.G.GameManager.GetHumanData(Name.Player);
        _speed = Game.G.Values.PLAYER_SPEED;
    }

    private void Update() {
        if (Game.G.GameManager.GetGameState() != GAMESTATE.RUNNING)
            return;

        if (Input.GetKeyDown(KeyCode.R) && _data.Hunger > 0)
            _napCoroutine = StartCoroutine(INap());

        if (!_data.IsNapping) {
            Move();
        }

        if (_data.Hunger <= 0 && _data.IsNapping)
            StopNapping();
    }
    private void Move() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            if (!isRunning) {
                StartRunning();
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            if (isRunning)
                StopRunning();
        }

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        if (moveDirection != Vector3.zero) {
            // Déplace le joueur
            transform.Translate(moveDirection * _speed * Time.deltaTime, Space.World);

            // Tourne le joueur vers la direction de mouvement
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
    }

    private void StartRunning() {
        _speed = Game.G.Values.PLAYER_SPEED * 2;
        isRunning = true;
        Game.G.Time.RegisterRecurringCallback(GetComponent<NeedsManager>().UpdateEnergy, 2);
    }
    private void StopRunning() {
        _speed = Game.G.Values.PLAYER_SPEED;
        isRunning = false;
        Game.G.Time.RemoveRecurringCallback(GetComponent<NeedsManager>().UpdateEnergy);
    }

    private IEnumerator INap() {

        _data.IsNapping = true;
        Game.G.Time.SetTimeSpeed(Game.G.Values.TIME_SPEED_WHILE_NAPPING);

        yield return new WaitForSeconds(Game.G.Values.NAPPING_DURATION);

        Game.G.Time.SetTimeSpeed(Game.G.Values.TIME_SPEED);
        _data.IsNapping = false;
    }
    private void StopNapping() {
        _data.IsNapping = false;

        if (_napCoroutine != null) {
            StopCoroutine(_napCoroutine);
            _napCoroutine = null;
        }
        Game.G.Time.SetTimeSpeed(Game.G.Values.TIME_SPEED);
    }
    public void Sleep() {
        StartCoroutine(ISleep());
    }
    public IEnumerator ISleep() {

        _data.IsSleeping = true;
        Game.G.Scene.Transition();

        yield return new WaitForSeconds(1);

        Game.G.Time.SetNewDay(Game.G.Values.WAKE_UP_HOUR);

        _data.IsSleeping = false;

        _data.ClampStats();
    }

    public void Fishing(FishData fish) {
        Game.G.Inv.Get(InvTag.Player).AddItem(fish);
    }

    public void Equip(ItemInInventory itemValues) {
        _data.EquippedItem = itemValues.Data;
        itemValues.Equipped = true;
    }
    public void UnEquip(ItemInInventory itemValues) {
        _data.EquippedItem = null;
        itemValues.Equipped = false;
    }
    public void ThinkSomething(string thinking) {
        StartCoroutine(IThink(thinking));
    }
    private IEnumerator IThink(string thinking) {
        _characterThoughts.Display(thinking);
        yield return new WaitForSeconds(2);

        _characterThoughts.Hide();
    }

}