using System;
using System.Collections;
using UnityEngine;

namespace Assets.Promises
{
    namespace Promises
    {
        public class CoroutineRunner : MonoBehaviour
        {
            private static CoroutineRunner instance;

            public static Coroutine<T> StartRoutine<T>(IEnumerator coroutine)
            {
                if (!instance)
                {
                    var go = new GameObject("CoroutineManager");
                    instance = go.AddComponent<CoroutineRunner>();
                }
                return instance.StartCoroutine<T>(coroutine);
            }

            public static Coroutine<T> StartRoutine<T>(IEnumerator coroutine, Action<Coroutine<T>> callback)
            {
                if (!instance)
                {
                    var go = new GameObject("CoroutineManager");
                    instance = go.AddComponent<CoroutineRunner>();
                }
                return instance.StartCoroutine(coroutine, callback);
            }

            public Coroutine<T> StartCoroutine<T>(IEnumerator coroutine)
            {
                var coroutineObject = new Coroutine<T>();
                coroutineObject.coroutine = StartCoroutine(coroutineObject.InternalRoutine(coroutine));
                return coroutineObject;
            }

            public Coroutine<T> StartCoroutine<T>(IEnumerator coroutine, Action<Coroutine<T>> callback)
            {
                var coroutineObject = new Coroutine<T>();
                coroutineObject.coroutine = StartCoroutine(coroutineObject.InternalRoutine(coroutine));
                StartCoroutine(CallbackRoutine(coroutineObject, callback));
                return coroutineObject;
            }

            private void Awake()
            {
                DontDestroyOnLoad(gameObject);
            }

            private IEnumerator CallbackRoutine<T>(Coroutine<T> coroutine, Action<Coroutine<T>> callback)
            {
                yield return coroutine.coroutine;
                callback(coroutine);
            }
        }
    }
}