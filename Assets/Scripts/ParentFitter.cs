using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[ExecuteAlways]
#endif
[RequireComponent(typeof(Image))]
public class ParentFitter : AspectRatioFitter
{
    private Image image;
    private RectTransform rectTransform;
    private float nativeRatio;

    protected override void Awake() {
        base.Awake();
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        aspectMode = AspectMode.FitInParent;
    }

    protected override void Start() {
        base.Start();
        CalculateAspectRatio();
    }

#if UNITY_EDITOR
    protected override void OnValidate() {
        base.OnValidate();
        if (aspectMode != AspectMode.FitInParent) aspectMode = AspectMode.FitInParent;
        if (aspectRatio != nativeRatio) CalculateAspectRatio();
    }
#endif

    public void CalculateAspectRatio() {
        if (image == null) return;
        if (image.sprite == null) return;
        float width = image.sprite.rect.width;
        float height = image.sprite.rect.height;

        nativeRatio = width / height;
        aspectRatio = nativeRatio;

        this.SetDirty();
    }
}
