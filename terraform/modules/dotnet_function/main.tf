resource "yandex_iam_service_account" "sa" {
  name = "${var.name}-handler-sa"
}

resource "yandex_resourcemanager_folder_iam_member" "roles" {
  for_each = toset(var.roles)

  folder_id = var.folder_id
  role      = each.value
  member    = "serviceAccount:${yandex_iam_service_account.sa.id}"
}

resource "yandex_iam_service_account_static_access_key" "sa-key" {
  service_account_id = yandex_iam_service_account.sa.id
}

resource "yandex_function" "handler" {
  name              = "${var.name}-handler"
  user_hash         = var.github_sha
  runtime           = "dotnet8"
  entrypoint        = "${var.namespace}.Handler"
  memory            = var.memory
  execution_timeout = var.execution_timeout
  package {
    bucket_name = var.bucket
    object_name = "${var.name}-handler/${var.github_sha}.zip"
  }
  environment = merge(
    {
      AWS_ACCESS_KEY_ID     = yandex_iam_service_account_static_access_key.sa-key.access_key
      AWS_SECRET_ACCESS_KEY = yandex_iam_service_account_static_access_key.sa-key.secret_key
      AWS_DEFAULT_REGION    = "ru-central1"
    },
    var.environment
  )
  service_account_id = yandex_iam_service_account.sa.id
}

resource "yandex_function" "failure-handler" {
  name              = "${var.name}-failure-handler"
  user_hash         = var.github_sha
  runtime           = "dotnet8"
  entrypoint        = "${var.namespace}.FailureHandler"
  memory            = var.memory
  execution_timeout = var.execution_timeout
  package {
    bucket_name = var.bucket
    object_name = "${var.name}-handler/${var.github_sha}.zip"
  }
  environment = merge(
    {
      AWS_ACCESS_KEY_ID     = yandex_iam_service_account_static_access_key.sa-key.access_key
      AWS_SECRET_ACCESS_KEY = yandex_iam_service_account_static_access_key.sa-key.secret_key
      AWS_DEFAULT_REGION    = "ru-central1"
    },
    var.environment
  )
  service_account_id = yandex_iam_service_account.sa.id
}

resource "yandex_message_queue" "queue" {
  name                       = "${var.name}-handler-queue"
  visibility_timeout_seconds = var.execution_timeout
  message_retention_seconds  = 1209600

  access_key = var.deployer_access_key
  secret_key = var.deployer_secret_key

  redrive_policy = jsonencode({
    deadLetterTargetArn = yandex_message_queue.deadletter-queue.arn
    maxReceiveCount     = var.max_retries
  })
}

resource "yandex_message_queue" "deadletter-queue" {
  name                       = "${var.name}-handler-deadletter-queue"
  visibility_timeout_seconds = var.execution_timeout

  access_key = var.deployer_access_key
  secret_key = var.deployer_secret_key
}

resource "yandex_function_trigger" "trigger" {
  name = "${var.name}-handler-trigger"
  message_queue {
    queue_id           = yandex_message_queue.queue.arn
    service_account_id = yandex_iam_service_account.trigger-sa.id
    batch_size         = "1"
    batch_cutoff       = "0"
  }
  function {
    id                 = yandex_function.handler.id
    service_account_id = yandex_iam_service_account.trigger-sa.id
  }
}

resource "yandex_function_trigger" "failure-trigger" {
  name = "${var.name}-failure-handler-trigger"
  message_queue {
    queue_id           = yandex_message_queue.deadletter-queue.arn
    service_account_id = yandex_iam_service_account.trigger-sa.id
    batch_size         = "1"
    batch_cutoff       = "0"
  }
  function {
    id                 = yandex_function.failure-handler.id
    service_account_id = yandex_iam_service_account.trigger-sa.id
  }
}

resource "yandex_iam_service_account" "trigger-sa" {
  name = "${var.name}-handler-trigger-sa"
}

resource "yandex_resourcemanager_folder_iam_member" "trigger-editor" {
  folder_id = var.folder_id
  role      = "editor"
  member    = "serviceAccount:${yandex_iam_service_account.trigger-sa.id}"
}

resource "yandex_resourcemanager_folder_iam_member" "trigger-function-invoker" {
  folder_id = var.folder_id
  role      = "functions.functionInvoker"
  member    = "serviceAccount:${yandex_iam_service_account.trigger-sa.id}"
}