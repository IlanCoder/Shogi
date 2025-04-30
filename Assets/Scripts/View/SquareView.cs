using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SquareView : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] Image imageComponent;

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

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        imageComponent.enabled = false;
    }

    public void SetSquare(int x, int y)
    {
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
}
