namespace Api.Endpoints.ConfirmRecognitionCorrectness;

public class ConfirmRecognitionCorrectnessRequest
{
	public Guid MovieRecognitionId { get; set; }
	public bool RecognizedCorrectly { get; set; }
}