using Orkanoid.UI;
using SunsetSystems.Utils;
using System.Threading.Tasks;
using UnityEngine;

namespace Orkanoid.Core.Levels
{
    [RequireComponent(typeof(Tagger))]
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private LevelGenerator levelGenerator;

        public async Task NextLevel(int seed)
        {
            EnsureDependencies();
            await levelGenerator.GenerateLevel(seed);

            void EnsureDependencies()
            {
                if (!levelGenerator)
                    levelGenerator = this.FindFirstComponentWithTag<LevelGenerator>(TagConstants.LEVEL_GENERATOR);
            }
        }
    }
}
