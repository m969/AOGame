using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using EGamePlay;
using System;

namespace AO
{
    public class MonoFunctionComponent : MonoBehaviour
    {
        public bool _DontDestroyOnLoad;


        private void Awake()
        {
            if (_DontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}