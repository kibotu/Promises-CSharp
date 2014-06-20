﻿using UnityEngine;
using Promises;
using System;

public class PromiseWrapper : MonoBehaviour {

    public event Fulfilled OnFulfilled;
    public event Failed OnFailed;
    public event Progressed OnProgressed;

    public delegate void Fulfilled(object result);
    public delegate void Failed(Exception error);
    public delegate void Progressed(float progress);

    object _result;
    Exception _error;
    float _progress;
    bool _updateProgress;

    public static PromiseWrapper Wrap<T>(Promise<T> promise) {
        var wrapper = new GameObject("Promise").AddComponent<PromiseWrapper>();
        wrapper.init(promise);
        return wrapper;
    }

    void init<T>(Promise<T> promise) {
        promise.OnFulfilled += result => _result = result;
        promise.OnFailed += error => _error = error;
        promise.OnProgressed += progress => {
            _progress = progress;
            _updateProgress = true;
        };
    }

    void Update() {
        if (_result != null) {
            if (OnFulfilled != null)
                OnFulfilled(_result);
            cleanup();
        }
        if (_error != null) {
            if (OnFailed != null)
                OnFailed(_error);
            cleanup();
        }
        if (_updateProgress) {
            _updateProgress = false;
            if (OnProgressed != null)
                OnProgressed(_progress);
        }
    }

    void cleanup() {
        OnFulfilled = null;
        OnFailed = null;
        OnProgressed = null;
        Destroy(gameObject);
    }
}
