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

resource "yandex_iam_service_account" "sa-yandex-reverse-image-search" {
  name        = "yandex-reverse-image-search-sa"
  description = "Service account to invoke yandex-reverse-image-search"
}

resource "yandex_iam_service_account_api_key" "sa-yandex-reverse-image-search-api-key" {
  service_account_id = yandex_iam_service_account.sa-yandex-reverse-image-search.id
}

resource "yandex_function_iam_binding" "yandex-reverse-image-search-iam" {
  function_id = yandex_function.yandex-reverse-image-search.id
  role        = "functions.functionInvoker"

  members = [
    "serviceAccount:${yandex_iam_service_account.sa-yandex-reverse-image-search.id}",
  ]
}
