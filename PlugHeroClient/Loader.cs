using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Common;
using Common.Wrappers;

namespace PlugHero
{
    public class Loader
    {

        public void LoadTweak()
        {
            WrapperBase.InitializeLoaders();
            if (this.gameObject != null)
                return;

            this.gameObject = new GameObject("PlugHero by RileyTheFox", new Type[] 
            {
                typeof(PlugHeroTweak)
            });

            Debug.Log("Loaded PlugHero");
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            this.gameObject.SetActive(true);
        }

        public void UnloadTweak()
        {
            if (this.gameObject != null)
            {
                PlugHeroTweak tweak = gameObject.GetComponent<PlugHeroTweak>();
                tweak.chPlug.Socket.CloseAsync();

                UnityEngine.Object.DestroyImmediate(this.gameObject);
                this.gameObject = null;
            }
        }

        private GameObject gameObject;

    }
}
