# Unity-MultiInput
Multiple Keyboards / Mice support for Unity3D
Currently this project only supports Linux editor and standalone.

## Platform specific information
### Linux
The Linux submodule uses /dev/input events. As such, the current user has to be in the `input` group to use it.
You can add the current user to the `input` group by running `sudo gpasswd -a $USER input`.

Note that Touchpads are currently not supported, although they *do* show up in the mouse list.

## Related links
### Linux
https://stackoverflow.com/questions/69414090/how-to-create-global-keyboard-hook-with-c-sharp-in-linux
https://unix.stackexchange.com/questions/523108/getting-type-of-evdev-device
https://www.kernel.org/doc/html/v4.14/input/event-codes.html

### Windows
https://stackoverflow.com/questions/91234/multiple-keyboards-and-low-level-hooks
https://forum.unity.com/threads/recieve-window_commands-in-unity.213741/
https://stackoverflow.com/questions/27866645/registering-a-touch-screen-hid-with-registerrawinputdevices-does-not-work


https://stackoverflow.com/questions/6967108/is-idisposable-dispose-called-automatically
