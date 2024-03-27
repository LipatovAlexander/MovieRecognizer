resource "yandex_iam_service_account" "frontend-sa" {
  name = "frontend-sa"
}

resource "yandex_container_registry_iam_binding" "frontend-sa-puller" {
  registry_id = var.frontend_registry
  role        = "container-registry.images.puller"

  members = [
    "serviceAccount:${yandex_iam_service_account.frontend-sa.id}",
  ]
}

resource "yandex_serverless_container" "frontend-container" {
  name               = "frontend"
  memory             = 128
  service_account_id = yandex_iam_service_account.frontend-sa.id
  image {
    url = "cr.yandex/${var.frontend_registry}/${var.frontend_repository}:${var.github_sha}"
  }
}

output "frontend-url" {
  value = yandex_serverless_container.frontend-container.url
}