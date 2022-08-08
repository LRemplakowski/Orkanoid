using UnityEngine;

namespace Orkanoid.Game
{
    public interface IBrick
    {
        bool CountsTowardsWin { get; }

        void TakeHit(int damage);

        int GetHealthLeft();

        int GetPointValue();

        int GetHitsTaken();

        Transform GetTransform();

        GameObject GetGameObject();

        BrickType GetBrickType();

        void ResetBrick();

        string GetBrickID();

        void SetGridPosition(int x, int y);

        void SetGridPosition(Vector2Int position)
        {
            SetGridPosition(position.x, position.y);
        }

        Vector2Int GetGridPosition();
    }
}