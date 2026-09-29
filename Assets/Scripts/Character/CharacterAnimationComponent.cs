using System.Collections;
using UnityEngine;

public class CharacterAnimationComponent : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [Header("Settings")]
    [SerializeField] private float crossFadeTime = 0.2f;
    [Header("Random Idles")]
    [SerializeField] private string[] idleAnimations;

    private string currentAnimation;

    private void Start()
    {
        StartCoroutine(PlayRandomAnimations());
    }
    private IEnumerator PlayRandomAnimations()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(7, 14));
            Play(idleAnimations[Random.Range(0, idleAnimations.Length)]);
            yield return new WaitForSeconds(Random.Range(3, 5));
            Play("Idle");
        }
    }
    public void Play(string animName)
    {
        if (string.IsNullOrEmpty(animName))
            return;

        if (currentAnimation == animName)
            return;

        currentAnimation = animName;

        animator.CrossFade(animName, crossFadeTime);
    }
}