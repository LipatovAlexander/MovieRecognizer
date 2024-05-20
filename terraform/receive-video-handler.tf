module "receive-video-handler" {
  source = "./modules/dotnet_function"

  bucket              = var.function_packages_bucket
  deployer_access_key = var.deployer_access_key
  deployer_secret_key = var.deployer_secret_key
  environment         = merge(
    local.data_env,
    local.message_queue_env,
    local.proxy_env
  )
  folder_id   = var.folder_id
  github_sha  = var.github_sha
  max_retries = 5
  name        = "receive-video"
  namespace   = "ReceiveVideoHandler"
  roles       = ["ymq.writer"]
}