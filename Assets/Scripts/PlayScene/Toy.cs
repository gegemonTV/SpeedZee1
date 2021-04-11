using UnityEngine;

public class Toy
{
    private string type;
    private int Upgrade = 0;
    private int GemsUpdatePrice = 0;
    public void SetUpgrade(int num)
    {
        Upgrade = num;
    }

    public void IncreaseUpgrade()
    {
        Upgrade += 1;
        switch (type)
        {
            case "Zero":
                switch (Upgrade)
                {
                    case 1:
                        GemsUpdatePrice = 25000;
                        break;
                    case 2:
                        GemsUpdatePrice = 60000;
                        break;
                    case 3:
                        GemsUpdatePrice = 80000;
                        break;

                }
                break;
            case "One":
                switch (Upgrade)
                {
                    case 1:
                        GemsUpdatePrice = 120000;
                        break;
                    case 2:
                        GemsUpdatePrice = 160000;
                        break;
                    case 3:
                        GemsUpdatePrice = 220000;
                        break;

                }
                break;
            case "Two":
                switch (Upgrade)
                {
                    case 1:
                        GemsUpdatePrice = 180000;
                        break;
                    case 2:
                        GemsUpdatePrice = 260000;
                        break;
                    case 3:
                        GemsUpdatePrice = 320000;
                        break;

                }
                break;
        }
    }
    public int GetUpgrade()
    {
        return Upgrade;
    }

    public int GetPrice()
    {
        return GemsUpdatePrice;
    }
    public void SaveUpgrade()
    {
        PlayerPrefs.SetInt(type + "Upgrades", Upgrade);
        PlayerPrefs.Save();
        EventManage.CallOnResourceUpdate("Upgrade");
    }

    public Toy(string TypeUpgrade)
    {
        type = TypeUpgrade;
        if (!PlayerPrefs.HasKey(type + "Upgrades"))
            PlayerPrefs.SetInt(type + "Upgrades", 0); 
        Upgrade = PlayerPrefs.GetInt(type + "Upgrades");
        switch (type)
        {
            case "Zero":
                switch (Upgrade)
                {
                    case 0:
                        GemsUpdatePrice = 10000;
                        break;
                    case 1:
                        GemsUpdatePrice = 25000;
                        break;
                    case 2:
                        GemsUpdatePrice = 60000;
                        break;
                    case 3:
                        GemsUpdatePrice = 80000;
                        break;

                }
                break;
            case "One":
                switch (Upgrade)
                {
                    case 0:
                        GemsUpdatePrice = 100000;
                        break;
                    case 1:
                        GemsUpdatePrice = 120000;
                        break;
                    case 2:
                        GemsUpdatePrice = 160000;
                        break;
                    case 3:
                        GemsUpdatePrice = 220000;
                        break;

                }
                break;
            case "Two":
                switch (Upgrade)
                {
                    case 0:
                        GemsUpdatePrice = 160000;
                        break;
                    case 1:
                        GemsUpdatePrice = 180000;
                        break;
                    case 2:
                        GemsUpdatePrice = 260000;
                        break;
                    case 3:
                        GemsUpdatePrice = 320000;
                        break;

                }
                break;
        }
    }
}