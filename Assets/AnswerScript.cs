using UnityEngine;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer!");
            // Add your score handling here
        }
        else
        {
            Debug.Log("Wrong Answer!");
            // Add wrong answer handling here
        }

        quizManager.NextQuestion();
    }
}