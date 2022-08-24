using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundUpdater : MonoBehaviour {
    private const float WIDTH = 24.36f * 2 / 5f, HEIGHT = 9.46f * 2 / 5f, ZBACK = 30f;

    public float z = 1f, z2 = 1.5f, zback = 2.5f;
    public GameObject cam, backdrop;
    public SpriteRenderer bg1, bg2;

    public Background[] backgrounds;
    private Background current;

    void Start() {
        if(current == null) current = backgrounds[0];
        Init();
    }

    void Update() {
        backdrop.transform.position = CalcPos(zback, current.scrollx, current.scrolly);

        bg1.transform.position = CalcPos(z, current.scrollx, current.scrolly);
        bg2.transform.position = CalcPos(z2, current.scrollx, current.scrolly);
    }

    private Vector3 CalcPos(float z, bool scrollx, bool scrolly) {
        return new Vector3(scrollx ? -clampX(cam.transform.position.x * 0.05f / z) : 0f
            , scrolly ? -clampY(cam.transform.position.y * 0.05f / z) : 0f, 
            z + ZBACK);
    }

    private void Init() {
        bg1.sprite = current.layer1;
        bg2.sprite = current.layer2;
    }

    private float clampX(float x) {
        return x % WIDTH - WIDTH / 2f;
    }

    private float clampY(float y) {
        return y % HEIGHT - HEIGHT / 2f;
    }

    [System.Serializable]
    public class Background {
        public bool scrollx = true, scrolly = true;
        public Sprite layer1, layer2;

        public float minY = 3f, maxY = 300f;
    }
}
