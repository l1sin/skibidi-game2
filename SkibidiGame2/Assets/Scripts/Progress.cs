using System;

[Serializable]
public class Progress
{
    public int Money;
    public int Level;
    public int[] UpgradeLevels;

    public Progress()
    {
        Money = 0;
        Level = 0;
        UpgradeLevels = new int[10] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }
}
