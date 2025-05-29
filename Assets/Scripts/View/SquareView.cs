using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SquareView : MonoBehaviour
{
    [Header("Components")]
    TextMeshProUGUI text;
    [SerializeField] Image imageComponent;
    View view;
    Button buttonComponent;

    [Header("Sprites")]
    [SerializeField] Sprite pawnSprite;
    [SerializeField] Sprite spearSprite;
    [SerializeField] Sprite horseSprite;
    [SerializeField] Sprite silverSprite;
    [SerializeField] Sprite goldSprite;
    [SerializeField] Sprite towerSprite;
    [SerializeField] Sprite bishopSprite;
    [SerializeField] Sprite whiteKingSprite;
    [SerializeField] Sprite blackKingSprite;

    [Header("Upgraded Sprites")]
    [SerializeField] Sprite upPawnSprite;
    [SerializeField] Sprite upSpearSprite;
    [SerializeField] Sprite upHorseSprite;
    [SerializeField] Sprite upSilverSprite;
    [SerializeField] Sprite upTowerSprite;
    [SerializeField] Sprite upBishopSprite;

    int2 gridPos;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        buttonComponent = GetComponent<Button>();
        imageComponent.enabled = false;
    }

    private void OnEnable() {
        buttonComponent.onClick.AddListener(OnSelectSquare);
    }

    private void OnDisable() {
        buttonComponent.onClick.RemoveListener(OnSelectSquare);
    }

    public void SetSquare(int x, int y, View view)
    {
        this.view = view;
        gridPos = new int2(x, y);
        text.text = $"{x},{y}";
    }

    public void AddPiece(ref Piece piece)
    {
        text.enabled = false;
        imageComponent.enabled = true;

        imageComponent.sprite = piece.type switch {
            PieceType.Pawn => pawnSprite,
            PieceType.Spear => spearSprite,
            PieceType.Horse => horseSprite,
            PieceType.Silver => silverSprite,
            PieceType.Gold => goldSprite,
            PieceType.Tower => towerSprite,
            PieceType.Bishop => bishopSprite,
            PieceType.King => piece.team == Team.White ? whiteKingSprite : blackKingSprite,
            PieceType.UpPawn => upPawnSprite,
            PieceType.UpSpear => upSpearSprite,
            PieceType.UpHorse => upHorseSprite,
            PieceType.UpSilver => upSilverSprite,
            PieceType.UpTower => upTowerSprite,
            PieceType.UpBishop => upBishopSprite,
            _ => null
        };

        imageComponent.gameObject.transform.rotation = piece.team switch { 
            Team.White => Quaternion.Euler(0, 0, 0),
            Team.Black => Quaternion.Euler(0, 0, 180),
            _ => Quaternion.identity
        };
    }

    public void RemovePiece()
    {
        imageComponent.enabled = false;
        text.enabled = true;
    }

    void OnSelectSquare() { 
        view?.SelectSquare(gridPos);
    }
}
