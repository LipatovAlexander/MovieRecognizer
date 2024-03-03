data "archive_file" "receive-video-handler" {
  type        = "zip"
  source_dir = "${path.module}/../receive-video-handler/"
  output_path = "${path.module}/receive-video-handler.zip"
}

resource "yandex_function" "receive-video-handler" {
  name               = "receive-video-handler"
  user_hash          = data.archive_file.receive-video-handler.output_size
  runtime            = "dotnet8"
  entrypoint         = "ReceiveVideoHandler.Handler"
  memory             = "128"
  execution_timeout  = "10"
  content {
    zip_filename = data.archive_file.receive-video-handler.output_path
  }
}
