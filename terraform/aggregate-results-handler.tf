module "aggregate-results-handler" {
  source = "./modules/dotnet_function"

  bucket              = var.function_packages_bucket
  deployer_access_key = var.deployer_access_key
  deployer_secret_key = var.deployer_secret_key
  environment         = merge(
    local.data_env
  )
  folder_id         = var.folder_id
  github_sha        = var.github_sha
  max_retries       = 5
  name              = "aggregate-results"
  namespace         = "AggregateResultsHandler"
  roles             = []
  execution_timeout = "10"
}