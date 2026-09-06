using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Displays one formula slot and publishes its own selection.</summary>
public sealed class HexFormulaButton : MonoBehaviour
{
    [SerializeField] private int formulaIndex;
    [SerializeField] private GameObject formulaObject;

    private Button button;
    private TextMeshProUGUI formulaText;

    private void Awake()
    {
        button = formulaObject.GetComponent<Button>();
        formulaText = formulaObject.GetComponentInChildren<TextMeshProUGUI>(true);
    }

    private void OnEnable()
    {
        button.onClick.AddListener(Select);
        EventBus<FormulaInventoryChangedEvent>.Subscribe(Refresh);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(Select);
        EventBus<FormulaInventoryChangedEvent>.Unsubscribe(Refresh);
    }

    private void Select() => EventBus<FormulaSelectedEvent>.Publish(new FormulaSelectedEvent(formulaIndex));

    private void Refresh(FormulaInventoryChangedEvent payload)
    {
        bool hasFormula = formulaIndex < payload.Formulas.Count;
        formulaObject.SetActive(hasFormula);
        if (hasFormula)
        {
            formulaText.text = payload.Formulas[formulaIndex].ToString();
        }
    }
}
