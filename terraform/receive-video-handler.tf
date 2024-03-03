data "archive_file" "receive-video-handler" {
  type        = "zip"
  source_dir = "${path.module}/../receive-video-handler/"
  output_path = "${path.module}/receive-video-handler.zip"
}

resource "yandex_function" "yandex-reverse-image-search" {
  name               = "receive-video-handler"
  user_hash          = data.archive_file.receive-video-handler.output_sha256
  runtime            = "nodejs18"
  entrypoint         = "src/function.handler"
  memory             = "128"
  execution_timeout  = "10"
  content {
    zip_filename = data.archive_file.receive-video-handler.output_path
  }
}
