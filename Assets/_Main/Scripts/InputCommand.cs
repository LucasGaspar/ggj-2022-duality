using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class InputCommand : MonoBehaviour
{
    private const int MaximumStoredCommands = 50;
    private const int UndefiniedStoredCommand = -1;
    private const string CommandNotFoundError = "COMMAND NOT FOUND";
    private const string CommandLeft = "LEFT";
    private const string CommandRight = "RIGHT";
    private const string CommandStop = "STOP";
    private const string CommandJump = "JUMP";

    [SerializeField] private Character character = null;

    [SerializeField] private List<string> previousCommands = new List<string>();

    private Color inputColor = Color.green;
    private Color errorColor = Color.red;
    private int[] values = null;
    private TMP_Text commandText = null;
    private int currentCommandIndex = UndefiniedStoredCommand;

    private void Awake()
    {
        values = Enumerable.Range((int)KeyCode.A, (int)KeyCode.Z)
            .Append((int)KeyCode.Backspace)
            .Append((int)KeyCode.Return)
            .Append((int)KeyCode.UpArrow)
            .Append((int)KeyCode.DownArrow)
            .ToArray();
        commandText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (!Input.anyKeyDown)
            return;

        if (commandText.text == CommandNotFoundError)
        {
            commandText.text = string.Empty;
            commandText.color = inputColor;
        }

        for (int i = 0; i < values.Length; i++)
        {
            if (Input.GetKeyDown((KeyCode)values[i]))
            {
                if (values[i] == (int)KeyCode.Return)
                    ProcessReturn();
                else if (values[i] == (int)KeyCode.Backspace)
                    ProcessBackspace();
                else if (values[i] == (int)KeyCode.UpArrow)
                    SelectPreviousCommand();
                else if (values[i] == (int)KeyCode.DownArrow)
                    SelectNextCommand();
                else
                    ProcessLetter(values[i]);
            }
        }
    }

    private void ProcessReturn()
    {
        if (commandText.text.Length <= 0)
            return;

        previousCommands.Insert(0, commandText.text);
        if (previousCommands.Count() > MaximumStoredCommands)
            previousCommands.RemoveAt(previousCommands.Count - 1);

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
        currentCommandIndex = UndefiniedStoredCommand;
    }

    private void ProcessBackspace()
    {
        if (commandText.text.Length > 0)
            commandText.text = commandText.text.Substring(0, commandText.text.Length - 1);
    }

    private void SelectPreviousCommand()
    {
        if (previousCommands.Count == 0)
            return;

        if (currentCommandIndex < previousCommands.Count - 1)
            currentCommandIndex++;

        commandText.text = previousCommands[currentCommandIndex];
    }

    private void SelectNextCommand()
    {
        if (previousCommands.Count == 0)
            return;

        if (currentCommandIndex >= 0)
            currentCommandIndex--;

        if (currentCommandIndex == UndefiniedStoredCommand)
            commandText.text = string.Empty;
        else
            commandText.text = previousCommands[currentCommandIndex];
    }

    private void ProcessLetter(int keycode)
    {
        commandText.text += (KeyCode)keycode;
    }
}
