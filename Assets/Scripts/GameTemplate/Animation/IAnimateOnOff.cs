using System.Collections;
using System.Collections.Generic;
using Promises;
using UnityEngine;

public interface IAnimateOnOff {

	IPromise AnimateOn();

	IPromise AnimateOff();
}
