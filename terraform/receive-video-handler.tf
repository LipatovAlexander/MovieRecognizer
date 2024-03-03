data "archive_file" "receive-video-handler" {
  type        = "zip"
  source_dir = "${path.module}/../receive-video-handler/"
  output_path = "${path.module}/receive-video-handler.zip"
}

resource "yandex_function" "receive-video-handler" {
  name               = "receive-video-handler"
  user_hash          = data.archive_file.receive-video-handler.output_sha256
  runtime            = "dotnet8"
  entrypoint         = "ReceiveVideoHandler.Handler"
  memory             = "128"
  execution_timeout  = "10"
  content {
    zip_filename = data.archive_file.receive-video-handler.output_path
  }
}

resource "yandex_message_queue" "receive-video-handler-queue" {
  name                        = "receive-video-handler-queue"
  visibility_timeout_seconds  = 10
  message_retention_seconds   = 1209600

  redrive_policy              = jsonencode({
    deadLetterTargetArn = yandex_message_queue.receive-video-handler-deadletter-queue.arn
    maxReceiveCount     = 5
  })
}

resource "yandex_message_queue" "receive-video-handler-deadletter-queue" {
  name                        = "receive-video-handler-deadletter-queue"
}