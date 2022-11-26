using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectFrame : MonoBehaviour {
    [Header("References")]
    public LevelCollection levels;
    public AudioSource bgm;

    public Button prevLevelButton, nextLevelButton, playButton, levelInfoButton;
    public TextMeshProUGUI levelNameText, levelGemsText;

    public GameObject previewCenter, currentPreview;

    [Header("Debug")]
    public int current = 0; //this is the level id, not the index in unlockedLevels list
    [System.NonSerialized] public List<Level> unlockedLevels = new List<Level>();

    private void Start() {
        PrepareList();
        current = Settings.LastLevel;
        SetUI(GetCurrentLevel());
    }

    private void SetUI(Level level) {
        levelNameText.text = level.name;
        levelGemsText.text = string.Format("\ue005{0}/{1}", LevelUtils.GetGems(level.id), level.maxGems);
    }

    private void PrepareList() {
        //todo
        unlockedLevels.Clear();
        foreach (Level level in levels.levels) {
            unlockedLevels.Add(level);
        }
    }

    private int GetCurrentIndex() {
        int index = -1;
        for (int i = 0; i < unlockedLevels.Count; i++) {
            if (unlockedLevels[i].id == current) {
                index = i;
                break;
            }
        }
        return index;
    }

    private Level GetCurrentLevel() {
        for (int i = 0; i < unlockedLevels.Count; i++) {
            if (unlockedLevels[i].id == current) {
                return unlockedLevels[i];
            }
        }
        return null;
    }

    private Level GetPrevLevel() {
        int index = GetCurrentIndex();
        if(index == 0) return unlockedLevels[unlockedLevels.Count - 1];
        return unlockedLevels[index - 1];
    }

    private Level GetNextLevel() {
        int index = GetCurrentIndex();
        if (index == unlockedLevels.Count - 1) return unlockedLevels[0];
        return unlockedLevels[index + 1];
    }

    public void StartLevel(Level level) {
        Settings.LastLevel = level.id;
        bgm.FadeOut(1.2f, this);
        UI.CircleFade(false, 1.5f, () => {
            LevelUtils.StartLevel(level);
        });
    }
}
