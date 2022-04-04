using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class InputManagerLinux : InputManagerAbstract
    {
        private readonly Dictionary<string, KeyboardLinux> keyboards = new Dictionary<string, KeyboardLinux>();
        private readonly Dictionary<string, MouseLinux> mice = new Dictionary<string, MouseLinux>();

        public InputManagerLinux()
        {
            ScanForDevices();
        }

        private void ScanForDevices()
        {
            var keyboardFiles = Directory.GetFiles("/dev/input/by-path/", "*-event-kbd");
            var mouseFiles = Directory.GetFiles("/dev/input/by-path/", "*-event-mouse");
            // var files = Directory.GetFiles("/dev/input", "event*");

            foreach (var file in keyboardFiles)
            {
                if (keyboards.ContainsKey(file)) continue;
                var keyboard = new KeyboardLinux(file, InvokeKeyboardRemoved, InvokeAnyKeyboardPress);
                keyboards.Add(file, keyboard);
                InvokeKeyboardAdded(keyboard);
            }

            foreach (var file in mouseFiles)
            {
                if (mice.ContainsKey(file)) continue;
                var mouse = new MouseLinux(file, InvokeMouseRemoved, InvokeAnyMouseMovement, InvokeAnyMouseEvent,
                    InvokeAnyMouseWheel);
                mice.Add(file, mouse);
                InvokeMouseAdded(mouse);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Dispose()
        {
            foreach (var (_, keyboard) in keyboards)
            {
                keyboard.Dispose();
            }

            foreach (var (_, mouse) in mice)
            {
                mouse.Dispose();
            }

            keyboards.Clear();
            mice.Clear();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void ScanForNewDevices()
        {
            ScanForDevices();
        }
    }
}