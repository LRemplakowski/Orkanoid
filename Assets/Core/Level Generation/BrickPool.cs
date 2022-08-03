using Orkanoid.Game;
using SunsetSystems.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orkanoid.Core
{
    [RequireComponent(typeof(Tagger))]
    public class BrickPool : MonoBehaviour
    {
        public AbstractBrick GetBrickFromPool(AbstractBrick template)
        {
            return Instantiate(template);
        }
    }
}
