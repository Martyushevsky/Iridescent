using System.Collections.Generic;
using UnityEngine.Networking;

public static class AccountManager
{
    static List<UserAccount> accounts = new List<UserAccount>();

    public static bool AddAccount(UserAccount account)
    {
        if (accounts.Find(acc => acc.login == account.login) == null)
        {
            accounts.Add(account);
            return true;
        }
        return false;
    }

    public static void RemoveAccount(UserAccount account)
    {
        accounts.Remove(account);
    }

    public static UserAccount GetAccount(NetworkConnection conn)
    {
        return accounts.Find(acc => acc.conn == conn);
    }
}
