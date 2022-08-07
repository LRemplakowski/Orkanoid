using SunsetSystems.Utils;
using SunsetSystems.Utils.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Orkanoid.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Tagger))]
    public class FadePanel : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private bool fadedOut = true;

        // Start is called before the first frame update
        private void Start()
        {
            if (!animator)
                animator = GetComponent<Animator>();
        }

        public void OnFadedOut()
        {
            fadedOut = true;
        }

        public void OnFadedIn()
        {
            fadedOut = false;
        }

        public async Task FadeOut(float time = 1.0f)
        {
            Dispatcher dispatcher = Dispatcher.Instance;
            await Task.Run(async () =>
            {
                if (fadedOut)
                    return;
                dispatcher.Invoke(() =>
                {
                    animator.speed = 1f / time;
                    animator.SetTrigger("SwitchFade");
                });
                while (!fadedOut)
                    await Task.Yield();
            });
        }

        public async Task FadeIn(float time = 1.0f)
        {
            Dispatcher dispatcher = Dispatcher.Instance;
            await Task.Run(async () =>
            {
                if (!fadedOut)
                    return;
                dispatcher.Invoke(() =>
                {
                    animator.speed = 1f / time;
                    animator.SetTrigger("SwitchFade");
                });
                while (fadedOut)
                    await Task.Yield();
            });
        }

    }
}
