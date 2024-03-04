resource "yandex_iam_service_account" "api-sa" {
  name        = "api-sa"
}

resource "yandex_serverless_container" "test-container" {
  name               = "api"
  memory             = 128
  service_account_id = yandex_iam_service_account.api-sa.id
  image {
    url = "cr.yandex/${var.api_registry}/${var.api_repository}:${var.github_sha}"
  }
}