using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPG.Main
{
    public class CreatingPermaObj : MonoBehaviour
    {
        [SerializeField] GameObject permaObjPrefab;

        static bool createdBefore = false;
        private void Awake()
        {
            if (createdBefore) return;

            CreatePermaObj();

            createdBefore = true;
        }

        private void CreatePermaObj()
        {
            GameObject permaObj = Instantiate(permaObjPrefab);
            DontDestroyOnLoad(permaObj);
        }
    }
}
