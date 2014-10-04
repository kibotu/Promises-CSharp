using System;
using UnityEngine;

namespace Assets.Promises
{
    public static class PromiseWrapperExtension
    {
        public static PromiseWrapper QueueOnMainThread<T>(this Promise<T> promise)
        {
            return PromiseWrapper.Wrap(promise);
        }
    }

    public class PromiseWrapper : MonoBehaviour
    {
        public delegate void Failed(Exception error);

        public delegate void Fulfilled(object result);

        public delegate void Progressed(float progress);

        private Exception _error;
        private float _progress;
        private object _result;
        private bool _updateProgress;
        public event Fulfilled OnFulfilled;
        public event Failed OnFailed;
        public event Progressed OnProgressed;

        public static PromiseWrapper Wrap<T>(Promise<T> promise, string name = "Promise")
        {
            var wrapper = new GameObject(name).AddComponent<PromiseWrapper>();
            wrapper.init(promise.Wrap<object>());
            return wrapper;
        }

        private void init(Promise<object> promise)
        {
            promise.OnFulfilled += result => _result = result;
            promise.OnFailed += error => _error = error;
            promise.OnProgressed += progress =>
            {
                _progress = progress;
                _updateProgress = true;
            };
        }

        private void Update()
        {
            if (_updateProgress)
            {
                _updateProgress = false;
                if (OnProgressed != null)
                    OnProgressed(_progress);
            }
            if (_result != null)
            {
                if (OnFulfilled != null)
                    OnFulfilled(_result);
                cleanup();
            }
            if (_error != null)
            {
                if (OnFailed != null)
                    OnFailed(_error);
                cleanup();
            }
        }

        private void cleanup()
        {
            OnFulfilled = null;
            OnFailed = null;
            OnProgressed = null;
            Destroy(gameObject);
        }
    }
}