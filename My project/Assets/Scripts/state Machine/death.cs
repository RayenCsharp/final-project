using UnityEngine;

public class FadeRemoveBehaviour : StateMachineBehaviour
{
    public float fadeTime = 1f;
    private float timeElapsed = 0f;

    private GameObject objToRemove;
    private Renderer[] renderers;
    private Material[] originalMaterials;
    private Color[] originalColors;

    // Called when entering the animation state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeElapsed = 0f;
        objToRemove = animator.gameObject;
        renderers = objToRemove.GetComponentsInChildren<Renderer>();

        originalMaterials = new Material[renderers.Length];
        originalColors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterials[i] = renderers[i].material;

            if (originalMaterials[i].HasProperty("_Color"))
            {
                originalColors[i] = originalMaterials[i].color;
                SetMaterialTransparent(originalMaterials[i]);
            }
            else
            {
                originalColors[i] = Color.white;
            }
        }

    }

    // Called every frame while in this state
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        timeElapsed += Time.deltaTime;
        float alpha = Mathf.Lerp(originalColors[0].a, 0f, timeElapsed / fadeTime);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (originalMaterials[i].HasProperty("_Color"))
            {
                Color c = originalColors[i];
                c.a = alpha;
                originalMaterials[i].color = c;
            }
        }


        if (timeElapsed >= fadeTime)
        {
            Object.Destroy(objToRemove);
        }
    }

    // Utility to switch material to transparent mode
    private void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Mode", 2); // Fade mode
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 1); // ✅ Enable depth writing
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
    }
}

