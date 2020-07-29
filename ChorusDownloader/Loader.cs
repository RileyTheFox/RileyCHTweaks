using Common.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChorusDownloader
{
    public class Loader
    {

        public void LoadTweak()
        {
            WrapperBase.InitializeLoaders();
            if (this.gameObject != null)
                return;

            this.gameObject = new GameObject("ChorusDownloader by RileyTheFox", new Type[]
            {
                typeof(ChorusTweak)
            });

            Debug.Log("Loaded ChorusDownloader");
            UnityEngine.Object.DontDestroyOnLoad(this.gameObject);
            this.gameObject.SetActive(true);
        }

        public void UnloadTweak()
        {
            if (this.gameObject != null)
            {
                ChorusTweak tweak = gameObject.GetComponent<ChorusTweak>();

                UnityEngine.Object.DestroyImmediate(this.gameObject);
                this.gameObject = null;
            }
        }

        private GameObject gameObject;

    }

}