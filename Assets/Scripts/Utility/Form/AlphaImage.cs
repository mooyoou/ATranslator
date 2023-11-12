using UnityEngine.UI;

public class AlphaImage : Image
{
    protected override void Awake()
    {
        base.Awake();
        alphaHitTestMinimumThreshold = 0.5f;
    }
}
