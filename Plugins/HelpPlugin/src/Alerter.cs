using Rocket.Unturned.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HelpPlugin
{
    public class Alerter : MonoBehaviour
    {

        public void Awake()
        {
            var coroutine = MsgCoroutine();
            StartCoroutine(coroutine);
        }

        public void OnDestroy() { }

        public static IEnumerator MsgCoroutine()
        {
            WaitForSeconds delay = new WaitForSeconds(Main.AlertInterval);

            while (true)
            {
                yield return delay;
                UnturnedChat.Say(string.Format(Main.Instance.Translate("ALERT"), "/"), true);
            }
        }
    }
}
