using Orkanoid.Game;
using SunsetSystems.Utils;
using UnityEngine;

namespace Orkanoid.Core
{
    [RequireComponent(typeof(Tagger))]
    public class BrickPool : MonoBehaviour
    {
        public delegate void BrickTakenFromPoolHandler(IBrick brick);
        public static event BrickTakenFromPoolHandler BrickTakenFromPool;

        public delegate void BrickReturnedToPoolHandler(IBrick brick);
        public static event BrickReturnedToPoolHandler BrickReturnedToPool;

        public IBrick GetBrickFromPool(IBrick template)
        {
            IBrick brick = Instantiate(template.GetGameObject()).GetComponent<IBrick>();
            BrickTakenFromPool?.Invoke(brick);
            return brick;
        }

        public void ReturnToPool(IBrick brick)
        {
            BrickReturnedToPool?.Invoke(brick);
            Destroy(brick.GetGameObject());
        }
    }
}
