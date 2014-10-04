using System;
using System.Collections;
using UnityEngine;

namespace Assets.Promises
{
    namespace Promises
    {
        public class Coroutine<T>
        {
            public Coroutine coroutine;
            private Exception e;
            private T returnVal;

            public T Value
            {
                get
                {
                    if (e != null)
                    {
                        throw e;
                    }
                    return returnVal;
                }
            }

            public IEnumerator InternalRoutine(IEnumerator coroutine)
            {
                while (true)
                {
                    try
                    {
                        if (!coroutine.MoveNext())
                        {
                            yield break;
                        }
                    }
                    catch (Exception e)
                    {
                        this.e = e;
                        yield break;
                    }
                    object yielded = coroutine.Current;
                    if (yielded != null && !(yielded is YieldInstruction || yielded is IEnumerator))
                    {
                        returnVal = (T) yielded;
                        yield break;
                    }
                    yield return coroutine.Current;
                }
            }
        }
    }
}