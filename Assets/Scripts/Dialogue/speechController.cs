using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class speechController : MonoBehaviour
{
    public TextMeshProUGUI textUI;

    public IEnumerator RevealText(string text)
    {
        Debug.Log("reveltext called");
        // Clear the text
        textUI.text = "";

        // Calculate the time interval for each letter
        float interval = 1.0f / text.Length; // Ensure all letters are revealed within 4 seconds

        // Iterate through each letter in the joke
        for (int i = 0; i < text.Length; i++)
        {
            // Append the current letter to the text
            textUI.text += text[i];

            // Wait for the specified interval before revealing the next letter
            yield return new WaitForSeconds(interval);
        }

        // Wait for the specified duration to display the revealed joke
        yield return new WaitForSeconds(1);

        // Clear the text after the display duration
        textUI.text = "";
    }
}
