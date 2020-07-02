using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PasswordPanel : MonoBehaviour
{
    public string correctPassword = "3248";
    public char symbol;

    string _password;
    public string password
    {
        get
        {
            return _password;
        }
        set
        {
            _password = value;
            tmpro.text = _password;
        }
    }

    public string debugWrittenPassword;
    
    public TextMeshPro tmpro;

    public UnityEvent onCorrectPassword;
    public UnityEvent onIncorrectPassword;

    void Start()
    {
        password = "";
    }

    void Update()
    {
        debugWrittenPassword = password;
    }

    public void Write(string s)
    {
        if (password.Length == correctPassword.Length)
            password = "";

        password += s[0];
    }

    public void Evaluate()
    {
        if (password == correctPassword)
            onCorrectPassword.Invoke();
        else
            onIncorrectPassword.Invoke();

        password = "";
    }

    public void Remove()
    {
        if (password.Length > 0)
        {
            password = password.Remove(password.Length - 1);
        }
    }
}
