using System;

public interface IMobileJoystickEvents
{
    void OnJoystickHorizontalLeftPressed(object sender, EventArgs e);
    void OnJoystickHorizontalLeftUnpressed(object sender, EventArgs e);
    void OnJoystickHorizontalRightPressed(object sender, EventArgs e);
    void OnJoystickHorizontalRightUnpressed(object sender, EventArgs e);
    void OnJoystickVerticalUpPressed(object sender, EventArgs e);
    void OnJoystickVerticalUpUnpressed(object sender, EventArgs e);
    void OnJoystickVerticalDownPressed(object sender, EventArgs e);
    void OnJoystickVerticalDownUnpressed(object sender, EventArgs e);
}
