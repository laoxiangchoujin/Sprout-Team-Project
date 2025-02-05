using UnityEngine;
using UnityEngine.UI;

public class imageRaycastAlpha : MonoBehaviour,ICanvasRaycastFilter
{
    private Image image;
    public float alphaThreshold = 0.1f;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    //这是ICanvasRaycastFilter接口要求实现的方法
    //每次鼠标点击或悬停在UI元素上时都会触发
    //sp:screen position
    public bool IsRaycastLocationValid(Vector2 sp,Camera eventCamera)//检测这个屏幕坐标位置是不是valid
    {
        if (image == null) return true;

        //将屏幕坐标转换为图片的local坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, sp, eventCamera, out Vector2 localPoint);//应该localPoint就是？

        //将图片的本地坐标转换为图片的UV坐标
        Rect rect=image.rectTransform.rect;
        Vector2 normalized = new Vector2((localPoint.x+rect.width*0.5f)/rect.width,
            (localPoint.y+rect.height*0.5f)/rect.height);

        //检查点击位置是否在图片范围内
        if (normalized.x < 0f || normalized.x > 1f || normalized.y < 0f || normalized.y > 1f)
            return false;

        //获取该位置的像素颜色
        try
        {
            Color pixelColor = image.sprite.texture.GetPixelBilinear(normalized.x, normalized.y);
            return pixelColor.a >= alphaThreshold;
        }
        catch//如果获取像素颜色失败，返回true
        {
            return true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
