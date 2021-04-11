using UnityEngine;

public class VipTimer
{
    private float timer = 0f;
    private bool isEndless = false;
    public VipTimer(float timer = 0f, bool isEndless = false)
    {
        this.timer = timer;
        this.isEndless = isEndless;
        if (isEndless || timer > 0)
        {
            PlayerPrefs.SetInt("VIP", 2);
        }
        else
        {
            PlayerPrefs.SetInt("VIP", 1);
        }
    }
    public void decreaseTimer(float delta)
    {
        timer -= delta;
    }
    public bool getIsEndless()
    {
        return isEndless;
    }
    public void restartTimer(float time)
    {
        timer = time;
    }
    public void setIsEndless(bool state)
    {
        isEndless = state;
    }
    public override string ToString()
    {
        if (timer > 0 && !isEndless)
        {
            int time = (int)timer;
            return $"{time / 3600}:{time / 600 % 6}{time / 60 % 10}:{time % 60 / 10 % 6}{time % 10}";
        }
        else
        {
            if (isEndless)
            {
                return "навсегда";
            }
            else
            {
                return "нет";
            }
        }
    }
    public bool getState()
    {
        if (timer > 0 || isEndless)
        {
            return true;
        }
        return false;
    }
    public void saveState()
    {
        PlayerPrefs.SetFloat("VipTimer", timer);
        PlayerPrefs.SetInt("VipEndless", System.Convert.ToInt32(isEndless));
    }
}
