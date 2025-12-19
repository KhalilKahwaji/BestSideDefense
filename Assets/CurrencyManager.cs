using TMPro;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [Header("Currency Settings")]
    public int startingCurrency = 100;
    public int maxCurrency = 999;

    [Header("Passive Income")]
    public int passiveIncomeAmount = 1;
    public float passiveIncomeInterval = 1f;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currencyText;

    public int CurrentCurrency { get; private set; }

    private float passiveTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CurrentCurrency = Mathf.Clamp(startingCurrency, 0, maxCurrency);
        UpdateCurrencyText();
    }

    private void Update()
    {
        HandlePassiveIncome();
        UpdateCurrencyText();

        // === TEST INPUT (REMOVE LATER IF YOU WANT) ===
        if (Input.GetKeyDown(KeyCode.E))
            AddCurrency(10);

        if (Input.GetKeyDown(KeyCode.Q))
            RemoveCurrency(10);
    }

    // =======================
    // Currency Logic
    // =======================

    public bool CanAfford(int amount)
    {
        return CurrentCurrency >= amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (!CanAfford(amount))
            return false;

        CurrentCurrency -= amount;
        return true;
    }

    public void AddCurrency(int amount)
    {
        CurrentCurrency = Mathf.Clamp(CurrentCurrency + amount, 0, maxCurrency);
    }

    public void RemoveCurrency(int amount)
    {
        CurrentCurrency = Mathf.Clamp(CurrentCurrency - amount, 0, maxCurrency);
    }

    // =======================
    // Passive Income
    // =======================

    private void HandlePassiveIncome()
    {
        passiveTimer += Time.deltaTime;

        if (passiveTimer >= passiveIncomeInterval)
        {
            AddCurrency(passiveIncomeAmount);
            passiveTimer = 0f;
        }
    }

    // =======================
    // Enemy Kill Reward
    // =======================

    public void OnEnemyKilled(int rewardAmount)
    {
        AddCurrency(rewardAmount);
    }

    // =======================
    // UI
    // =======================

    private void UpdateCurrencyText()
    {
        if (currencyText != null)
        {
            currencyText.text = "Brainrot: " + CurrentCurrency;
        }
    }
}