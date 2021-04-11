using UnityEngine;

public class ExchangeSwap : MonoBehaviour
{

    public Animator RightAnim;
    public Animator LeftAnim;
    public Animator AlphaChannel;
    public RectTransform SunTrans;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void leftSwipe() {
        RightAnim.Play("LeftSwipe");
        LeftAnim.Play("LeftSwipe");
        AlphaChannel.Play("AlphaAnim");
    }

    public void RightSwipe()
    {
        
        RightAnim.Play("RightSwipe");
        LeftAnim.Play("RightSwipe");
        AlphaChannel.Play("AlphaAnimL");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
