using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using DatabaseControl;

public class UserAccount
{

    public string login, pass;
    public UserData data;
    public NetworkConnection conn;

    public UserAccount(NetworkConnection conn)
    {
        this.conn = conn;
    }

    public IEnumerator Login(string login, string pass)
    {
        IEnumerator eLogin = DCF.Login(login, pass);
        while (eLogin.MoveNext())
        {
            yield return eLogin.Current;
        }
        string response = eLogin.Current as string;

        if (response == "Success")
        {
            Debug.Log("server login success");
            this.login = login;
            this.pass = pass;
            if (AccountManager.AddAccount(this))
            {
                IEnumerator eLoad = LoadData();
                while (eLoad.MoveNext())
                {
                    yield return eLoad.Current;
                }
                response = eLoad.Current as string;
                if (response == "Error") yield return eLoad.Current;
                else yield return eLogin.Current;
            }
            else
            {
                Debug.Log("account already use");
                yield return "Already use";
            }
        }
        else
        {
            Debug.Log("server login fail. " + response);
            yield return eLogin.Current;
        }
    }

    public IEnumerator Quit()
    {
        IEnumerator eSave = SaveData();
        while (eSave.MoveNext())
        {
            yield return eSave.Current;
        }
        AccountManager.RemoveAccount(this);
    }

    IEnumerator LoadData()
    {
        IEnumerator e = DCF.GetUserData(login, pass);
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;
        if (response == "Error")
        {
            Debug.LogError("UserData for user " + login + " load error with code: " + response);
        }
        else
        {
            Debug.Log("UserData for user " + login + " completely load.");
            Debug.Log(response);
            if (response != "")
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserData));
                data = (UserData)xmlSerializer.Deserialize(new StringReader(response));
            }
            else
            {
                data = new UserData();
            }
        }
    }

    IEnumerator SaveData()
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserData));
        StringWriter writer = new StringWriter();
        xmlSerializer.Serialize(writer, data);
        IEnumerator e = DCF.SetUserData(login, pass, writer.ToString());
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string;
        if (response == "Success") Debug.Log("UserData for user " + login + " completely save.");
        else Debug.LogError("UserData for user " + login + " save error with code: " + response);
    }
}

[Serializable]
public class UserData
{
    public NetworkHash128 characterHash = new NetworkHash128();
    public Vector3 posCharacter;
    public List<int> inventory = new List<int>();
    public List<int> equipment = new List<int>();

    public int level, statPoints;
    public float exp, nextLevelExp;

    public int curHealth;
    public int statDamage, statArmor, statMoveSpeed;
}