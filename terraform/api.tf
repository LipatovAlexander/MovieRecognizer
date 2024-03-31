resource "yandex_iam_service_account" "api-sa" {
  name = "api-sa"
}

resource "yandex_resourcemanager_folder_iam_member" "api-sa-ymq-writer" {
  folder_id = var.folder_id
  role      = "ymq.writer"
  member    = "serviceAccount:${yandex_iam_service_account.api-sa.id}"
}

resource "yandex_container_registry_iam_binding" "api-sa-puller" {
  registry_id = var.api_registry
  role        = "container-registry.images.puller"

  members = [
    "serviceAccount:${yandex_iam_service_account.api-sa.id}",
  ]
}

resource "yandex_iam_service_account_static_access_key" "api-sa-key" {
  service_account_id = yandex_iam_service_account.api-sa.id
}

resource "yandex_serverless_container" "test-container" {
  name               = "api"
  memory             = 512
  service_account_id = yandex_iam_service_account.api-sa.id
  image {
    url         = "cr.yandex/${var.api_registry}/${var.api_repository}:${var.github_sha}"
    environment = merge(
      {
        AWS_ACCESS_KEY_ID     = yandex_iam_service_account_static_access_key.api-sa-key.access_key
        AWS_SECRET_ACCESS_KEY = yandex_iam_service_account_static_access_key.api-sa-key.secret_key
        AWS_DEFAULT_REGION    = "ru-central1"
      },
      local.data_env,
      local.message_queue_env,
      local.file_storage_env
    )
  }
}

output "api-url" {
  value = "${yandex_serverless_container.test-container.url}swagger"
}