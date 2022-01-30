using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class InputCommand : MonoBehaviour
{
    private const string CommandNotFoundError = "COMMAND NOT FOUND";
    private const string CommandLeft = "LEFT";
    private const string CommandRight = "RIGHT";
    private const string CommandStop = "STOP";
    private const string CommandJump = "JUMP";

    [SerializeField] private Character character = null;

    private Color inputColor = Color.green;
    private Color errorColor = Color.red;
    private int[] values = null;
    private TMP_Text commandText = null;

    private void Awake()
    {
        values = Enumerable.Range((int)KeyCode.A, (int)KeyCode.Z)
            .Append((int)KeyCode.Backspace)
            .Append((int)KeyCode.Return).ToArray();
        commandText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (!Input.anyKeyDown)
            return;

        if (commandText.text == CommandNotFoundError)
            commandText.text = string.Empty;

        for (int i = 0; i < values.Length; i++)
        {
            if (Input.GetKeyDown((KeyCode)values[i]))
            {
                if (values[i] == (int)KeyCode.Return)
                    ProcessReturn();
                else if (values[i] == (int)KeyCode.Backspace)
                    ProcessBackspace();
                else
                    ProcessLetter(values[i]);
            }
        }
    }

    private void ProcessReturn()
    {
        if (commandText.text.Length <= 0)
            return;

        switch (commandText.text)
        {
            case CommandLeft: character.MoveLeft(); break;
            case CommandRight: character.MoveRight(); break;
            case CommandStop: character.Stop(); break;
            case CommandJump: character.Jump(); break;
            default:
                commandText.color = errorColor;
                commandText.text = CommandNotFoundError;
                return;
        }

        commandText.text = string.Empty;
    }

    private void ProcessBackspace()
    {
        if (commandText.text.Length > 0)
            commandText.text = commandText.text.Substring(0, commandText.text.Length - 1);
    }

    private void ProcessLetter(int keycode)
    {
        commandText.color = inputColor;
        commandText.text += (KeyCode)keycode;
    }
}
