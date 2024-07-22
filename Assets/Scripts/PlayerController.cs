using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public static PlayerController Instance;

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
    public ItemData Equipped() { return _playerData.EquippedItem; }

    [SerializeField] float _speed = 5f; // Vitesse de déplacement
    private Rigidbody _rb;

    private bool isNapping = false;
    private Coroutine _napCoroutine;

    private void Awake() {
        Instance = this;

        _playerData = GameManager.Instance.GetPlayerData();
    }
    void Start() {
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        if (GameManager.Instance.GetGameState() != GAMESTATE.RUNNING)
            return;

        if (!isNapping) {
            Move();
        }
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
            _napCoroutine = StartCoroutine(INap());
    }
    private void Move() {
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

    public void Eat(ItemData item) {
        if (item.Type != ItemType.Food) {
            Debug.LogError("ERREUR : On essaye de manger quelque chose qui n'est pas de la nourriture");
        }

        _playerData.Hunger += item.SatietyValue;
        _playerData.Thirst += item.ThirstValue;

        _playerData.CheckMaxValues();
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
    public IEnumerator ISleep() {

        StartCoroutine(SceneController.Instance.ITransition());

        yield return new WaitForSeconds(1);

        TimeManager.Instance.SetNewDay(ConstGlobalValues.Instance.WAKE_UP_HOUR);

        _playerData.Energy = 100;
        // Nombre d'heures de sommeil converties en minutes / 5
        UpdateHunger((((ConstGlobalValues.Instance.WAKE_UP_HOUR + 24) - TimeManager.Instance.GetHour()) * 60) / 5, -1);
        UpdateThirst((((ConstGlobalValues.Instance.WAKE_UP_HOUR + 24) - TimeManager.Instance.GetHour()) * 60) / 5, -1);
        UpdateEnergy((((ConstGlobalValues.Instance.WAKE_UP_HOUR + 24) - TimeManager.Instance.GetHour()) * 60) / 5, 1);

        _playerData.CheckMaxValues();
    }

    IEnumerator INap() {
        isNapping = true;
        TimeManager.Instance.SetTimeSpeed(ConstGlobalValues.Instance.TIME_SPEED_WHILE_NAPPING);

        yield return new WaitForSeconds(ConstGlobalValues.Instance.NAPPING_DURATION);

        TimeManager.Instance.SetTimeSpeed(ConstGlobalValues.Instance.TIME_SPEED);
        isNapping = false;

        UpdateHunger((ConstGlobalValues.Instance.NAPPING_DURATION * 60) / 5, -1);
        UpdateThirst((ConstGlobalValues.Instance.NAPPING_DURATION * 60) / 5, -1);
        UpdateEnergy((ConstGlobalValues.Instance.NAPPING_DURATION * 60) / 5, 1);
    }

    public void Equip(ItemInInventory itemValues) {
        _playerData.EquippedItem = itemValues.Data;
        itemValues.Equipped = true;
    }
    public void UnEquip(ItemInInventory itemValues) {
        _playerData.EquippedItem = null;
        itemValues.Equipped = false;
    }

    public void UpdateStatsByTime() {
        UpdateHunger(1, -1);
        UpdateThirst(1, -1);
        UpdateEnergy(1, -1);
    }

    private void UpdateHunger(float time, int posOrNeg) {
        _playerData.Hunger += 3 * time * posOrNeg;
        _playerData.CheckMaxValues();
    }
    private void UpdateThirst(float time, int posOrNeg) {
        _playerData.Thirst += 5 * time * posOrNeg;
        _playerData.CheckMaxValues();
    }
    private void UpdateEnergy(float time, int posOrNeg) {
        _playerData.Energy += 2 * time * posOrNeg;
        _playerData.CheckMaxValues();
    }
}
