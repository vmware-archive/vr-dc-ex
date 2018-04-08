using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {
    public static readonly Color STAT_WHITE_COLOR = new Color(1f, 1f, 1f, 1f);
    public static readonly Color STAT_ORANGE_COLOR = new Color(1f, 0.655f, 0.286f, 1f);
    public static readonly Color STAT_RED_COLOR = new Color(1f, 0.4f, 0.4f, 1f);
    public static readonly Color STAT_BLUE_COLOR = new Color(0.435f, 0.914f, 0.89f, 1f);

    private GameObject tagPrefab;
    private Text title;
    private Text stat1Label;
    private Text stat1Count;
    private Text stat2Label;
    private Text stat2Count;
    private Text stat3Label;
    private Text stat3Count;
    private Image resourceStatLeftImage;
    private Text resourceStatLeftLabel;
    private Text resourceStatLeftUsage;
    private Text resourceStatLeftAmount;
    private Image resourceStatRightImage;
    private Text resourceStatRightLabel;
    private Text resourceStatRightUsage;
    private Text resourceStatRightAmount;
    private CanvasGroup hudGroup;
    private GameObject resourceStatsGroup;
    private Transform tagContainerFirstRow;
    private Transform tagContainerSecondRow;

    public enum ColorFamily {
        White = 0,
        Orange,
        Red,
        Blue
    }

    // TODO: add more fields to control appearance
    public class Resource {
        public string type;
        public int amount;
        public float usage;

        public Resource(string type, int amount, float usage) {
            this.type = type;
            this.amount = amount;
            this.usage = usage;
        }
    }

    // TODO: add more fields to control appearance
    public class Statistic {
        public string type;
        public string value;
        public virtual string StringValue { get { return value; } }

        public Statistic(string type, string value) {
            this.type = type;
            this.value = value;
        }
    }

    public interface IHudVisible {
        string Name { get; }
        string Type { get; }
        ColorFamily ColorFamily { get; }
        List<Statistic> Statistics { get; }
        List<Resource> Resources { get; }
        Dictionary<string, string> Tags { get; }
    }

    public void updateHud(IHudVisible hudVisible) {
        // Clear the hud
        title.text = stat1Label.text = stat1Count.text = stat2Label.text = stat2Count.text = stat3Label.text = stat3Count.text = "";

        // Clean up tags
        foreach (Transform child in tagContainerFirstRow) {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in tagContainerSecondRow) {
            GameObject.Destroy(child.gameObject);
        }

        // Hide stats
        resourceStatsGroup.SetActive(false);

        if (hudVisible == null) {
            title.text = "";
            return;
        }

        if (hudVisible.Type != null && hudVisible.Name != null) {
            title.text = hudVisible.Name;
        }

        if (hudVisible.Statistics.Count >= 3) {
            stat1Label.text = hudVisible.Statistics[0].type;
            stat1Count.text = hudVisible.Statistics[0].StringValue;
            stat2Label.text = hudVisible.Statistics[1].type;
            stat2Count.text = hudVisible.Statistics[1].StringValue;
            stat3Label.text = hudVisible.Statistics[2].type;
            stat3Count.text = hudVisible.Statistics[2].StringValue;
        }

        if (hudVisible.Resources.Count >= 2) {
            resourceStatLeftImage.fillAmount = hudVisible.Resources[0].usage;
            resourceStatLeftLabel.text = hudVisible.Resources[0].type;
            resourceStatLeftUsage.text = hudVisible.Resources[0].usage.ToString("P0");
            resourceStatLeftAmount.text = hudVisible.Resources[0].amount.ToString("N0");
            resourceStatRightImage.fillAmount = hudVisible.Resources[1].usage;
            resourceStatRightLabel.text = hudVisible.Resources[1].type; ;
            resourceStatRightUsage.text = hudVisible.Resources[1].usage.ToString("P0");
            resourceStatRightAmount.text = formatMemory(hudVisible.Resources[1].amount);

            if (hudVisible.ColorFamily == ColorFamily.Orange) {
                resourceStatLeftImage.color = STAT_ORANGE_COLOR;
                resourceStatRightImage.color = STAT_ORANGE_COLOR;
            } else if (hudVisible.ColorFamily == ColorFamily.Red) {
                resourceStatLeftImage.color = STAT_RED_COLOR;
                resourceStatRightImage.color = STAT_RED_COLOR;
            } else if (hudVisible.ColorFamily == ColorFamily.Blue) {
                resourceStatLeftImage.color = STAT_BLUE_COLOR;
                resourceStatRightImage.color = STAT_BLUE_COLOR;
            } else {
                resourceStatLeftImage.color = STAT_WHITE_COLOR;
                resourceStatRightImage.color = STAT_WHITE_COLOR;
            }

            resourceStatsGroup.SetActive(true);
        } else {
            resourceStatsGroup.SetActive(false);
        }

        if (hudVisible.Tags != null) {
            int tagCounter = 0;

            foreach (KeyValuePair<string, string> tag in hudVisible.Tags) {
                GameObject tagGameObj = (GameObject)Instantiate(tagPrefab);

                Text label = tagGameObj.transform.Find("Label").GetComponent<Text>();
				label.alignment = TextAnchor.MiddleCenter;
				label.text = tag.Key + ": " + tag.Value;

                tagCounter++;

                if (tagCounter <= 3) {
                    tagGameObj.transform.SetParent(tagContainerFirstRow);
                } else if (tagCounter <= 6) {
                    tagGameObj.transform.SetParent(tagContainerSecondRow);
                }
                tagGameObj.transform.localPosition = Vector3.zero;
                tagGameObj.transform.localRotation = Quaternion.identity;
            }
        }
    }

    // Use this for initialization
    void Awake() {
        tagPrefab = Resources.Load<GameObject>("Prefabs/Tag");

        hudGroup = GameObject.Find("HUD").GetComponent<CanvasGroup>();

        title = GameObject.Find("Title").GetComponent<Text>();
        stat1Label = GameObject.Find("Stat1Label").GetComponent<Text>();
        stat1Count = GameObject.Find("Stat1Count").GetComponent<Text>();
        stat2Label = GameObject.Find("Stat2Label").GetComponent<Text>();
        stat2Count = GameObject.Find("Stat2Count").GetComponent<Text>();
        stat3Label = GameObject.Find("Stat3Label").GetComponent<Text>();
        stat3Count = GameObject.Find("Stat3Count").GetComponent<Text>();

        resourceStatLeftImage = GameObject.Find("LeftStatImage").GetComponent<Image>();
        resourceStatLeftLabel = GameObject.Find("LeftStatLabel").GetComponent<Text>();
        resourceStatLeftUsage = GameObject.Find("LeftStatUsage").GetComponent<Text>();
        resourceStatLeftAmount = GameObject.Find("LeftStatAmount").GetComponent<Text>();
        resourceStatRightImage = GameObject.Find("RightStatImage").GetComponent<Image>();
        resourceStatRightLabel = GameObject.Find("RightStatLabel").GetComponent<Text>();
        resourceStatRightUsage = GameObject.Find("RightStatUsage").GetComponent<Text>();
        resourceStatRightAmount = GameObject.Find("RightStatAmount").GetComponent<Text>();
        resourceStatsGroup = GameObject.Find("ResourceStats");
        tagContainerFirstRow = GameObject.Find("Tags/FirstRow").transform;
        tagContainerSecondRow = GameObject.Find("Tags/SecondRow").transform;

        // TODO: Hacky way of setting initial state. Need more elegent way to do this. For now set it to inActive after init to avoid NPE
        resourceStatsGroup.SetActive(false);
    }

    private string formatNumber(float value, string format) {
        string formattedValue = "";

        if (value < 1000) {
            formattedValue = value.ToString(format);
        } else if (value < 1000000) {
            formattedValue = (value / 1000).ToString(format) + "K";
        } else {
            formattedValue = (value / 1000000).ToString(format) + "M";
        }

        return formattedValue;
    }

    private string formatMemory(int valueInMB) {
        string formattedValue = "";

        if (valueInMB < 1024) {
            formattedValue = valueInMB.ToString("N0") + "MB";
        } else if (valueInMB < 1024 * 1024) {
            formattedValue = (valueInMB / 1024).ToString("N0") + "GB";
        } else {
            formattedValue = (valueInMB / (1024 * 2014)).ToString("N0") + "TB";
        }

        return formattedValue;
    }
}
