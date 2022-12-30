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

    public PreviewUpdater currentPreview;

    [Header("Debug")]
    public int currentid = 0; //this is the level id, not the index in unlockedLevels list
    public bool currentIsLocked = false;
    [System.NonSerialized] public List<Level> unlockedLevels = new List<Level>();

    private bool animationLeft = false;

    private void Start() {
        PrepareList();
        currentid = Settings.LastLevel;
        animationLeft = false;

        playButton.onClick.AddListener(StartCurrentLevel);
        nextLevelButton.onClick.AddListener(NextLevelClicked);
        prevLevelButton.onClick.AddListener(PrevLevelClicked);

        SetUI(GetCurrentLevel());
    }

    private void SetUI(Level level) {
        levelNameText.text = level.name;
        levelGemsText.text = string.Format("\ue005{0}/{1}", LevelUtils.GetGems(level.id), level.maxGems);

        playButton.interactable = !currentIsLocked;
        levelInfoButton.interactable = !currentIsLocked;

        //todo preview swap animation
        currentPreview.Animate(level, animationLeft);
        animationLeft = false;
    }

    private void PrepareList() {
        unlockedLevels.Clear();
        foreach (Level level in levels.levels) {
            if (level.IsHidden()) continue;
            unlockedLevels.Add(level);
        }
    }

    public void SetLevel(Level level) {
        if (!unlockedLevels.Contains(level)) {
            level = unlockedLevels[0];
            Debug.LogWarning("Warning: tried to set level to unknown or hidden level");
        }

        currentid = level.id;
        currentIsLocked = !LevelUtils.IsUnlocked(level);

        SetUI(level);
    }

    private int GetCurrentIndex() {
        int index = -1;
        for (int i = 0; i < unlockedLevels.Count; i++) {
            if (unlockedLevels[i].id == currentid) {
                index = i;
                break;
            }
        }
        return index;
    }

    private Level GetCurrentLevel() {
        for (int i = 0; i < unlockedLevels.Count; i++) {
            if (unlockedLevels[i].id == currentid) {
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
        if (level == null || !LevelUtils.IsUnlocked(level)) return;//double-check
        Debug.Log("START LEVEL");

        Settings.LastLevel = level.id;
        bgm.FadeOut(1.2f, this);
        UI.CircleFade(false, 1.5f, () => {
            LevelUtils.StartLevel(level);
        });
    }

    private void StartCurrentLevel() {
        if (currentIsLocked) return;
        StartLevel(GetCurrentLevel());
    }

    private void NextLevelClicked() {
        animationLeft = false;
        SetLevel(GetNextLevel());
    }

    private void PrevLevelClicked() {
        animationLeft = true;
        SetLevel(GetPrevLevel());
    }
}
