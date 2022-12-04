using UnityEngine;

public class ScaleToFitTest : MonoBehaviour
{
    private SpriteRenderer sr;
    public bool isCheckOnUpdate = false;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        AdjustBackground();
    }

    private void AdjustBackground()
    {
        // world height is always camera's orthographicSize * 2
        float worldScreenHeight = Camera.main.orthographicSize * 2;

        // world width is calculated by diving world height with screen heigh
        // then multiplying it with screen width
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // to scale the game object we divide the world screen width with the
        // size x of the sprite, and we divide the world screen height with the
        // size y of the sprite
        transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x,
            worldScreenHeight / sr.sprite.bounds.size.y, 1);
    }
    
    public void Update()
    {
        if (isCheckOnUpdate)
        {
            AdjustBackground();
        }
    }
}
