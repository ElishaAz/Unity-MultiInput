using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MultiInput.Internal.Platforms.Linux
{
    /// <summary>
    /// The linux implementation of multi-input.
    /// If you get the following error:
    /// <code>UnauthorizedAccessException: Access to the path "/dev/input/even*" is denied.</code>
    /// You'll need to add the current user to the input group by calling:
    /// <code>sudo gpasswd -a $USER input</code>
    /// And then logging out / restarting.
    ///
    /// <br/><br/>
    /// This code is based on the content of the AggregateInputReader class from the stackoverflow answer.
    /// Its documentation goes as follows:
    /// <list><item>
    /// Since I'm looking to handle input from every device anywhere on the system,
    /// I've put together this classes to aggregate the input events from all the files
    /// in the "/dev/input" folder.
    /// <br/><br/>
    /// Known issue - This code will throw an exception if a usb device is removed while it's running.
    /// I do intend to fix it in my own app implementation, but I don't have time to take care of it now.
    /// </item></list>
    /// </summary>
    public class InputManagerLinux : IInputManager
    {
        private readonly List<KeyboardLinux> keyboards = new List<KeyboardLinux>();

        public InputManagerLinux()
        {
            var keyboardFiles = Directory.GetFiles("/dev/input/by-path/", "*-event-kbd");
            // var mouseFiles = Directory.GetFiles("/dev/input/by-path/", "*-event-mouse");
            // var files = Directory.GetFiles("/dev/input", "event*");

            foreach (var file in keyboardFiles)
            {
                var reader = new InputReader(file);
                var keyboard = new KeyboardLinux(reader, InvokeAnyKeyboardPress);
                keyboards.Add(keyboard);
            }
        }

        public void Dispose()
        {
            foreach (var keyboard in keyboards)
            {
                keyboard.Dispose();
            }

            keyboards.Clear();
        }

        private void InvokeAnyKeyboardPress(KeyCode code, KeyboardLinux keyboard)
        {
            OnAnyKeyboardPress?.Invoke(code, keyboard);
        }

        public IReadOnlyList<IKeyboardInternal> Keyboards => keyboards;

        public event AnyKeyboardAction OnAnyKeyboardPress;
    }
}