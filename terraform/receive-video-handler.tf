resource "yandex_iam_service_account" "receive-video-handler-sa" {
  name = "receive-video-handler-sa"
}

resource "yandex_resourcemanager_folder_iam_member" "receive-video-handler-sa-ymq-writer" {
  folder_id = var.folder_id
  role      = "ymq.writer"
  member    = "serviceAccount:${yandex_iam_service_account.receive-video-handler-sa.id}"
}

resource "yandex_iam_service_account_static_access_key" "receive-video-handler-sa-key" {
  service_account_id = yandex_iam_service_account.receive-video-handler-sa.id
}

resource "yandex_function" "receive-video-handler" {
  name              = "receive-video-handler"
  user_hash         = var.github_sha
  runtime           = "dotnet8"
  entrypoint        = "ReceiveVideoHandler.Handler"
  memory            = "128"
  execution_timeout = "10"
  package {
    bucket_name = var.function_packages_bucket
    object_name = "receive-video-handler/${var.github_sha}.zip"
  }
  environment = merge(
    {
      AWS_ACCESS_KEY_ID     = yandex_iam_service_account_static_access_key.receive-video-handler-sa-key.access_key
      AWS_SECRET_ACCESS_KEY = yandex_iam_service_account_static_access_key.receive-video-handler-sa-key.secret_key
      AWS_DEFAULT_REGION    = "ru-central1"
    },
    local.data_env,
    local.message_queue_env,
  )
  service_account_id = yandex_iam_service_account.receive-video-handler-sa.id
}

resource "yandex_message_queue" "receive-video-handler-queue" {
  name                       = "receive-video-handler-queue"
  visibility_timeout_seconds = 10
  message_retention_seconds  = 1209600

  access_key = var.deployer_access_key
  secret_key = var.deployer_secret_key

  redrive_policy = jsonencode({
    deadLetterTargetArn = yandex_message_queue.receive-video-handler-deadletter-queue.arn
    maxReceiveCount     = 5
  })
}

resource "yandex_message_queue" "receive-video-handler-deadletter-queue" {
  name = "receive-video-handler-deadletter-queue"

  access_key = var.deployer_access_key
  secret_key = var.deployer_secret_key
}

resource "yandex_function_trigger" "receive-video-handler-trigger" {
  name = "receive-video-handler-trigger"
  message_queue {
    queue_id           = yandex_message_queue.receive-video-handler-queue.arn
    service_account_id = yandex_iam_service_account.receive-video-handler-trigger-sa.id
    batch_size         = "1"
    batch_cutoff       = "0"
  }
  function {
    id                 = yandex_function.receive-video-handler.id
    service_account_id = yandex_iam_service_account.receive-video-handler-trigger-sa.id
  }
}

resource "yandex_iam_service_account" "receive-video-handler-trigger-sa" {
  name = "receive-video-handler-trigger-sa"
}

resource "yandex_resourcemanager_folder_iam_member" "receive-video-handler-trigger-editor" {
  folder_id = var.folder_id
  role      = "editor"
  member    = "serviceAccount:${yandex_iam_service_account.receive-video-handler-trigger-sa.id}"
}

resource "yandex_resourcemanager_folder_iam_member" "receive-video-handler-function-trigger-invoker" {
  folder_id = var.folder_id
  role      = "functions.functionInvoker"
  member    = "serviceAccount:${yandex_iam_service_account.receive-video-handler-trigger-sa.id}"
}