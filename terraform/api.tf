resource "yandex_iam_service_account" "api-sa" {
  name        = "api-sa"
}

resource "yandex_container_registry_iam_binding" "api-sa-puller" {
  registry_id = var.api_registry
  role        = "container-registry.images.puller"

  members = [
    "serviceAccount:${yandex_iam_service_account.api-sa.id}",
  ]
}

resource "yandex_serverless_container" "test-container" {
  name               = "api"
  memory             = 128
  service_account_id = yandex_iam_service_account.api-sa.id
  image {
    url = "cr.yandex/${var.api_registry}/${var.api_repository}:${var.github_sha}"
  }
}