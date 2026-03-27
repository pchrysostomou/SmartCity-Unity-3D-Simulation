using UnityEngine;

[System.Serializable]
public class QuestionAndAnswers : MonoBehaviour
{
    [Header("Question Settings")]
    public string Question;
    [Tooltip("Add your possible answers here")]
    public string[] Answers = new string[4];  // Array to hold 4 possible answers
    [Tooltip("Enter the number (1-4) of the correct answer")]
    public int CorrectAnswer;

    void Reset()
    {
        // This ensures the Answers array is initialized with 4 elements when the component is first added
        Answers = new string[4] { "Answer 1", "Answer 2", "Answer 3", "Answer 4" };
        CorrectAnswer = 1;  // Default to first answer
    }

    // Optional: Validate that the correct answer number is within bounds
    void OnValidate()
    {
        if (CorrectAnswer < 1) CorrectAnswer = 1;
        if (CorrectAnswer > 4) CorrectAnswer = 4;

        if (Answers.Length != 4)
        {
            Debug.LogWarning("QuestionAndAnswers must have exactly 4 answers. Resizing array.");
            System.Array.Resize(ref Answers, 4);
        }
    }

    // Helper method to get all question data
    public QuestionData GetQuestionData()
    {
        return new QuestionData
        {
            question = Question,
            answers = Answers,
            correctAnswerIndex = CorrectAnswer - 1  // Convert from 1-based to 0-based index
        };
    }
}

// Struct to hold the question data in a more convenient format
[System.Serializable]
public struct QuestionData
{
    public string question;
    public string[] answers;
    public int correctAnswerIndex;
}