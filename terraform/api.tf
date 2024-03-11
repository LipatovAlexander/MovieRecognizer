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
  memory             = 128
  service_account_id = yandex_iam_service_account.api-sa.id
  image {
    url         = "cr.yandex/${var.api_registry}/${var.api_repository}:${var.github_sha}"
    environment = {
      YDB_ENDPOINT          = "grpcs://${yandex_ydb_database_serverless.main-db.ydb_api_endpoint}"
      YDB_DATABASE_PATH     = yandex_ydb_database_serverless.main-db.database_path
      AWS_ACCESS_KEY_ID     = yandex_iam_service_account_static_access_key.api-sa-key.access_key
      AWS_SECRET_ACCESS_KEY = yandex_iam_service_account_static_access_key.api-sa-key.secret_key
      AWS_DEFAULT_REGION    = "ru-central1"
      AWS_SQS_ENDPOINT      = "https://message-queue.api.cloud.yandex.net"
      RECEIVE_VIDEO_QUEUE   = yandex_message_queue.receive-video-handler-queue.id
    }
  }
}