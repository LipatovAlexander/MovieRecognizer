data "archive_file" "yandex-reverse-image-search" {
  type        = "zip"
  source_dir = "${path.module}/../yandex-reverse-image-search/"
  output_path = "${path.module}/yandex-reverse-image-search.zip"
}

resource "yandex_function" "yandex-reverse-image-search" {
  name               = "yandex-reverse-image-search"
  user_hash          = data.archive_file.yandex-reverse-image-search.output_sha256
  runtime            = "nodejs18"
  entrypoint         = "src/function.handler"
  memory             = "128"
  execution_timeout  = "10"
  content {
    zip_filename = data.archive_file.yandex-reverse-image-search.output_path
  }
}