public interface ICharacterEvents
{
    void OnLanding(object sender, System.EventArgs e);
    void OnAirbourne(object sender, System.EventArgs e);
    void OnFalling(object sender, System.EventArgs e);
    void OnJump(object sender, System.EventArgs e);
    void OnHorizontalMovementChanges(object sender, int movementDirection);
}
