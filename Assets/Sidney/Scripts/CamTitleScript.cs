using UnityEngine;

public class CamTitleScript : MonoBehaviour
{
    [SerializeField] public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            animator.SetBool("isplaying", true);
            
        }
    }
}
