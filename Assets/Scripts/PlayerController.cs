using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private PlayerData _playerData;

    public float Hunger() {
        return _playerData.Hunger;
    }
    public float Thirst() { 
        return _playerData.Thirst;
    }
    public float Energy() { 
        return _playerData.Energy;
    }
    public float Life() {
        return _playerData.Life;
    }
    public void LoseEnergy() {
        _playerData.Energy -= 1;
    }
    public ItemData Equipped() { return _playerData.EquippedItem; }

    float _speed; // Vitesse de déplacement

    private bool isNapping = false;
    private bool isSleeping = false;
    private bool isRunning = false;
    private Coroutine _napCoroutine;

    private const int _hungerLoss = 2;
    private const int _thirstLoss = 3;
    private const int _energyLoss = 1;
    private const int _lifeLoss = 1;

    private const int _intervalLoss = 10;

    [SerializeField] CharacterThoughts _characterThoughts;

    void Start() {
        _playerData = Game.G.GameManager.GetPlayerData();
        _speed = Game.G.Values.PLAYER_SPEED;
        Game.G.Time.RegisterRecurringCallback(UpdateStatsByTime, _intervalLoss);
    }
    private void Update() {
        if (Game.G.GameManager.GetGameState() != GAMESTATE.RUNNING)
            return;

        if (Input.GetKeyDown(KeyCode.R) && Hunger() > 0)
            _napCoroutine = StartCoroutine(INap());

        if (!isNapping) {
            Move();
        }
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
        Game.G.Time.RegisterRecurringCallback(UpdateEnergy, 2);
    }
    private void StopRunning() {
        _speed = Game.G.Values.PLAYER_SPEED;
        isRunning = false;
        Game.G.Time.RemoveRecurringCallback(UpdateEnergy);
    }

    public void Eat(ItemData food) {
        if (food.Type != ItemType.Food) {
            Debug.LogError("ERREUR : On essaye de manger quelque chose qui n'est pas de la nourriture");
        }

        _playerData.Hunger += food.SatietyValue;
        _playerData.Thirst += food.ThirstValue;

        _playerData.ClampStats();
    }

    public void Eat(RecipeData food) {
        _playerData.Hunger += food.SatietyValue;
        _playerData.Thirst += food.ThirstValue;
        
        _playerData.ClampStats();
    }

    public void Drink(ItemData item) {
        if (item.Type != ItemType.Food) {
            Debug.LogError("ERREUR : On essaye de manger quelque chose qui n'est pas de la nourriture");
        }

        _playerData.Thirst += item.ThirstValue;
    }
    public void Drink() {
        _playerData.Thirst = 100;
    }

    public void Fishing() {
        PlayerInventory.Instance.AddItem(ItemData.Fish());
    }

    public void Sleep() {
        StartCoroutine(ISleep());
    }

    public IEnumerator ISleep() {

        isSleeping = true;
        Game.G.Scene.Transition();

        yield return new WaitForSeconds(1);

        Game.G.Time.SetNewDay(Game.G.Values.WAKE_UP_HOUR);

        isSleeping = false;

        _playerData.ClampStats();
    }
    private IEnumerator INap() {

        isNapping = true;
        Game.G.Time.SetTimeSpeed(Game.G.Values.TIME_SPEED_WHILE_NAPPING);

        yield return new WaitForSeconds(Game.G.Values.NAPPING_DURATION);

        Game.G.Time.SetTimeSpeed(Game.G.Values.TIME_SPEED);
        isNapping = false;
    }
    private void StopNapping() {
        isNapping = false;

        if(_napCoroutine != null) {
            StopCoroutine(_napCoroutine);
            _napCoroutine = null;
        }
        Game.G.Time.SetTimeSpeed(Game.G.Values.TIME_SPEED);
    }
    public void Equip(ItemInInventory itemValues) {
        _playerData.EquippedItem = itemValues.Data;
        itemValues.Equipped = true;
    }
    public void UnEquip(ItemInInventory itemValues) {
        _playerData.EquippedItem = null;
        itemValues.Equipped = false;
    }
    private void UpdateStatsByTime() {

        _playerData.Hunger -= _hungerLoss;

        if (Hunger() <= 0 && isNapping)
            StopNapping();

        _playerData.Thirst -= _thirstLoss;

        if(Thirst() <= 0 && !isSleeping) {
            _playerData.Life -= _lifeLoss;
        }

        _playerData.Energy = isNapping|| isSleeping ? _playerData.Energy + _energyLoss : _playerData.Energy - _energyLoss;

        _playerData.ClampStats();
    }
    private void UpdateEnergy() {
        _playerData.Energy -= _energyLoss;
    }

    public void ThinkSomething(ThoughtsSituation situation) {
        _characterThoughts.Display(situation);
        StartCoroutine(IThink());
    }
    
    private IEnumerator IThink() {
        yield return new WaitForSeconds(2);

        _characterThoughts.Hide();
    }
}