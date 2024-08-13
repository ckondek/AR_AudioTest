using UnityEngine;

public class AnimateShaderOnCollision : MonoBehaviour
{
    public Material material; // Assign this in the inspector
    public string parameterName = "_Speed"; // The exact parameter name in Shader Graph
    public float targetValue = 1.0f; // The value to animate to
    public float duration = 1.0f; // Duration of the animation

    private float initialValue;
    private bool isAnimating = false;
    private float animationTime = 0.0f;

    private void Start()
    {
        if (material == null)
        {
            Debug.LogError("Material is not assigned!");
            return;
        }

        initialValue = material.GetFloat(parameterName);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MainCamera"))
        {
            isAnimating = true;
            animationTime = 0.0f;
        }
    }

    private void Update()
    {
        if (isAnimating)
        {
            animationTime += Time.deltaTime;
            float t = animationTime / duration;
            if (t > 1.0f)
            {
                t = 1.0f;
                isAnimating = false;
            }
            float currentValue = Mathf.Lerp(initialValue, targetValue, t);
            material.SetFloat(parameterName, currentValue);
        }
    }
}
