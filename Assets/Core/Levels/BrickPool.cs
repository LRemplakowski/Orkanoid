using Orkanoid.Game;
using SunsetSystems.Utils;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Orkanoid.Core
{
    [RequireComponent(typeof(Tagger))]
    public class BrickPool : MonoBehaviour
    {
        public delegate void BrickTakenFromPoolHandler(IBrick brick);
        public static event BrickTakenFromPoolHandler BrickTakenFromPool;

        public delegate void BrickReturnedToPoolHandler(IBrick brick);
        public static event BrickReturnedToPoolHandler BrickReturnedToPool;

        private readonly Dictionary<string, List<IBrick>> brickPools = new();

        public IBrick GetBrickFromPool(IBrick template)
        {
            IBrick brick;
            if (brickPools.TryGetValue(template.GetBrickID(), out List<IBrick> pool))
            {
                brick = pool.Find(brick => brick != null);
                if (brick != null)
                {
                    brick.GetGameObject().SetActive(true);
                }
                else
                {
                    brick = Instantiate(template.GetGameObject(), transform).GetComponent<IBrick>();
                    pool.Add(brick);
                    brick.GetGameObject().SetActive(true);
                }
                pool.Remove(brick);
            }
            else
            {
                List<IBrick> brickPool = new();
                brick = Instantiate(template.GetGameObject(), transform).GetComponent<IBrick>();
                brickPool.Add(brick);
                brick.GetGameObject().SetActive(true);
                brickPools.Add(template.GetBrickID(), brickPool);
                brickPool.Remove(brick);
            }
            BrickTakenFromPool?.Invoke(brick);
            return brick;
        }

        public void ReturnToPool(IBrick brick)
        {
            if (brick == null)
                return;
            if (brickPools.TryGetValue(brick.GetBrickID(), out List<IBrick> pool))
            {
                brick.GetTransform().parent = this.transform;
                brick.ResetBrick();
                brick.GetGameObject().SetActive(false);
                pool.Add(brick);
            }
            BrickReturnedToPool?.Invoke(brick);
        }
    }
}
