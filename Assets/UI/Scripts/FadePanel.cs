using SunsetSystems.Utils;
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
            if (fadedOut)
                return;
            animator.speed = 1f / time;
            animator.SetTrigger("SwitchFade");
            while (!fadedOut)
                await Task.Yield();
        }

        public async Task FadeIn(float time = 1.0f)
        {
            if (!fadedOut)
                return;
            animator.speed = 1f / time;
            animator.SetTrigger("SwitchFade");
            while (fadedOut)
                await Task.Yield();
        }

    }
}
