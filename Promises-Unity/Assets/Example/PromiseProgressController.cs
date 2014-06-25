﻿using UnityEngine;
using Promises;
using System.Threading;

public class PromiseProgressController : MonoBehaviour {
    void Start() {
        transform.localScale = Vector3.zero;
        var promise = getTenWithThen();
        var wrapper = PromiseWrapper.Wrap(promise, "Then");
        wrapper.OnProgressed += progress => transform.localScale = new Vector3(progress * 10, 1f, 1f);
        wrapper.OnFulfilled += result => new GameObject("Then done");
    }

    Promise<int> getTenWithThen() {
        var promise = Promise.WithAction(() => {
            Thread.Sleep(500);
            return 0;
        });

        for (int i = 0; i < 9; i++)
            promise = promise.Then<int>(sleepAction);

        return promise;
    }

    int sleepAction(int result) {
        Thread.Sleep(500);
        return 0;
    }
}
