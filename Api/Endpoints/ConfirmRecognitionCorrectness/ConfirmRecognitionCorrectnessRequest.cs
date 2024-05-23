namespace Api.Endpoints.ConfirmRecognitionCorrectness;

public class ConfirmRecognitionCorrectnessRequest
{
	public Guid UserId { get; set; }
	public Guid MovieRecognitionId { get; set; }
	public bool RecognizedCorrectly { get; set; }
}