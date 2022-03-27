# Unity-MultiInput
Multiple Keyboards / Mice support for Unity3D
Currently this project only supports Linux editor and standalone.

## Platform specific information
### Linux
The Linux submodule uses /dev/input events. As such, the current user has to be in the `input` group to use it.
You can add the current user to the `input` group by running `sudo gpasswd -a $USER input`.