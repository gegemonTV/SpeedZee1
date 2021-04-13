using UnityEngine;
using UnityEngine.UI;

public class EventHandlers : MonoBehaviour
{
    // Текст Количества гемов
    public Text GemsText;
    public Text GemsRating;

    // Слайдер + текст энергии
    public Slider EnergySlider;
    public Text EnergyCounter;

    // Слайдер улучшений на главной странице
    public Slider UpgradesSlider;
    public Text UpgradesText;

    // КлючЕки для замочЕков
    public Text KeysText;

    // Слайдер каждой игрушки по отдельности + текст
    public Slider ElSlider;
    public Slider StarSlider;
    public Slider BrilliantSlider;

    public Text ElText;
    public Text StarText;
    public Text BrilliantText;

    public Text ElGemsText;
    public Text StarGemsText;
    public Text BrilliantGemsText;

    // Игрушки
    private Toy El;
    private Toy Star;
    private Toy Brilliant;

    void Start()
    {
        El = new Toy("Zero");
        Star = new Toy("One");
        Brilliant = new Toy("Two");

    }
    void OnEnable()
    {
        EventManage.eventOnResourceUpdate += ResourceUpdater;

    }

    private void ResourceUpdater(string _resID)
    {
        switch (_resID)
        {
            case "Gems":
                GemsText.text = PlayerPrefs.GetInt("Gems").ToString();
                GemsRating.text = PlayerPrefs.GetInt("Gems").ToString();
                break;
            case "Energy":
                EnergySlider.value = PlayerPrefs.GetFloat("Energy") / 28800f;
                EnergyCounter.text = TimeUpdates(PlayerPrefs.GetFloat("Energy"));
                break;
            case "Upgrade":
                switch (PlayerPrefs.GetInt("LeverType"))
                {
                    case 0:
                        UpgradesSlider.value = 0.25f * El.GetUpgrade();
                        UpgradesText.text = $"{25 * El.GetUpgrade()}%";
                        break;
                    case 1:
                        UpgradesSlider.value = 0.25f * Star.GetUpgrade();
                        UpgradesText.text = $"{25 * Star.GetUpgrade()}%";
                        break;
                    case 2:
                        UpgradesSlider.value = 0.25f * Brilliant.GetUpgrade();
                        UpgradesText.text = $"{25 * Brilliant.GetUpgrade()}%";
                        break;
                }
                ElSlider.value = 0.25f * El.GetUpgrade();
                StarSlider.value = 0.25f * Star.GetUpgrade();
                BrilliantSlider.value = 0.25f * Brilliant.GetUpgrade();

                ElText.text = $"{25 * El.GetUpgrade()}%";
                StarText.text = $"{25 * Star.GetUpgrade()}%";
                BrilliantText.text = $"{25 * Brilliant.GetUpgrade()}%";

                ElGemsText.text = El.GetPrice().ToString();
                StarGemsText.text = Star.GetPrice().ToString();
                BrilliantGemsText.text = Brilliant.GetPrice().ToString();
                break;
            case "Keys":
                KeysText.text = PlayerPrefs.GetInt("Keys").ToString();
                break;
        }
    }

    private string TimeUpdates(float t)
    {
        int time = (int)t;
        if (time > 86400f)
        {
            return (time / 86400f).ToString() + " DAYS";
        }
        else
        {
            return $"{time / 3600}:{time / 600 % 6}{time / 60 % 10}:{time % 60 / 10 % 6}{time % 10}";
        }
    }

    void OnDisable()
    {
        EventManage.eventOnResourceUpdate -= ResourceUpdater;
    }

}
