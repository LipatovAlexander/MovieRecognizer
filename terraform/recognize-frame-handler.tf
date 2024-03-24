module "recognize-frame-handler" {
  source = "./modules/dotnet_function"

  bucket              = var.function_packages_bucket
  deployer_access_key = var.deployer_access_key
  deployer_secret_key = var.deployer_secret_key
  environment         = merge(
    local.data_env,
    local.message_queue_env,
    local.file_storage_env,
    {
      YANDEX_REVERSE_IMAGE_SEARCH_URL     = "https://functions.yandexcloud.net/${yandex_function.yandex-reverse-image-search.id}"
      YANDEX_REVERSE_IMAGE_SEARCH_API_KEY = yandex_iam_service_account_api_key.sa-yandex-reverse-image-search-api-key.secret_key
    }
  )
  folder_id         = var.folder_id
  github_sha        = var.github_sha
  max_retries       = 5
  name              = "recognize-frame"
  namespace         = "RecognizeFrameHandler"
  roles             = ["ymq.writer"]
  execution_timeout = "10"
}