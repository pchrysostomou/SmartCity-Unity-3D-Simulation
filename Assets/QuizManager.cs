using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public QuestionAndAnswers[] questionPool;  // Array of QuestionAndAnswers components
    public GameObject[] options;  // The answer buttons
    public TMP_Text QuestionTxt;  // The question text display
    private int currentQuestion;

    void Start()
    {
        generateQuestion();
    }

    void SetAnswers()
    {
        QuestionData currentQData = questionPool[currentQuestion].GetQuestionData();

        for (int i = 0; i < options.Length; i++)
        {
            // Reset the button state
            options[i].GetComponent<AnswerScript>().isCorrect = false;

            // Set the answer text
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = currentQData.answers[i];

            // Mark the correct answer
            if (i == currentQData.correctAnswerIndex)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
            }
        }
    }

    void generateQuestion()
    {
        if (questionPool.Length == 0)
        {
            Debug.LogError("No questions in the pool!");
            return;
        }

        currentQuestion = Random.Range(0, questionPool.Length);
        QuestionTxt.text = questionPool[currentQuestion].Question;
        SetAnswers();
    }

    public void NextQuestion()
    {
        generateQuestion();
    }
}