using UnityEngine;
using System.Collections;

public class BlocksGamemode : MonoBehaviour
{
    public Renderer feedbackBlock;
    private Coroutine colorRoutine;

    public LetterMatchMachine letterMachine;
    public BlockSpawner blockSpawner;
    private int streak;

    private void SetFeedbackColor(Color color)
    {
        if (colorRoutine != null)
            StopCoroutine(colorRoutine);

        colorRoutine = StartCoroutine(SetColor(color));
    }
    private IEnumerator SetColor(Color color)
    {
        feedbackBlock.material.color = color;
        yield return new WaitForSeconds(5);
        feedbackBlock.material.color = Color.white;
    }

    public void HandleCorrect()
    {
        streak++;

        for (int i = 0; i < streak; i++)
            blockSpawner.SpawnCube();

        letterMachine.ResetLetters();

        SetFeedbackColor(Color.green);
    }
    public void HandleIncorrect()
    {
        streak = 0;
        SetFeedbackColor(Color.red);
    }
}
