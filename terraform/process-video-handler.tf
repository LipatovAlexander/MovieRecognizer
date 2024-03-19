resource "yandex_iam_service_account" "process-video-handler-sa" {
  name = "process-video-handler-sa"
}

resource "yandex_resourcemanager_folder_iam_member" "process-video-handler-sa-ymq-writer" {
  folder_id = var.folder_id
  role      = "ymq.writer"
  member    = "serviceAccount:${yandex_iam_service_account.process-video-handler-sa.id}"
}

resource "yandex_iam_service_account_static_access_key" "process-video-handler-sa-key" {
  service_account_id = yandex_iam_service_account.process-video-handler-sa.id
}

resource "yandex_function" "process-video-handler" {
  name              = "process-video-handler"
  user_hash         = var.github_sha
  runtime           = "dotnet8"
  entrypoint        = "ProcessVideoHandler.Handler"
  memory            = "128"
  execution_timeout = "60"
  package {
    bucket_name = var.function_packages_bucket
    object_name = "process-video-handler/${var.github_sha}.zip"
  }
  environment = merge(
    {
      AWS_ACCESS_KEY_ID     = yandex_iam_service_account_static_access_key.process-video-handler-sa-key.access_key
      AWS_SECRET_ACCESS_KEY = yandex_iam_service_account_static_access_key.process-video-handler-sa-key.secret_key
      AWS_DEFAULT_REGION    = "ru-central1"
    },
    local.data_env,
    local.message_queue_env,
    local.file_storage_env
  )
  service_account_id = yandex_iam_service_account.process-video-handler-sa.id
}

resource "yandex_function" "process-video-failure-handler" {
  name              = "process-video-failure-handler"
  user_hash         = var.github_sha
  runtime           = "dotnet8"
  entrypoint        = "ProcessVideoHandler.FailureHandler"
  memory            = "128"
  execution_timeout = "60"
  package {
    bucket_name = var.function_packages_bucket
    object_name = "process-video-handler/${var.github_sha}.zip"
  }
  environment = merge(
    {
      AWS_ACCESS_KEY_ID     = yandex_iam_service_account_static_access_key.process-video-handler-sa-key.access_key
      AWS_SECRET_ACCESS_KEY = yandex_iam_service_account_static_access_key.process-video-handler-sa-key.secret_key
      AWS_DEFAULT_REGION    = "ru-central1"
    },
    local.data_env,
    local.message_queue_env,
    local.file_storage_env
  )
  service_account_id = yandex_iam_service_account.process-video-handler-sa.id
}

resource "yandex_message_queue" "process-video-handler-queue" {
  name                       = "process-video-handler-queue"
  visibility_timeout_seconds = 60
  message_retention_seconds  = 1209600

  access_key = var.deployer_access_key
  secret_key = var.deployer_secret_key

  redrive_policy = jsonencode({
    deadLetterTargetArn = yandex_message_queue.process-video-handler-deadletter-queue.arn
    maxReceiveCount     = 5
  })
}

resource "yandex_message_queue" "process-video-handler-deadletter-queue" {
  name = "process-video-handler-deadletter-queue"

  access_key = var.deployer_access_key
  secret_key = var.deployer_secret_key
}

resource "yandex_function_trigger" "process-video-handler-trigger" {
  name = "process-video-handler-trigger"
  message_queue {
    queue_id           = yandex_message_queue.process-video-handler-queue.arn
    service_account_id = yandex_iam_service_account.process-video-handler-trigger-sa.id
    batch_size         = "1"
    batch_cutoff       = "0"
  }
  function {
    id                 = yandex_function.process-video-handler.id
    service_account_id = yandex_iam_service_account.process-video-handler-trigger-sa.id
  }
}

resource "yandex_function_trigger" "process-video-failure-handler-trigger" {
  name = "process-video-failure-handler-trigger"
  message_queue {
    queue_id           = yandex_message_queue.process-video-handler-deadletter-queue.arn
    service_account_id = yandex_iam_service_account.process-video-handler-trigger-sa.id
    batch_size         = "1"
    batch_cutoff       = "0"
  }
  function {
    id                 = yandex_function.process-video-failure-handler.id
    service_account_id = yandex_iam_service_account.process-video-handler-trigger-sa.id
  }
}

resource "yandex_iam_service_account" "process-video-handler-trigger-sa" {
  name = "process-video-handler-trigger-sa"
}

resource "yandex_resourcemanager_folder_iam_member" "process-video-handler-trigger-editor" {
  folder_id = var.folder_id
  role      = "editor"
  member    = "serviceAccount:${yandex_iam_service_account.process-video-handler-trigger-sa.id}"
}

resource "yandex_resourcemanager_folder_iam_member" "process-video-handler-function-trigger-invoker" {
  folder_id = var.folder_id
  role      = "functions.functionInvoker"
  member    = "serviceAccount:${yandex_iam_service_account.process-video-handler-trigger-sa.id}"
}