using UnityEngine;

namespace Orkanoid.Game
{
    public interface IBrick
    {
        void TakeHit(int damage);

        int GetHealthLeft();

        int GetPointValue();

        Transform GetTransform();

        GameObject GetGameObject();

        BrickType GetBrickType();
    }
}