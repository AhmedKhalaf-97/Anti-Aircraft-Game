using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelTesting : MonoBehaviour
{
    private delegate void TestDelegate();
    private TestDelegate testDelegate;

    private delegate bool TestBoolDelegate(int i);
    private TestBoolDelegate testBoolDelegate;

    private Action testAction;
    private Action<int, float> testIntFloatAction;

    private Func<bool> testBoolFunc;
    private Func<int, bool> testIntBoolFunc;

    private void OnEnable()
    {
        testDelegate += delegate () { print("Anonymous Method"); };
        testDelegate += () => { print("Lambda Method"); };

        testDelegate += TestDelegateMethod;
        testDelegate += SecondTestDelegateMethod;

        //testBoolDelegate += (int i) => { return i > 5; };
        testBoolDelegate += TestBoolDelegateMethod;

        testAction += TestActionMethod;
        testIntFloatAction += TestIntFloatAction;
        
        testBoolFunc += TestBoolFunc;
        testIntBoolFunc += TestBoolDelegateMethod;
    }

    private void OnDisable()
    {
        testDelegate -= TestDelegateMethod;
        testDelegate -= SecondTestDelegateMethod;

        testBoolDelegate -= TestBoolDelegateMethod;

        testAction -= TestActionMethod;
        testIntFloatAction -= TestIntFloatAction;

        testBoolFunc -= TestBoolFunc;
        testIntBoolFunc -= TestBoolDelegateMethod;
    }

    private void Start()
    {
        if (testDelegate != null)
            testDelegate();

        if (testBoolDelegate != null)
            print(testBoolDelegate(1));

        if (testAction != null)
            testAction();

        if (testIntFloatAction != null)
            testIntFloatAction(2, 3.5f);

        if (testBoolFunc != null)
            print(testBoolFunc());

        if (testIntBoolFunc != null)
            print(testIntBoolFunc(6));
    }

    void TestDelegateMethod()
    {
        print("Test is Done");
    }

    void SecondTestDelegateMethod()
    {
        print("Second Test Is Done");
    }

    bool TestBoolDelegateMethod(int i)
    {
        return i < 5;
    }

    void TestActionMethod()
    {
        print("Action is Done");
    }

    void TestIntFloatAction(int i, float x)
    {
        print("i is " + i + ", x is " + x);
    }

    bool TestBoolFunc()
    {
        return false;
    }
}