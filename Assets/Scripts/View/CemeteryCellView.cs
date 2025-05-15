using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CemeteryCellView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countText;

    View view;
    Button buttonComponent;

    PieceType pieceType;

    private void Awake() {
        buttonComponent = GetComponent<Button>();
    }
    public void Start()
    {
        countText.text = "0";
    }

    private void OnEnable()
    {
        buttonComponent.onClick.AddListener(SelectCemeteryCell);
    }

    private void OnDisable()
    {
        buttonComponent.onClick.RemoveListener(SelectCemeteryCell);
    }

    public void SetCemeteryView(View view, PieceType pieceType) {
        this.view = view;
        this.pieceType = pieceType;
    }

    public void EnableButton(bool enabled = true) {
        buttonComponent.enabled = enabled;
    }

    public void UpdateCountText(int count)
    {
        countText.text = count.ToString();
    }

    private void SelectCemeteryCell()
    {
        view?.SelectCemetaryPiece(pieceType);
    }
}
