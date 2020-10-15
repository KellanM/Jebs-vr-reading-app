using UnityEngine;
using System.Collections;

public class BlocksGamemode : MonoBehaviour
{
    public Renderer feedbackBlock;
    private Coroutine colorRoutine;

    public LetterMatchMachine letterMachine;
    public BlockSpawner blockSpawner;
    public GameLoop roundLoop;
    private int streak;

    void Start()
    {
        RestartRound();
    }
    
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

    public void HandleWin()
    {
        SetFeedbackColor(Color.green);
        RestartRound();
    }
    public void HandleLose()
    {
        SetFeedbackColor(Color.red);
        RestartRound();
    }

    public void RestartRound()
    {
        blockSpawner.DespawnAll();
        streak = 0;
        letterMachine.ResetLetters();
        roundLoop.StartRound();
    }
}
