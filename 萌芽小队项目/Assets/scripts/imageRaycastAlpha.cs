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

    //����ICanvasRaycastFilter�ӿ�Ҫ��ʵ�ֵķ���
    //ÿ�����������ͣ��UIԪ����ʱ���ᴥ��
    //sp:screen position
    public bool IsRaycastLocationValid(Vector2 sp,Camera eventCamera)//��������Ļ����λ���ǲ���valid
    {
        if (image == null) return true;

        //����Ļ����ת��ΪͼƬ��local����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(image.rectTransform, sp, eventCamera, out Vector2 localPoint);//Ӧ��localPoint���ǣ�

        //��ͼƬ�ı�������ת��ΪͼƬ��UV����
        Rect rect=image.rectTransform.rect;
        Vector2 normalized = new Vector2((localPoint.x+rect.width*0.5f)/rect.width,
            (localPoint.y+rect.height*0.5f)/rect.height);

        //�����λ���Ƿ���ͼƬ��Χ��
        if (normalized.x < 0f || normalized.x > 1f || normalized.y < 0f || normalized.y > 1f)
            return false;

        //��ȡ��λ�õ�������ɫ
        try
        {
            Color pixelColor = image.sprite.texture.GetPixelBilinear(normalized.x, normalized.y);
            return pixelColor.a >= alphaThreshold;
        }
        catch//�����ȡ������ɫʧ�ܣ�����true
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
