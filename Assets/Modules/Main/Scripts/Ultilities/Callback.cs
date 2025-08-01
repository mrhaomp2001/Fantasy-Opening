using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Callback
{
    public Action onSuccess;
    public Action<string> onFail;
    public Action onNext;
}
