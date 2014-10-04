using System;
using System.Collections;
using UnityEngine;

namespace Assets.Promises
{
    namespace Promises
    {
        public class CoroutineRunner : MonoBehaviour
        {
            private static CoroutineRunner _instance;

            public static Coroutine<T> StartRoutine<T>(IEnumerator coroutine)
            {
                if (!_instance)
                {
                    var go = new GameObject("CoroutineManager") {hideFlags = HideFlags.HideInHierarchy};
                    _instance = go.AddComponent<CoroutineRunner>();
                }
                return _instance.StartCoroutine<T>(coroutine);
            }

            public static Coroutine<T> StartRoutine<T>(IEnumerator coroutine, Action<Coroutine<T>> callback)
            {
                if (!_instance)
                {
                    var go = new GameObject("CoroutineManager") {hideFlags = HideFlags.HideInHierarchy};
                    _instance = go.AddComponent<CoroutineRunner>();
                }
                return _instance.StartCoroutine(coroutine, callback);
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