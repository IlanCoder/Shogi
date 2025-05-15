using UnityEngine;
using UnityEngine.UI;

public class CemeteryView : MonoBehaviour
{
    [SerializeField] CemeteryCellView pawnView;
    [SerializeField] CemeteryCellView spearView;
    [SerializeField] CemeteryCellView horseView;
    [SerializeField] CemeteryCellView silverView;
    [SerializeField] CemeteryCellView goldView;
    [SerializeField] CemeteryCellView towerView;
    [SerializeField] CemeteryCellView bishopView;

    public void SetCemetaryView(View view) {
        pawnView.SetCemeteryView(view, PieceType.Pawn);
        spearView.SetCemeteryView(view, PieceType.Spear);
        horseView.SetCemeteryView(view, PieceType.Horse);
        silverView.SetCemeteryView(view, PieceType.Silver);
        goldView.SetCemeteryView(view, PieceType.Gold);
        towerView.SetCemeteryView(view, PieceType.Tower);
        bishopView.SetCemeteryView(view, PieceType.Bishop);
    }

    public void EnableCemetaryView(bool enabled = true) {
        pawnView.EnableButton(enabled);
        spearView.EnableButton(enabled);
        horseView.EnableButton(enabled);
        silverView.EnableButton(enabled);
        goldView.EnableButton(enabled);
        towerView.EnableButton(enabled);
        bishopView.EnableButton(enabled);
    }

    public void UpdateCellView(PieceType pieceType, int count) {
        switch (pieceType) {
            case PieceType.Pawn:
                pawnView.UpdateCountText(count);
                break;
            case PieceType.Spear:
                spearView.UpdateCountText(count);
                break;
            case PieceType.Horse:
                horseView.UpdateCountText(count);
                break;
            case PieceType.Silver:
                silverView.UpdateCountText(count);
                break;
            case PieceType.Gold:
                goldView.UpdateCountText(count);
                break;
            case PieceType.Tower:
                towerView.UpdateCountText(count);
                break;
            case PieceType.Bishop:
                bishopView.UpdateCountText(count);
                break;
        }
    }
}
