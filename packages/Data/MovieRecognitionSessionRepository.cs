using System.Text.Json;
using Domain;
using Ydb.Sdk.Services.Table;
using Ydb.Sdk.Value;

namespace Data;

public interface IMovieRecognitionSessionRepository : ISessionRepository<MovieRecognition, Guid>
{
	Task<(IReadOnlyCollection<MovieRecognition>, Transaction?)> ListByUserIdAsync(Guid userId, TxControl txControl);

	Task<MovieRecognitionStatistics> GetStatisticsAsync();
}

public class MovieRecognitionSessionRepository(Session session) : IMovieRecognitionSessionRepository
{
	private readonly Session _session = session;

	public async Task<(MovieRecognition?, Transaction?)> TryGetAsync(
		Guid id,
		TxControl txControl)
	{
		const string query = """
		                     DECLARE $id AS Utf8;

		                     SELECT *
		                     FROM movie_recognition
		                     WHERE id = $id;
		                     """;

		var parameters = new Dictionary<string, YdbValue>
		{
			["$id"] = YdbValue.MakeUtf8(id.ToString())
		};

		var response = await _session.ExecuteDataQuery(query, txControl, parameters);

		response.Status.EnsureSuccess();

		var resultSet = response.Result.ResultSets[0];
		var row = resultSet.Rows.FirstOrDefault();

		if (row is null)
		{
			return (null, response.Tx);
		}

		var returnedId = Guid.Parse(row["id"].GetUtf8());
		var userId = Guid.Parse(row["user_id"].GetUtf8());
		var videoUrl = new Uri(row["video_url"].GetUtf8());
		var status = Enum.Parse<MovieRecognitionStatus>(row["status"].GetUtf8());
		var createdAt = row["created_at"].GetDatetime();
		var rawVideoId = row["video_id"].GetOptionalUtf8();
		var videoId = rawVideoId is null ? null as Guid? : Guid.Parse(rawVideoId);
		var recognizedMovieJson = row["recognized_movie"].GetOptionalJson();
		var recognizedMovie = recognizedMovieJson is not null
			? JsonSerializer.Deserialize<RecognizedTitle>(recognizedMovieJson)
			: null;
		var failureMessage = row["failure_message"].GetOptionalUtf8();
		var recognizedCorrectly = row["recognized_correctly"].GetOptionalBool();

		var movieRecognition = new MovieRecognition(userId, videoUrl)
		{
			Id = returnedId,
			Status = status,
			CreatedAt = createdAt,
			VideoId = videoId,
			RecognizedMovie = recognizedMovie,
			FailureMessage = failureMessage,
			RecognizedCorrectly = recognizedCorrectly
		};

		return (movieRecognition, response.Tx);
	}

	public async Task<Transaction?> SaveAsync(
		MovieRecognition entity,
		TxControl txControl)
	{
		const string query = """
		                     DECLARE $id AS Utf8;
		                     DECLARE $user_id AS Utf8;
		                     DECLARE $video_url AS Utf8;
		                     DECLARE $created_at AS Datetime;
		                     DECLARE $status AS Utf8;
		                     DECLARE $video_id AS Utf8?;
		                     DECLARE $failure_message AS Utf8?;
		                     DECLARE $recognized_movie AS Json?;
		                     DECLARE $recognized_correctly AS Bool?;

		                     UPSERT INTO movie_recognition(id, user_id, video_url, created_at, status, video_id, failure_message, recognized_movie, recognized_correctly)
		                     VALUES ($id, $user_id, $video_url, $created_at, $status, $video_id, $failure_message, $recognized_movie, $recognized_correctly);
		                     """;

		var parameters = new Dictionary<string, YdbValue>
		{
			["$id"] = YdbValue.MakeUtf8(entity.Id.ToString()),
			["$user_id"] = YdbValue.MakeUtf8(entity.UserId.ToString()),
			["$video_url"] = YdbValue.MakeUtf8(entity.VideoUrl.ToString()),
			["$created_at"] = YdbValue.MakeDatetime(entity.CreatedAt),
			["$status"] = YdbValue.MakeUtf8(entity.Status.ToString()),
			["$video_id"] = YdbValue.MakeOptionalUtf8(entity.VideoId?.ToString()),
			["$recognized_movie"] = YdbValue.MakeOptionalJson(
				entity.RecognizedMovie is not null
					? JsonSerializer.Serialize(entity.RecognizedMovie)
					: null),
			["$failure_message"] = YdbValue.MakeOptionalUtf8(entity.FailureMessage),
			["$recognized_correctly"] = YdbValue.MakeOptionalBool(entity.RecognizedCorrectly)
		};

		var response = await _session.ExecuteDataQuery(query, txControl, parameters);

		response.Status.EnsureSuccess();

		return response.Tx;
	}

	public async Task<(IReadOnlyCollection<MovieRecognition>, Transaction?)> ListByUserIdAsync(Guid userId,
		TxControl txControl)
	{
		const string query = """
		                     DECLARE $user_id AS Utf8;

		                     SELECT *
		                     FROM movie_recognition VIEW idx_user AS mr
		                     WHERE mr.user_id = $user_id;
		                     """;

		var parameters = new Dictionary<string, YdbValue>
		{
			["$user_id"] = YdbValue.MakeUtf8(userId.ToString())
		};

		var response = await _session.ExecuteDataQuery(query, txControl, parameters);

		response.Status.EnsureSuccess();

		var resultSet = response.Result.ResultSets[0];
		var rows = resultSet.Rows;

		var movieRecognitions = rows
			.Select(row =>
			{
				var id = Guid.Parse(row["id"].GetUtf8());
				var returnedUserId = Guid.Parse(row["user_id"].GetUtf8());
				var videoUrl = new Uri(row["video_url"].GetUtf8());
				var status = Enum.Parse<MovieRecognitionStatus>(row["status"].GetUtf8());
				var createdAt = row["created_at"].GetDatetime();
				var rawVideoId = row["video_id"].GetOptionalUtf8();
				var videoId = rawVideoId is null ? null as Guid? : Guid.Parse(rawVideoId);
				var recognizedMovieJson = row["recognized_movie"].GetOptionalJson();
				var recognizedMovie = recognizedMovieJson is not null
					? JsonSerializer.Deserialize<RecognizedTitle>(recognizedMovieJson)
					: null;
				var failureMessage = row["failure_message"].GetOptionalUtf8();
				var recognizedCorrectly = row["recognized_correctly"].GetOptionalBool();

				return new MovieRecognition(returnedUserId, videoUrl)
				{
					Id = id,
					Status = status,
					CreatedAt = createdAt,
					VideoId = videoId,
					RecognizedMovie = recognizedMovie,
					FailureMessage = failureMessage,
					RecognizedCorrectly = recognizedCorrectly
				};
			})
			.OrderByDescending(x => x.CreatedAt)
			.ToArray();

		return (movieRecognitions, response.Tx);
	}

	public async Task<MovieRecognitionStatistics> GetStatisticsAsync()
	{
		const string recognitionCorrectnessQuery = """
		                                           SELECT COUNT(*) AS count, recognized_correctly
		                                           FROM movie_recognition
		                                           GROUP BY recognized_correctly
		                                           HAVING recognized_correctly IS NOT NULL;
		                                           """;

		var recognitionCorrectnessResponse = await _session.ExecuteDataQuery(
			recognitionCorrectnessQuery,
			TxControl.BeginSerializableRW());

		recognitionCorrectnessResponse.Status.EnsureSuccess();
		recognitionCorrectnessResponse.Tx.EnsureNotNull();

		var recognitionCorrectnessResultSet = recognitionCorrectnessResponse.Result.ResultSets[0];
		var recognitionCorrectnessRows = recognitionCorrectnessResultSet.Rows;

		var recognitionCorrectness = recognitionCorrectnessRows
			.Select(row =>
				(Count: row["count"].GetUint64(), RecognizedCorrectly: row["recognized_correctly"].GetOptionalBool()))
			.ToArray();

		var correctlyRecognized = recognitionCorrectness
			.Single(x => x.RecognizedCorrectly.HasValue && x.RecognizedCorrectly.Value)
			.Count;
		var incorrectlyRecognized = recognitionCorrectness
			.Single(x => x.RecognizedCorrectly.HasValue && !x.RecognizedCorrectly.Value)
			.Count;

		const string totalRecognizedQuery = """
		                                    SELECT COUNT_IF(recognized_movie IS NOT NULL) AS count
		                                    FROM movie_recognition;
		                                    """;

		var totalRecognizedResponse = await _session.ExecuteDataQuery(
			totalRecognizedQuery,
			TxControl.Tx(recognitionCorrectnessResponse.Tx).Commit());

		totalRecognizedResponse.EnsureSuccess();

		var totalRecognizedRow = totalRecognizedResponse.Result.ResultSets[0].Rows.First();
		var totalRecognized = totalRecognizedRow["count"].GetUint64();

		return new MovieRecognitionStatistics(totalRecognized, correctlyRecognized, incorrectlyRecognized);
	}
}